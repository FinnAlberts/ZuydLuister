using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ZuydLuister
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScenariosOverviewPage : ContentPage
    {
        public ScenariosOverviewPage()
        {
            InitializeComponent();
        }

        private void scenarioListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private void newScenarioButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EditScenarioPage());
        }
    }
}