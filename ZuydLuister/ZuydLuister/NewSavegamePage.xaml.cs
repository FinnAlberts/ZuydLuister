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
    public partial class NewSavegamePage : ContentPage
    {
        public NewSavegamePage()
        {
            InitializeComponent();
        }

        private void startButton_Clicked(object sender, EventArgs e)
        {
            // Find StartScenario
            int startScenarioId;
            using (SQLiteConnection connection = new SQLiteConnection(App.GameDatabaseLocation))
            {
                connection.CreateTable<Scenario>();
                var scenarios = connection.Table<Scenario>();
                startScenarioId = (from scenario in scenarios where scenario.IsStartScenario == true select scenario.ScenarioId).ToList()[0];
            }
            
            // Check if name is unique
            bool foundName = false;
            using (SQLiteConnection connection = new SQLiteConnection(App.UserDatabaseLocation))
            {
                connection.CreateTable<Savegame>();
                var savegames = connection.Table<Savegame>().ToList();
                foreach (Savegame savegame in savegames)
                {
                    if (savegameNameEntry.Text == savegame.SavegameName) // Name is not unique
                    {
                        foundName = true;
                        DisplayAlert("Mislukt", "Deze naam wordt al gebruikt. Probeer een andere naam.", "Oke");
                        break;
                    }
                }

                // Name is unique
                if (!foundName /*&& savegamePasswordEntry.Text == confirmSavegamePasswordEntry.Text && passwordCheckBox.IsChecked*/)
                {
                    Savegame savegame = new Savegame() { SavegameName = savegameNameEntry.Text, ScenarioId = startScenarioId };
                    bool passwordsMatch = true;
                    if (passwordCheckBox.IsChecked) // A password will be set
                    {
                        if (savegamePasswordEntry.Text == confirmSavegamePasswordEntry.Text) // Passwords match
                        {
                            savegame.SavegamePassword = savegamePasswordEntry.Text;
                        } 
                        else
                        {
                            passwordsMatch = false;
                        }
                    }

                    if (passwordsMatch) // Check if passwords match
                    {
                        int rows = connection.Insert(savegame);

                        if (rows > 0)
                        {
                            DisplayAlert("Succes", "Je hebt succesvol een savegame aangemaakt.", "Oke");
                            Navigation.PushAsync(new ScenarioPage());
                        }
                        else
                        {
                            DisplayAlert("Fout", "Er is iets mis gegaan. Probeer het nog eens.", "Oke");
                        }
                    } 
                    else
                    {
                        DisplayAlert("Fout", "De ingevulde wachtwoorden komen niet overeen.", "Oke");
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
