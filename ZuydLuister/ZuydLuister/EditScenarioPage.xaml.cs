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
                // Hide delete button for new scenarios
                deleteButton.IsVisible = false;
                displayAmountOfAnswers(0);
            } else
            {
                // Fill in entries for existing scenarios
                nameEntry.Text = scenario.ScenarioName;
                textEditor.Text = scenario.ScenarioContent;
                imageEntry.Text = scenario.ScenarioImage;
                startScenarioCheckBox.IsChecked = scenario.IsStartScenario;
            }

            using (SQLiteConnection connection = new SQLiteConnection(App.GameDatabaseLocation))
            {
                connection.CreateTable<ScoreCategory>();

                // Load ScoreCategories into Picker
                var scoreCategories = connection.Table<ScoreCategory>().ToList();
                scoreCategoryPicker.ItemsSource = scoreCategories;

                // Load scenarios into Pickers for selecting to which scenario the answer should take the player to
                var scenarios = connection.Table<Scenario>().ToList();

                // When editing a scenario, remove that scenario
                if (!isNew)
                {
                    scenarios = (from scenario in scenarios where scenario.ScenarioId != this.scenario.ScenarioId select scenario).ToList();
                }
                
                scenarios.Add(new Scenario { ScenarioName = "Ga naar het einde", ScenarioId = -1 });
                nextScenarioAPicker.ItemsSource = scenarios;
                nextScenarioBPicker.ItemsSource = scenarios;
                nextScenarioCPicker.ItemsSource = scenarios;
                nextScenarioDPicker.ItemsSource = scenarios;

                if (!isNew)
                {
                    // Select current ScoreCategory in Picker
                    var selectedScoreCategory = (from scoreCategory in scoreCategories where scoreCategory.ScoreCategoryId == scenario.ScoreCategoryId select scoreCategory).ToList();

                    if (selectedScoreCategory.Count != 0)
                    {
                        scoreCategoryPicker.SelectedItem = selectedScoreCategory[0];
                    }                    

                    // Select correct amount of answers in Picker
                    connection.CreateTable<Answer>();

                    var answers = connection.Table<Answer>();
                    var scenarioAnswers = (from answer in answers where answer.ScenarioId == scenario.ScenarioId select answer).ToList();
                    Console.WriteLine(scenarioAnswers.Count);

                    answerPicker.SelectedIndex = scenarioAnswers.Count - 1;
                    displayAmountOfAnswers(scenarioAnswers.Count);
                 
                    // Put answers in entries, select correct 'next' scenario in Pickers and put correct amount of points in entry
                    if (scenarioAnswers.Count > 0) // Answer A
                    {
                        answerAEntry.Text = scenarioAnswers[0].AnswerContent;
                        pointAEntry.Text = scenarioAnswers[0].AnswerScore.ToString();

                        var nextScenario = (from scenario in scenarios where scenario.ScenarioId == scenarioAnswers[0].NextScenarioId select scenario).ToList();
                        if (nextScenario.Count != 0) // Check if referenced scenario still exists and is not deleted
                        {
                            nextScenarioAPicker.SelectedItem = nextScenario[0];
                        }

                        if (scenarioAnswers.Count > 1) // Answer B
                        {
                            answerBEntry.Text = scenarioAnswers[1].AnswerContent;
                            pointBEntry.Text = scenarioAnswers[1].AnswerScore.ToString();

                            nextScenario = (from scenario in scenarios where scenario.ScenarioId == scenarioAnswers[1].NextScenarioId select scenario).ToList();
                            if (nextScenario.Count != 0) // Check if referenced scenario still exists and is not deleted
                            {
                                nextScenarioBPicker.SelectedItem = nextScenario[0];
                            }

                            if (scenarioAnswers.Count > 2) // Answer C
                            {
                                answerCEntry.Text = scenarioAnswers[2].AnswerContent;
                                pointCEntry.Text = scenarioAnswers[2].AnswerScore.ToString();

                                nextScenario = (from scenario in scenarios where scenario.ScenarioId == scenarioAnswers[2].NextScenarioId select scenario).ToList();
                                if (nextScenario.Count != 0) // Check if referenced scenario still exists and is not deleted
                                {
                                    nextScenarioCPicker.SelectedItem = nextScenario[0];
                                }

                                if (scenarioAnswers.Count > 3) // Answer D
                                {
                                    answerDEntry.Text = scenarioAnswers[3].AnswerContent;
                                    pointDEntry.Text = scenarioAnswers[3].AnswerScore.ToString();

                                    nextScenario = (from scenario in scenarios where scenario.ScenarioId == scenarioAnswers[3].NextScenarioId select scenario).ToList();
                                    if (nextScenario.Count != 0) // Check if referenced scenario still exists and is not deleted
                                    {
                                        nextScenarioDPicker.SelectedItem = nextScenario[0];
                                    }
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
                if (amountOfAnswers > 0) // Answer A
                {
                    if (String.IsNullOrEmpty(answerAEntry.Text) || String.IsNullOrEmpty(pointAEntry.Text) || nextScenarioAPicker.SelectedIndex == -1)
                    {
                        everythingFilledIn = false;
                    }

                    if (amountOfAnswers > 1) // Answer B
                    {
                        if (String.IsNullOrEmpty(answerBEntry.Text) || String.IsNullOrEmpty(pointBEntry.Text) || nextScenarioBPicker.SelectedIndex == -1)
                        {
                            everythingFilledIn = false;
                        }

                        if (amountOfAnswers > 2) // Answer C
                        {
                            if (String.IsNullOrEmpty(answerCEntry.Text) || String.IsNullOrEmpty(pointCEntry.Text) || nextScenarioCPicker.SelectedIndex == -1)
                            {
                                everythingFilledIn = false;
                            }

                            if (amountOfAnswers > 3) // Answer D
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

            if (!everythingFilledIn) // Not everything is filled in
            {
                DisplayAlert("Fout", "Niet alle velden zijn ingevuld.", "Oke");
            } 
            else
            {
                // Everything is filled in, continue

                // Read ScoreCategory from Picker
                ScoreCategory scoreCategory = scoreCategoryPicker.SelectedItem as ScoreCategory;

                if (isNew) // Scenario is a new scenario
                {
                    using (SQLiteConnection connection = new SQLiteConnection(App.GameDatabaseLocation))
                    {
                        connection.CreateTable<Scenario>();
                        connection.CreateTable<Answer>();

                        // Check if name already exists
                        var scenarios = connection.Table<Scenario>().ToList();
                        int amountOfScenarios = (from scenario in scenarios where scenario.ScenarioName == nameEntry.Text select scenario).ToList().Count;

                        if (amountOfScenarios > 0) // Name already exists
                        {
                            DisplayAlert("Fout", "De opgegeven scenarionaam bestaat al. Kies een andere naam.", "Oke");
                        } else // Name does not yet exist
                        {
                            // Check if new scenario is going to be the startScenario. If so, set all scenarios in the database to IsStartScenario is false
                            if (startScenarioCheckBox.IsChecked)
                            {
                                var currentStartScenario = (from scenario in scenarios where scenario.IsStartScenario == true select scenario).ToList();
                                foreach (Scenario startScenario in currentStartScenario)
                                {
                                    startScenario.IsStartScenario = false;
                                    connection.Update(startScenario);
                                }
                            }
                            
                            // Create a new scenario and insert it into the database
                            scenario = new Scenario { ScenarioContent = textEditor.Text, ScenarioImage = imageEntry.Text, ScenarioName = nameEntry.Text, ScoreCategoryId = scoreCategory.ScoreCategoryId, IsStartScenario = startScenarioCheckBox.IsChecked };
                            int rows = connection.Insert(scenario);

                            // Read amount of answers from Picker
                            int amountOfAnswers = answerPicker.SelectedIndex + 1;

                            // For each answer, create an Answer object and insert it into the database
                            if (amountOfAnswers > 0) // Answer A
                            {
                                int nextScenarioId = (nextScenarioAPicker.SelectedItem as Scenario).ScenarioId;
                                Answer answer = new Answer { AnswerContent = answerAEntry.Text, AnswerScore = Int32.Parse(pointAEntry.Text), NextScenarioId = nextScenarioId, ScenarioId = scenario.ScenarioId };

                                rows += connection.Insert(answer);

                                if (amountOfAnswers > 1) // Answer B
                                {
                                    nextScenarioId = (nextScenarioBPicker.SelectedItem as Scenario).ScenarioId;
                                    answer = new Answer { AnswerContent = answerBEntry.Text, AnswerScore = Int32.Parse(pointBEntry.Text), NextScenarioId = nextScenarioId, ScenarioId = scenario.ScenarioId };

                                    rows += connection.Insert(answer);

                                    if (amountOfAnswers > 2) // Answer C
                                    {
                                        nextScenarioId = (nextScenarioCPicker.SelectedItem as Scenario).ScenarioId;
                                        answer = new Answer { AnswerContent = answerCEntry.Text, AnswerScore = Int32.Parse(pointCEntry.Text), NextScenarioId = nextScenarioId, ScenarioId = scenario.ScenarioId };

                                        rows += connection.Insert(answer);

                                        if (amountOfAnswers > 3) // Answer D
                                        {
                                            nextScenarioId = (nextScenarioDPicker.SelectedItem as Scenario).ScenarioId;
                                            answer = new Answer { AnswerContent = answerDEntry.Text, AnswerScore = Int32.Parse(pointDEntry.Text), NextScenarioId = nextScenarioId, ScenarioId = scenario.ScenarioId };

                                            rows += connection.Insert(answer);
                                        }
                                    }
                                }
                            }

                            // Check for errors while inserting
                            if (rows != 1 + amountOfAnswers)
                            {
                                DisplayAlert("Fout", "Er is iets misgegaan tijdens het toevoegen van het scenario. Probeer het opnieuw.", "Oke");
                            } 
                            else // No errors
                            {
                                DisplayAlert("Succes", "Het scenario is succesvol toegevoegd.", "Oke");
                                Navigation.PopAsync();
                            }
                        }
                    }
                } else // Editing existing scenario
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
                        scenario.IsStartScenario = startScenarioCheckBox.IsChecked;

                        // Check if new scenario is going to be the startScenario. If so, set all scenarios in the database to IsStartScenario is false
                        if (startScenarioCheckBox.IsChecked)
                        {
                            var scenarios = connection.Table<Scenario>().ToList();
                            var currentStartScenario = (from scenario in scenarios where scenario.IsStartScenario == true select scenario).ToList();
                            foreach (Scenario startScenario in currentStartScenario)
                            {
                                startScenario.IsStartScenario = false;
                                connection.Update(startScenario);
                            }
                        }

                        int rows = connection.Update(scenario);

                        // Delete all answers and insert them again to deal with deletion/insertion of answers (amount of answers may change)

                        // Find all answers from scenario and delete them
                        var answers = connection.Table<Answer>().ToList();
                        var scenarioAnswers = (from answer in answers where answer.ScenarioId == scenario.ScenarioId select answer).ToList();
                        foreach (Answer answer in scenarioAnswers)
                        {
                            rows += connection.Delete(answer);
                        }

                        // Get amount of answers from Picker
                        int amountOfAnswers = answerPicker.SelectedIndex + 1;

                        // For each answer, create an Answer object and insert it into the database
                        if (amountOfAnswers > 0) // Answer A
                        {
                            int nextScenarioId = (nextScenarioAPicker.SelectedItem as Scenario).ScenarioId;
                            Answer answer = new Answer { AnswerContent = answerAEntry.Text, AnswerScore = Int32.Parse(pointAEntry.Text), NextScenarioId = nextScenarioId, ScenarioId = scenario.ScenarioId };

                            rows += connection.Insert(answer);

                            if (amountOfAnswers > 1) // Answer B
                            {
                                nextScenarioId = (nextScenarioBPicker.SelectedItem as Scenario).ScenarioId;
                                answer = new Answer { AnswerContent = answerBEntry.Text, AnswerScore = Int32.Parse(pointBEntry.Text), NextScenarioId = nextScenarioId, ScenarioId = scenario.ScenarioId };

                                rows += connection.Insert(answer);

                                if (amountOfAnswers > 2) // Answer C
                                {
                                    nextScenarioId = (nextScenarioCPicker.SelectedItem as Scenario).ScenarioId;
                                    answer = new Answer { AnswerContent = answerCEntry.Text, AnswerScore = Int32.Parse(pointCEntry.Text), NextScenarioId = nextScenarioId, ScenarioId = scenario.ScenarioId };

                                    rows += connection.Insert(answer);

                                    if (amountOfAnswers > 3) // Answer D
                                    {
                                        nextScenarioId = (nextScenarioDPicker.SelectedItem as Scenario).ScenarioId;
                                        answer = new Answer { AnswerContent = answerDEntry.Text, AnswerScore = Int32.Parse(pointDEntry.Text), NextScenarioId = nextScenarioId, ScenarioId = scenario.ScenarioId };

                                        rows += connection.Insert(answer);
                                    }
                                }
                            }
                        }

                        // Check for errors while updating
                        if (rows != 1 + amountOfAnswers + scenarioAnswers.Count)
                        {
                            DisplayAlert("Fout", "Er is iets misgegaan tijdens het bewerken van het scenario. Probeer het opnieuw.", "Oke");
                        }
                        else // No errors
                        {
                            DisplayAlert("Succes", "Het scenario is succesvol bewerkt.", "Oke");
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

                var answers = connection.Table<Answer>().ToList();
                var scenarioAnswers = (from answer in answers where answer.ScenarioId == scenario.ScenarioId select answer).ToList();
                foreach (Answer answer in scenarioAnswers)
                {
                    rows += connection.Delete(answer);
                }

                // Check for errors while updating
                if (rows != 1 + scenarioAnswers.Count)
                {
                    DisplayAlert("Fout", "Er is iets misgegaan tijdens het verwijderen van het scenario. Probeer het opnieuw.", "Oke");
                }
                else // No errors
                {
                    DisplayAlert("Succes", "Het scenario is succesvol verwijderd.", "Oke");
                    Navigation.PopAsync();
                }
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
            answerAGrid.IsVisible = false;
            answerBGrid.IsVisible = false;
            answerCGrid.IsVisible = false;
            answerDGrid.IsVisible = false;

            if (amountOfAnswers > 0)
            {
                answerAGrid.IsVisible = true;

                if (amountOfAnswers > 1)
                {
                    answerBGrid.IsVisible = true;

                    if (amountOfAnswers > 2)
                    {
                        answerCGrid.IsVisible = true;

                        if (amountOfAnswers > 3)
                        {
                            answerDGrid.IsVisible = true;
                        }
                    }
                }
            }
        }
    }
}
