using HowTo.WebApi.DataAccess;
using HowTo.WebApi.DataAccess.Entities;
using HowTo.WebApi.Extensions;
using HowTo.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace HowTo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    //[RequiredScope("WebAppScope,ReadOnlyScope")]
    public class CustomerController : ControllerBase
    {
        public CustomerController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var customer = await _repository.LoadCustomerAsync(id);
            if (customer == null)
                return new JsonResult(new ResponseModel<CustomerModel>(false) { ErrorMessage = "Customer not found" });
            return new JsonResult(new ResponseModel<CustomerModel>(true) { Data = customer.ToModel() });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCustomerModel customer)
        {
            if (string.IsNullOrEmpty(customer.FirstName))
                return new JsonResult(new ResponseModel<CustomerModel>(false) { ErrorMessage = "First Name is required" });

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

        private readonly IRepository _repository;
    }
}
