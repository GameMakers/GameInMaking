using UnityEngine;
using System.Collections.Generic;

public class Smith : Shop {

	protected List<Item> craftableItems;
	protected List<Item> salvageableItems;


	public List<Item> getCraftableItems(){
		return craftableItems;
	}

	public List<Item> getSalvageableItems(){
		return salvageableItems;
	}
}
