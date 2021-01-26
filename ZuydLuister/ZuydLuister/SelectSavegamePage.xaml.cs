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
            if (savegameListView.SelectedItem != null)
            {
                var selectedSavegame = savegameListView.SelectedItem as Savegame;
                currentSavegame = selectedSavegame;
                if (String.IsNullOrEmpty(selectedSavegame.SavegamePassword))
                {
                    await Navigation.PushAsync(new ScenarioPage(currentSavegame));
                }
                else
                {
                    string password = await DisplayPromptAsync("Wachtwoord", "Vul hier je wachtwoord in:", "Oke", "Annuleren");

                    if (!String.IsNullOrEmpty(password))
                    {
                        if (password == selectedSavegame.SavegamePassword)
                        {
                            await Navigation.PushAsync(new ScenarioPage(currentSavegame));
                        }
                        else
                        {
                            await DisplayAlert("Fout", "Je hebt een verkeerd wachtwoord ingevoerd. Probeer het nog eens.", "Oke");
                        }
                    }
                    else
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