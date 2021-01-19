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
    public partial class EditScenarioPage : ContentPage
    {
        private bool isNew;
        private Scenario scenario;

        // New scenario
        public EditScenarioPage()
        {
            InitializeComponent();

            isNew = true;
        }

        // Editing an existing scenario
        public EditScenarioPage(Scenario scenario)
        {
            InitializeComponent();

            isNew = false;
            this.scenario = scenario;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (isNew == true)
            {
                deleteButton.IsVisible = false;
                answerPicker.SelectedIndex = 0;
                displayAmountOfAnswers(1);
            } else
            {
                nameEntry.Text = scenario.ScenarioName;
                textEditor.Text = scenario.ScenarioContent;
                imageEntry.Text = scenario.ScenarioImage;
            }

            using (SQLiteConnection connection = new SQLiteConnection(App.GameDatabaseLocation))
            {
                connection.CreateTable<ScoreCategory>();

                // Load ScoreCategories into Picker
                var scoreCategories = connection.Table<ScoreCategory>().ToList();
                scoreCategoryPicker.ItemsSource = scoreCategories;

                // Load scenarios into Pickers for selecting to which scenario the answer should take the player to
                var scenarios = connection.Table<Scenario>().ToList();
                nextScenarioAPicker.ItemsSource = scenarios;
                nextScenarioBPicker.ItemsSource = scenarios;
                nextScenarioCPicker.ItemsSource = scenarios;
                nextScenarioDPicker.ItemsSource = scenarios;

                if (isNew == false)
                {
                    // Select current ScoreCategory in Picker
                    var selectedScoreCategory = (from scoreCategory in scoreCategories where scoreCategory.ScoreCategoryId == scenario.ScoreCategoryId select scoreCategory);

                    scoreCategoryPicker.SelectedItem = selectedScoreCategory;

                    // Select correct amount of answers in Picker
                    connection.CreateTable<Answer>();

                    var answers = connection.Table<Answer>();
                    var scenarioAnswers = (from answer in answers where answer.ScenarioId == scenario.ScenarioId select answer).ToList();

                    displayAmountOfAnswers(scenarioAnswers.Count);
                 
                    // Select correct 'next' scenario in Pickers
                    if (scenarioAnswers.Count > 0)
                    {
                        answerAEntry.Text = scenarioAnswers[0].AnswerContent;
                        pointAEntry.Text = scenarioAnswers[0].AnswerScore.ToString();

                        var nextScenario = (from scenario in scenarios where scenario.ScenarioId == scenarioAnswers[0].NextScenarioId select scenario);
                        nextScenarioAPicker.SelectedItem = nextScenario;

                        if (scenarioAnswers.Count > 1)
                        {
                            answerBEntry.Text = scenarioAnswers[1].AnswerContent;
                            pointBEntry.Text = scenarioAnswers[1].AnswerScore.ToString();

                            nextScenario = (from scenario in scenarios where scenario.ScenarioId == scenarioAnswers[1].NextScenarioId select scenario);
                            nextScenarioBPicker.SelectedItem = nextScenario;

                            if (scenarioAnswers.Count > 2)
                            {
                                answerCEntry.Text = scenarioAnswers[2].AnswerContent;
                                pointCEntry.Text = scenarioAnswers[2].AnswerScore.ToString();

                                nextScenario = (from scenario in scenarios where scenario.ScenarioId == scenarioAnswers[2].NextScenarioId select scenario);
                                nextScenarioCPicker.SelectedItem = nextScenario;

                                if (scenarioAnswers.Count > 3)
                                {
                                    answerDEntry.Text = scenarioAnswers[3].AnswerContent;
                                    pointDEntry.Text = scenarioAnswers[3].AnswerScore.ToString();

                                    nextScenario = (from scenario in scenarios where scenario.ScenarioId == scenarioAnswers[3].NextScenarioId select scenario);
                                    nextScenarioDPicker.SelectedItem = nextScenario;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void saveButton_Clicked(object sender, EventArgs e)
        {
            ScoreCategory scoreCategory = scoreCategoryPicker.SelectedItem as ScoreCategory;

            if (isNew == true)
            {
                using (SQLiteConnection connection = new SQLiteConnection(App.GameDatabaseLocation))
                {
                    connection.CreateTable<Scenario>();
                    connection.CreateTable<Answer>();

                    Scenario scenario = new Scenario { ScenarioContent = textEditor.Text, ScenarioImage = imageEntry.Text, ScenarioName = nameEntry.Text, ScoreCategoryId = scoreCategory.ScoreCategoryId };
                    int rows = connection.Insert(scenario);

                    if (rows == 0)
                    {
                        DisplayAlert("Fout", "Het scenario kon niet worden toegevoegd. Probeer het opnieuw", "Oke");
                    }

                    int amountOfAnswers = answerPicker.SelectedIndex + 1;

                    if (amountOfAnswers > 0)
                    {
                        int nextScenarioId = (nextScenarioAPicker.SelectedItem as Scenario).ScenarioId;
                        Answer answer = new Answer { AnswerContent = answerAEntry.Text, AnswerScore = Int32.Parse(pointAEntry.Text), NextScenarioId = nextScenarioId };

                        rows = connection.Insert(answer);

                        if (rows == 0)
                        {
                            DisplayAlert("Fout", "Het scenario kon niet worden toegevoegd. Probeer het opnieuw", "Oke");
                        }

                        if (amountOfAnswers > 1)
                        {
                            nextScenarioId = (nextScenarioBPicker.SelectedItem as Scenario).ScenarioId;
                            answer = new Answer { AnswerContent = answerBEntry.Text, AnswerScore = Int32.Parse(pointBEntry.Text), NextScenarioId = nextScenarioId };

                            rows = connection.Insert(answer);

                            if (rows == 0)
                            {
                                DisplayAlert("Fout", "Het scenario kon niet worden toegevoegd. Probeer het opnieuw", "Oke");
                            }

                            if (amountOfAnswers > 2)
                            {
                                nextScenarioId = (nextScenarioCPicker.SelectedItem as Scenario).ScenarioId;
                                answer = new Answer { AnswerContent = answerCEntry.Text, AnswerScore = Int32.Parse(pointCEntry.Text), NextScenarioId = nextScenarioId };

                                rows = connection.Insert(answer);

                                if (rows == 0)
                                {
                                    DisplayAlert("Fout", "Het scenario kon niet worden toegevoegd. Probeer het opnieuw", "Oke");
                                }

                                if (amountOfAnswers > 3)
                                {
                                    nextScenarioId = (nextScenarioDPicker.SelectedItem as Scenario).ScenarioId;
                                    answer = new Answer { AnswerContent = answerDEntry.Text, AnswerScore = Int32.Parse(pointDEntry.Text), NextScenarioId = nextScenarioId };

                                    rows = connection.Insert(answer);

                                    if (rows == 0)
                                    {
                                        DisplayAlert("Fout", "Het scenario kon niet worden toegevoegd. Probeer het opnieuw", "Oke");
                                    }
                                }
                            }
                        }
                        Navigation.PopAsync();
                    }
                }
            }
        }

        private void deleteButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void menuToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MenuPage());
        }

        private void answerPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            displayAmountOfAnswers(answerPicker.SelectedIndex + 1);
        }

        // Function for showing correct amount of answer inputs
        private void displayAmountOfAnswers(int amountOfAnswers)
        {
            answerAStackLayout.IsVisible = false;
            answerBStackLayout.IsVisible = false;
            answerCStackLayout.IsVisible = false;
            answerDStackLayout.IsVisible = false;

            if (amountOfAnswers > 0)
            {
                answerAStackLayout.IsVisible = true;

                if (amountOfAnswers > 1)
                {
                    answerBStackLayout.IsVisible = true;

                    if (amountOfAnswers > 2)
                    {
                        answerCStackLayout.IsVisible = true;

                        if (amountOfAnswers > 3)
                        {
                            answerDStackLayout.IsVisible = true;
                        }
                    }
                }
            }
        }
    }
}
