using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZuydLuister.Model;

namespace ZuydLuister
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditSavegamePage : ContentPage
    {
        private Savegame selectedSavegame;

        public EditSavegamePage()
        {
            InitializeComponent();
        }

        public EditSavegamePage(Savegame savegameInput)
        {
            InitializeComponent();

            selectedSavegame = savegameInput;
            savegameNameEntry.Text = savegameInput.SavegameName;
        }

        private void saveEditSavegameButton_Clicked(object sender, EventArgs e)
        {

        }

        private void deleteSavegameButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SelectSavegamePage());
        }

        private void menuToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MenuPage());
        }
    }
}
