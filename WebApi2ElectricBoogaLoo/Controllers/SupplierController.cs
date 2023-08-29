namespace NorthWindTradersApi.Controllers
{
    using global::NorthWindTradersApi.Controllers.shared;
    using global::NorthWindTradersApi.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    namespace NorthWindTradersApi.Controllers
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
                        Category = p.Category != null ? new CategoryDto
                        {
                            CategoryId = p.Category.CategoryId,
                            CategoryName = p.Category.CategoryName,
                            Description = p.Category.Description,
                            Picture = p.Category.Picture != null
                                ? EncodePictureToString.FromOLE(p.Category.Picture)
                                : null
                        } : null
                    }).ToList()
                };
            }
        }
    }

}
