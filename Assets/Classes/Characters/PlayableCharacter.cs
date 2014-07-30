using UnityEngine;
using System.Collections.Generic;

public class PlayableCharacter : Character {

	protected CharacterClass characterClass;

	protected Weapon equipedWeapon;
	protected OffhandEquip equipedOffhand;

	protected List<Item> inventory;



	public CharacterClass getCharacterClass(){
		return characterClass;
	}

	public Weapon getWeapon(){
		return equipedWeapon;
	}

	public OffhandEquip getOffhand(){
		return equipedOffhand;
	}

	public List<Item> getInventory(){
		return inventory;
	}
}
