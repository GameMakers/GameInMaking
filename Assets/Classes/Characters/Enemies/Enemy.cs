using UnityEngine;
using System.Collections.Generic;

public class Enemy : Character {

	//exp gained when the enemy is killed
	protected int expValue;
	//money gained when the enemy is killed
	protected int baseMoneyValue;
	//percentage chance that the enemy will drop an item at all
	protected int dropPercent;
	//map of items as keys and values representing the percentage chance of the item being selected as a drop
	protected Dictionary<Item, int> itemDrops;

	protected List<Attack> attackTypes;

	protected bool canFly;
	protected bool canSwim;
	protected bool canClimb;
	//the enemy can see through walls
	protected bool hasXray;
	//from how far away the enemy can see a player
	protected int visionDistance;
	//from how far away the enemy can hear a player
	protected int hearingDistance;

	//inital location - UPDATE THIS TO A BETTER STRUCTURE	
	protected int x;
	protected int y;
	//how far from their initial location the enemy may venture
	protected int movementRadius;
	//overall behaviour of the enemy
	protected AI inelligenceType;
	//how likely the enemy is to engage in combat
	protected int baseAggro;
	//passive, defensive, patrolling, aggressive, etc
	protected int currentAction;
	protected PlayableCharacter target;






	public int getExp(){
		return expValue;
	}

	public int getMoney(){
		//USE RANDOMIZATION OVER SOME RANGE TO VARY THE AMOUNT OF MONEY DROPPED
		return baseMoneyValue;
	}

	public Item dropItem(){
		//APPLY DROP PERCENTAGE TO CHECK IF AN ITEM WILL BE DROPPED
			//IF AN ITEM IS TO BE DROPPED, SELECT ONE FROM THE LIST OF DROPS BASED ON THEIR PERCENTAGES
		return null;
	}

	public List<Attack> getAttackTypes(){
		return attackTypes;
	}


	//CREATE ENEMY BEHAVIOUR FUNCTIONALITY BASED ON THE RELEVANT ATTRIBUTES
	
}
