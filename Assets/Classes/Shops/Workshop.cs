using UnityEngine;
using System.Collections.Generic;

public class Workshop : Shop {

	protected List<Item> upgradableItems;
	
	
	public List<Item> getUpgradableItems(){
		return upgradableItems;
	}

}
