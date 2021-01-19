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
    public partial class ScenariosOverviewPage : ContentPage
    {
        public ScenariosOverviewPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLiteConnection connection = new SQLiteConnection(App.GameDatabaseLocation))
            {
                connection.CreateTable<Scenario>();

                var scenarios = connection.Table<Scenario>().ToList();

                scenarioListView.ItemsSource = scenarios;
            }
        }

        private void scenarioListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Scenario selectedScenario = scenarioListView.SelectedItem as Scenario;

            Navigation.PushAsync(new EditScenarioPage(selectedScenario));
        }

        private void newScenarioButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EditScenarioPage());
        }
    }
}