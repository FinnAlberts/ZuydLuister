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
        public static int permissions;
        
        public LoginPage()
        {
            InitializeComponent();
        }

        private void loginButton_Clicked(object sender, EventArgs e)
        {
            var UsernameEmpty = string.IsNullOrEmpty(usernameEntry.Text);
            var PasswordEmpty = string.IsNullOrEmpty(passwordEntry.Text);

            if (UsernameEmpty || PasswordEmpty)
            {
                DisplayAlert("Fout", "Beide velden moeten worden ingevuld.", "Oke");
            }
            else
            {
                Navigation.PushAsync(new MenuPage());
            }
        }
    }
}
