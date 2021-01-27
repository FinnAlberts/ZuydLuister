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

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (LoginPage.permissions > 0)
            {
                adminButton.IsVisible = true;
            }
            else
            {
                adminButton.IsVisible = false;
            }
        }

        private void adminButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AdministratorPage());
        }

        async void logoutButton_Clicked(object sender, EventArgs e)
        {
            bool logout = await DisplayAlert("Uitloggen", "Weet je zeker dat je wilt uitloggen?", "Nee", "Ja");

            if (!logout)
            {
                await Navigation.PopToRootAsync();
            }
        }

        private void playButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SelectSavegamePage());
        }

        private void aboutButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AboutPage());
        }
    }
}