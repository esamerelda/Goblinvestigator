using UnityEngine;
using System.Collections;

public class ItemController : MonoBehaviour {

	public int weaponDamage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//if item is not on the ground, it can cause damage to either the player or the monster
		//if it hits the monster, it would be cool if it stuck to the monster
	
	}

	public int GetWeaponDamage()
	{
		return weaponDamage;
	}
}
