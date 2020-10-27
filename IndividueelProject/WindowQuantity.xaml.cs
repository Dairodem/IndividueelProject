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
using System.Windows.Shapes;

namespace IndividueelProject
{
    /// <summary>
    /// Interaction logic for WindowQuantity.xaml
    /// </summary>
    public partial class WindowQuantity : Window
    {
        public int Quantity { get; set; }
        private int quantity = 0;
        public WindowQuantity()
        {
            InitializeComponent();

            quantity = 0;
            txtQuantity.Text = quantity.ToString();
        }
        private void AddQuantity(int number)
        {
            quantity += number;
            txtQuantity.Text = quantity.ToString();
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            AddQuantity(1);
        }

        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            AddQuantity(5);
        }


        private void btn10_Click(object sender, RoutedEventArgs e)
        {
            AddQuantity(10);
        }

        private void btn50_Click(object sender, RoutedEventArgs e)
        {
            AddQuantity(50);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Quantity = quantity;
            DialogResult = true;
        }
    }
}
