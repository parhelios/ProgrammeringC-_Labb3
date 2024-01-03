using System.Text.Json.Serialization;
using Labb3ProgTemplate.DataModels.Products;
using Labb3ProgTemplate.Enums;

namespace Labb3ProgTemplate.DataModels.ProductCategories;

public class Meat : Product
{
    [JsonConstructor]
    public Meat(string name, double price, string imageUrl) : base(name, price, ProductEnums.Meat, imageUrl)
    {
    }
}