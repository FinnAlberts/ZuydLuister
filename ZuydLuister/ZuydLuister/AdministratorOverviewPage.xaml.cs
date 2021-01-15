using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ZuydLuister
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdministratorOverviewPage : ContentPage
    {
        public AdministratorOverviewPage()
        {
            InitializeComponent();
        }

        private void adminListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private void newAdminButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EditAdministratorPage());
        }
    }
}