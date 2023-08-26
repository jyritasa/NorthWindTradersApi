using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi2ElectricBoogaLoo.Controllers.shared;
using WebApi2ElectricBoogaLoo.Models;

namespace WebApi2ElectricBoogaLoo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly NorthwindContext context = new();

        [HttpGet(Name = "GetAllOrders")]
        public IActionResult GetAllOrders()
        {
            var orders = context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.Category)
                .Include(o => o.ShipViaNavigation)
                .Select(o => new OrderDto
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
                    Customer = new CustomerDto
                    {
                        CustomerId = o.Customer.CustomerId,
                        CompanyName = o.Customer.CompanyName,
                        ContactName = o.Customer.ContactName,
                        ContactTitle = o.Customer.ContactTitle,
                        City = o.Customer.City,
                        Region = o.Customer.Region,
                        PostalCode = o.Customer.PostalCode,
                        Country = o.Customer.Country,
                        Phone = o.Customer.Phone,
                        Fax = o.Customer.Fax
                    },
                    Employee = new EmployeeDto
                    {
                        EmployeeId = o.Employee.EmployeeId,
                        FirstName = o.Employee.FirstName,
                        LastName = o.Employee.LastName
                    },
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
                            Category = new CategoryDto
                            {
                                CategoryId = od.Product.Category.CategoryId,
                                CategoryName = od.Product.Category.CategoryName,
                                Description = od.Product.Category.Description,
                                Picture = od.Product.Category.Picture
                            }
                        }
                    }).ToList(),
                    ShipViaNavigation = new ShipperDto
                    {
                        ShipperId = o.ShipViaNavigation.ShipperId,
                        CompanyName = o.ShipViaNavigation.CompanyName,
                        Phone = o.ShipViaNavigation.Phone
                    }
                })
                .ToList();

            return Ok(orders);
        }

        [HttpGet("{id}", Name = "GetOrderById")]
        public IActionResult GetOrderById(int id)
        {
            var order = context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.Category)
                .Include(o => o.ShipViaNavigation)
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            var orderDto = new OrderDto
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                ShippedDate = order.ShippedDate,
                Freight = order.Freight,
                ShipName = order.ShipName,
                ShipAddress = order.ShipAddress,
                ShipCity = order.ShipCity,
                ShipRegion = order.ShipRegion,
                ShipPostalCode = order.ShipPostalCode,
                ShipCountry = order.ShipCountry,
                Customer = new CustomerDto
                {
                    CustomerId = order.Customer.CustomerId,
                    CompanyName = order.Customer.CompanyName,
                    ContactName = order.Customer.ContactName,
                    ContactTitle = order.Customer.ContactTitle,
                    City = order.Customer.City,
                    Region = order.Customer.Region,
                    PostalCode = order.Customer.PostalCode,
                    Country = order.Customer.Country,
                    Phone = order.Customer.Phone,
                    Fax = order.Customer.Fax
                },
                Employee = new EmployeeDto
                {
                    EmployeeId = order.Employee.EmployeeId,
                    FirstName = order.Employee.FirstName,
                    LastName = order.Employee.LastName
                },
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailDto
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
                        Category = new CategoryDto
                        {
                            CategoryId = od.Product.Category.CategoryId,
                            CategoryName = od.Product.Category.CategoryName,
                            Description = od.Product.Category.Description,
                            Picture = od.Product.Category.Picture
                        }
                    }
                }).ToList(),
                ShipViaNavigation = new ShipperDto
                {
                    ShipperId = order.ShipViaNavigation.ShipperId,
                    CompanyName = order.ShipViaNavigation.CompanyName,
                    Phone = order.ShipViaNavigation.Phone
                }
            };

            return Ok(orderDto);
        }
    }
}