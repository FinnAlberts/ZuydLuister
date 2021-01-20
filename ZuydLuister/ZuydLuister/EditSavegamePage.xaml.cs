using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ZuydLuister
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditSavegamePage : ContentPage
    {
        private Savegame selectedSavegame;
        bool isChecked;

        public EditSavegamePage()
        {
            InitializeComponent();
        }

        public EditSavegamePage(Savegame savegameInput)
        {
            InitializeComponent();

            selectedSavegame = savegameInput;
            savegameNameEntry.Text = savegameInput.SavegameName;
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

                if (!foundName && savegamePasswordEntry.Text == confirmSavegamePasswordEntry.Text && isChecked)
                {
                    //Savegame savegame = new Savegame() { SavegameName = savegameNameEntry.Text, SavegamePassword = savegamePasswordEntry.Text };

                    connection.CreateTable<Savegame>();
                    int rows = connection.Update(selectedSavegame);

                    if (rows > 0)
                    {
                        DisplayAlert("Gelukt", "Je hebt succesvol een savegame gewijzigd.", "Oke");
                        Navigation.PushAsync(new ScenarioPage());
                    }
                    else
                    {
                        DisplayAlert("Fout", "Er is iets mis gegaan. Probeer het nog eens", "Oke");
                    }
                }
                else if (!foundName && savegamePasswordEntry.Text != confirmSavegamePasswordEntry.Text && isChecked)
                {
                    DisplayAlert("Fout", "Je hebt twee verschillende wachtwoorden ingevuld. probeer het nog eens.", "Oke");
                }
                else if (!foundName && !isChecked)
                {
                    selectedSavegame.SavegameName = savegameNameEntry.Text;
                    
                    if (isChecked)
                    {
                        selectedSavegame.SavegamePassword = savegamePasswordEntry.Text;
                    }
                    else
                    {
                        selectedSavegame.SavegamePassword = null;
                    }

                    int rows = connection.Update(selectedSavegame);

                    if (rows > 0)
                    {
                        DisplayAlert("Gelukt", "Je hebt succesvol een savegame gewijzigd.", "Oke");
                        Navigation.PopAsync();
                    }
                    else
                    {
                        DisplayAlert("Fout", "Er is iets mis gegaan. Probeer het nog eens", "Oke");
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

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            isChecked = e.Value;
            savegamePasswordEntry.IsVisible = e.Value;
            confirmSavegamePasswordEntry.IsVisible = e.Value;
        }
    }
}
