using HowTo.WebApi.DataAccess;
using HowTo.WebApi.DataAccess.Entities;
using HowTo.WebApi.Extensions;
using HowTo.WebApi.Logging;
using HowTo.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.Net;

namespace HowTo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [RequiredScope("WebAppScope,ReadOnlyScope")]
    public class CustomerController : ControllerBase
    {
        public CustomerController(IRepository repository, ILogRepository logRepository)
        {
            _repository = repository;
            _logRepository = logRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            string guid = Guid.NewGuid().ToString();
            string username = HttpContext.GetUsername();
            try
            {
                await _logRepository.LogInformationAsync(this, t => t.AddTransactionId(guid).AddData(id).AddUsername(username));

                var customer = await _repository.LoadCustomerAsync(id);
                if (customer == null)
                    return new JsonResult(new ResponseModel<CustomerModel>(false) { ErrorMessage = $"{guid} - Customer not found" });

                return new JsonResult(new ResponseModel<CustomerModel>(true) { Data = customer.ToModel() });
            }
            catch (Exception ex)
            {
                await _logRepository.LogErrorAsync(this, t => t.AddTransactionId(guid).AddData(ex));
                return new ObjectResult($"{guid} - {ex.Message}") { StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCustomerModel customer)
        {
            string guid = Guid.NewGuid().ToString();
            string username = HttpContext.GetUsername();
            try
            {
                await _logRepository.LogInformationAsync(this, t => t.AddTransactionId(guid).AddData(customer).AddUsername(username));

                if (string.IsNullOrEmpty(customer.FirstName))
                    return new JsonResult(new ResponseModel<CustomerModel>(false) { ErrorMessage = $"{guid} - First Name is required" });

                var entity = new CustomerEntity
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    EmailAddress = customer.EmailAddress,
                    Addresses = new List<AddressEntity>
                    {
                        new AddressEntity
                        {
                            AddressType = "Shipping",
                            StreetAddress = customer.ShippingStreetAddress,
                            State = customer.ShippingState,
                            City = customer.ShippingCity,
                            Country = customer.ShippingCountry,
                            IsPrimary = true,
                            PostalCode = customer.ShippingPostalCode
                        },
                        new AddressEntity
                        {
                            AddressType = "Billing",
                            PostalCode = customer.BillingPostalCode,
                            IsPrimary = true,
                            City = customer.BillingCity,
                            Country = customer.BillingCountry,
                            State = customer.BillingState,
                            StreetAddress = customer.BillingStreetAddress
                        }
                    }
                };
                await _repository.CreateCustomerAsync(entity);
                return new JsonResult(new ResponseModel<string>(true) { Data = entity._id.ToString() });
            }
            catch (Exception ex)
            {
                await _logRepository.LogErrorAsync(this, t => t.AddTransactionId(guid).AddData(ex));
                return new ObjectResult($"{guid} - {ex.Message}") { StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }

        private readonly IRepository _repository;
        private readonly ILogRepository _logRepository;
    }
}
