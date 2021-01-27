using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZuydLuister.Model;

namespace ZuydLuister
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdministratorOverviewPage : ContentPage
    {
        public AdministratorOverviewPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLiteConnection connection = new SQLiteConnection(App.GameDatabaseLocation))
            {
                connection.CreateTable<Administrator>();
                var administrators = connection.Table<Administrator>().ToList();

                adminListView.ItemsSource = administrators;
            }
        }

        private void adminListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (adminListView.SelectedItem != null)
            {
                Administrator selectedAdmin = adminListView.SelectedItem as Administrator;

                if (selectedAdmin.AdminEmail == "luister@zuyd.nl")
                {
                    DisplayAlert("Fout", "Luister@zuyd.nl kan niet worden bewerkt.", "Oke");
                    adminListView.SelectedItem = null;
                }
                else
                {
                    Navigation.PushAsync(new EditAdministratorPage(selectedAdmin));
                }
            }
        }

        private void newAdminButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EditAdministratorPage());
        }
    }
}