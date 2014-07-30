using UnityEngine;
using System.Collections;

public class StatusCondition : MonoBehaviour {

	protected string conditionType;

	protected int totalDuration;
	protected int remainingDuration;
	protected int damagePerSecond;
	//percentages by which the stats of the afflicted character are reduced while the condition is in effect
	protected int accuracyReduction;
	protected int damageReduction;
	protected int agilityReduction;
	protected int defenseReduction;
	protected bool reversesControls;



	public string toString(){
		return conditionType;
	}

	public int getTotalDuration(){
		return totalDuration;
	}

	public int getRemainingDuration(){
		return remainingDuration;
	}

	public int getDamagePerSecond(){
		return damagePerSecond;
	}

	public int getAccuracyReduction(){
		return accuracyReduction;
	}

	public int getDamageReduction(){
		return damageReduction;
	}

	public int getAgilityReduction(){
		return agilityReduction;
	}

	public int getDefenseReduction(){
		return defenseReduction;
	}

	public bool areControlsReversed(){
		return reversesControls;
	}
	
}
