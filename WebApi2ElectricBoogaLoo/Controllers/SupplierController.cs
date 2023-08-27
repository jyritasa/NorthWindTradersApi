﻿namespace WebApi2ElectricBoogaLoo.Controllers
{
    using global::WebApi2ElectricBoogaLoo.Controllers.shared;
    using global::WebApi2ElectricBoogaLoo.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    namespace WebApi2ElectricBoogaLoo.Controllers
    {
        [ApiController]
        [Route("[controller]")]
        public class SuppliersController : ControllerBase
        {
            private readonly NorthwindContext context = new();

            [HttpGet(Name = "GetAllSuppliers")]
            public IActionResult GetAllSuppliers()
            {
                var suppliers = context.Suppliers
                    .Include(s => s.Products) // Include related products
                    .Select(s => CreateSupplierDto(s))
                    .ToList();

                return Ok(suppliers);
            }

            [HttpGet("{id}", Name = "GetSupplierById")]
            public IActionResult GetSupplierById(int id)
            {
                var supplier = context.Suppliers
                    .Include(s => s.Products) // Include related products
                    .FirstOrDefault(s => s.SupplierId == id);

                if (supplier == null)
                {
                    return NotFound();
                }

                return Ok(CreateSupplierDto(supplier));
            }

            static private SupplierDto CreateSupplierDto(Supplier s)
            {
                return new SupplierDto
                {
                    SupplierId = s.SupplierId,
                    CompanyName = s.CompanyName,
                    ContactName = s.ContactName,
                    ContactTitle = s.ContactTitle,
                    City = s.City,
                    Region = s.Region,
                    PostalCode = s.PostalCode,
                    Country = s.Country,
                    Phone = s.Phone,
                    Fax = s.Fax,
                    HomePage = s.HomePage,
                    Products = s.Products.Select(p => new ProductDto
                    {
                        ProductId = p.ProductId,
                        ProductName = p.ProductName,
                        QuantityPerUnit = p.QuantityPerUnit,
                        UnitPrice = p.UnitPrice,
                        UnitsInStock = p.UnitsInStock,
                        UnitsOnOrder = p.UnitsOnOrder,
                        ReorderLevel = p.ReorderLevel,
                        Discontinued = p.Discontinued,
                        Category = new CategoryDto
                        {
                            CategoryId = p.Category.CategoryId,
                            CategoryName = p.Category.CategoryName,
                            Description = p.Category.Description,
                            Picture = EncodePictureToString.FromOLE(p.Category.Picture)
                        }
                    }).ToList()
                };
            }
        }
    }

}