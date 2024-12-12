using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachineGUI.Models
{
    
    
    public class Stock : INotifyPropertyChanged //Class signs contrat with an interface to notify and refresh when changes are made to quantities.(binding purposes)
    {
        #region INSTANCE_FIELDS
        private string _name; // storing name of my items for displaying purposes on receipt or shoping cart
        private int _quantity;//saving quatity of my products
        private double _price;//storing price of items. helpful for my calculated property, GetTotal
        private int _startingValue;// used for reset purposes. it is saving the old starting value of my quantities so when the user reset everything, quantities are reset to what they were before
        #endregion

        #region DEFAULT_CONSTRUCTOR
        public Stock()//default constructor
        {
            
        }
        #endregion

        #region MEMBER_PROPERTIES
        public int StartingValue // I added this property to track the starting quantity of my item. if user reset. quantities are reset properly to their starting value
        {
            get { return _startingValue; }
            set {
                if (value < 0)
                {   
                    throw new ArgumentOutOfRangeException("Starting quantity cannot be less than 0");
                }
                _startingValue = value; 
            }
        }
        public string Name { 

            get { return _name; }
            set { 
                //validation added for name since name cannot be empty or be null
                if(value == null || value == string.Empty) 
                    throw new ArgumentNullException("The name of an item cannot be empty or null"); //throws exception

                _name = value; // if value meets the criteria. we assign it to our backing field
            }

        }

        public int Quantity { 
            get { return _quantity;} 
            set {
                if (value < 0)// validation added: the quantities of my vending machine cannot be less than 0 because of real world examples and logic, we cannot have a negative quantity
                {
                    throw new ArgumentException("The quantity cannot be less than 0");
                }
                _quantity = value;
                OnPropertyChanged();// notifies that my property value change so it is time to refresh it. (binding purposes)
            }
        }
        public double Price
        {
            get { return _price;} //returns price when called
            set { 
                if(value <= 0)// validation price cannot be 0 or negative for logical uderstanding. In a vending machine there is not free items. Or negative prices
                {
                    throw new ArgumentException("Price cannot be negative or 0");//throws exception
                }
                _price = value; 
            }
        }
        // calculated property to get the total price of a same item bought multiple times
        public double GetTotal
        {
            get { return Price * Quantity; } // returns a double
        }
        #endregion

        #region MEMBER_METHODS
        public override string ToString()//overriden to string method from parent class object
        {
            return $"{Quantity} {Name} ... ${GetTotal}\n";//I want to display exactly this when ToString is called. It will return an string. 

        }
        #endregion

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged; //reference:https://stackoverflow.com/questions/50247182/gui-not-updating-if-property-of-other-class-object-changed
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
