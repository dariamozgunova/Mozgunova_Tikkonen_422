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
    /// Логика взаимодействия для CalcPage.xaml
    /// </summary>
    public partial class CalcPage : Page
    {
        private string current = "";
        private string previous = "";
        private string op = "";
        private bool resultShown = false;

        public CalcPage()
        {
            InitializeComponent();
        }

        private void Number_Click(object sender, RoutedEventArgs e)
        {
            string val = (string)((Button)sender).Content;

            if (resultShown)
            {
                current = "";
                resultShown = false;
            }

            if (val == "." && current.Contains(".")) return;
            current += val;
            Display.Text = current;
        }

        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            if (current == "") return;

            if (previous != "" && op != "")
                Calculate();

            op = (string)((Button)sender).Content;
            previous = current;
            current = "";
        }

        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            if (previous == "" || current == "" || op == "") return;
            Calculate();
            op = "";
        }

        private void Calculate()
        {
            double a, b;
            if (!double.TryParse(previous, out a) || !double.TryParse(current, out b))
            {
                Display.Text = "Ошибка";
                current = previous = op = "";
                return;
            }

            double res = 0;

            switch (op)
            {
                case "+": res = a + b; break;
                case "−": res = a - b; break;
                case "×": res = a * b; break;
                case "÷":
                    if (b == 0)
                    {
                        Display.Text = "Ошибка!";
                        current = previous = op = "";
                        return;
                    }
                    res = a / b;
                    break;
            }

            Display.Text = res.ToString();
            current = res.ToString();
            previous = "";
            resultShown = true;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            current = "";
            previous = "";
            op = "";
            Display.Text = "0";
        }

        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            if (current.Length > 0)
            {
                current = current.Substring(0, current.Length - 1);
                Display.Text = current == "" ? "0" : current;
            }
        }

        private void Negate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(current)) return;

            if (current.StartsWith("-"))
                current = current.Substring(1);
            else
                current = "-" + current;

            Display.Text = current;
        }
    }
}
