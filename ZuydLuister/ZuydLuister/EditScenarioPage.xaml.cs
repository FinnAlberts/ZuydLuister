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

            if (isNew)
            {
                deleteButton.IsVisible = false;
                displayAmountOfAnswers(0);
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
                scenarios.Add(new Scenario { ScenarioName = "Ga naar het einde", ScenarioId = -1 });
                nextScenarioAPicker.ItemsSource = scenarios;
                nextScenarioBPicker.ItemsSource = scenarios;
                nextScenarioCPicker.ItemsSource = scenarios;
                nextScenarioDPicker.ItemsSource = scenarios;

                if (!isNew)
                {
                    // Select current ScoreCategory in Picker
                    var selectedScoreCategory = (from scoreCategory in scoreCategories where scoreCategory.ScoreCategoryId == scenario.ScoreCategoryId select scoreCategory).ToList()[0];

                    scoreCategoryPicker.SelectedItem = selectedScoreCategory;

                    // Select correct amount of answers in Picker
                    connection.CreateTable<Answer>();

                    var answers = connection.Table<Answer>();
                    var scenarioAnswers = (from answer in answers where answer.ScenarioId == scenario.ScenarioId select answer).ToList();
                    Console.WriteLine(scenarioAnswers.Count);

                    answerPicker.SelectedIndex = scenarioAnswers.Count - 1;
                    displayAmountOfAnswers(scenarioAnswers.Count);
                 
                    // Select correct 'next' scenario in Pickers
                    if (scenarioAnswers.Count > 0)
                    {
                        answerAEntry.Text = scenarioAnswers[0].AnswerContent;
                        pointAEntry.Text = scenarioAnswers[0].AnswerScore.ToString();

                        var nextScenario = (from scenario in scenarios where scenario.ScenarioId == scenarioAnswers[0].NextScenarioId select scenario).ToList()[0];
                        nextScenarioAPicker.SelectedItem = nextScenario;

                        if (scenarioAnswers.Count > 1)
                        {
                            answerBEntry.Text = scenarioAnswers[1].AnswerContent;
                            pointBEntry.Text = scenarioAnswers[1].AnswerScore.ToString();

                            nextScenario = (from scenario in scenarios where scenario.ScenarioId == scenarioAnswers[1].NextScenarioId select scenario).ToList()[0];
                            nextScenarioBPicker.SelectedItem = nextScenario;

                            if (scenarioAnswers.Count > 2)
                            {
                                answerCEntry.Text = scenarioAnswers[2].AnswerContent;
                                pointCEntry.Text = scenarioAnswers[2].AnswerScore.ToString();

                                nextScenario = (from scenario in scenarios where scenario.ScenarioId == scenarioAnswers[2].NextScenarioId select scenario).ToList()[0];
                                nextScenarioCPicker.SelectedItem = nextScenario;

                                if (scenarioAnswers.Count > 3)
                                {
                                    answerDEntry.Text = scenarioAnswers[3].AnswerContent;
                                    pointDEntry.Text = scenarioAnswers[3].AnswerScore.ToString();

                                    nextScenario = (from scenario in scenarios where scenario.ScenarioId == scenarioAnswers[3].NextScenarioId select scenario).ToList()[0];
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
            // Check if everything is filled in
            bool everythingFilledIn = true;

            if (String.IsNullOrEmpty(nameEntry.Text) || String.IsNullOrEmpty(textEditor.Text) || String.IsNullOrEmpty(imageEntry.Text) || scoreCategoryPicker.SelectedIndex == -1 || answerPicker.SelectedIndex == -1)
            {
                everythingFilledIn = false;
            } else
            {
                int amountOfAnswers = answerPicker.SelectedIndex + 1;
                if (amountOfAnswers > 0)
                {
                    if (String.IsNullOrEmpty(answerAEntry.Text) || String.IsNullOrEmpty(pointAEntry.Text) || nextScenarioAPicker.SelectedIndex == -1)
                    {
                        everythingFilledIn = false;
                    }

                    if (amountOfAnswers > 1)
                    {
                        if (String.IsNullOrEmpty(answerBEntry.Text) || String.IsNullOrEmpty(pointBEntry.Text) || nextScenarioBPicker.SelectedIndex == -1)
                        {
                            everythingFilledIn = false;
                        }

                        if (amountOfAnswers > 2)
                        {
                            if (String.IsNullOrEmpty(answerCEntry.Text) || String.IsNullOrEmpty(pointCEntry.Text) || nextScenarioCPicker.SelectedIndex == -1)
                            {
                                everythingFilledIn = false;
                            }

                            if (amountOfAnswers > 3)
                            {
                                if (String.IsNullOrEmpty(answerDEntry.Text) || String.IsNullOrEmpty(pointDEntry.Text) || nextScenarioDPicker.SelectedIndex == -1)
                                {
                                    everythingFilledIn = false;
                                }
                            }
                        }
                    }
                }
            }

            if (!everythingFilledIn)
            {
                DisplayAlert("Fout", "Niet alle velden zijn ingevuld", "Oke");
            } 
            else
            {
                ScoreCategory scoreCategory = scoreCategoryPicker.SelectedItem as ScoreCategory;

                if (isNew)
                {
                    using (SQLiteConnection connection = new SQLiteConnection(App.GameDatabaseLocation))
                    {
                        connection.CreateTable<Scenario>();
                        connection.CreateTable<Answer>();

                        // Check if name already exists
                        var scenarios = connection.Table<Scenario>().ToList();
                        int amountOfScenarios = (from scenario in scenarios where scenario.ScenarioName == nameEntry.Text select scenario).ToList().Count;

                        if (amountOfScenarios > 0)
                        {
                            DisplayAlert("Fout", "De opgegeven scenarionaam bestaat al. Kies een andere naam.", "Oke");
                        } else
                        {
                            scenario = new Scenario { ScenarioContent = textEditor.Text, ScenarioImage = imageEntry.Text, ScenarioName = nameEntry.Text, ScoreCategoryId = scoreCategory.ScoreCategoryId };
                            int rows = connection.Insert(scenario);

                            if (rows == 0)
                            {
                                DisplayAlert("Fout", "Het scenario kon niet worden toegevoegd. Probeer het opnieuw", "Oke");
                            }

                            int amountOfAnswers = answerPicker.SelectedIndex + 1;

                            if (amountOfAnswers > 0)
                            {
                                int nextScenarioId = (nextScenarioAPicker.SelectedItem as Scenario).ScenarioId;
                                Answer answer = new Answer { AnswerContent = answerAEntry.Text, AnswerScore = Int32.Parse(pointAEntry.Text), NextScenarioId = nextScenarioId, ScenarioId = scenario.ScenarioId };

                                rows = connection.Insert(answer);

                                if (rows == 0)
                                {
                                    DisplayAlert("Fout", "Het scenario kon niet worden toegevoegd. Probeer het opnieuw", "Oke");
                                }

                                if (amountOfAnswers > 1)
                                {
                                    nextScenarioId = (nextScenarioBPicker.SelectedItem as Scenario).ScenarioId;
                                    answer = new Answer { AnswerContent = answerBEntry.Text, AnswerScore = Int32.Parse(pointBEntry.Text), NextScenarioId = nextScenarioId, ScenarioId = scenario.ScenarioId };

                                    rows = connection.Insert(answer);

                                    if (rows == 0)
                                    {
                                        DisplayAlert("Fout", "Het scenario kon niet worden toegevoegd. Probeer het opnieuw", "Oke");
                                    }

                                    if (amountOfAnswers > 2)
                                    {
                                        nextScenarioId = (nextScenarioCPicker.SelectedItem as Scenario).ScenarioId;
                                        answer = new Answer { AnswerContent = answerCEntry.Text, AnswerScore = Int32.Parse(pointCEntry.Text), NextScenarioId = nextScenarioId, ScenarioId = scenario.ScenarioId };

                                        rows = connection.Insert(answer);

                                        if (rows == 0)
                                        {
                                            DisplayAlert("Fout", "Het scenario kon niet worden toegevoegd. Probeer het opnieuw", "Oke");
                                        }

                                        if (amountOfAnswers > 3)
                                        {
                                            nextScenarioId = (nextScenarioDPicker.SelectedItem as Scenario).ScenarioId;
                                            answer = new Answer { AnswerContent = answerDEntry.Text, AnswerScore = Int32.Parse(pointDEntry.Text), NextScenarioId = nextScenarioId, ScenarioId = scenario.ScenarioId };

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
                } else
                {
                    using (SQLiteConnection connection = new SQLiteConnection(App.GameDatabaseLocation))
                    {
                        connection.CreateTable<Scenario>();
                        connection.CreateTable<Answer>();

                        // Updating the scenario object
                        scenario.ScenarioName = nameEntry.Text;
                        scenario.ScenarioContent = textEditor.Text;
                        scenario.ScenarioImage = imageEntry.Text;
                        scenario.ScoreCategoryId = scoreCategory.ScoreCategoryId;
                        connection.Update(scenario);

                        // Delete all answers and insert them again to deal with deletion/insertion of answers (amount of answers may change)
                        var answers = connection.Table<Answer>().ToList();
                        var scenarioAnswers = (from answer in answers where answer.ScenarioId == scenario.ScenarioId select answer).ToList();
                        foreach (Answer answer in scenarioAnswers)
                        {
                            int rows = connection.Delete(answer);

                            if (rows == 0)
                            {
                                DisplayAlert("Fout", "Er is iets misgegaan bij het bewerken van het scenario. Probeer het opniew.", "Oke");
                            }
                        }

                        int amountOfAnswers = answerPicker.SelectedIndex + 1;

                        if (amountOfAnswers > 0)
                        {
                            int nextScenarioId = (nextScenarioAPicker.SelectedItem as Scenario).ScenarioId;
                            Answer answer = new Answer { AnswerContent = answerAEntry.Text, AnswerScore = Int32.Parse(pointAEntry.Text), NextScenarioId = nextScenarioId, ScenarioId = scenario.ScenarioId };

                            int rows = connection.Insert(answer);

                            if (rows == 0)
                            {
                                DisplayAlert("Fout", "Er is iets misgegaan bij het bewerken van het scenario. Probeer het opniew.", "Oke");
                            }

                            if (amountOfAnswers > 1)
                            {
                                nextScenarioId = (nextScenarioBPicker.SelectedItem as Scenario).ScenarioId;
                                answer = new Answer { AnswerContent = answerBEntry.Text, AnswerScore = Int32.Parse(pointBEntry.Text), NextScenarioId = nextScenarioId, ScenarioId = scenario.ScenarioId };

                                rows = connection.Insert(answer);

                                if (rows == 0)
                                {
                                    DisplayAlert("Fout", "Er is iets misgegaan bij het bewerken van het scenario. Probeer het opniew.", "Oke");
                                }

                                if (amountOfAnswers > 2)
                                {
                                    nextScenarioId = (nextScenarioCPicker.SelectedItem as Scenario).ScenarioId;
                                    answer = new Answer { AnswerContent = answerCEntry.Text, AnswerScore = Int32.Parse(pointCEntry.Text), NextScenarioId = nextScenarioId, ScenarioId = scenario.ScenarioId };

                                    rows = connection.Insert(answer);

                                    if (rows == 0)
                                    {
                                        DisplayAlert("Fout", "Er is iets misgegaan bij het bewerken van het scenario. Probeer het opniew.", "Oke");
                                    }

                                    if (amountOfAnswers > 3)
                                    {
                                        nextScenarioId = (nextScenarioDPicker.SelectedItem as Scenario).ScenarioId;
                                        answer = new Answer { AnswerContent = answerDEntry.Text, AnswerScore = Int32.Parse(pointDEntry.Text), NextScenarioId = nextScenarioId, ScenarioId = scenario.ScenarioId };

                                        rows = connection.Insert(answer);

                                        if (rows == 0)
                                        {
                                            DisplayAlert("Fout", "Er is iets misgegaan bij het bewerken van het scenario. Probeer het opniew.", "Oke");
                                        }
                                    }
                                }
                            }
                            Navigation.PopAsync();
                        }
                    }
                }
            }
        }

        private void deleteButton_Clicked(object sender, EventArgs e)
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.GameDatabaseLocation))
            {
                connection.CreateTable<Scenario>();
                connection.CreateTable<Answer>();

                int rows = connection.Delete(scenario);
                if (rows == 0)
                {
                    DisplayAlert("Fout", "Er is iets misgegaan bij het verwijderen van het scenario. Probeer het opniew.", "Oke");
                }

                var answers = connection.Table<Answer>().ToList();
                var scenarioAnswers = (from answer in answers where answer.ScenarioId == scenario.ScenarioId select answer).ToList();
                foreach (Answer answer in scenarioAnswers)
                {
                    rows = connection.Delete(answer);

                    if (rows == 0)
                    {
                        DisplayAlert("Fout", "Er is iets misgegaan bij het bewerken van het scenario. Probeer het opniew.", "Oke");
                    }
                }
                Navigation.PopAsync();
            }
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
