using System.Threading.Tasks;
using System.Windows;
using Labb3ProgTemplate.Managerrs;

namespace Labb3ProgTemplate
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            InitializeContent();
        }

        public void InitializeContent()
        {
            //Utskrift och inläsning av användare och produkter

            Task.Run(() =>
            {
                ProductManager.SaveProductsToFile();
                UserManager.SaveUsersToFile();

                ProductManager.LoadProductsFromFile();
                UserManager.LoadUsersFromFile();
            });
        }
    }
}
