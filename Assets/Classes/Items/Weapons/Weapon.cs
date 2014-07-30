using UnityEngine;
using System.Collections.Generic;

public class Weapon : Item {

	protected bool isTwoHanded;
	//attacks which can be used with this weapon (character must have the attack in order to use it with the weapon)
	protected List<Attack> attackTypes;
	//skills which can be used with this weapon (character must have the skill in order to use it with the weapon)
	protected List<Skill> skillTypes;



	public bool isTwoHandedWep(){
		return isTwoHanded;
	}

	public List<Attack> getAttackTypes(){
		return attackTypes;
	}
	
	public List<Skill> getSkillTypes(){
		return skillTypes;
	}

}
