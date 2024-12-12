using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using VendingMachineGUI.Models;
using VendingMachineGUI.Views;

namespace VendingMachineGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region CONSTANTS
        private const double MIN_AMOUNT_FOR_PAYING_CARD = 5;
        private const double BILL = 5;// constant variable with purposes of changing 
        private const double COIN = 1;
        const string FILEPATH = "./../../../../stock.txt"; //File path to acces the csv file in the .exe directory with information of our stock
        #endregion

        #region CLASS_BACKINGFIELDS
        // myStock list is a static list that will just saved the values from the csv file and display them in main window(binding purposes)
        private static List<Stock> myStock = new List<Stock>();
        // I created a list of buttons to store buttons i have used, so if the user decide to reset and the button is disable, the program can make it active again with my static method ResetStock()
        private static List<Button> buttons = new List<Button>();
        //I created a list of storedItems to store items the user choose and pass them to cart window or receipt
        private static List<Stock> storedItems = new List<Stock>();
        
        private static double _totalCost;
        private static bool _isCash;//helpful to know if user is paying cash or card
        private static int _itemsInCart;// shows count of items user bought
        #endregion

        #region INSTACE_BACKINGFIELDS
        private double _receivedMoney;//cash user puts in the machine
        private double[] bills = { 100, 50, 20, 10, 5, 2, 1, 0.25, 0.10 };//array used for breakdown
        #endregion


        #region CLASS_PROPERTIES
        public static int ItemsInCart
        {
            get { return _itemsInCart; }
            set {
                if (value < 0)//items in cart cannot be less than 0
                    throw new ArgumentException("Items in cart cannot be negative");//throw exception
                _itemsInCart = value; }
        }

        public static double TotalCost
        {
            get { return _totalCost; }
            set {
                if(value < 0)//total cost cannot be less than 0
                    throw new ArgumentException("Total cost cannot be negative");//throw exception
                _totalCost = value; }
        }

        public static bool IsCash
        {
            get { return _isCash; }
            set { _isCash = value; }
        }
        #endregion

        #region INSTANCES_PROPERTIES
        public double ReceivedMoney
        {
            get { return _receivedMoney; }
            set {
                if (value < 0)//received money cannot be less than 0
                    throw new ArgumentException("Received monay cannot be negative");//throw exception

                _receivedMoney = value; }
        }
        #endregion

        #region DEFAULT_CONSTRUCTOR
        public MainWindow()//default constructor. Every time and instance of MainWindow is created, the default values will be assigned
        {
            ReadFromFile(FILEPATH); // method to read the csv file
            InitializeComponent();//initialize components
            //all these values should be set to 0 when stating the app or when a new transaction has started
            ItemsInCart = 0;
            TotalCost = 0;
            ReceivedMoney = 0;
            IsCash = false;// should be set to false always at the beginning of transaction
            this.DataContext = myStock; // binding
        }
        #endregion

        #region CLASS_METHODS
        public static string RemoveSpecialCharacters(string str)//this method remove special characters
        {
            StringBuilder stringBuilder = new StringBuilder();

            //Method to remove any special characters from the passed parameter using char numbers to check every character.
            foreach (char c in str)
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                    stringBuilder.Append(c);
            return stringBuilder.ToString();
        }


        
        /// <summary>
        /// Reset stock is only called from VendingMachineCart when the user wants to reset everything
        /// Decide to create a static method in main window so when user hits the exit button it will reset everything in cart
        /// and then called the static method having access to my static backing fields or properties and also reset them
        /// </summary>
        public static void ResetStock()// static void method
        {
            for (int i = 0; i < myStock.Count; i++)//loop throug stock quantity list and resets them to old value they had before user adding to cart, binding will update this to the user main window view
                myStock[i].Quantity = myStock[i].StartingValue;

            for (int i = 0; i < buttons.Count; i++)//if some buttons are disable they will get active again in this loop
                buttons[i].IsEnabled = true;

            storedItems.Clear();// clear stored items so it also reset list and add elements from the beginning
            buttons.Clear(); // removing record of buttons clicked
        }
        #endregion

        #region INSTANCE_METHODS
        public void ReadFromFile(string fileName)
        {
            if (File.Exists(fileName))// verify if file exist
            {
                StreamReader streamR = null;// create stream reader to read files, stay null if file does not oppen
                string line;

                try//io prone to excetption, so best practice is a try catch.
                {
                    streamR = new StreamReader(fileName);// open file
                    while ((line = streamR.ReadLine()) != null) // start reading in a loop while not null
                    {
                        string[] data = line.Split(',');// split data in an array of strings
                        Stock stock = new Stock();// create instance of stock to store data on each field
                        stock.Name = data[0];// parse to string to int
                        stock.Quantity = int.Parse(data[1]);
                        stock.Price = double.Parse(data[2]);
                        stock.StartingValue = stock.Quantity;

                        myStock.Add(stock);// we add to list
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error reading file: " + e.Message);
                }
                finally
                {
                    if (streamR is not null) // != null
                        streamR.Close(); // closing stream reader
                }
            }
            else
                throw new IOException("The file does not exist");// throw exception if file does not exist
        }

        public void UpdateStock(object sender) // method that updates my quantity values. 
        {
            Button buttonClicked = sender as Button;
            myStock[Convert.ToInt32(buttonClicked.Uid)].Quantity -= 1;//this will get the exact number of the button where the user click and change that element in the list

        }

        public void CheckStock(object sender) // CheckStock checks if quantity is 0. if it is button is disable
        {
            Button buttonClicked = sender as Button;
            if (myStock[Convert.ToInt32(buttonClicked.Uid)].Quantity == 1)
            {
                buttonClicked.IsEnabled = false;
                MessageBox.Show($"{myStock[Convert.ToInt32(buttonClicked.Uid)].Name} is out of stock now.", "Vending Machine", MessageBoxButton.OK);
            }
                

            buttons.Add(buttonClicked);//add button to our record. reset purposes
        }

        private void BtnAddToCart(object sender, RoutedEventArgs e)
        {
            Button buttonClicked = sender as Button;
            try
            {
                if (myStock[Convert.ToInt32(buttonClicked.Uid)].Quantity != 0)
                {
                    ItemsInCart++;
                    // Get the money from the user when they select one of the products.
                    CheckStock(sender);
                    UpdateStock(sender);

                    string recievedMoney = Convert.ToString(buttonClicked.DataContext);
                    double money = Convert.ToDouble(RemoveSpecialCharacters(recievedMoney));

                    // Adds all the items to a field called total cost, then displays the total on the main view.
                    TotalCost += money;
                    txtBlkTotalAmount.Text = "$" + TotalCost.ToString("#.##");
                    txtCartItemNumber.Text = Convert.ToString(ItemsInCart);

                    // Creates new items to add to list from selected products added by user.
                    string name = myStock[Convert.ToInt32(buttonClicked.Uid)].Name;
                    bool addItem = true;//useful to see if item has been added

                    //logic for adding items in list stored items. is item has alredy been quanty of that item will increment by 1(search algorirthm, linear)
                    for (int i = 0; i < storedItems.Count; i++)
                    {
                        if (storedItems[i].Name == name)
                        {
                            storedItems[i].Quantity += 1;
                            addItem = false;
                        }
                    }
                    if (addItem)//if item is not on stored list, then it will be added with a starting qunatity of 1
                    {
                        Stock item = new Stock();
                        item.Name = name;
                        item.Price = money;
                        item.Quantity = 1;
                        storedItems.Add(item);//add stock object to list
                    }
                }
                else
                {
                    throw new ArgumentException("Sorry!! We are out of stock.");
                }
            }
            catch(ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Vending Machine", MessageBoxButton.OK, MessageBoxImage.Error);
                buttonClicked.IsEnabled = false;
            }
          
        }

        //this method is for switching to shopping cart
        private void BtnShoppingCart(object sender, RoutedEventArgs e)
        {
            // Opens up the shopping cart menu for the user
            //cartOpen = true;
            VendingMachineCart vendingMachineCart = new VendingMachineCart(storedItems, this);// creates instance of cart
            //Cart = vendingMachineCart;
            vendingMachineCart.Show();
            this.Hide();//hide this window
        }

        // method to pay with cash
        private void BtnInsertMoney(object sender, RoutedEventArgs e)
        {
            // Get recieved money from user when they click on one of the five bill options.
            if (TotalCost > 0)
            {
                Button buttonClicked = sender as Button;
                string recievedMoney = Convert.ToString(buttonClicked.Content);
                double money = Convert.ToDouble(RemoveSpecialCharacters(recievedMoney));

                ReceivedMoney += money;
                txtInsertedAmount.Text = Convert.ToString(ReceivedMoney);

                if (TotalCost <= ReceivedMoney)//if cash is not enought yet i wont pass the transaction. app will wait until user decides to add another bill
                {
                    IsCash = true;//set is cash to true so when doing receipt we know if we want to do a breakdown or no
                    MakingReceipt();//method that makes the receipt
                    storedItems.Clear();//clear stored items when transaction is done
                    myStock.Clear();// clear also the list that display our products
                    this.Close();// close this window since receipt will be open afterwards
                }
            }
            else
            {
                MessageBox.Show("You have not add any item to your shopping cart.", "Vending Machine", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           

        }

       

        private void BtnPayWithCard(object sender, RoutedEventArgs e)// this method will add the functionality to the button pay with card
        {
            try
            {
                if (TotalCost < MIN_AMOUNT_FOR_PAYING_CARD)
                {
                    throw new ArgumentException("Sorry you can only pay with card if the total amount of your purchase is greater than $5.");
                }
                else
                {
                    MakingReceipt();//making receipt
                                    //clear list
                    storedItems.Clear();
                    myStock.Clear();
                    this.Close();// close this window since receipt will be open afterwards
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Vending Machine", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
           
        }
        private void MakingReceipt()
        {

            StringBuilder items = new StringBuilder();// declare string builder to appen what the user will see in receipt
            for (int i = 0; i < storedItems.Count; i++)
            {
                items.Append(storedItems[i].ToString());
            }
            string breakdown = "";
            if (IsCash)//if we paid with cash a breakdown will be shown and change
            {
                items.Append($"\nPayment type: Cash\n\nTotal Cost: {(TotalCost).ToString("C2")}\nAmount recieved: {ReceivedMoney.ToString("C2")}\nChange: {(ReceivedMoney - TotalCost).ToString("C2")}");
                breakdown = BreakDown();

                items.Append("\n\nThanks for using our lightweight vending machine");
                //MessageBox.Show(message.ToString(), "Vending Machine Receipt", MessageBoxButton.OK);
            }
            else
            {
                items.Append($"Payment type: Credit/Debit\n\nTaxes: {(0.15 * TotalCost).ToString("C2")}\n\nTotal Cost: {(TotalCost + 0.15 * TotalCost).ToString("C2")}\n\nThanks for using our lightweight vending machine");
                //MessageBox.Show(message.ToString(), "Vending Machine Receipt", MessageBoxButton.OK);
            }
            WriteToFile();
            VendingMachineReceipt vendingMachineReceipt = new VendingMachineReceipt(items.ToString(), breakdown);

            vendingMachineReceipt.Show();

        }
        private string BreakDown()// method does the breakdown
        {
            double change = ReceivedMoney - TotalCost;//calculating change
            StringBuilder sb = new StringBuilder();//string builder to create the breakdown
            sb.Append("Breakdown");

            int count = 0;
            for (int i = 0; i < bills.Length; i++)//nested loop for calculating breakdown
            {
                bool ok = true;
                while (ok)
                {
                    if (bills[i] <= change)
                    {
                        change -= bills[i];
                        count++;
                    }
                    else
                    {
                        ok = false;
                    }
                }
                if (count != 0)
                {
                    //system for chosing change money type.
                    if (bills[i] >= BILL)
                    {
                        sb.Append($"\n{count} {bills[i].ToString("C2")} bill(s)");
                    }
                    else if (bills[i] >= COIN && bills[i] <= BILL)
                    {
                        sb.Append($"\n{count} {bills[i].ToString("C2")} coin(s)");
                    }
                    else
                    {
                        sb.Append($"\n{count} {bills[i].ToString("C2")} cent(s)");
                    }

                }

                count = 0;
            }
            return sb.ToString();
        }
        private void WriteToFile()//method write to file
        {
            try
            {
                File.Delete(FILEPATH);//deleting file before it writes new quantities and other infos
                StringBuilder sb = new StringBuilder();

                foreach (Stock product in myStock)
                    sb.AppendLine($"{product.Name},{product.Quantity},{product.Price}");//apending new quantities and other infos to string builder


                File.WriteAllText(FILEPATH, sb.ToString());//creates file in the file path given and then write string inside

                sb.Clear();// just in case but in this case useless

            }
            catch (Exception ex)
            {

                throw new Exception("Error saving data to file." + ex.Message);
            }
        }
        #endregion

    }
}
