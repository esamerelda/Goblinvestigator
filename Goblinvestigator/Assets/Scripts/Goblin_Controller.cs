using UnityEngine;
using System.Collections;

public class Goblin_Controller: MonoBehaviour {

	private Animator animController;

	public AudioClip goblin_death;

	private AudioSource sound;

	private bool destinationSet = false;

	public enum GOBLIN_STATE { DEAD, IDLE, TALK, WALK };
	private GOBLIN_STATE ActiveState;

	public float arrivalDistance = 2f;
	public float bl = 0f;
	public float blAttack = 0.05f;
	public float blDecay = 0.01f;
	private float chillTime;				//time between reaching a walking point and starting toward a new one.
	private float chillTimeMax = 15f;
	private float chillTimeMin = 3f;
	private float currentSpeed;
	//private float stopSpeed = 0f;			//used for when player pushes goblin around
	private float turnSpeed = 1f;			//speed it takes for goblin to turn toward new destination
	private float WalkDistance = 10.0f;
	private float walkSpeed = 2.5f;

	private NotificationsManager Notifications;
	private GameObject player;
	private GameObject[] walkingPoints;

	private string goblinName;          //for testing

	private Transform thisTransform;
	[SerializeField]
	private Vector3 destination;
	private Vector3 playerPosition;
	
	void Awake()
	{
		ActiveState = GOBLIN_STATE.IDLE;
		Notifications = GameObject.Find("GameManager").GetComponent<NotificationsManager>();
		player = GameObject.Find("Player Capsule");
		sound = GetComponent<AudioSource>();
		thisTransform = gameObject.GetComponent<Transform>();
		walkingPoints = GameObject.FindGameObjectsWithTag("WalkingPoint");
		chillTime = Random.Range(chillTimeMin, chillTimeMax);
	}

	void Start() {

		animController = gameObject.GetComponent<Animator> ();
		destinationSet = false;
		goblinName = GetComponent<GoblinData>().goblinName;//for testing
		ChangeState(ActiveState);
		chillTime = Random.Range(chillTimeMin, chillTimeMax);
	}

	void Update() {
		if (ActiveState == GOBLIN_STATE.IDLE)
		{
			if(chillTime > 0)
			{
				bl = Mathf.Clamp(bl -= blDecay, 0, 1);              //update animation to 
				chillTime -= Time.deltaTime;
			}
			else
			{
				destinationSet = false;
				ChangeState(GOBLIN_STATE.WALK);
			}
			
		}

		if(ActiveState == GOBLIN_STATE.TALK)
		{
			transform.LookAt(playerPosition);
			//Quaternion toRotation = Quaternion.FromToRotation(transform.forward, playerPosition);
			//transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, .01f * Time.time);
			//GOBLINS JUST HATE MAKING EYE CONTACT.
		}

		if (ActiveState == GOBLIN_STATE.WALK)
		{
			if (!destinationSet)
			{
				//bl = Mathf.Clamp(bl += blAttack, 0, 1);         //update animation
																//get destination
				int destinationIndex = Random.Range(0, walkingPoints.Length);   //get index
				destination = walkingPoints[destinationIndex].transform.position;
				destinationSet = true;
			}
			

			//turn toward it
			transform.LookAt(destination);

			//start walking
			if(Vector3.Distance(destination, transform.position) > arrivalDistance)
			{
				bl = Mathf.Clamp(bl += blAttack, 0, 1);         //update animation
																//transform.position += destination * Time.deltaTime * walkSpeed;
				transform.position = Vector3.MoveTowards(transform.position, destination, (walkSpeed * Time.deltaTime));
			}
			else
			{
				//Debug.Log("Destination hit");
				destinationSet = false;
				chillTime = Random.Range(chillTimeMin, chillTimeMax);
				ChangeState(GOBLIN_STATE.IDLE);
			}
			
			
		}

		//------------testing--------------
		if (Input.GetKeyDown (KeyCode.J))
		{
			animController.SetBool("Talking", false);
			ChangeState(GOBLIN_STATE.WALK);
		}
		else if (Input.GetKeyDown(KeyCode.K))
		{
			animController.SetBool("Talking", false);
			ChangeState(GOBLIN_STATE.IDLE);
		}

		animController.SetFloat ("Blend", bl);
	}

	public void ChangeState(GOBLIN_STATE State)
	{
		ActiveState = State;
	}


	private void OnTalkToGoblin()
	{
		playerPosition = player.transform.position;
		ChangeState(GOBLIN_STATE.TALK);
		animController.SetBool("Talking", true);
	}

	private void OnEndTalkToGoblin()
	{
		animController.SetBool("Talking", false);
		ChangeState(GOBLIN_STATE.IDLE);
	}

	private void Die()
	{
		sound.PlayOneShot(goblin_death);
		animController.SetBool("Dead", true);
		ChangeState(GOBLIN_STATE.DEAD);
		GameObject.Find("GameManager").GetComponent<InterfaceScript>().GoblinPlayerKilledName = GetComponent<GoblinData>().goblinName;
	}

	//called from animation Goblin_Death, triggered at end of death animation
	private void EndGame()
	{
		if (GetComponent<GoblinData>().isMurderer)
		{
			GameObject.Find("GameManager").SendMessage("WinGame", SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			GameObject.Find("GameManager").SendMessage("LoseGame", SendMessageOptions.DontRequireReceiver);
		}
	}
}
