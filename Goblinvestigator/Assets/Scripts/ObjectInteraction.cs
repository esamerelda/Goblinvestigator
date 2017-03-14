using UnityEngine;
using System.Collections;

public class ObjectInteraction : MonoBehaviour {


	private bool armed;
	private bool beingThrown;
	private bool canPickUp;
	private bool holdingE;

	private Camera cam;

	private float throwSpeed = 2000.0f;

	private SoundManager sound;
	public AudioClip pickUpSound;

	private GameObject gameManager;
	private GameObject hand;
	private GameObject weapon;

	private Rigidbody weaponRb;

	void Awake()
	{
		cam = GameObject.Find("Main Camera").GetComponent<Camera>();
		gameManager = GameObject.Find("GameManager");
		hand = GameObject.Find("Invisible Hand");
		sound = gameManager.GetComponent<SoundManager>();
	}

	// Use this for initialization
	void Start () {
		armed = false;
		beingThrown = false;
		canPickUp = false;
	
	}
	
	void Update () {
		//if player has an item in hand...
		if (armed){

			//and holds E then clicks the mouse button...
			if ((Input.GetKey(KeyCode.E)) && (Input.GetMouseButtonDown(0)))
			{
				//...set to not kinematic
				weaponRb.isKinematic = false;

				//...send item flying
				weaponRb.AddForce(cam.transform.forward * throwSpeed);
				//...remove parent
				weapon.transform.parent = null;
				//...nullify weapon and weaponRb
				weapon = null;
				weaponRb = null;

				//...set armed to false
				armed = false;
			}
		}
		
		
		if (canPickUp)
		{
			//Debug.Log("Click to pick this up");
			if (Input.GetMouseButtonDown(0))
			{
				//make object child of hand
				weapon.transform.parent = hand.transform;
				//reset rotation of object

				//set item to kinematic to prevent from flying around
				//sound.PlaySound(pickUpSound);
				weaponRb = weapon.GetComponent<Rigidbody>();
				weaponRb.isKinematic = true;
				armed = true;
				canPickUp = false;
			}
		}
		else
		{
		}
	
	}

	public void OnBeingThrown()
	{
		beingThrown = true;
	}
	

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Ground"))									//hits ground, nullify attack
		{
			beingThrown = false;
		}
		if ((other.gameObject.CompareTag("Goblin")) && (beingThrown == true))		//hits goblin, trigger goblin response/death
		{
			beingThrown = false;        //disable to allow ricochet damage!
										//other.gameObject.GetComponent<GoblinData>().OnHitWithWeapon();
			other.gameObject.SendMessage("Die");
		}
	}
	//collision detection
	void OnTriggerEnter(Collider other)
	{
		if ((!armed) && (other.tag == "Item")) {
			canPickUp = true;
			weapon = other.gameObject;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Item")
		{
			//Debug.Log("Can't pick up anything");
			canPickUp = false;
		}
		//close info ui
	}
	
}
