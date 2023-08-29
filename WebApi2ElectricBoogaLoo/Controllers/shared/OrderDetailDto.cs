using NorthWindTradersApi.Controllers.shared;
using NorthWindTradersApi.Models;

namespace NorthWindTradersApi.Controllers
{
    public class OrderDetailDto
    {
        public int OrderId { get; set; }
        public int? ProductId { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? Quantity { get; set; }
        public float? Discount { get; set; }
        public ProductDto? Product { get; set; }

    }
}