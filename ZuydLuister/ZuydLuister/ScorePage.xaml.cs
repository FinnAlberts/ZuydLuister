using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZuydLuister.Model;

namespace ZuydLuister
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScorePage : ContentPage
    {
        Savegame savegame;
        public ScorePage(Savegame savegame)
        {
            InitializeComponent();
            this.savegame = savegame;
            NavigationPage.SetHasBackButton(this, false);
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Initialize variables
            int maxScore = 0;
            int achievedScore = 0;
            int scorePercentage = 0;
            List<string> scoreList = new List<string>();
            var myScores = new List<Score>();

            // Get achieved and max scores and save in myScores
            using (SQLiteConnection connection = new SQLiteConnection(App.UserDatabaseLocation))
            {
                connection.CreateTable<Score>();
                var scores = connection.Table<Score>().ToList();
                myScores = (from score in scores where score.SavegameId == savegame.SavegameId select score).ToList();
            }

            // Create a nice text for each score
            using (SQLiteConnection connection = new SQLiteConnection(App.GameDatabaseLocation))
            {
                connection.CreateTable<ScoreCategory>();
                var scoreCategories = connection.Table<ScoreCategory>().ToList();

                // For each score in myScores, get the corresponding category name and create a string. Add this string to scoreList variable.
                foreach (Score score in myScores)
                {
                    var categoryName = (from category in scoreCategories where category.ScoreCategoryId == score.ScoreCategoryId select category.ScoreCategoryName).ToList();
                    if (categoryName.Count != 0)
                    {
                        achievedScore += score.AchievedScore;
                        maxScore += score.MaxScore;
                        scoreList.Add(categoryName[0] + ": " + score.AchievedScore + "/" + score.MaxScore);
                    }
                }
            }

            // Load scoreList into ListView
            scoreListView.ItemsSource = scoreList;

            // Show the total achieved and max score
            averageScoreLabel.Text = "Je totale score is: " + achievedScore + "/" + maxScore;

            // Calculate percentage and give advice
            if (maxScore > 0)
            {
                scorePercentage = achievedScore * 100 / maxScore;
                if (scorePercentage <= 25)
                {
                    contactLabel.Text = "Het lijkt erop dat het niet zo goed met je gaat. Wil je met iemand hierover praten? Klik dan hieronder.";
                }
                else if (scorePercentage <= 50)
                {
                    contactLabel.Text = "Het lijkt erop dat het minder goed met je gaat. Wil je met iemand hierover praten? Klik dan hieronder.";
                }
                else if (scorePercentage <= 75)
                {
                    contactLabel.Text = "Het lijkt erop dat het goed met je gaat. Wil je alsnog met iemand praten? Klik dan hieronder.";
                }
                else
                {
                    contactLabel.Text = "Het lijkt erop dat het uitstekend met je gaat. Wil je alsnog met iemand praten? Klik dan hieronder. Klik ook hieronder als je zelf geïntreseerd bent om luisterstudent te worden en zo anderen te helpen.";
                }
            }
            else
            {
                contactLabel.Text = "Het lijkt erop dat er geen score is. Wil je alsnog met iemand praten? Klik dan hieronder.";
            }
        }

        private void contactButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ContactPage());

        }

        private async void workshopsButton_Clicked(object sender, EventArgs e)
        {
            string link = "https://moodle.zuyd.nl/course/view.php?id=6992&sectionid=265207#section-16";
            await Browser.OpenAsync(link, BrowserLaunchMode.SystemPreferred);
        }

        private void backToMenuButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MenuPage());
        }

        private void menuToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MenuPage());
        }

        private void editToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EditSavegamePage(savegame));
        }

        private void scoreListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Check if item is not null
            if (scoreListView.SelectedItem != null)
            {
                // Get the scorecategory string
                var selectedCategory = scoreListView.SelectedItem as string;

                // Shorten the string to only the category name
                string selectedCategoryName = selectedCategory.Substring(0, selectedCategory.IndexOf(":"));
                
                // Show a popup with the description of the category
                using (SQLiteConnection connection = new SQLiteConnection(App.GameDatabaseLocation))
                {
                    connection.CreateTable<ScoreCategory>();
                    var scoreCategories = connection.Table<ScoreCategory>().ToList();
                    var description = (from scoreCategory in scoreCategories
                                       where scoreCategory.ScoreCategoryName == selectedCategoryName
                                       select scoreCategory.ScoreCategoryDescription).ToList()[0];
                    DisplayAlert("Beschrijving " + selectedCategoryName, description, "Oke");
                }
            }            
            scoreListView.SelectedItem = null;
        }
    }
}