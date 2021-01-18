using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ZuydLuister
{
    public partial class App : Application
    {
        public static string UserDatabaseLocation = string.Empty;
        public static string GameDatabaseLocation = string.Empty;
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());
        }

        public App(string userDbLocation, string gameDbLocation)
        {
            InitializeComponent();
            UserDatabaseLocation = userDbLocation;
            GameDatabaseLocation = gameDbLocation;
            MainPage = new NavigationPage(new LoginPage());            
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
