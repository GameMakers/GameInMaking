using UnityEngine;
using System.Collections.Generic;

public class Merchant : Shop {

	protected Dictionary<Item, int> itemsForSale;




	public Dictionary<Item, int> getItems(){
		return itemsForSale;
	}
	
	public void sellItem(int itemIndex){
		//SELL THE ITEM AT THE SPECIFIED INDEX TO THE CHARACTER
	}
	
	public void purchaseItem(Item item){
		//PURCHASE AN ITEM FROM A CHARACTER
	}

}
