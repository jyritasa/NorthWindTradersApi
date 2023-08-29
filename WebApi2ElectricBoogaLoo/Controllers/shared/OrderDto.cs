using NorthWindTradersApi.Controllers.shared;
using NorthWindTradersApi.Models;

namespace NorthWindTradersApi.Controllers
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string? CustomerId { get; set; }

        public int? EmployeeId { get; set; }

        public DateTime? OrderDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public CustomerDto? Customer { get; set; }
        public EmployeeDto? Employee { get; set; }
        public int? ShipVia { get; set; }

        public decimal? Freight { get; set; }

        public string? ShipName { get; set; }

        public string? ShipAddress { get; set; }

        public string? ShipCity { get; set; }

        public string? ShipRegion { get; set; }

        public string? ShipPostalCode { get; set; }

        public string? ShipCountry { get; set; }

        public List<OrderDetailDto>? OrderDetails { get; set; } = new List<OrderDetailDto>();
        public ShipperDto? ShipViaNavigation { get; set; }
    }
}