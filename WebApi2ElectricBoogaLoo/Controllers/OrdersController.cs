using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthWindTradersApi.Controllers.shared;
using NorthWindTradersApi.Models;

namespace NorthWindTradersApi.Controllers
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
                .Select(o => CreateOrderDto(o))
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

            return Ok(CreateOrderDto(order));
        }

        [HttpPost(Name = "PostOrder")]
        public IActionResult PostOrder(OrderDto orderDto)
        {
            if (orderDto == null)
            {
                return BadRequest("Order data is missing.");
            }


            // Create a new Order entity based on the DTO
            var newOrder = new Order
            {
                OrderDate = orderDto.OrderDate,
                ShippedDate = orderDto.ShippedDate,
                Freight = orderDto.Freight,
                ShipName = orderDto.ShipName,
                ShipAddress = orderDto.ShipAddress,
                ShipCity = orderDto.ShipCity,
                ShipRegion = orderDto.ShipRegion,
                ShipPostalCode = orderDto.ShipPostalCode,
                ShipCountry = orderDto.ShipCountry,
                CustomerId = orderDto.CustomerId,
                EmployeeId = orderDto.EmployeeId
            };

            // Create and add new OrderDetails to the newOrder
            if (orderDto.OrderDetails != null)
            {
                foreach (var orderDetailDto in orderDto.OrderDetails)
                {
                    var newOrderDetail = new OrderDetail
                    {
                        ProductId = orderDetailDto.ProductId ?? 0,
                        UnitPrice = orderDetailDto.UnitPrice ?? 0,
                        Quantity = orderDetailDto.Quantity ?? 0,
                        Discount = orderDetailDto.Discount ?? 0
                    };
                    newOrder.OrderDetails.Add(newOrderDetail);
                }
            }

            // Set ShipViaNavigation using orderDto.ShipViaNavigation if available
            if (orderDto.ShipViaNavigation != null)
            {
                newOrder.ShipViaNavigation = context.Shippers.Find(orderDto.ShipViaNavigation.ShipperId);
            }

            // Add newOrder to the context and save changes
            context.Orders.Add(newOrder);
            context.SaveChanges();

            return CreatedAtRoute("GetOrderById", new { id = newOrder.OrderId }, CreateOrderDto(newOrder));
        }

        [HttpPut("{id}", Name = "UpdateOrder")]
        public IActionResult UpdateOrder(int id, OrderDto orderDto)
        {
            var existingOrder = context.Orders.FirstOrDefault(o => o.OrderId == id);

            if (existingOrder == null)
            {
                return NotFound();
            }

            // Update properties based on the DTO
            existingOrder.OrderDate = orderDto.OrderDate;
            existingOrder.ShippedDate = orderDto.ShippedDate;
            existingOrder.Freight = orderDto.Freight;
            existingOrder.ShipName = orderDto.ShipName;
            existingOrder.ShipAddress = orderDto.ShipAddress;
            existingOrder.ShipCity = orderDto.ShipCity;
            existingOrder.ShipRegion = orderDto.ShipRegion;
            existingOrder.ShipPostalCode = orderDto.ShipPostalCode;
            existingOrder.ShipCountry = orderDto.ShipCountry;
            existingOrder.CustomerId = orderDto.CustomerId;
            existingOrder.EmployeeId = orderDto.EmployeeId;

            // Update OrderDetails
            existingOrder.OrderDetails.Clear(); // Remove existing details
            if (orderDto.OrderDetails != null)
            {
                foreach (var orderDetailDto in orderDto.OrderDetails)
                {
                    var newOrderDetail = new OrderDetail
                    {
                        ProductId = orderDetailDto.ProductId ?? 0,
                        UnitPrice = orderDetailDto.UnitPrice ?? 0,
                        Quantity = orderDetailDto.Quantity ?? 0,
                        Discount = orderDetailDto.Discount ?? 0
                    };
                    existingOrder.OrderDetails.Add(newOrderDetail);
                }
            }

            // Update ShipViaNavigation
            if (orderDto.ShipViaNavigation != null)
            {
                existingOrder.ShipViaNavigation = context.Shippers.Find(orderDto.ShipViaNavigation.ShipperId);
            }
            else
            {
                existingOrder.ShipViaNavigation = null; // Clear if not provided
            }

            // Save changes
            context.SaveChanges();

            return Ok(CreateOrderDto(existingOrder)); // Return updated OrderDto
        }


        [HttpDelete("{id}", Name = "DeleteOrder")]
        public IActionResult DeleteOrder(int id)
        {
            var existingOrder = context.Orders.Include(o => o.OrderDetails)
                                              .FirstOrDefault(o => o.OrderId == id);

            if (existingOrder == null)
            {
                return NotFound();
            }

            // Remove OrderDetails
            context.OrderDetails.RemoveRange(existingOrder.OrderDetails);

            // Remove ShipViaNavigation
            existingOrder.ShipViaNavigation = null;

            // Remove the order
            context.Orders.Remove(existingOrder);
            context.SaveChanges();

            return NoContent(); // Return a success response
        }

        static private OrderDto CreateOrderDto(Order o)
        {
            return new OrderDto
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
                Customer = o.Customer != null ? new CustomerDto
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
                } : null,
                Employee = o.Employee != null ? new EmployeeDto
                {
                    EmployeeId = o.Employee.EmployeeId,
                    FirstName = o.Employee.FirstName,
                    LastName = o.Employee.LastName
                } : null,
                OrderDetails = o.OrderDetails.Select(od => new OrderDetailDto
                {
                    ProductId = od.ProductId,
                    UnitPrice = od.UnitPrice,
                    Quantity = od.Quantity,
                    Discount = od.Discount,
                    Product = od.Product != null ? new ProductDto
                    {
                        ProductId = od.Product.ProductId,
                        ProductName = od.Product.ProductName,
                        UnitPrice = od.Product.UnitPrice ?? 0,
                        Category = od.Product.Category != null ? new CategoryDto
                        {
                            CategoryId = od.Product.Category.CategoryId,
                            CategoryName = od.Product.Category.CategoryName,
                            Description = od.Product.Category.Description,
                            Picture = od.Product.Category.Picture != null
                                ? EncodePictureToString.FromOLE(od.Product.Category.Picture)
                                : null
                        } : null
                    } : null
                }).ToList(),
                ShipViaNavigation = o.ShipViaNavigation != null ? new ShipperDto
                {
                    ShipperId = o.ShipViaNavigation.ShipperId,
                    CompanyName = o.ShipViaNavigation.CompanyName,
                    Phone = o.ShipViaNavigation.Phone
                } : null
            };
        }
    }
}