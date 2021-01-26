using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Environment = System.Environment;
using System.IO;

namespace ZuydLuister.Droid
{
    [Activity(Label = "ZuydLuister", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            string userDBName = "UserDatabase.sqlite";
            string userfolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string userfullPath = Path.Combine(userfolderPath, userDBName);

            string gameDBName = "GameDatabase.sqlite";
            string gamefolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string gamefullPath = Path.Combine(gamefolderPath, gameDBName);

            Window.SetStatusBarColor(Android.Graphics.Color.Argb(255, 178, 117, 32));

            LoadApplication(new App(userfullPath, gamefullPath));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}