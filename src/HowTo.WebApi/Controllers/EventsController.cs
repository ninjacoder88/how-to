using AutoFixture;
using HowTo.DataAccess;
using HowTo.DataAccess.Models;
using HowTo.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace HowTo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    [RequiredScope("WebAppScope")]
    [ApiVersion("1.0")]
    public class EventsController : ControllerBase
    {
        public EventsController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? startDate, [FromQuery] string? startTime, [FromQuery] string? endDate, [FromQuery] string? endTime)
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            DateTimeOffset startDateTimeOffset = GetFromDateAndTime(startDate, startTime, today);
            DateTimeOffset endDateTimeOffset = GetFromDateAndTime(endDate, endTime, tomorrow);

            if (startDateTimeOffset >= endDateTimeOffset)
                return BadRequest("Start must be before end");

            var events = await _repository.LoadEventsAsync(startDateTimeOffset, endDateTimeOffset);
            return new JsonResult(new ResponseModel<List<EventModel>>(true) { Data = events });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateEventModel model)
        {
            if (model == null)
                return BadRequest("No data received");

            if (string.IsNullOrEmpty(model.Title))
                return BadRequest("Title must be set");

            if (string.IsNullOrEmpty(model.StartDate) || string.IsNullOrEmpty(model.StartTime))
                return BadRequest("Start date and time must be set");

            if (string.IsNullOrEmpty(model.EndDate) || string.IsNullOrEmpty(model.EndTime))
                return BadRequest("End date and time must be set");

            string startDateTime = $"{model.StartDate} {model.StartTime}";
            string endDateTime = $"{model.EndDate} {model.EndTime}";

            if (!DateTimeOffset.TryParse(startDateTime, out DateTimeOffset startDateTimeOffset))
                return BadRequest("Could not convert start date and time");

            if (!DateTimeOffset.TryParse(endDateTime, out DateTimeOffset endDateTimeOffset))
                return BadRequest("Could not convert end date and time");

            if (startDateTimeOffset >= endDateTimeOffset)
                return BadRequest("Start must be before end");

            var eventId = await _repository.CreateEventAsync(model.Title, model.Description, startDateTimeOffset, endDateTimeOffset);
            return new JsonResult(new ResponseModel<string>(true) { Data = eventId });
        }

        [HttpGet("init")]
        [AllowAnonymous]
        public async Task<IActionResult> Initialize([FromQuery] int eventCount)
        {
            if (eventCount < 1)
                return BadRequest("eventCount must be greater than 0");

            Fixture f = new Fixture();

            var now = new DateTimeOffset(DateTime.Now);
            Random random = new Random();
            for(int i = 0; i < eventCount; i++)
            {
                var start = now.AddDays(random.Next(0, 100));
                var end = start.AddHours(random.Next(1, 8));

                await _repository.CreateEventAsync(f.Create<string>(), f.Create<string>(), start, end);
            }
            return new JsonResult(true);
        }

        private static DateTimeOffset GetFromDateAndTime(string? date, string? time, DateTime today)
        {
            string dateTimeStr;

            if (string.IsNullOrEmpty(date))
                dateTimeStr = $"{today:yyyy-MM-dd} ";
            else
                dateTimeStr = $"{date} ";

            if (string.IsNullOrEmpty(time))
                dateTimeStr += $"00:00";
            else
                dateTimeStr += time;

            if (!DateTimeOffset.TryParse(dateTimeStr, out DateTimeOffset dateTimeOffset))
                return new DateTimeOffset(today);
            return dateTimeOffset;
        }

        private readonly IRepository _repository;
    }
}
