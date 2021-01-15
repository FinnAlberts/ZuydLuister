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
    public partial class EditSavegamePage : ContentPage
    {
        public EditSavegamePage()
        {
            InitializeComponent();
        }

        private void saveEditSavegameButton_Clicked(object sender, EventArgs e)
        {

        }

        private void deleteSavegameButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SelectSavegamePage());
        }
    }
}