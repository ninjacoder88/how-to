using HowTo.WebApi.DataAccess.Entities;
using HowTo.WebApi.Models;

namespace HowTo.WebApi.Extensions
{
    internal static class CustomerEntityExtensions
    {
        public static CustomerModel ToModel(this CustomerEntity entity)
        {
            return new CustomerModel
            {
                CustomerId = entity._id.ToString(),
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                EmailAddress = entity.EmailAddress,
                Addresses = entity.Addresses.Select(x => x.ToModel()).ToList(),
            };
        }

        private static AddressModel ToModel(this AddressEntity entity)
        {
            return new AddressModel
            {
                AddressType = entity.AddressType,
                City = entity.City,
                Country = entity.Country,
                State = entity.State,
                IsPrimary = entity.IsPrimary,
                PostalCode = entity.PostalCode,
                StreetAddress = entity.StreetAddress,
            };
        }
    }
}
