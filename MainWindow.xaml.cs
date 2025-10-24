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

namespace Mozgunova_Tikkonen_422
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // По умолчанию открываем модуль товаров
            MainFrame.Navigate(new Pages.ProductsPage());
        }

        private void BtnProducts_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Pages.ProductsPage());
        }

        private void BtnStatistics_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Модуль статистики будет реализован Разработчиком 2");
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Модуль поиска будет реализован Разработчиком 2");
        }

        private void BtnCalc_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Pages.CalcPage());
        }
    }
}
