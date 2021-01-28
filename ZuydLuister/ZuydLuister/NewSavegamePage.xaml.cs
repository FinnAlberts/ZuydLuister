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
            int startScenarioId = 0;
            using (SQLiteConnection connection = new SQLiteConnection(App.GameDatabaseLocation))
            {
                connection.CreateTable<Scenario>();
                var scenarios = connection.Table<Scenario>();
                var startScenarios = (from scenario in scenarios where scenario.IsStartScenario == true select scenario.ScenarioId).ToList();
                if (startScenarios.Count == 0) // No StartScenario has been set
                {
                    DisplayAlert("Fout", "Er kon geen nieuw spel worden gestart, omdat er door Zuyd L U I S T E R geen startscenario is ingesteld. Probeer het op een later moment opnieuw", "Oke");
                } else
                {
                    startScenarioId = startScenarios[0];
                }
            }
            
            if (startScenarioId != 0) // A StartScenario exists, continue
            {
                // Check if name is unique and is not null or empty
                bool foundName = false;
                var savegameNameEmpty = String.IsNullOrEmpty(savegameNameEntry.Text);
                using (SQLiteConnection connection = new SQLiteConnection(App.UserDatabaseLocation))
                {
                    // Check if savegame already exists
                    connection.CreateTable<Savegame>();
                    var savegames = connection.Table<Savegame>().ToList();
                    foreach (Savegame savegame in savegames)
                    {
                        if (savegameNameEntry.Text == savegame.SavegameName) // Name is not unique
                        {
                            foundName = true;
                            DisplayAlert("Fout", "Deze naam wordt al gebruikt. Probeer een andere naam.", "Oke");
                            break;
                        }
                    }

                    // Name is unique and entry not null or empty
                    if (!foundName && !savegameNameEmpty)
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
                            // Create savegame
                            int rows = connection.Insert(savegame);

                            // Error reporting
                            if (rows > 0)
                            {
                                DisplayAlert("Succes", "Je hebt succesvol een savegame aangemaakt.", "Oke");
                                Navigation.PushAsync(new ScenarioPage(savegame));
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
        }

        private void menuToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MenuPage());
        }
    }
}
