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
        }

        private void SavegameButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ScenarioPage());
        }

        private void NewGameButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewSavegamePage());
        }

        private void menuToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MenuPage());
        }
    }
}