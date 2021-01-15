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
    public partial class EditCategoryPage : ContentPage
    {
        public EditCategoryPage()
        {
            InitializeComponent();
        }

        private void saveButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CategoryOverviewPage());
        }

        private void deleteButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CategoryOverviewPage());
        }
    }
}