namespace NorthWindTradersApi.Controllers.Shared
{
    public class CustomerDemographicDto
    {
        public required string CustomerTypeId { get; set; }
        public string? CustomerDesc { get; set; }
    }
}