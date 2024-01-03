using System.Collections.Generic;
using Labb3ProgTemplate.DataModels.ProductCategories;
using Labb3ProgTemplate.DataModels.Products;
using Labb3ProgTemplate.DataModels.Users;

namespace Labb3ProgTemplate;

public class DataSource
{
    public List<Product> Products { get; } = new();

    public DataSource()
    {
        //Standardutbudet av produkter.
        Products.AddRange(new List<Product>()
        {
            new Meat("Bacon", 32, "https://cdn-icons-png.flaticon.com/512/1857/1857921.png"),
            new Meat("Wagyu A5", 3450, "https://thumbs.dreamstime.com/b/japanese-wagyu-beef-watercolor-drawing-style-your-food-drink-artwork-japanese-wagyu-beef-watercolor-drawing-style-285598307.jpg"),
            new Vegetables("Broccoli", 38, "https://cdn-icons-png.flaticon.com/512/2909/2909779.png"),
            new Vegetables("Onion", 18, "https://cdn-icons-png.flaticon.com/512/862/862810.png"),
            new Vegetables("The God-Emperor of Man", 1337, "https://3.bp.blogspot.com/-xMMcKUi8NsI/VQDEXm-sToI/AAAAAAAAHGw/iJCU4vBSSUw/s1600/Warhammer_God_Emperor.jpg"),
            new Fish("Halibut", 500,"https://upload.wikimedia.org/wikipedia/commons/thumb/5/52/Hippoglossus_hippoglossus2.jpg/640px-Hippoglossus_hippoglossus2.jpg"),
            new Fish("Giant shrimp", 720, "https://static.wikia.nocookie.net/godzilla/images/3/32/Ebirah2.jpg"),
            new Nuts("Macadamia", 275, "https://cdn2.iconfinder.com/data/icons/nuts-seeds/64/179_nut-macadamia-food-snack-512.png"),
            new Nuts("Maracona almonds", 400, "https://cdn-icons-png.flaticon.com/512/12443/12443350.png"),

        });
    }
}