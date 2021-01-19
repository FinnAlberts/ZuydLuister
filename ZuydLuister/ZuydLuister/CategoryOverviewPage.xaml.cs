using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLiteConnection connection = new SQLiteConnection(App.GameDatabaseLocation))
            {
                connection.CreateTable<ScoreCategory>();
                var categories = connection.Table<ScoreCategory>().ToList();
                categoryListView.ItemsSource = categories;
            }
        }

        private void categorieListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedCategory = categoryListView.SelectedItem as ScoreCategory;

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