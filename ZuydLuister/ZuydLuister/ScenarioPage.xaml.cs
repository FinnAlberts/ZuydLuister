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
    public partial class ScenarioPage : ContentPage
    {
        public ScenarioPage()
        {
            InitializeComponent();
        }

        private void menuToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MenuPage());
        }

        private void editToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EditSavegamePage());
        }
    }
}