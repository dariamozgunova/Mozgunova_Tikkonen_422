using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mozgunova_Tikkonen_422.Pages
{
    /// <summary>
    /// Логика взаимодействия для SearchPage.xaml
    /// </summary>
    public partial class SearchPage : Page
    {
        public SearchPage()
        {
            InitializeComponent();
            LoadCategories();
            UpdateProducts();
        }

        private void LoadCategories()
        {
            try
            {
                var context = new Mozgunova_Tikkonen_422Entities();
                var categories = context.Categories.ToList();

                categoryComboBox.Items.Clear();
                categoryComboBox.Items.Add(new ComboBoxItem { Content = "Все категории" });

                foreach (var category in categories)
                {
                    categoryComboBox.Items.Add(category);
                }

                categoryComboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}");
            }
        }

        private void clearFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            nameFilterTextBox.Text = "";
            categoryComboBox.SelectedIndex = 0;
        }

        private void nameFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void UpdateProducts()
        {
            if (!IsInitialized)
            {
                return;
            }

            try
            {
                var context = new Mozgunova_Tikkonen_422Entities();
                var currentProducts = context.Products.Include("Category").ToList();

                if (!string.IsNullOrWhiteSpace(nameFilterTextBox.Text))
                {
                    currentProducts = currentProducts.Where(x =>
                        x.Name.ToLower().Contains(nameFilterTextBox.Text.ToLower())).ToList();
                }
                if (categoryComboBox.SelectedItem is Category selectedCategory)
                {
                    currentProducts = currentProducts.Where(x => x.CategoryID == selectedCategory.CategoryID).ToList();
                }

                ProductsGrid.ItemsSource = currentProducts;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске товаров: {ex.Message}");
            }
        }

        
    }
}
