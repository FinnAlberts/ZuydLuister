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
    public partial class ContactPage : ContentPage
    {
        public ContactPage()
        {
            InitializeComponent();
        }

        private async void sendEmailButton_Clicked(object sender, EventArgs e)
        {
            await Xamarin.Essentials.Email.ComposeAsync("Contactaanvraag luisterstudent", emailEditor.Text, "luister@zuyd.nl");
            await Navigation.PushAsync(new MenuPage());
        }

        private void menuToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MenuPage());
        }
    }
}
