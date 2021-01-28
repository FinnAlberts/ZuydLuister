using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZuydLuister.Model;

namespace ZuydLuister
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectSavegamePage : ContentPage
    {
        public static Savegame currentSavegame;

        public SelectSavegamePage()
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

            // Load savegames into ListView
            using (SQLiteConnection connection = new SQLiteConnection(App.UserDatabaseLocation))
            {
                connection.CreateTable<Savegame>();
                var savegames = connection.Table<Savegame>().ToList();
                savegameListView.ItemsSource = savegames;
            }
        }

        private void newGameButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewSavegamePage());
        }

        private async void savegameListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Check if an item is selected
            if (savegameListView.SelectedItem != null)
            {
                // Get selected savegame
                var selectedSavegame = savegameListView.SelectedItem as Savegame;
                currentSavegame = selectedSavegame;

                // Check for password
                if (String.IsNullOrEmpty(selectedSavegame.SavegamePassword)) // No password
                {
                    await Navigation.PushAsync(new ScenarioPage(currentSavegame));
                }
                else // A password has been set
                {
                    // Ask for password
                    string password = await DisplayPromptAsync("Wachtwoord", "Vul hier je wachtwoord in:", "Oke", "Annuleren");

                    if (!String.IsNullOrEmpty(password)) // Password has been entered
                    {
                        if (password == selectedSavegame.SavegamePassword) // Password is correct
                        {
                            await Navigation.PushAsync(new ScenarioPage(currentSavegame));
                        }
                        else // Password is incorrect
                        {
                            await DisplayAlert("Fout", "Je hebt een verkeerd wachtwoord ingevoerd. Probeer het nog eens.", "Oke");
                        }
                    }
                    else // No password has been entered
                    {
                        await DisplayAlert("Fout", "Vul je wachtwoord in.", "Oke");
                    }
                }
            }

            savegameListView.SelectedItem = null;
        }

        private void menuToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MenuPage());
        }
    }
}