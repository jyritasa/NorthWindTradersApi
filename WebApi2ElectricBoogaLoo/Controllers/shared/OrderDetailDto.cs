using WebApi2ElectricBoogaLoo.Controllers.shared;
using WebApi2ElectricBoogaLoo.Models;

namespace WebApi2ElectricBoogaLoo.Controllers
{
    public class OrderDetailDto
    {
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }
        public ProductDto? Product { get; set; }

    }
}