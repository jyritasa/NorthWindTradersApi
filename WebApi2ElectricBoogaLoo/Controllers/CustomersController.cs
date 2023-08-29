using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthWindTradersApi.Controllers.shared;
using NorthWindTradersApi.Models;

namespace NorthWindTradersApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly NorthwindContext context = new();

        [HttpGet(Name = "GetAllCustomers")]
        public IActionResult GetAllCustomers()
        {
            var customers = context.Customers
                .Include(c => c.Orders)
                .ThenInclude(o => o.OrderDetails)
                .ThenInclude(o => o.Product)
                .ThenInclude(p => p.Category) 
                .Include(c => c.Orders)
                .ThenInclude(o => o.ShipViaNavigation) 
                .Select(c => CreateCustomerDto(c)).ToList();
            return Ok(customers);
        }


        [HttpGet("{id}", Name = "GetCustomerByID")]
        public IActionResult GetCustomerByID(string id)
        {
            var customer = context.Customers
                .Include(c => c.Orders)
                .ThenInclude(o => o.OrderDetails)
                .ThenInclude(o => o.Product)
                .ThenInclude(p => p.Category)
                .Include(c => c.Orders)
                .ThenInclude(o => o.ShipViaNavigation)
                .Where(c => c.CustomerId == id)
                .Select(c => CreateCustomerDto(c))
                .FirstOrDefault();

            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpGet("customerMinimal")]
        public IActionResult GetCustomerInfo()
        {
            var customerInfo = context.Customers.Select(c => new
            {
                c.CustomerId,
                c.CompanyName
            }).ToList();

            return Ok(customerInfo);
        }

        static private CustomerDto CreateCustomerDto(Customer c)
        {
            return new CustomerDto
            {
                CustomerId = c.CustomerId,
                CompanyName = c.CompanyName,
                ContactName = c.ContactName,
                ContactTitle = c.ContactTitle,
                City = c.City,
                Region = c.Region,
                PostalCode = c.PostalCode,
                Country = c.Country,
                Phone = c.Phone,
                Fax = c.Fax,
                Orders = c.Orders.Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    ShippedDate = o.ShippedDate,
                    Freight = o.Freight,
                    ShipName = o.ShipName,
                    ShipAddress = o.ShipAddress,
                    ShipCity = o.ShipCity,
                    ShipRegion = o.ShipRegion,
                    ShipPostalCode = o.ShipPostalCode,
                    ShipCountry = o.ShipCountry,
                    ShipViaNavigation = o.ShipViaNavigation != null ? new ShipperDto
                    {
                        ShipperId = o.ShipViaNavigation.ShipperId,
                        CompanyName = o.ShipViaNavigation.CompanyName,
                        Phone = o.ShipViaNavigation.Phone
                    } : null,
                    OrderDetails = o.OrderDetails.Select(od => new OrderDetailDto
                    {
                        ProductId = od.ProductId,
                        UnitPrice = od.UnitPrice,
                        Quantity = od.Quantity,
                        Discount = od.Discount,
                        Product = new ProductDto
                        {
                            ProductId = od.Product.ProductId,
                            ProductName = od.Product.ProductName,
                            UnitPrice = od.Product.UnitPrice,
                            Category = od.Product.Category != null ? new CategoryDto
                            {
                                CategoryId = od.Product.Category.CategoryId,
                                CategoryName = od.Product.Category.CategoryName,
                                Description = od.Product.Category.Description,
                                Picture = od.Product.Category.Picture != null
                                    ? EncodePictureToString.FromOLE(od.Product.Category.Picture)
                                    : null,
                            } : null
                        }
                    }).ToList()
                }).ToList()
            };
        }
    }
}
