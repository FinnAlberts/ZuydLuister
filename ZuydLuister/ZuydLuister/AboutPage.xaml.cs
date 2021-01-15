using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ZuydLuister
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
        }

        private void ContactButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ContactPage());
        }

        private async void WorkshopButton_Clicked(object sender, EventArgs e)
        {
            string link = "https://moodle.zuyd.nl/course/view.php?id=6992&sectionid=265207#section-16";
            await Browser.OpenAsync(link, BrowserLaunchMode.SystemPreferred);
        }

        private async void MoodleMOOIButton_Clicked(object sender, EventArgs e)
        {
            string link = "https://moodle.zuyd.nl/course/view.php?id=6992&sectionid=259691#section-2";
            await Browser.OpenAsync(link, BrowserLaunchMode.SystemPreferred);
        }
    }
}