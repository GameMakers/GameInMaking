using UnityEngine;
using System.Collections.Generic;

public class Upgrade : MonoBehaviour {

	protected string upgradeType;

	//base amounts of materials needed to make the upgrade
	protected Dictionary<Material, int> baseMaterials;
	//full items required in addition to the materials to make the upgrade
	protected Dictionary<Item, int> requiredItems;
	//upgrades which must be made prior to this one
	protected List<Upgrade> prerequisites;




	public string toString(){
		return upgradeType;
	}

	public Dictionary<Material, int> getBaseMaterials(){
		return baseMaterials;
	}

	public Dictionary<Item, int> getRequiredItems(){
		return requiredItems;
	}

	public List<Upgrade> getPrerequisities(){
		return prerequisites;
	}

}
