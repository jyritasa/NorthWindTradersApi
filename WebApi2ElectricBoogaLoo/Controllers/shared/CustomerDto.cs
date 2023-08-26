
using WebApi2ElectricBoogaLoo.Controllers.shared;
using WebApi2ElectricBoogaLoo.Controllers.Shared;
using WebApi2ElectricBoogaLoo.Models;

namespace WebApi2ElectricBoogaLoo.Controllers
{
    public class CustomerDto
    {
        public required string CustomerId { get; set; }

        public string? CompanyName { get; set; }

        public string? ContactName { get; set; }

        public string? ContactTitle { get; set; }
        public string? City { get; set; }

        public string? Region { get; set; }

        public string? PostalCode { get; set; }

        public string? Country { get; set; }

        public string? Phone { get; set; }

        public string? Fax { get; set; }
        public List<OrderDto>? Orders { get; set; } = new List<OrderDto>();
        public List<CustomerDemographicDto>? CustomerTypes { get; set; } = new List<CustomerDemographicDto>();
    }

}