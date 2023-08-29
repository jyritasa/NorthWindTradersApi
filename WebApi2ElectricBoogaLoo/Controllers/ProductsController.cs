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
        public class ProductsController : ControllerBase
        {
            private readonly NorthwindContext context = new();

            [HttpGet(Name = "GetAllProducts")]
            public IActionResult GetAllProducts()
            {
                var products = context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Supplier)
                    .Select(p => CreateProductDto(p))
                    .ToList();

                return Ok(products);
            }

            [HttpGet("{id}", Name = "GetProductById")]
            public IActionResult GetProductById(int id)
            {
                var product = context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Supplier)
                    .FirstOrDefault(p => p.ProductId == id);

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(CreateProductDto(product));
            }

            [HttpGet("productinfo")]
            public IActionResult GetProductInfo()
            {
                var productInfo = context.Products.Select(p => new
                {
                    p.ProductId,
                    p.ProductName,
                    p.UnitPrice
                }).ToList();

                return Ok(productInfo);
            }

            static private ProductDto CreateProductDto(Product p)
            {
                return new ProductDto
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
                    } : null,
                    Supplier = p.Supplier != null ? new SupplierDto
                    {
                        SupplierId = p.Supplier.SupplierId,
                        CompanyName = p.Supplier.CompanyName,
                        ContactName = p.Supplier.ContactName,
                        ContactTitle = p.Supplier.ContactTitle,
                        City = p.Supplier.City,
                        Region = p.Supplier.Region,
                        PostalCode = p.Supplier.PostalCode,
                        Country = p.Supplier.Country,
                        Phone = p.Supplier.Phone,
                        Fax = p.Supplier.Fax
                    } : null
                };
            }
        }
    }
}
