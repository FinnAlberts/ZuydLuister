using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZuydLuister.Model;

namespace ZuydLuister
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryOverviewPage : ContentPage
    {
        public CategoryOverviewPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Load categories into ListView
            using (SQLiteConnection connection = new SQLiteConnection(App.GameDatabaseLocation))
            {
                connection.CreateTable<ScoreCategory>();
                var categories = connection.Table<ScoreCategory>().ToList();
                categoryListView.ItemsSource = categories;
            }
        }

        private void categorieListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // Get category to be edited
            var selectedCategory = categoryListView.SelectedItem as ScoreCategory;

            // Check if selected category is not null, else open category
            if (selectedCategory != null)
            {
                Navigation.PushAsync(new EditCategoryPage(selectedCategory));
            }
        }

        private void newCategoryButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EditCategoryPage());
        }
    }
}
