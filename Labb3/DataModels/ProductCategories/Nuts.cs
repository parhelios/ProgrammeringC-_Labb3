using Labb3ProgTemplate.DataModels.Products;
using Labb3ProgTemplate.Enums;
using System.Text.Json.Serialization;

namespace Labb3ProgTemplate.DataModels.ProductCategories;

public class Nuts : Product
{
    [JsonConstructor]
    public Nuts(string name, double price, string imageUrl) : base(name, price, ProductEnums.Nuts, imageUrl)
    {
    }
}