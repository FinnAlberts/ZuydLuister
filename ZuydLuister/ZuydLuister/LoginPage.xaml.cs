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
                using (SQLiteConnection connection = new SQLiteConnection(App.GameDatabaseLocation))
                {
                    connection.CreateTable<Administrator>();
                    var administrators = connection.Table<Administrator>().ToList();
                    administrators = (from administrator in administrators where usernameEntry.Text == administrator.AdminEmail select administrator).ToList();

                    if (administrators.Count > 0)
                    {
                        if (administrators[0].IsMasterAdmin)
                        {
                            permissions = 2;
                        }
                        else
                        {
                            permissions = 1;
                        }
                    } 
                    else
                    {
                        permissions = 0;
                    }

                    if (usernameEntry.Text.ToLower() == "luister@zuyd.nl")
                    {
                        permissions = 2;
                    }
                }
                
                Navigation.PushAsync(new MenuPage());
            }
        }
    }
}
