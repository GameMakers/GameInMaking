using UnityEngine;
using System.Collections.Generic;

public class Attack : Ability {
	
	protected int baseDmg;
	protected int range;
	protected int angle;
	protected int width;
	protected int height;
	protected int speed;
	protected int armorPenetration;
	//how much impact the attack has on environment destruction
	protected int enironmentDestructability;
	//from how far away the attack can be heard
	protected int attackSoundDistance;
	//from how far away the impact can be heard
	protected int impactSoundDistance;

	//status conditions which the attack can inflict and percentage chance of infliction
	protected Dictionary<StatusCondition, int> conditions;

	//whether the attack can pass through walls
	protected bool isRail = false;
	//whether the attack will arc or travel in a straight line (if it is a projectile)
	protected bool hasArc = false;
	//whether the attack will detonate on impact
	protected bool hasImpactDetonation = false;
	//whether the attack will detonate after a period of time
	protected bool hasTimedDetonation = false;
	protected int detonationRadius = 0;
	protected int detonationDamage = 0;

	protected string animationType;


	


	public int getDmg(){
		return baseDmg;
	}

	public int getRange(){
		return range;
	}

	public int getAngle(){
		return angle;
	}

	public int getWidth(){
		return width;
	}

	public int getHeight(){
		return height;
	}

	public int getSpeed(){
		return speed;
	}

	public int getArmorPenetration(){
		return armorPenetration;
	}

	public int getEnironmentDestructability(){
		return enironmentDestructability;
	}

	public int getAttackSoundDistance(){
		return attackSoundDistance;
	}

	public int getImpactSoundDistance(){
		return impactSoundDistance;
	}

	public Dictionary<StatusCondition, int> getConditions(){
		return conditions;
	}

	public bool attackIsRail(){
		return isRail;
	}

	public bool attackHasArc(){
		return hasArc;
	}

	public bool attackHasImpactDetonation(){
		return hasImpactDetonation;
	}

	public bool attackHasTimedDetonation(){
		return hasTimedDetonation;
	}

	public int getDetonationRadius(){
		return detonationRadius;
	}

	public int getDetonationDamage(){
		return detonationDamage;
	}

	public string getAnimationType(){
		return animationType;
	}


	public List<StatusCondition> getConditionsInflicted(){
		List<StatusCondition> inflictedConditions = new List<StatusCondition>();
		//DETERMINE WHICH CONDITIONS WERE INFLICTED AND RETURN A LIST OF THEM
		return inflictedConditions;
	}

}
