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
    public partial class CategoryOverviewPage : ContentPage
    {
        public CategoryOverviewPage()
        {
            InitializeComponent();
        }

        private void categorieListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private void newCategoryButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EditCategoryPage());
        }
    }
}