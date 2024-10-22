﻿using System;
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
    public partial class EditCategoryPage : ContentPage
    {
        private ScoreCategory selectedCategory;
        private bool isNew;

        public EditCategoryPage()
        {
            InitializeComponent();
            isNew = true;
            deleteButton.IsVisible = false;
        }

        public EditCategoryPage(ScoreCategory categoryInput)
        {
            InitializeComponent();
            isNew = false;
            selectedCategory = categoryInput;

            // Load existing data
            nameEntry.Text = selectedCategory.ScoreCategoryName;
            descriptionEditor.Text = selectedCategory.ScoreCategoryDescription;
        }

        private void saveButton_Clicked(object sender, EventArgs e)
        {
            // Check if everything is filled in
            if (String.IsNullOrEmpty(nameEntry.Text) || String.IsNullOrEmpty(descriptionEditor.Text))
            {
                DisplayAlert("Fout", "Niet alle velden zijn ingevuld.", "Oke");
            } else
            {
                using (SQLiteConnection connection = new SQLiteConnection(App.GameDatabaseLocation))
                {
                    connection.CreateTable<ScoreCategory>();
                    var categories = connection.Table<ScoreCategory>().ToList();
                    
                    // Check if category already exists
                    bool found = false;
                    foreach (var category in categories)
                    {
                        if (category.ScoreCategoryName == nameEntry.Text)
                        {
                            if (isNew)
                            {
                                found = true;
                            } 
                            else if (selectedCategory.ScoreCategoryName != nameEntry.Text) 
                            {
                                found = true;
                            }
                        }
                    }                       

                    if (!found && isNew) // Register a new category
                    {
                        ScoreCategory category = new ScoreCategory() { ScoreCategoryName = nameEntry.Text, ScoreCategoryDescription = descriptionEditor.Text};
                        int rows = connection.Insert(category);

                        if (rows > 0)
                        {
                            DisplayAlert("Succes", "Je hebt succesvol een scorecategorie aangemaakt.", "Oke");
                            Navigation.PopAsync();
                        }
                        else
                        {
                            DisplayAlert("Fout", "Er is iets mis gegaan. Probeer het nog eens.", "Oke");
                        }
                    }
                    else if (!found && !isNew) // Update a category
                    {
                        selectedCategory.ScoreCategoryName = nameEntry.Text;
                        selectedCategory.ScoreCategoryDescription = descriptionEditor.Text;
                        int rows = connection.Update(selectedCategory);

                        if (rows > 0)
                        {
                            DisplayAlert("Succes", "Je hebt de categorie succesvol bijgewerkt.", "Oke");
                            Navigation.PopAsync();
                        }
                    }
                    else if (found) // Category already exists
                    {
                        DisplayAlert("Fout", "De ingevulde categorie bestaat al. Probeer een andere naam.", "Oke");
                    }
                }
            }
        }

        private async void deleteButton_Clicked(object sender, EventArgs e)
        {
            // Ask for confirmation
            var answer = await DisplayAlert("Verwijderen", "Weet je zeker dat je deze categorie wilt verwijderen?", "Nee", "Ja");
            if (!answer) // Deletion confirmed
            {
                using (SQLiteConnection connection = new SQLiteConnection(App.GameDatabaseLocation))
                {
                    // Delete category
                    connection.CreateTable<ScoreCategory>();
                    int rows = connection.Delete(selectedCategory);

                    // Error reporting
                    if (rows > 0)
                    {
                        await DisplayAlert("Succes", "Je hebt deze categorie succesvol verwijderd.", "Oke");
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
