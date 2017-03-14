using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon_Spawner : MonoBehaviour {

	private Dictionary<string, GameObject> dict_weapons = new Dictionary<string, GameObject>();

	private string weaponName;
	public string WeaponName{
		get{ return weaponName; }
		set{weaponName = value;}
	}

	private string weaponType;
	public string WeaponType
	{
		set
		{
			weaponType = value;
			Debug.Log(weaponType);
		}
	}

	private GameObject weaponPrefab = null;
	public GameObject WeaponPrefab
	{
		set
		{
			weaponPrefab = value;
			GameObject newWeapon = Instantiate(weaponPrefab, transform.position, Quaternion.identity) as GameObject;
			//newWeapon.transform.parent = transform;
			newWeapon.gameObject.name = WeaponName;
			newWeapon.transform.parent = null;
			//Debug.Log("weapon name = " + WeaponName);
		}
	}
}
