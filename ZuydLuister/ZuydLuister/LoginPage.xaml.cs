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
                    var all_administrators = connection.Table<Administrator>().ToList();
                    var zuydLuisterAdmins = (from administrator in all_administrators where administrator.AdminEmail == "luister@zuyd.nl" select administrator).ToList();

                    if (zuydLuisterAdmins.Count == 0)
                    {
                        Administrator zuydLuisterAdministrator = new Administrator { AdminEmail = "luister@zuyd.nl", IsMasterAdmin = true };
                        int rows = connection.Insert(zuydLuisterAdministrator);
                        if (rows == 0)
                        {
                            DisplayAlert("Fout", "Luister@zuyd.nl kon niet worden geregistreerd als beheerder.", "Oke");
                        }
                    }

                    var administrators = (from administrator in all_administrators where usernameEntry.Text.ToLower() == administrator.AdminEmail select administrator).ToList();

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
                }
                
                Navigation.PushAsync(new MenuPage());
            }
        }
    }
}
