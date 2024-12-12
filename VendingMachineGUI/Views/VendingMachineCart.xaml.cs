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
using VendingMachineGUI.Models;

namespace VendingMachineGUI.Views
{
    /// <summary>
    /// Interaction logic for VendingMachineCart.xaml
    /// </summary>
    public partial class VendingMachineCart : Window
    {
        private MainWindow previousView;
        private int _currentRow;

        public int CurrentRow
        {
            get { return _currentRow; }
            set { _currentRow = value; }
        }

        // Used to get variables and other information from the main window and update it accordingly.
        public MainWindow PreviousView
        {
            get { return previousView; }
            set { previousView = value; }
        }

        // Used just to get previous data.
        public VendingMachineCart(List<Stock> storeditems, MainWindow previousView)
        {
            InitializeComponent();
            UpdateShoppingCart(storeditems);
            PreviousView = previousView;
            this.DataContext = storeditems;
        }

        // Updates the shopping cart according to the items that the user has added.
        public void UpdateShoppingCart(List<Stock> storeditems)
        {
            string myItem = "";
            string myPrice = "";
            string myQuantity = "";
            double myTotal = 0;
            double mySubtotal = 0;
            double myBigTotal =0;
            double myTax = 0;
            
            for(int i = 0; i < storeditems.Count; i++)
            {
                myItem = $"\n{storeditems[i].Name}";
                item.Text += myItem;
                myPrice = $"\n${storeditems[i].Price}";
                price.Text += myPrice;
                myQuantity = $"\n{storeditems[i].Quantity}";
                quantity.Text += myQuantity;
                myTotal = storeditems[i].Price * storeditems[i].Quantity;
                total.Text += $"\n{myTotal.ToString("C2")}";
                mySubtotal += myTotal;

                subtotal.Text = $"{mySubtotal.ToString("C2")}";
                myTax = 0.15 * mySubtotal;
                taxes.Text = $"{myTax.ToString("C2")}";
                myBigTotal = myTax + mySubtotal; 
                bigTotal.Text = $"{myBigTotal.ToString("C2")}";
            }
        }

        // Allows the user to switch back and forth between the main window and the cart view.
        private void BtnVendingMachine(object sender, RoutedEventArgs e)
        {
            this.Close();
            PreviousView.Visibility = Visibility.Visible;
        }

        // Resets the cart with the original values without any items or prices.
        public void BtnResetCart(object sender, RoutedEventArgs e)
        {
            item.Text = "Item";
            price.Text = "Price";
            quantity.Text = "Quantity";
            total.Text = "Total";
            subtotal.Text = "$0";
            bigTotal.Text = "$0";
            taxes.Text = "$0";
            previousView.txtBlkTotalAmount.Text = "$0.00";
            previousView.txtCartItemNumber.Text = "0";
            previousView.txtInsertedAmount.Text = "0";
            previousView.ReceivedMoney=0;
            MainWindow.TotalCost = 0;
            MainWindow.ItemsInCart = 0;
            MainWindow.ResetStock();
        }
     

    }
}
