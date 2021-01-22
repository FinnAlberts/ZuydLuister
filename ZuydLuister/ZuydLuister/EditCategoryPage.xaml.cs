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
            nameEntry.Text = selectedCategory.ScoreCategoryName;
        }

        private void saveButton_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(nameEntry.Text))
            {
                DisplayAlert("Fout", "Niet alle velden zijn ingevuld.", "Oke");
            } else
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
                            DisplayAlert("Succes", "Je hebt succesvol een scorecategorie aangemaakt.", "Oke");
                            Navigation.PopAsync();
                        }
                        else
                        {
                            DisplayAlert("Fout", "Er is iets mis gegaan. Probeer het nog eens.", "Oke");
                        }
                    }
                    else if (!found && !isNew)
                    {
                        selectedCategory.ScoreCategoryName = nameEntry.Text;
                        int rows = connection.Update(selectedCategory);

                        if (rows > 0)
                        {
                            DisplayAlert("Succes", "Je hebt de categorie succesvol bijgewerkt.", "Oke");
                            Navigation.PopAsync();
                        }
                    }
                    else if (found)
                    {
                        DisplayAlert("Fout", "De ingevulde categorie bestaat al. Probeer een andere naam.", "Oke");
                    }
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
                        DisplayAlert("Succes", "Je hebt deze categorie succesvol verwijderd.", "Oke");
                        Navigation.PopAsync();
                    }
                    else
                    {
                        DisplayAlert("Fout", "Er is iets mis gegaan met het verwijderen. Probeer het nog eens.", "Oke");
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
