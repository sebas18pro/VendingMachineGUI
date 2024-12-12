using System.Windows;

namespace VendingMachineGUI.Views
{
    /// <summary>
    /// vending machine receipt it's a class for showing the receipt to the user
    /// </summary>
    public partial class VendingMachineReceipt : Window
    {
        #region CONSTRUCTOR
        public VendingMachineReceipt(string receipt, string breakdown)
        {
            InitializeComponent();
            WriteReceipt(receipt, breakdown);//passing string values from previous window

        }
        #endregion

        #region METHODS
        //Method that writes receipt text in the receipt window
        private void WriteReceipt(string receipt, string breakdown)
        {
            items.Text = receipt;// write string value from previous window in receipt window
            extra.Text = breakdown; // write string value from previous window in receipt window
        }
        //Method that add functionality to my exit button in the receipt window
        private void BtnExitReceipt_Click(object sender, RoutedEventArgs e)
        {
            // When exit from receipt, a new main window is open, and user from
            MainWindow newWindow = new MainWindow();
            newWindow.Show();//opening new main window
            this.Close();//closing this window
        }
        #endregion
    }
}
