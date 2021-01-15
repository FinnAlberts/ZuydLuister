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
    public partial class EditAdministratorPage : ContentPage
    {
        public EditAdministratorPage()
        {
            InitializeComponent();
        }

        private void saveAdminButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void deleteAdminButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void menuToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MenuPage());
        }
    }
}
