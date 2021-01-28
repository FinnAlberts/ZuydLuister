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
    public partial class EditAdministratorPage : ContentPage
    {
        private Administrator administrator;
        private bool isNew;
        
        public EditAdministratorPage()
        {
            InitializeComponent();
            isNew = true;
        }

        public EditAdministratorPage(Administrator selectedAdministrator)
        {
            InitializeComponent();
            administrator = selectedAdministrator;
            isNew = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Check if this is a new administrator
            if (!isNew) // Not new
            {
                // Fill entries with existing data
                emailEntry.Text = administrator.AdminEmail;
                masterAdminCheckBox.IsChecked = administrator.IsMasterAdmin;
                deleteAdminButton.IsVisible = true;
            } 
            else // New
            {
                // Hide delete button
                deleteAdminButton.IsVisible = false;
            }
        }

        private void saveAdminButton_Clicked(object sender, EventArgs e)
        {
            // Check if everything is filled in
            if (!String.IsNullOrEmpty(emailEntry.Text))
            {
                using (SQLiteConnection connection = new SQLiteConnection(App.GameDatabaseLocation))
                {
                    // Check if administrator is already registered
                    connection.CreateTable<Administrator>();
                    var administrators = connection.Table<Administrator>().ToList();

                    if (!isNew) 
                    {
                        administrators = (from admin in administrators where administrator.AdminId != admin.AdminId select admin).ToList();
                    }

                    administrators = (from admin in administrators where admin.AdminEmail.ToLower() == emailEntry.Text.ToLower() select admin).ToList();

                    if (administrators.Count == 0) // Administrator is not yet registered
                    {
                        // Add administrator to database
                        int rows;
                        if (isNew)
                        {
                            administrator = new Administrator { AdminEmail = emailEntry.Text.ToLower(), IsMasterAdmin = masterAdminCheckBox.IsChecked };
                            rows = connection.Insert(administrator);
                        }
                        else
                        {
                            administrator.AdminEmail = emailEntry.Text.ToLower();
                            administrator.IsMasterAdmin = masterAdminCheckBox.IsChecked;

                            rows = connection.Update(administrator);
                        }

                        // check for errors
                        if (rows > 0)
                        {
                            DisplayAlert("Succes", "De administrator is succesvol bijgewerkt", "Oke");
                            Navigation.PopAsync();
                        } else
                        {
                            DisplayAlert("Fout", "De administrator kon niet worden bijgewerkt.", "Oke");
                        }
                    } 
                    else
                    {
                        DisplayAlert("Fout", "Dit e-mailadres staat al ingesteld als beheerder.", "Oke");
                    }
                }
            }
            else
            {
                DisplayAlert("Fout", "Niet alle velden zijn ingevuld.", "Oke");
            }
        }

        private async void deleteAdminButton_Clicked(object sender, EventArgs e)
        {
            // Ask for confirmation
            var answer = await DisplayAlert("Verwijderen", "Weet je zeker dat je deze administrator wilt verwijderen?", "Nee", "Ja");
            if (!answer) // Deletion confirmed
            {
                using (SQLiteConnection connection = new SQLiteConnection(App.GameDatabaseLocation))
                {
                    // Delete the administrator
                    connection.CreateTable<Administrator>();
                    int rows = connection.Delete(administrator);

                    // Error reporting
                    if (rows > 0)
                    {
                        await DisplayAlert("Succes", "Je hebt deze administrator succesvol verwijderd.", "Oke");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Fout", "Er is iets mis gegaan met het verwijderen. Probeer het nog eens.", "Oke");
                    }
                }
            }
        }

        private void menuToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MenuPage());
        }
    }
}
