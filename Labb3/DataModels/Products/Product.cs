using System.Text.Json.Serialization;
using Labb3ProgTemplate.Enums;

namespace Labb3ProgTemplate.DataModels.Products;

public abstract class Product
{
    public string Name { get; set; }

    public double Price { get; set; }

    public double Amount { get; set; }

    public ProductEnums ProductCategory { get; set; }

    public string ImageUrl { get; set; }

    [JsonConstructor]
    protected Product(string name, double price, ProductEnums productCategory, string imageUrl)
    {
        Name = name;
        Price = price;
        ProductCategory = productCategory;
        ImageUrl = imageUrl;
    }
}