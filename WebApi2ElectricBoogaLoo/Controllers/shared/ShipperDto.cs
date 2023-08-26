namespace WebApi2ElectricBoogaLoo.Controllers.shared
{
    public class ShipperDto
    {
        public required int ShipperId { get; set; }
        public string? CompanyName { get; set; }
        public string? Phone { get; set; }
    }
}
