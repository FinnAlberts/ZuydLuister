﻿using System;
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
    public partial class NewSavegamePage : ContentPage
    {
         bool isChecked = false;

        public NewSavegamePage()
        {
            InitializeComponent();
        }

        private void startButton_Clicked(object sender, EventArgs e)
        {
            bool foundName = false;
            using (SQLiteConnection connection = new SQLiteConnection(App.UserDatabaseLocation))
            {
                connection.CreateTable<Savegame>();
                var savegames = connection.Table<Savegame>().ToList();
                foreach (Savegame savegame in savegames)
                {
                    if (savegameNameEntry.Text == savegame.SavegameName)
                    {
                        foundName = true;
                        DisplayAlert("Mislukt", "Deze naam wordt al gebruikt. Probeer een andere naam.", "Oke");
                        break;
                    }
                }

                if (!foundName && savegamePasswordEntry.Text == confirmSavegamePasswordEntry.Text && isChecked)
                {
                    Savegame savegame = new Savegame() { SavegameName = savegameNameEntry.Text, SavegamePassword = savegamePasswordEntry.Text};

                    int rows = connection.Insert(savegame);

                    if (rows > 0)
                    {
                        DisplayAlert("Gelukt", "Je hebt succesvol een savegame aangemaakt.", "Oke");
                        Navigation.PushAsync(new ScenarioPage());
                    }
                    else
                    {
                        DisplayAlert("Mislukt", "Er is iets mis gegaan. Probeer het nog eens", "Oke");
                    }
                }
                else if (!foundName && savegamePasswordEntry.Text != confirmSavegamePasswordEntry.Text && isChecked)
                {
                    DisplayAlert("Mislukt", "Je hebt twee verschillende wachtwoorden ingevuld. probeer het nog eens.", "Oke");
                }
                else if (!foundName && !isChecked)
                {
                    Savegame savegame = new Savegame() { SavegameName = savegameNameEntry.Text, SavegamePassword = null };

                    int rows = connection.Insert(savegame);

                    if (rows > 0)
                    {
                        DisplayAlert("Gelukt", "Je hebt succesvol een savegame aangemaakt.", "Oke");
                        Navigation.PushAsync(new ScenarioPage());
                    }
                    else
                    {
                        DisplayAlert("Mislukt", "Er is iets mis gegaan. Probeer het nog eens", "Oke");
                    }
                }
            }
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
