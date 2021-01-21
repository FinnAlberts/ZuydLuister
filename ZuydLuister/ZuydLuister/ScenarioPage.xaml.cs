using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZuydLuister.Model;

namespace ZuydLuister
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScenarioPage : ContentPage
    {
        Savegame savegame;
        Scenario currentScenario;
        int maxScore = 0;

        public ScenarioPage(Savegame savegame)
        {
            InitializeComponent();
            this.savegame = savegame;
            NavigationPage.SetHasBackButton(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (SQLiteConnection connection = new SQLiteConnection(App.GameDatabaseLocation))
            {
                connection.CreateTable<Answer>();
                connection.CreateTable<Scenario>();
                var scenarios = connection.Table<Scenario>().ToList();
                currentScenario = (from scenario in scenarios where scenario.ScenarioId == savegame.ScenarioId select scenario).ToList()[0];
                scenarioLabel.Text = currentScenario.ScenarioContent;
                var answers = connection.Table<Answer>().ToList();
                var currentAnswers = (from answer in answers where answer.ScenarioId == currentScenario.ScenarioId select answer).ToList();
                foreach (Answer answer in currentAnswers)
                {
                    if (maxScore < answer.AnswerScore)
                    {
                        maxScore = answer.AnswerScore;
                    }
                }
                answerListView.ItemsSource = currentAnswers;
            }

        }

        private void menuToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MenuPage());
        }

        private void editToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EditSavegamePage(savegame));
        }       

        private void answerListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Answer selectedAnswer = answerListView.SelectedItem as Answer;            

            using (SQLiteConnection connection = new SQLiteConnection(App.UserDatabaseLocation))
            {
                connection.CreateTable<Score>();
                var scores = connection.Table<Score>().ToList();
                var currentScore = (from score in scores where score.SavegameId == savegame.SavegameId 
                                    && score.ScoreCategoryId == currentScenario.ScoreCategoryId select score).ToList();

                if (currentScore.Count == 0)
                {
                    Score score = new Score()
                    { AchievedScore = selectedAnswer.AnswerScore,
                      MaxScore = maxScore,
                      SavegameId = savegame.SavegameId,
                      ScoreCategoryId = currentScenario.ScoreCategoryId
                    };
                    int rows = connection.Insert(score);
                    if (rows == 0)
                    {
                        DisplayAlert("Fout", "Er is iets misgegaan bij het aanmaken van je scores.", "Oke");
                    }
                }
                else
                {
                    currentScore[0].AchievedScore += selectedAnswer.AnswerScore;
                    currentScore[0].MaxScore += maxScore;
                    int rows = connection.Update(currentScore[0]);

                    if (rows == 0)
                    {
                        DisplayAlert("Fout", "Er is iets misgegaan bij het aanmaken van je scores.", "Oke");
                    }
                }
                if (selectedAnswer.NextScenarioId == -1) //to do check if scenario exists
                {
                    Navigation.PushAsync(new ScorePage(savegame));
                }
                else
                {                    
                    connection.CreateTable<Savegame>();
                    savegame.ScenarioId = selectedAnswer.NextScenarioId;
                    int rows = connection.Update(savegame);
                    if (rows > 0)
                    {
                        Navigation.PushAsync(new ScenarioPage(savegame));
                    }
                    else
                    {
                        DisplayAlert("Fout", "Er is iets misgegaan bij het updaten van je savegame.", "Oke");
                    }                    
                }
            }            
        }
    }
}