using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


//this script acts as a mediator between Randomized Data and GoblinData script instances attached to living goblins in scene
public class GameSetup : MonoBehaviour {

	//refs to other scripts
	private RandomizedData randomizedData;

	private Dictionary<string, GoblinData> GoblinDict = new Dictionary<string, GoblinData>();  //key = goblin name, value = ref to that goblin's goblinData script

	private GameObject[] goblins;               //array of all goblins in scene
	private GameObject[] array_bedroomsInScene;
	private GameObject[] array_containersInFolder;
	private GameObject[] array_containerSpawnersInScene;
	private GameObject[] array_hatsInFolder;
	private GameObject[] array_hatsInScene;
	private GameObject murderer;                //the murderer

	private int knowsMurdererChance = 20;       //percent chance that each goblin has to know who the murderer is.  Affects dialogue.

	private InterfaceScript uiScript;

	private List<GameObject> list_bedrooms = new List<GameObject>();
	private List<GameObject> list_hats = new List<GameObject>();

	private Material[] array_goblinMats;
	private NotificationsManager Notifications = null;

	private string bName;
	private string bWeapon;
	private string murdererName;
	private string murdererWeapon;
	public string MurdererWeapon
	{
		//get called by nothing.
		get { return murdererWeapon; }
		//set by this script in setGoblinBasicInfo().
		set { murdererWeapon = value; }
	}

	private string victimName;

	void Awake()
	{
		Notifications = GameObject.Find("GameManager").GetComponent<NotificationsManager>();

		//get references
		randomizedData = GetComponent<RandomizedData>();

		//get array of goblins in the scene
		goblins = GameObject.FindGameObjectsWithTag("Goblin");
		uiScript = GameObject.Find("GameManager").GetComponent<InterfaceScript>();
		array_bedroomsInScene = GameObject.FindGameObjectsWithTag("Bedroom");
		array_goblinMats = Resources.LoadAll("Materials", typeof(Material)).Cast<Material>().ToArray();
		array_hatsInFolder = Resources.LoadAll("Prefabs/Hats", typeof(GameObject)).Cast<GameObject>().ToArray();        //can only get contents of folder into array, I guess
		array_containersInFolder = Resources.LoadAll("Prefabs/Containers", typeof(GameObject)).Cast<GameObject>().ToArray();


		ChooseMurderer();                           //done first so we can choose who knows who the murderer is in the next function call
		SetGoblinBasicInfo();                       //names all goblins and stores names and scripts in dict, knowsMurderer, weapon, bedroom
		SetGoblinDialogue1_and_2();                 //sets victim name, feelings, and dialogue options 1 and 2
		SetGoblinDialogue3();                       //sets dialogue option 3.  Must come after 1 & 2 because all feelings must be set for this to work.
													//TestDialogues();
		SetGoblinHats();
		SetUpContainers();
		SetUpVictim();
		SetUserInterfaceData();

	}

	void Start()
	{
		Notifications.PostNotification(this, "OnGameSetupComplete");
		//Debug.Log("ugh.");
	}

	private void ChooseMurderer()
	{
		int murdererIndex = Random.Range(0, goblins.Length);        //choose random number from the number of total goblins in the scene
		murderer = goblins[murdererIndex];                          //store a variable of which one is the murderer...  like it's his address or something.
		murderer.GetComponent<GoblinData>().isMurderer = true;      //kindly inform the goblin that it is the murderer
	}


	private void SetUserInterfaceData()
	{
		uiScript.Goblins = GoblinDict;
		uiScript.DialogueChoice_Victim = victimName;
		uiScript.DialogueChoice_thinkOfVictim = victimName;
	}


	private void SetGoblinBasicInfo()
	{
		//prepare randomized data
		randomizedData.LoadNames();
		randomizedData.LoadTitles();
		randomizedData.LoadWeapons();

		foreach (GameObject bedroom in array_bedroomsInScene)       //get a list of all bedrooms from the array
		{
			list_bedrooms.Add(bedroom);
		}

		int weaponIndex = 0;

		LoadHatList();


		foreach (GameObject goblin in goblins)
		{
			//SKIN MATERIAL
			int skindex = Random.Range(0, array_goblinMats.Length);
			goblin.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = array_goblinMats[skindex];

			//NAME
			string newGoblinName = randomizedData.GetName();
			goblin.GetComponent<GoblinData>().goblinName = newGoblinName;
			//goblin.GetComponent<GoblinData>().goblinName = randomizedData.GetName();                            //get random name + title from randomizedData

			//WEAPON
			string newWeapon = randomizedData.GetWeapon(weaponIndex);
			goblin.GetComponent<GoblinData>().weapon = newWeapon;
			//goblin.GetComponent<GoblinData>().weapon = randomizedData.GetWeapon(weaponIndex);
			weaponIndex++;
			if (weaponIndex > 5)
			{
				weaponIndex = 0;
			}
			if (goblin.GetComponent<GoblinData>().isMurderer)
			{
				MurdererWeapon = goblin.GetComponent<GoblinData>().weapon;
			}

			//KNOWS MURDERER
			int odds = Random.Range(1, 100);
			if (goblin.GetComponent<GoblinData>().isMurderer)           //if this goblin is the murderer, they know who the murderer is
			{
				goblin.GetComponent<GoblinData>().knowsMurderer = true;
			}
			else if (odds <= knowsMurdererChance)                       //if random number = or is less than the percent chance to know the murderer, set the variable in that goblin's GoblinData script instance
			{
				goblin.GetComponent<GoblinData>().knowsMurderer = true;
			}

			//add each name and ref to that goblin's script as key-value pair
			GoblinDict[goblin.GetComponent<GoblinData>().goblinName] = goblin.GetComponent<GoblinData>();

			//SET UP BEDROOM
			int roomIndex = Random.Range(0, list_bedrooms.Count);												//get a random index for the bedroom
			GameObject goblinRoom = list_bedrooms[roomIndex];                                                   //choose the room corresponding to that number
			Text signText = goblinRoom.GetComponentInChildren<Text>();                                          //find UI Text component in children
			signText.text = newGoblinName;                                                                      //set its text to match the name of the currently selected goblin
			Weapon_Spawner[] weaponSpawners = goblinRoom.GetComponentsInChildren<Weapon_Spawner>();
			foreach(Weapon_Spawner spawner in weaponSpawners)
			{
				//spawner.WeaponType = newWeapon;
				spawner.GetComponent<Weapon_Spawner>().WeaponName = newWeapon;
				spawner.WeaponPrefab = randomizedData.GetWeaponPrefab(newWeapon);

				
			}

			list_bedrooms.RemoveAt(roomIndex);																	//remove bedroom from list to prevent from being overwritten by another goblin


		}


		randomizedData.GoblinDict = GoblinDict;             //set RandomizedData's GoblinDict to match this ones for use in 3rd dialogue option
		murdererName = murderer.GetComponent<GoblinData>().goblinName;
		uiScript.MurdererName = murdererName;


	}


	private void SetGoblinDialogue1_and_2()
	{
		//the victim
		victimName = randomizedData.GetName();                          //randomly pick a name for victim
		uiScript.VictimName = victimName;                               //tell InterfaceScript victim's name for updating intro screen

		//the living goblins
		foreach (GameObject goblinA in goblins)                         //iterate through each goblin to set dialogue options 1 and 2
		{

			//SET FEELINGS ABOUT VICTIM AND FIRST DIALOGUE OPTION
			GoblinData goblinData = goblinA.GetComponent<GoblinData>();                          //ref to current goblin's goblinData script instance
			string feeling = "neutral";                                                         //create var to store feeling
			if (!goblinData.isMurderer)                                                         //if current goblin is not the murderer
			{
				feeling = randomizedData.GetFeeling();                                          //get random feeling about victim
			}
			else
			{
				feeling = "hate";                                                               //if current goblin is the murderer, set their feeling about victim to hate
			}

			goblinData.victimFeelings = feeling;

			string dialogue = randomizedData.GetEmotionalDialogue(feeling, victimName);         //generate current goblin's dialogue about victim
			goblinData.victimDialogue = dialogue;                                               //set current goblin's dialogue about victim


			//Set feelings 
			List<GameObject> otherGoblins = new List<GameObject>();                         //create list of other goblins				   
			otherGoblins.AddRange(goblins);                                                 //add all goblins to the list
			Dictionary<string, string> otherFeels = new Dictionary<string, string>();       //create temp dictionary to store names and associated feelings for other goblins							 
			foreach (GameObject g in otherGoblins)                                          //for each OTHER goblin
			{
				string otherName = g.GetComponent<GoblinData>().goblinName;                 //get other goblin's name (goblin B)
				if (otherName != goblinData.goblinName)                                     //if other goblin's name is not the same as current goblin's name...
				{

					string otherFeeling = randomizedData.GetFeeling();                      //get random feeling

					otherFeels.Add(otherName, otherFeeling);                                //set name and feeling in temp dict


				}
			}
			goblinData.goblinFeelings = otherFeels;                                                 //set current goblin's feelings dictionary to match the one we just created

			Dictionary<string, string> Dialogue2 = new Dictionary<string, string>();                //create temp dictionary to store names and dialogue for Dialogue Option 2
			foreach (KeyValuePair<string, string> pair in goblinData.goblinFeelings)                //for every entry we just added (other goblins)
			{
				//CHOOSE FOR #2
				string newDialogue2 = randomizedData.GetEmotionalDialogue(pair.Value, pair.Key);    //get random 2nd dialogue for emotion for that specific other goblin
				Dialogue2.Add(pair.Key, newDialogue2);                                              //add it to temp Dictionary
			}
			goblinData.goblinDialogue2 = Dialogue2;                                                 //set current Goblin's dialogues for Dialogue #2			
		}
	}

	private void SetGoblinDialogue3()
	{
		foreach (GameObject goblinA in goblins)                                         //for each goblin (A) in the array
		{
			GoblinData A = goblinA.GetComponent<GoblinData>();
			Dictionary<string, string> SpokenFeels = new Dictionary<string, string>();  //temp dict to store spoken feels, possible lies.
			Dictionary<string, string> Dialogue3 = new Dictionary<string, string>();    //create temp dictionary to store names and dialogue for Dialogue Option 2

			foreach (GameObject goblinB in goblins)                                     //for each goblin (B) in the array
			{
				GoblinData B = goblinB.GetComponent<GoblinData>();
				if (goblinA.GetInstanceID() != goblinB.GetInstanceID())                 //if A and B are not the same Goblin
				{
					string aName = A.goblinName;
					bName = B.goblinName;               //set global goblin info variables to prepare to get string
					bWeapon = B.weapon;

					//FOR TESTING ONLY
					string newFeel = "UNSET FIX THIS";
					newFeel = GetTrueFeel(bName);
					//get variables
					//bool aIsMurderer = aData.isMurderer;
					//bool aKnowsMurderer = aData.knowsMurderer;
					//bool bIsMurderer = goblinB.GetComponent<GoblinData>().isMurderer;

					//PUT THE COMPLEX FORMULA HERE
					if (A.isMurderer)                           //if Goblin A is the murderer...
					{
						if (A.goblinFeelings[bName] == "hate")      //...and A hates B...
						{
							int odds = Random.Range(0, 100);
							if (odds <= 25)
							{
								newFeel = "hate";                       //...%25 chance to lie - Goblin B hated the victim
							}
							else if (odds > 25 && odds <= 50)
							{
								newFeel = "murdered";                   //...%25 chance to lie - Goblin B murdered victim
							}
							else
							{
								newFeel = GetTrueFeel(bName);           //...%50 chance to tell Goblin B's true feelings about victim
							}
						}
						else                                        //...and A neutrals/loves B...
						{
							newFeel = GetTrueFeel(bName);               //...tells the truth.
						}
					}
					else                                        //If Goblin A is NOT the murderer...
					{
						if (B.isMurderer)                           //... but B IS the murderer...
						{
							if (A.knowsMurderer)                        //...and A knows it...
							{
								if (A.goblinFeelings[bName] == "love")      //...and A loves B...
								{
									int odds = Random.Range(0, 100);
									if (odds <= 25)
									{
										newFeel = "love";                       //...25% chance to lie - Goblin B loved the victim.
									}
									else if (odds > 25 && odds <= 50)
									{
										newFeel = "hate";                       //...25% chance to tell truth about feeling.
									}
									else if (odds > 50 && odds <= 75)
									{
										newFeel = "neutral";                    //...25% chance to lie - Goblin B felt neutral about victim.
									}
									else
									{
										newFeel = "murdered";                   //...25% chance to admit Goblin B murdered victim.
									}
								}
								else                                        //...and A neutrals/hates B...
								{
									newFeel = GetTrueFeel(bName);               //...A tells the truth.
								}
							}
							else                                        //...and A doesn't know B is the murderer...
							{
								newFeel = GetTrueFeel(bName);               //...A tells the truth.
							}
						}
						else                                        //...and B is also NOT the murderer...
						{
							if ((A.knowsMurderer) && (A.goblinFeelings[murdererName] == "love"))        //...and A knows who the murderer is and loves them
							{
								if (A.goblinFeelings[bName] == "hate")                              //...and A hates B...
								{
									int odds = Random.Range(0, 100);
									if (odds <= 50)                                                     //...50% chance to frame B for murder
									{
										newFeel = "murdered";
									}
								}
							}
						}
					}





					//TESTING - gets true feeling
					string newDialogue3 = getD3(newFeel);
					SpokenFeels.Add(bName, newFeel);
					Dialogue3.Add(bName, newDialogue3);                                 //add new name / dialogue to temp dictionary
				}
			}
			A.spokenFeelings = SpokenFeels;
			A.goblinDialogue3 = Dialogue3;
		}
	}


	private string GetTrueFeel(string gName)    //takes the name of a goblin and gets their true feeling about the victim
	{
		string trueFeel = GoblinDict[gName].victimFeelings;

		return trueFeel;
	}


	private void LoadHatList()
	{
		foreach (GameObject hat in array_hatsInFolder)
		{
			list_hats.Add(hat);
		}
	}


	private void SetGoblinHats()
	{
		array_hatsInScene = GameObject.FindGameObjectsWithTag("GoblinHat");
		//Debug.Log(array_hatsInScene.Length);

		foreach (GameObject goblinHead in array_hatsInScene)
		{
			int index = Random.Range(0, list_hats.Count);
			//GameObject newHat = Instantiate(list_hats[index], goblinHead.transform.position, Quaternion.identity) as GameObject;
			GameObject newHat = Instantiate(list_hats[index], goblinHead.transform.position, goblinHead.transform.rotation) as GameObject;
			//GameObject newHat = Instantiate(list_hats[index], new Vector3(goblinHead.transform.position.x, 0), goblinHead.transform.rotation) as GameObject;
			newHat.transform.parent = goblinHead.transform;
			list_hats.RemoveAt(index);                      //remove that hat from hat list
			if (list_hats == null)
			{
				LoadHatList();                              //if we run out of hats, reload them to prevent errors from not having enough assets
			}
		}
	}

	private void SetUpContainers()
	{
		array_containerSpawnersInScene = GameObject.FindGameObjectsWithTag("ContainerSpawner");
		int murderWeaponContainer_index = Random.Range(0, array_containerSpawnersInScene.Length);

		foreach (GameObject spawner in array_containerSpawnersInScene)
		{
			//get random prefab
			int index = Random.Range(0, array_containersInFolder.Length);
			GameObject newContainer = Instantiate(array_containersInFolder[index], spawner.transform.position, transform.rotation) as GameObject;
			newContainer.transform.parent = spawner.transform;

			if (spawner.GetInstanceID() == array_containerSpawnersInScene[murderWeaponContainer_index].GetInstanceID())
			{
				spawner.gameObject.GetComponent<Container_Data>().ContainerText = "You found a bloody " + MurdererWeapon + "!";
			}
		}
	}

	private void SetUpVictim()
	{
		string damageType = "";
		if ((MurdererWeapon == "sword") || (MurdererWeapon == "axe"))
		{
			damageType = "slashy";
		}
		else if ((MurdererWeapon == "dagger") || (MurdererWeapon == "spear"))
		{
			damageType = "stabby";
		}
		else if ((MurdererWeapon == "brick") || (MurdererWeapon == "club"))
		{
			damageType = "bashy";
		}
		else { damageType = "ERROR"; }

		string words = "It seems that " + victimName + " was killed with some sort of " + damageType + " weapon.";

		GameObject[] victimObject = GameObject.FindGameObjectsWithTag("Victim");
		foreach (GameObject victim in victimObject)
		{
			victim.GetComponent<Victim_Data>().InspectionText = words;
		}

	}


	private void TestDialogues()	//DON'T DELETE - for testing AND reference!
	{
		foreach (KeyValuePair<string, GoblinData> goblinA in GoblinDict)
		{
			string gobA = goblinA.Value.goblinName;

			//option 1
			//Debug.Log("#1 - " + gobA + " " + goblinA.Value.victimFeelings + "s " + victimName);
			Debug.Log("#1 - " + gobA + ":  " + goblinA.Value.victimDialogue);

			//option 2
			foreach (KeyValuePair<string, GoblinData> goblinB in GoblinDict)
			{
				string gobB = goblinB.Value.goblinName;
				if (gobA != gobB)
				{
					//Debug.Log("#2 - " + gobA + " " + goblinA.Value.goblinFeelings[gobB] + "s " + gobB);
					Debug.Log("#2 - " + gobA + ":  " + goblinA.Value.goblinDialogue2[gobB]);

					//Debug.Log("#3 - True:  " + GetTrueFeel(gobB) + " // Spoken:  " + goblinA.Value.spokenFeelings[gobB]);
					Debug.Log("#3 - " + gobA + ":  " + goblinA.Value.goblinDialogue3[gobB]);
				}
			}
		}
	}


	private string getD3(string feel)		//makes getting actual string less redundant since there are a ton of if statements to decide which feel to choose
	{
		string d3 = randomizedData.GetDialogue3(bName, bWeapon, victimName, feel);
		return d3;
	}


	private void SetVictimDialogue()
	{
		foreach (GameObject goblin in goblins)
		{
			
			GoblinData goblinData = goblin.GetComponent<GoblinData>();						//ref to current goblin's goblinData script instance
																	  
			string feeling = randomizedData.GetFeeling();									//set current goblin's feeling about victim
			goblinData.victimFeelings = feeling;
			
			string dialogue = randomizedData.GetEmotionalDialogue(feeling, victimName);		//set current goblin's dialogue about victim
			goblinData.victimDialogue = dialogue;

		}
	}
}
