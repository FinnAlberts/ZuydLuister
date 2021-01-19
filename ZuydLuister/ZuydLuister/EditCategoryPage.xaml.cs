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
    public partial class EditCategoryPage : ContentPage
    {
        static ScoreCategory selectedCategory;
        static bool isNew;

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
            nameEntry.Text = selectedCategory.ScoreCategoryName;
        }

        private void saveButton_Clicked(object sender, EventArgs e)
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.GameDatabaseLocation))
            {
                connection.CreateTable<ScoreCategory>();
                var categories = connection.Table<ScoreCategory>().ToList();
                bool found = false;

                foreach (var category in categories)
                {
                    if (category.ScoreCategoryName == nameEntry.Text)
                    {
                        found = true;
                    }
                }

                if (!found && isNew)
                {
                    ScoreCategory category = new ScoreCategory() { ScoreCategoryName = nameEntry.Text };
                    int rows = connection.Insert(category);

                    if (rows > 0)
                    {
                        DisplayAlert("Gelukt", "Je hebt succesvol een scorecategorie aangemaakt.", "OK");
                        Navigation.PopAsync();
                    }
                    else
                    {
                        DisplayAlert("Mislukt", "Er is iets mis gegaan. Probeer het nog eens.", "OK");
                    }
                }
                else if (!found && !isNew)
                {
                    selectedCategory.ScoreCategoryName = nameEntry.Text;
                    int rows = connection.Update(selectedCategory);

                    if (rows > 0)
                    {
                        DisplayAlert("Gelukt", "Je hebt de categorie succesvol bijgewerkt.", "OK");
                        Navigation.PopAsync();
                    }
                }
                else if (found)
                {
                    DisplayAlert("Mislukt", "De ingevulde categorie bestaat al. Probeer een andere naam.", "OK");
                }
            }
        }

        private async void deleteButton_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Verwijderen", "Weet je zeker dat je deze categorie wilt verwijderen?", "Nee", "Ja");
            if (!answer)
            {
                using (SQLiteConnection connection = new SQLiteConnection(App.GameDatabaseLocation))
                {
                    connection.CreateTable<ScoreCategory>();
                    int rows = connection.Delete(selectedCategory);

                    if (rows > 0)
                    {
                        DisplayAlert("Gelukt", "Je hebt deze categorie succesvol verwijderd.", "OK");
                        Navigation.PopAsync();
                    }
                    else
                    {
                        DisplayAlert("Mislukt", "Er is iets mis gegaan met het verwijderen. Probeer het nog eens.", "OK");
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
