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
    public partial class ScorePage : ContentPage
    {
        public ScorePage()
        {
            InitializeComponent();
        }

        private void contactButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ContactPage());

        }

        private void workshopsButton_Clicked(object sender, EventArgs e)
        {

        }

        private void backToMenuButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MenuPage());
        }
    }
}