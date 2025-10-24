using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        private Mozgunova_Tikkonen_422Entities context;

        public ProductsPage()
        {
            InitializeComponent();
            context = new Mozgunova_Tikkonen_422Entities();
            LoadCategories();
            LoadProducts();
        }

        private void LoadCategories()
        {
            try
            {
                cmbCategory.ItemsSource = context.Category.ToList();
                cmbCategory.DisplayMemberPath = "CategoryName";
                cmbCategory.SelectedValuePath = "CategoryID";
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}");
            }
        }

        private void LoadProducts()
        {
            try
            {
                var products = context.Product
                    .Include("Category")
                    .ToList();

                productsGrid.ItemsSource = products;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}");
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cmbCategory.SelectedValue == null)
            {
                MessageBox.Show("Выберите категорию");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название товара");
                return;
            }

            try
            {
                var newProduct = new Product
                {
                    Name = txtName.Text,
                    CategoryID = (int)cmbCategory.SelectedValue,
                    Price = decimal.Parse(txtPrice.Text),
                    Quantity = int.Parse(txtQuantity.Text)
                };

                context.Product.Add(newProduct);
                context.SaveChanges();

                LoadProducts();
                ClearForm();
                MessageBox.Show("Товар успешно добавлен");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка добавления: {ex.Message}");
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (productsGrid.SelectedItem is Product selectedProduct)
            {
                if (cmbCategory.SelectedValue == null)
                {
                    MessageBox.Show("Выберите категорию");
                    return;
                }

                try
                {
                    var productToUpdate = context.Product.Find(selectedProduct.ProductID);

                    if (productToUpdate != null)
                    {
                        productToUpdate.Name = txtName.Text;
                        productToUpdate.CategoryID = (int)cmbCategory.SelectedValue;
                        productToUpdate.Price = decimal.Parse(txtPrice.Text);
                        productToUpdate.Quantity = int.Parse(txtQuantity.Text);

                        context.SaveChanges();
                        LoadProducts();
                        ClearForm();
                        MessageBox.Show("Товар успешно обновлен");
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Ошибка обновления: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для редактирования");
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (productsGrid.SelectedItem is Product selectedProduct)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить этот товар?",
                                           "Подтверждение удаления",
                                           MessageBoxButton.YesNo,
                                           MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var productToDelete = context.Product.Find(selectedProduct.ProductID);
                        if (productToDelete != null)
                        {
                            context.Product.Remove(productToDelete);
                            context.SaveChanges();

                            LoadProducts();
                            ClearForm();
                            MessageBox.Show("Товар успешно удален");
                        }
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show($"Ошибка удаления: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для удаления");
            }
        }

        private void ProductsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (productsGrid.SelectedItem is Product selectedProduct)
            {
                txtName.Text = selectedProduct.Name;
                cmbCategory.SelectedValue = selectedProduct.CategoryID;
                txtPrice.Text = selectedProduct.Price.ToString();
                txtQuantity.Text = selectedProduct.Quantity.ToString();
            }
        }

        private void ClearForm()
        {
            txtName.Text = "";
            txtPrice.Text = "0";
            txtQuantity.Text = "0";
            cmbCategory.SelectedIndex = -1;
            productsGrid.SelectedItem = null;
        }
    }
}
