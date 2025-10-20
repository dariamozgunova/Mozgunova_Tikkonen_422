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
    /// Логика взаимодействия для StatisticsPage.xaml
    /// </summary>
    public partial class StatisticsPage : Page
    {
        private Mozgunova_Tikkonen_422Entities context;

        public StatisticsPage()
        {
            InitializeComponent();
            context = new Mozgunova_Tikkonen_422Entities();
            LoadStatistics();
        }

        public class CategoryStatistics
        {
            public string CategoryName { get; set; }
            public int ProductCount { get; set; }
            public decimal AveragePrice { get; set; }
            public decimal TotalValue { get; set; }
            public decimal MinPrice { get; set; }
            public decimal MaxPrice { get; set; }
        }

        private void LoadStatistics()
        {
            try
            {
                var products = context.Products.Include("Category").ToList();

                var totalProducts = products.Sum(p => p.Quantity);
                var totalValue = products.Sum(p => p.Price * p.Quantity);
                var averagePrice = products.Any() ? products.Average(p => p.Price) : 0;
                var totalCategories = context.Categories.Count();

                txtTotalValue.Text = $"{totalValue:N2} руб";
                txtAveragePrice.Text = $"{averagePrice:N2} руб";

                var categoryStats = (from p in products
                                     group p by p.Category into g
                                     where g.Key != null
                                     select new CategoryStatistics
                                     {
                                         CategoryName = g.Key.CategoryName,
                                         ProductCount = g.Sum(x => x.Quantity),
                                         AveragePrice = g.Average(x => x.Price),
                                         TotalValue = g.Sum(x => x.Price * x.Quantity),
                                         MinPrice = g.Min(x => x.Price),
                                         MaxPrice = g.Max(x => x.Price)
                                     }).ToList();

                categoryStatsGrid.ItemsSource = categoryStats;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки статистики: {ex.Message}");
            }
        }
    }
}
