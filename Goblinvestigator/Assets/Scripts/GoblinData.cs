using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GoblinData : MonoBehaviour {

	//NAME
	[SerializeField]				//shows goblin names in inspector when testing
	private string _goblinName;
	public string goblinName
	{
		get
		{
			return _goblinName;
		}
		set
		{
			_goblinName = value;
			Text Text_Name = transform.Find("Goblin Name Canvas").transform.Find("Goblin Name Text").GetComponent<Text>();
			Text_Name.text = _goblinName;
		}
	}

	//dialogue about other living goblins
	private Dictionary<string, string> _goblinDialogue2 = new Dictionary<string, string>();
	public Dictionary<string, string> goblinDialogue2
	{
		get
		{
			return _goblinDialogue2;
		}
		set
		{
			_goblinDialogue2 = value;
		}
	}

	//dialogue about other living goblins' relationships to victim
	private Dictionary<string, string> _goblinDialogue3 = new Dictionary<string, string>();
	public Dictionary<string, string> goblinDialogue3
	{
		get
		{
			return _goblinDialogue3;
		}
		set
		{
			_goblinDialogue3 = value;
		}
	}

	//feelings about other living goblins
	private Dictionary<string, string> _goblinFeelings;
	public Dictionary<string, string> goblinFeelings
	{
		get
		{
			return _goblinFeelings;
		}
		set
		{
			_goblinFeelings = value;
		}
	}

	//IS MURDERER!?
	[SerializeField]				//shows if goblin is murderer in inspector while testing
	private bool _isMurderer = false;
	public bool isMurderer
	{
		get
		{
			return _isMurderer;
		}
		set
		{
			_isMurderer = value;
		}
	}

	//Knows who murderer is?
	[SerializeField]
	private bool _knowsMurderer = false;
	public bool knowsMurderer
	{
		get
		{
			return _knowsMurderer;
		}
		set
		{
			_knowsMurderer = value;
		}
	}


	//What This goblin will tell you Goblin B feels about the victim, set by GameSetup
	private Dictionary<string, string> _spokenFeelings;
	public Dictionary<string, string> spokenFeelings
	{
		get
		{
			return _spokenFeelings;
		}
		set
		{
			_spokenFeelings = value;
		}
	}


	//FAVORITE WEAPON
	[SerializeField]
	private string _weapon;
	public string weapon
	{
		get
		{
			return _weapon;
		}
		set
		{
			_weapon = value;
		}
	}

	

	//feelings about victim
	[SerializeField]
	private string _victimFeelings;
	public string victimFeelings
	{
		get
		{
			return _victimFeelings;
		}
		set
		{
			_victimFeelings = value;
		}
	}

	

	//dialogue about victim
	[SerializeField]
	private string _victimDialogue;
	public string victimDialogue
	{
		get
		{
			return _victimDialogue;
		}
		set
		{
			_victimDialogue = value;
		}
	}

	public void OnHitWithWeapon()
	{
		if (isMurderer)		//if you kill the murderer
		{
			//GameObject.Find("GameManager").SendMessage("WinGame", SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			//GameObject.Find("GameManager").SendMessage("LoseGame", SendMessageOptions.DontRequireReceiver);
		}
	}
}
