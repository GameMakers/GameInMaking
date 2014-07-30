using UnityEngine;
using System.Collections;

public class Ability : MonoBehaviour {

	protected string abilityType;

	protected int abilityLevel;
	
	
	
	public string toString(){
		return abilityType;
	}

	public int getAbilityLevel(){
		return abilityLevel;
	}
}
