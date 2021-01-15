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
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginButton_Clicked(object sender, EventArgs e)
        {
            var UsernameEmpty = string.IsNullOrEmpty(UsernameEntry.Text);
            var PasswordEmpty = string.IsNullOrEmpty(PasswordEntry.Text);

            if (UsernameEmpty || PasswordEmpty)
            {
                DisplayAlert("Login Error", "Beide velden moeten worden ingevuld.", "OK");
            }
            else
            {
                Navigation.PushAsync(new MenuPage());
            }
        }
    }
}