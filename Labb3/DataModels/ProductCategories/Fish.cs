using System.Text.Json.Serialization;
using Labb3ProgTemplate.DataModels.Products;
using Labb3ProgTemplate.Enums;

namespace Labb3ProgTemplate.DataModels.ProductCategories;

public class Fish : Product
{
    [JsonConstructor]
    public Fish(string name, double price, string imageUrl) : base(name, price, ProductEnums.Fish, imageUrl)
    {
    }
}