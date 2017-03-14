using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerInteraction : MonoBehaviour {

	public AudioClip crate_open;
	public AudioClip goblin_talk;
	public AudioClip weapon_pickup;
	public AudioClip weapon_throw;

	private AudioSource sound;

	private bool armed;
	private bool canPickUp;
	private bool holdingE;
	private bool infoBoxEnabled;
	private bool stopGoblinTalking = false;

	private enum PlayerState { none, TalkingToGoblin};
	private PlayerState playerState;

	private float rayLength = 10f;
	private float throwSpeed = 1500.0f;

	private GameManager gameManager;

	private GameObject currentGoblin;
	private GameObject weapon;

	private InterfaceScript uiScript = null;

	private List<string> list_goblinsTalkedTo = new List<string>();

	private NotificationsManager Notifications = null;
	private ObjectInteraction weaponScript;

	public RaycastHit hit;
	private Rigidbody weaponRb;

	private string[] array_goblinsTalkedTo;

	void Awake()
	{
		sound = GetComponent<AudioSource>();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		Notifications = GameObject.Find("GameManager").GetComponent<NotificationsManager>();
		uiScript = GameObject.Find("GameManager").GetComponent<InterfaceScript>();
		playerState = PlayerState.none;
		
	}


	void Start()
	{
		Notifications.AddListener(this, "OnEndTalkToGoblin");

		armed = false;
		canPickUp = false;
		infoBoxEnabled = false;
		stopGoblinTalking = false;
	}


	void FixedUpdate()
	{
		//Scanning environment for interactions
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));  //position of ray/origin

		if(Physics.Raycast(ray, out hit, rayLength))
		{
			//Debug.Log(hit.collider.gameObject.name);
			if (hit.collider.gameObject.tag == "Container")
			{
				Container_Data containerScript = hit.collider.gameObject.transform.parent.GetComponent<Container_Data>();
                if (!containerScript.Searched)
				{
					uiScript.InfoBoxText = "Left-click to search this container";

					//SEARCHING CRATES
					if ((playerState == PlayerState.none) && (Input.GetMouseButtonDown(0)))
					{
						PlaySound(crate_open);
						containerScript.Searched = true;
						uiScript.InfoBoxText = "";
					}
				}
				else
				{
					uiScript.InfoBoxText = containerScript.ContainerText;
				}
				
				
			}


			//else if (hit.collider.gameObject.tag == "Goblin")
			else if (hit.collider.gameObject.CompareTag("Goblin"))
			{
				

				//display name of goblin somewhere on the screen
				string gobName = hit.collider.gameObject.GetComponent<GoblinData>().goblinName;				//get name of goblin you're lookin at
				if (!armed)
				{
					//uiScript.EnableInfoBox("Left-click to talk to " + gobName);                             //turn on info box and set text
					uiScript.InfoBoxText = "Left-click to talk to " + gobName;
                }
				

				if ((playerState == PlayerState.none) && (Input.GetMouseButtonDown(0)) && (!armed))
				{
					PlaySound(goblin_talk);
					currentGoblin = hit.collider.gameObject;
					playerState = PlayerState.TalkingToGoblin;
					if (!list_goblinsTalkedTo.Contains(gobName))
					{
						list_goblinsTalkedTo.Add(gobName);
					}
					uiScript.GoblinA_Name = gobName;
					uiScript.List_DialogueDropdownChoices = list_goblinsTalkedTo;
					currentGoblin.SendMessage("OnTalkToGoblin", this);
					Notifications.PostNotification(this, "OnTalkToGoblin");
					
				}
				if (stopGoblinTalking)
				{
					currentGoblin.SendMessage("OnEndTalkToGoblin");
					stopGoblinTalking = false;
				}

			}


			else if (hit.collider.gameObject.tag == "Weapon")
			{
				//Debug.Log("Weapon");
				if (!armed)												//if player isn't currently holding anything
				{
					//TODO - display "Pick Up" prompt somewhere on screen
					canPickUp = true;
					weapon = hit.collider.gameObject;
				}

				if (canPickUp)
				{
					//uiScript.EnableInfoBox("Left-click to pick up");
					//uiScript.InfoBoxText = "Left-click to pick up " + hit.collider.gameObject.transform.parent.gameObject.GetComponent<Weapon_Spawner>().WeaponName;
					string weaponType = hit.collider.gameObject.name;
					string damageType = "";
					if ((weaponType == "sword") || (weaponType == "axe"))
					{
						damageType = "slashy";
					}
					else if ((weaponType == "dagger") || (weaponType == "spear"))
					{
						damageType = "stabby";
					}
					else if ((weaponType == "brick") || (weaponType == "club"))
					{
						damageType = "bashy";
					}
					else { damageType = "ERROR"; }

					uiScript.InfoBoxText = "Left-click to pick up the " +damageType+ " " + weaponType;

					//PLAYER PICKS STUFF UP
                    if (Input.GetMouseButtonDown(0))					//if player left-clicks mouse
					{
						PlaySound(weapon_pickup);
						weapon.transform.parent = transform;            //make object child of hand

						weaponScript = weapon.GetComponent<ObjectInteraction>();
						weaponRb = weapon.GetComponent<Rigidbody>();
						weaponRb.isKinematic = true;                    //set item to kinematic to prevent from flying around
						armed = true;
						canPickUp = false;
						uiScript.InfoBoxText = "";
					}
				}
				else if (armed)
				{
					//uiScript.EnableInfoBox("Hold E + Left-click to throw");
					uiScript.InfoBoxText = "Hold E + Left-click to throw, or right-click to drop " + hit.collider.gameObject.name;
                }
			}
			
			else if (hit.collider.gameObject.CompareTag("Victim"))
			{
				uiScript.InfoBoxText = hit.collider.gameObject.GetComponent<Victim_Data>().InspectionText;
			}
			else
			{
				uiScript.InfoBoxText = "";
			}

		}
		else { uiScript.InfoBoxText = ""; }

		//Throwing & Dropping Stuff
		if (armed)
		{
			//DROP WEAPON if player right clicks
			if (Input.GetMouseButtonDown(1))
			{
				PlaySound(weapon_pickup);
				uiScript.InfoBoxText = "";
				weaponRb.isKinematic = false;                                   //...set to not kinematic - allows weapon to move again
				weapon.transform.parent = null;                                 //...remove parent					   
				weapon = null;                                                  //...nullify weapon and weaponRb
				weaponRb = null;

				armed = false;                                                  //...set armed to false
			}

			//THROW WEAPON if player holds E then clicks the mouse button...
			if ((Input.GetKey(KeyCode.E)) && (Input.GetMouseButtonDown(0)))
			{
				PlaySound(weapon_throw);
				uiScript.InfoBoxText = "";
				weaponScript.OnBeingThrown();
				weaponRb.isKinematic = false;                                   //...set to not kinematic - allows weapon to move again
				weaponRb.AddForce(transform.forward * throwSpeed);          //...send item flying												  
				weapon.transform.parent = null;                                 //...remove parent					   
				weapon = null;                                                  //...nullify weapon and weaponRb
				weaponRb = null;

				armed = false;                                                  //...set armed to false
			}
		}

	}

	private void OnEndTalkToGoblin()
	{
		playerState = PlayerState.none;
		stopGoblinTalking = true;
	}

	public void PlaySound(AudioClip clip)
	{
		sound.PlayOneShot(clip);
	}
}
