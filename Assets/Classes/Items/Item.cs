using UnityEngine;
using System.Collections.Generic;

public class Item : MonoBehaviour {

	protected string itemType;

	//materials needed to craft the item
	protected Dictionary<Material, int> craftingMaterials;
	//base materials obtained from salvaging the item
	protected Dictionary<Material, int> salvageMaterials;
	//upgrades which can be made to the item and multipliers for the material costs
	protected Dictionary<Upgrade, double> availableUpgrades;



	public string toString(){
		return itemType;
	}

	public Dictionary<Material, int> getCraftingMaterials(){
		return craftingMaterials;
	}
	
	public Dictionary<Material, int> getSalvageMaterials(){
		return salvageMaterials;
	}

	public Dictionary<Upgrade, double> getAvailableUpgrades(){
		return availableUpgrades;
	}

}
