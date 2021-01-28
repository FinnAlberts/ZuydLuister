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
            // Read the entries
            var UsernameEmpty = string.IsNullOrEmpty(usernameEntry.Text);
            var PasswordEmpty = string.IsNullOrEmpty(passwordEntry.Text);

            // Check if everything is filled in
            if (UsernameEmpty || PasswordEmpty)
            {
                DisplayAlert("Fout", "Beide velden moeten worden ingevuld.", "Oke");
            }
            else
            {
                using (SQLiteConnection connection = new SQLiteConnection(App.GameDatabaseLocation))
                {
                    // Check if luister@zuyd.nl is registered
                    connection.CreateTable<Administrator>();
                    var all_administrators = connection.Table<Administrator>().ToList();
                    var zuydLuisterAdmins = (from administrator in all_administrators where administrator.AdminEmail == "luister@zuyd.nl" select administrator).ToList();

                    if (zuydLuisterAdmins.Count == 0) // Luister@zuyd.nl is not yet registered, register now
                    {
                        Administrator zuydLuisterAdministrator = new Administrator { AdminEmail = "luister@zuyd.nl", IsMasterAdmin = true };
                        int rows = connection.Insert(zuydLuisterAdministrator);
                        all_administrators = connection.Table<Administrator>().ToList();

                        // Error reporting
                        if (rows == 0)
                        {
                            DisplayAlert("Fout", "Luister@zuyd.nl kon niet worden geregistreerd als beheerder.", "Oke");
                        }
                    }

                    // Check if user logging in is an administrator
                    var administrators = (from administrator in all_administrators where usernameEntry.Text.ToLower() == administrator.AdminEmail select administrator).ToList();

                    if (administrators.Count > 0) // User is an admin
                    {
                        if (administrators[0].IsMasterAdmin) // User is masteradmin
                        {
                            permissions = 2;
                        }
                        else
                        {
                            permissions = 1;
                        }
                    } 
                    else // User is not an admin
                    {
                        permissions = 0;
                    }
                }
                
                Navigation.PushAsync(new MenuPage());
            }
        }
    }
}
