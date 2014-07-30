using UnityEngine;
using System.Collections.Generic;

public class Character : MonoBehaviour {

	protected string characterName;

	protected int level;
	protected int totalHealth;
	protected int currentHealth;
	protected int power;
	protected int accuracy;
	protected int defense;
	protected int agility;

	protected Dictionary<StatusCondition, int> conditionResistances;

	//conditions currently being suffered (bleeding, poision, etc)
	protected List<StatusCondition> statusConditions;
	
	
	
	
	public string toString(){
		return characterName;
	}

	public int getLevel(){
		return level;
	}

	public int getTotalHealth(){
		return totalHealth;
	}

	public int getCurrentHealth(){
		return currentHealth;
	}

	public int getPower(){
		return power;
	}

	public int getAccuracy(){
		return accuracy;
	}

	public int getDefense(){
		return defense;
	}

	public int getAgility(){
		return agility;
	}

	public Dictionary<StatusCondition, int> getConditionResistances(){
		return conditionResistances;
	}




	public void takeDamage(int baseDmg){
		//PREFORM DAMAGE CALCULATION
	}

	public void applyConditionDamages(){
		//CALCULATE DAMAGE TAKEN FROM CONDITIONS BASED ON RESISTANCES
	}

	public void levelUp(){
		level++;
	}

}
