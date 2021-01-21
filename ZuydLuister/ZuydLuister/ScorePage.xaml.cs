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
            //Navigation.PushAsync(new EditSavegamePage());
        }       
    }
}