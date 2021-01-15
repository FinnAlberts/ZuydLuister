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
    public partial class SelectSavegamePage : ContentPage
    {
        public SelectSavegamePage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
        }

        private void NewGameButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewSavegamePage());
        }

        private void SavegameListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Navigation.PushAsync(new ScenarioPage());
        }
    }
}