using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Labb3ProgTemplate.DataModels.ProductCategories;
using Labb3ProgTemplate.DataModels.Products;
using Labb3ProgTemplate.Enums;
using Labb3ProgTemplate.Managerrs;

namespace Labb3ProgTemplate.Views
{
    /// <summary>
    /// Interaction logic for AdminView.xaml
    /// </summary>
    public partial class AdminView : UserControl
    {
        public ObservableCollection<Product> ProductsList { get; set; } = new();

        public ObservableCollection<Product> SortedProductsList { get; set; } = new();

        public Product SelectedProduct { get; set; }

        public AdminView()
        {
            InitializeComponent();
            UserManager.CurrentUserChanged += UserManager_CurrentUserChanged;
            ProductManager.ProductListChanged += ProductManagerOnProductListChanged;

            DataContext = this;

            var types = Enum.GetNames<ProductEnums>();
            foreach (var type in types)
            {
                ProductCategory.Items.Add(type);
                SortProductCategory.Items.Add(type);
            }
        }
        private void ProductManagerOnProductListChanged()
        {
            //Var gång ProductList uppdateras så sparas de aktuella produkterna till fil.

            PopulateProductsList();
            ProductManager.SaveProductsToFile();
        }

        private void PopulateProductsList()
        {
            //Används när produktlistan uppdateras.

            ProductsList.Clear();

            if (ProductManager.Products is null)
            {
                return;
            }

            foreach (var prod in ProductManager.Products)
            {
                ProductsList.Add(prod);
            }
        }

        private void UserManager_CurrentUserChanged()
        {
            PopulateProductsList();
        }

        private void ProdList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProdList.SelectedItem is Product selectedItem)
            {
                SelectedProduct = selectedItem;

                ProductName.Text = SelectedProduct.Name;
                ProductPrice.Text = SelectedProduct.Price.ToString();
                ProductCategory.Text = SelectedProduct.ProductCategory.ToString();
                ProductUrl.Text = SelectedProduct.ImageUrl;
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ProductName is null || ProductPrice is null || ProductCategory.SelectedValue is null)
            {
                return;
            }

            if (ProdList.SelectedItem is Product selectedItem)
            {
                //Läge för att editera existerande produkter.

                if (ProductManager.Products.Any(u => u.Name.ToLower() == selectedItem.Name.ToLower()))
                {
                    var editProduct = ProductManager.Products.FirstOrDefault(d => d.Name.ToLower() == selectedItem.Name.ToLower());

                    if (editProduct is null)
                    {
                        return;
                    }

                    editProduct.Name = ProductName.Text;
                    var editPriceCheck = Double.TryParse(ProductPrice.Text, out double tempPrice);

                    if (!editPriceCheck)
                    {
                        MessageBox.Show("Incorrect input: The prices may only contain numbers.");
                        return;
                    }

                    if (tempPrice < 1)
                    {
                        MessageBox.Show("That price is too low.");
                        return;
                    }

                    if (tempPrice > 1000000000)
                    {
                        MessageBox.Show("That's not a reasonable price.");
                        return;
                    }

                    editProduct.Price = tempPrice;
                    editProduct.ProductCategory = Enum.Parse<ProductEnums>((string)ProductCategory.SelectedValue);
                    editProduct.ImageUrl = ProductUrl.Text;

                    ProductManagerOnProductListChanged();
                    return;
                }
            }

            //Läge för att lägga till nya produkter.

            var prodName = ProductName.Text;
            var prodUrl = ProductUrl.Text;

            var existingProduct = ProductManager.Products.FirstOrDefault(d => d.Name.ToLower() == prodName.ToLower());

            if (existingProduct != null)
            {
                MessageBox.Show("A product with that name already exists.");
                return;
            }

            var prodPriceCheck = Double.TryParse(ProductPrice.Text, out double prodPrice);

            if (!prodPriceCheck)
            {
                MessageBox.Show("Incorrect input: The prices may only contain numbers.");
                return;
            }

            if (prodPrice < 1)
            {
                MessageBox.Show("That price is too low.");
                return;
            }

            if (prodPrice > 1000000000)
            {
                MessageBox.Show("That's not a reasonable price.");
                return;
            }

            var type1 = Enum.Parse<ProductEnums>((string)ProductCategory.SelectedValue);

            switch (type1)
            {
                case ProductEnums.Vegetables:
                    var tempVegetables = new Vegetables(prodName, prodPrice, prodUrl);
                    ProductManager.AddProduct(tempVegetables);
                    break;
                case ProductEnums.Meat:
                    var tempMeat = new Meat(prodName, prodPrice, prodUrl);
                    ProductManager.AddProduct(tempMeat);
                    break;
                case ProductEnums.Fish:
                    var tempFish = new Fish(prodName, prodPrice, prodUrl);
                    ProductManager.AddProduct(tempFish);
                    break;
                case ProductEnums.Nuts:
                    var tempNuts = new Nuts(prodName, prodPrice, prodUrl);
                    ProductManager.AddProduct(tempNuts);
                    break;
            }

            ProductName.Text = string.Empty;
            ProductPrice.Text = string.Empty;
            ProductCategory.Text = string.Empty;
            ProductUrl.Text = string.Empty;
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            //Tar bort produkt från _products

            if (ProdList.SelectedItem is Product selectedItem)
            {
                var selectedProduct = ProductManager.Products.FirstOrDefault(p => p.Name == SelectedProduct.Name);

                if (selectedProduct is null)
                {
                    return;
                }

                ProductManager.RemoveProduct(selectedProduct);

                PopulateProductsList();

                ProductName.Text = string.Empty;
                ProductPrice.Text = string.Empty;
                ProductCategory.Text = string.Empty;
            }
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            UserManager.LogOut();
        }

        private void SortProductCategory_OnDropDownClosed(object? sender, EventArgs e)
        {
            //Metod för att sortera produkterna i butiken baserat på produkttyp.

            if (SortProductCategory.SelectedValue is null)
            {
                return;
            }

            var type1 = Enum.Parse<ProductEnums>((string)SortProductCategory.SelectedValue);

            foreach (var product in ProductManager.Products)
            {
                if (product.ProductCategory == type1)
                {
                    SortedProductsList.Add(product);
                }
            }

            ProductsList.Clear();

            foreach (var product in SortedProductsList)
            {
                ProductsList.Add(product);
            }

            SortedProductsList.Clear();
        }

        private void ResetSortingBtn_OnClick(object sender, RoutedEventArgs e)
        {
            //Återställ sortering av produktkategorier.

            PopulateProductsList();
        }
    }
}
