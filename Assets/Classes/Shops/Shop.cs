using UnityEngine;
using System.Collections.Generic;

public class Shop : MonoBehaviour {

	protected string shopName;
	//text displayed when the player accesses the store
	protected string greetingText;





	public string toString(){
		return shopName;
	}

	public string getGreeting(){
		return greetingText;
	}


}
