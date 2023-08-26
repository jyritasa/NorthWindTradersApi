using System;
using System.Collections.Generic;

namespace WebApi2ElectricBoogaLoo.Models;

public partial class ProductsAboveAveragePrice
{
    public string ProductName { get; set; } = null!;

    public decimal? UnitPrice { get; set; }
}
