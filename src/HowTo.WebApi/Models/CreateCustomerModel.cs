namespace HowTo.WebApi.Models
{
    public class CreateCustomerModel
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? EmailAddress { get; set; }

        public string? BillingStreetAddress { get; set; }

        public string? BillingCity { get; set; }

        public string? BillingPostalCode { get; set; }

        public string? BillingState { get; set; }

        public string? BillingCountry { get; set; }

        public string? ShippingStreetAddress { get; set; }

        public string? ShippingCity { get; set; }

        public string? ShippingPostalCode { get; set; }

        public string? ShippingState { get; set; }

        public string? ShippingCountry { get; set; }
    }
}
