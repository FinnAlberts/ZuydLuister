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
    public partial class EditSavegamePage : ContentPage
    {
        private Savegame selectedSavegame;

        public EditSavegamePage(Savegame savegameInput)
        {
            InitializeComponent();

            selectedSavegame = savegameInput;
            savegameNameEntry.Text = savegameInput.SavegameName;
            if (!String.IsNullOrEmpty(savegameInput.SavegamePassword))
            {
                passwordCheckBox.IsChecked = true;
                savegamePasswordEntry.Text = savegameInput.SavegamePassword;
                confirmSavegamePasswordEntry.Text = savegameInput.SavegamePassword;
            }
        }

        private void saveEditSavegameButton_Clicked(object sender, EventArgs e)
        {
            bool foundName = false;
            using (SQLiteConnection connection = new SQLiteConnection(App.UserDatabaseLocation))
            {
                connection.CreateTable<Savegame>();
                var savegames = connection.Table<Savegame>().ToList();
                foreach (Savegame savegame in savegames)
                {
                    if (savegameNameEntry.Text == savegame.SavegameName && selectedSavegame.SavegameName != savegameNameEntry.Text)
                    {
                        foundName = true;
                        DisplayAlert("Mislukt", "Deze naam wordt al gebruikt. Probeer een andere naam.", "Oke");
                        break;
                    }
                }

                if (!foundName)
                {
                    connection.CreateTable<Savegame>();

                    selectedSavegame.SavegameName = savegameNameEntry.Text;

                    int rows = 0;
                    if (passwordCheckBox.IsChecked)
                    {
                        if (savegamePasswordEntry.Text == confirmSavegamePasswordEntry.Text)
                        {
                            selectedSavegame.SavegamePassword = savegamePasswordEntry.Text;
                            rows = connection.Update(selectedSavegame);
                        }
                        else
                        {
                            DisplayAlert("Mislukt", "De ingevulde wachtwoorden komen niet overeen.", "Oke");
                        }
                    }
                    else
                    {
                        selectedSavegame.SavegamePassword = null;
                        rows = connection.Update(selectedSavegame);
                    }

                    if (rows > 0)
                    {
                        DisplayAlert("Gelukt", "Je hebt succesvol een savegame gewijzigd.", "Oke");
                        Navigation.PushAsync(new ScenarioPage());
                    }
                    else
                    {
                        DisplayAlert("Fout", "Er is iets misgegaan. Probeer het nog eens", "Oke");
                    }
                }
            }
        }

        private void deleteSavegameButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SelectSavegamePage());
        }

        private void menuToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MenuPage());
        }
    }
}
