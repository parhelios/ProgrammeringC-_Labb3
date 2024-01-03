using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Labb3ProgTemplate.DataModels.ProductCategories;
using Labb3ProgTemplate.DataModels.Products;
using Labb3ProgTemplate.Enums;

namespace Labb3ProgTemplate.Managerrs;

public static class ProductManager
{
    private static readonly IEnumerable<Product>? _products = new List<Product>();
    public static IEnumerable<Product>? Products
    {
        get { return _products; }
        set => ProductListChanged.Invoke();
    }

    // Skicka detta efter att produktlistan ändrats eller lästs in
    public static event Action ProductListChanged;

    public static void AddBulkProducts(List<Product> products)
    {
        //Lägga till flera varor.

        if (Products is List<Product> prodList)
        {
            prodList.AddRange(products);
        }
    }

    public static void AddProduct(Product product)
    {
        //Lägga till enskilda varor.

        ((List<Product>)Products).Add(product);
        ProductListChanged.Invoke();
    }

    public static void RemoveProduct(Product product)
    {
        //Ta bort enskilda varor.

        ((List<Product>)Products).Remove(product);
        ProductListChanged.Invoke();
    }

    public static async Task SaveProductsToFile()
    {
        var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "parheliosLabb3");
        Directory.CreateDirectory(directory);
        var filepath = Path.Combine(directory, "products.json");

        DataSource dataSource = new();

        var jsonOptions = new JsonSerializerOptions();
        jsonOptions.WriteIndented = true;

        if (File.Exists(filepath))
        {
            if (UserManager.CurrentUser != null)
            {
                //Om en admin, manuellt, vill tömma products så möjliggörs detta här.
                if (UserManager.CurrentUser.Type is UserTypes.Admin)
                {
                    var json1 = JsonSerializer.Serialize(Products, jsonOptions);

                    using var sw1 = new StreamWriter(filepath);
                    sw1.WriteLine(json1);
                }
            }

            //För att kringgå ett problem där programmet skrev ut en tom json-fil.
            if (Products.Count() < 1)
            {
                return;
            }

            var json = JsonSerializer.Serialize(Products, jsonOptions);

            using var sw = new StreamWriter(filepath);
            sw.WriteLine(json);
        }

        if (!File.Exists(filepath))
        {
            var json = JsonSerializer.Serialize(dataSource.Products, jsonOptions);

            using var sw = new StreamWriter(filepath);
            sw.WriteLine(json);
        }
    }

    public static async Task LoadProductsFromFile()
    {
        var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "parheliosLabb3");
        var filepath = Path.Combine(directory, "products.json");

        if (File.Exists(filepath))
        {
            using var sr = new StreamReader(filepath);
            var text = sr.ReadToEnd();

            var deserializedProducts = new List<Product>();
            using (var jsonDoc = JsonDocument.Parse(text))
            {
                if (jsonDoc.RootElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var jsonElement in jsonDoc.RootElement.EnumerateArray())
                    {
                        Product a;
                        switch (jsonElement.GetProperty("ProductCategory").GetInt32())
                        {
                            case 0:
                                a = jsonElement.Deserialize<Vegetables>();
                                deserializedProducts.Add(a);
                                break;
                            case 1:
                                a = jsonElement.Deserialize<Meat>();
                                deserializedProducts.Add(a);
                                break;
                            case 2:
                                a = jsonElement.Deserialize<Fish>();
                                deserializedProducts.Add(a);
                                break;
                            case 3:
                                a = jsonElement.Deserialize<Nuts>();
                                deserializedProducts.Add(a);
                                break;
                        }
                    }
                    AddBulkProducts(deserializedProducts);
                }
            }
        }
    }
}