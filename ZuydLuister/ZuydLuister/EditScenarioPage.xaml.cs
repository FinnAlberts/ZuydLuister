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
    public partial class EditScenarioPage : ContentPage
    {
        public EditScenarioPage()
        {
            InitializeComponent();
        }

        private void saveButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void deleteButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}