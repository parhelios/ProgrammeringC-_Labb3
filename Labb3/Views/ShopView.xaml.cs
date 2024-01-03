using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Labb3ProgTemplate.DataModels.Products;
using Labb3ProgTemplate.Enums;
using Labb3ProgTemplate.Managerrs;

namespace Labb3ProgTemplate.Views
{
    /// <summary>
    /// Interaction logic for ShopView.xaml
    /// </summary>
    public partial class ShopView : UserControl
    {
        public ObservableCollection<Product> ProductsList { get; set; } = new();

        public ObservableCollection<Product> SortedProductsList { get; set; } = new();

        public ObservableCollection<Product> CustomerCartList { get; set; } = new();

        private static event Action CustomerCartListChanged;

        public Product SelectedProduct { get; set; }

        public ShopView()
        {
            InitializeComponent();
            UserManager.CurrentUserChanged += UserManager_CurrentUserChanged;
            ProductManager.ProductListChanged += ProductManager_ProductListChanged;
            CustomerCartListChanged += OnCustomerCartListChanged;

            DataContext = this;

            var types = Enum.GetNames<ProductEnums>();
            foreach (var type in types)
            {
                SortProductCategory.Items.Add(type);
            }
        }

        private void OnCustomerCartListChanged()
        {
            //Metod för att uppdatera användarens kundvagn när den förändras.

            ICollectionView view = CollectionViewSource.GetDefaultView(CustomerCartList);
            view.Refresh();
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

        private void ProductManager_ProductListChanged()
        {
            PopulateProductsList();
        }

        private void UserManager_CurrentUserChanged()
        {
            PopulateProductsList();
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            //Tar bort från användarens kundvagn.
            //Glorious return of the fulkod from labb 2.

            if (CustomerCartList.Count < 1)
            {
                MessageBox.Show("There are no items to remove.");
            }

            if (CartList.SelectedItem is Product selectedItem)
            {
                if (selectedItem.Amount == 1)
                {
                    CustomerCartList.Remove(selectedItem);
                }

                selectedItem.Amount--;

                if (selectedItem.Amount <= 0)
                {
                    selectedItem.Amount = 0;
                }

                CustomerCartListChanged.Invoke();
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            //Lägger till i användarens kundvagn.
            //Glorious return of the fulkod from labb 2, part II.

            if (ProdList.SelectedItem is Product selectedItem)
            {
                if (selectedItem.Amount < 1)
                {
                    CustomerCartList.Add(selectedItem);
                }

                selectedItem.Amount++;
                CustomerCartListChanged.Invoke();
            }
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            //Tömmer användaren kundvagn (förlåt Niklas) och skickar vidare till utloggning.
            //Detta är gammel-internet, här sparas ingenting mellan besöken! 

            CustomerCartList.Clear();
            UserManager.LogOut();
        }

        private void CheckoutBtn_Click(object sender, RoutedEventArgs e)
        {
            //Utcheckning av de produkter som användaren har i sin kundkorg.
            //Totalsumman räknas ut och därefter blir användaren utloggad.

            if (CustomerCartList.Count < 1)
            {
                MessageBox.Show("Your cart does not contain any items.");
                return;
            }

            double totalSum = 0;

            foreach (var product in CustomerCartList)
            {
                totalSum += product.Amount * product.Price;
            }

            MessageBox.Show($"Total sum: {totalSum} SEK \n\nYour payment couldn't be processed because you failed to provide the necessary amount of goats.");
            CustomerCartList.Clear();
            UserManager.LogOut();
        }

        private void ProdList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProdList.SelectedItem is Product selectedItem)
            {
                SelectedProduct = selectedItem;

                //För att förebygga kraschar om ingen bild har lagts till vid skapandet av produkt.
                if (selectedItem.ImageUrl == "")
                {
                    selectedItem.ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/c/c4/PM5544_with_non-PAL_signals.png";
                }

                var bitmapImage = new BitmapImage(new Uri(selectedItem.ImageUrl));
                productImage.Source = bitmapImage;
            }
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
