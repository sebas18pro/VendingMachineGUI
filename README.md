# Vending machine 💪
## Description:
Our project is a vending machine app called lightweight. The name comes from a famous quote of a bodybuilder who would say every time he lifts heavy weight saying demonstrating is not heavy or difficult. Our lightweight vending machine has the same mentality. Nothing is difficult for it. It has features like a reset button, shopping card, pay with bills, pay with debit or with credit card, add different products to the car, etc. The reset button is a feature which will reset everything to 0 to in the car. Let’s say our customer adds products to the cart and finds out when he goes to the shopping cart that it is too expensive. He can press the reset button and it will reset everything in a blink.  Now he can start shopping from scratch again.
Development Approach:
1.	Understanding the Problem:
-	We have a stock of items that we need to display for users in a vending machine app. We expect the user to be able to select any product, decreasing the stock and allowing them to purchase by card or cash (Showing change on cash).
2.	Formulating the Problem:
-	The stocks for each item will be stored in a text file, with the whole numbers ranging between 0 and whatever is available. When users purchase an item, this decreases the stock by the amount selected. Users will receive a receipt of their items, showing a string of their selected items and the total cost all added up and any change if needed.
3.	Developing the Algorithm:
-	Allow users to buy items from a vending machine application creating quantities for each item. Add allow users to select the different items and purchasing options. Display a counter for each item to show the current available stock. When users purchase an item decrease the total stock and increase the total price. Add different options for inserting cash amounts. When the user is finished, show the receipt and any change or tax.
4.	Implementing the Algorithm:
-	Using C# and the WPF application. Add buttons to allow the user to select the different items. If an item is selected, decrease the stock value within the text file, and increase the total price based on the value of the item. Add more buttons to allow the user to insert different values of bills using buttons with different values. When the received amount is greater than the total amount, display the receipt and show the selected items and change for the user, or show the tax if a card was used to pay. 
5.	Testing:
-	Testing was used by adding and removing large quantities of items at the same time. Increasing stocks to extremely high amounts, having negative quantities. Trying to add money before any purchase has been made.
OOP Design:
The app only requires one class that we needed to create, this was the Stock class, which includes the name, quantity, price and startingValue (first quantity read before adding items to cart). Properties: Name, Quantity, Price, StartingValue,GetTotalPrice which will get the total for each same item add to your shopping cart. A To string method was overwritten for displaying all important information of the item add to the cart.
 
WPF(since there is an important relationship): main window class make use of composition to contain a list of stock object. An error message is displayed when the user tries to buy an item that is already at 0. Or when he tries to pay with the card and the total amount is less than 5$.





 
 
## Contributions:
|Liam Section	                                           |Sebastian Section
|-----------------------------------------------------------|-------------------
|Created VendingMachine App.  	                           |Changed images to relative paths. 
|Created basic outline of items.                       	   |Stock class created. 
|Completed first display.  	                               |Data folder created and stock report. 
|Added a view for the cart.  	                           |Reading from file implemented. 
|Added price to GUI.  	                                   |Binding between GUI and stock of products from file. 
|Simplified code.  	                                       |BtnAddToCart is disabled if no more stock left. 
|Testing out methods.  	                                   |Created reset button in cart. 
|Created BtnInsertMoney method and created BtnAddToCart.   |Shopping cart code implemented and completed. 
|Added BtnShoppingCart.  	                               |ToString method override for receipt. 
|Fixed payment with credit card.                           |Showing receipt break down. 
|Switched button functionality with images. 	           |Saving quantities to file.(Writing from file) 
|Added an insert cash text block.                          |Fixed all possible bugs or error could occur with the codeHandling exceptions 
|Removed spaces, edited insert money. 




## Future Work
Future work may include adding the coupon section into the vending machine or greying out the images once their stock has been reduced to zero. Advanced future work can also include having a dynamic vending machine that cycles through different products daily. Also getting to sync the bill with the shopping cart correctly. Adding bill system for user that allows user to insert a code and see the bills they have stored in the machine.

 
 
	

