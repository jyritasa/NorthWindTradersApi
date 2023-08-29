﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthWindTradersApi.Controllers.shared;
using NorthWindTradersApi.Models;

namespace NorthWindTradersApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly NorthwindContext context = new();

        [HttpGet(Name = "GetAllEmployees")]
        public IActionResult GetAllEmployees()
        {
            var employees = context.Employees
                .Include(e => e.ReportsToNavigation)
                .Include(e => e.Orders)
                .ThenInclude(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.Category)
                .Include(e => e.Orders)
                .ThenInclude(o => o.ShipViaNavigation)
                .Select(e => CreateEmployeeDto(e))
                .ToList();

            return Ok(employees);
        }

        [HttpGet("{id}", Name = "GetEmployeeById")]
        public IActionResult GetEmployeeById(int id)
        {
            var employee = context.Employees
                .Include(e => e.ReportsToNavigation)
                .Include(e => e.Orders)
                .ThenInclude(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.Category)
                .Include(e => e.Orders)
                .ThenInclude(o => o.ShipViaNavigation)
                .FirstOrDefault(e => e.EmployeeId == id);

            if (employee == null)
            {
                return NotFound();
            }

            var employeeDto = CreateEmployeeDto(employee);

            return Ok(employeeDto);
        }

        [HttpGet("employeeinfo")]
        public IActionResult GetEmployeeInfo()
        {
            var employeeInfo = context.Employees.Select(e => new
            {
                e.EmployeeId,
                e.FirstName,
                e.LastName,

            }).ToList();

            return Ok(employeeInfo);
        }



        static private EmployeeDto CreateEmployeeDto(Employee e)
        {

            return new EmployeeDto
            {
                EmployeeId = e.EmployeeId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Title = e.Title,
                ReportsTo = e.ReportsTo,
                Photo = e.Photo != null
                    ? EncodePictureToString.FromOLE(e.Photo)
                    : null,
                ReportsToEmployee = e.ReportsToNavigation != null ? new EmployeeDto
                {
                    EmployeeId = e.ReportsToNavigation.EmployeeId,
                    FirstName = e.ReportsToNavigation.FirstName,
                    LastName = e.ReportsToNavigation.LastName,
                } : null,
                Orders = e.Orders.Select(o => new OrderDto
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
                                    : null
                            } : null
                        }
                    }).ToList()
                }).ToList()
            };
        }
    }
}