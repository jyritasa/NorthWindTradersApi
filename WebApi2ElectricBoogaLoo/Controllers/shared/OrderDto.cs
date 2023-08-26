using WebApi2ElectricBoogaLoo.Controllers.shared;
using WebApi2ElectricBoogaLoo.Models;

namespace WebApi2ElectricBoogaLoo.Controllers
{
    public class OrderDto
    {
        public int OrderId { get; set; }
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