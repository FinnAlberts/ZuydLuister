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
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
        }

        private void AdminButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AdministratorPage());
        }

        async void LogoutButton_Clicked(object sender, EventArgs e)
        {
            bool logout = await DisplayAlert("Uitloggen", "Weet je zeker dat je wilt uitloggen?", "Nee", "Ja");

            if (!logout)
            {
                Navigation.PopToRootAsync();
            }
        }

        private void PlayButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SelectSavegamePage());
        }

        private void AboutButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AboutPage());
        }
    }
}