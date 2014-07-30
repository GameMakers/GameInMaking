using UnityEngine;
using System.Collections.Generic;

public class CharacterClass : MonoBehaviour {

	protected string classType;

	protected CharacterType charType;
	protected List<Attack> attackTypes;
	protected List<Skill> skillTypes;

	//the base amounts at which the stats for a character of this class will begin
	protected int[] baseStats = new int[5];
	//the amount each stat will increase by upon levelling up for a character of this class
	protected int[] statProgression = new int[5];



	
	public string toString(){
		return classType;
	}

	public CharacterType getCharType(){
		return charType;
	}

	public List<Attack> getAttackTypes(){
		return attackTypes;
	}

	public List<Skill> getSkillTypes(){
		return skillTypes;
	}

	public int[] getBaseStats(){
		return baseStats;
	}

	public int[] getStatProgression(){
		return statProgression;
	}
}
