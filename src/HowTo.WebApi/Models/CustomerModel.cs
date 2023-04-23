namespace HowTo.WebApi.Models
{
    public class CustomerModel
    {
        public string? CustomerId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? EmailAddress { get; set; }

        public List<AddressModel>? Addresses { get; set; }
    }

    public class AddressModel
    {
        public string? StreetAddress { get; set; }

        public string? City { get; set; }

        public string? PostalCode { get; set; }

        public string? State { get; set; }

        public string? Country { get; set; }

        public string? AddressType { get; set; }

        public bool IsPrimary { get; set; }
    }
}
