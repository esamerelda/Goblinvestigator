using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour {
	
	//TODO - Add enum thing to set state i.e running, crouching, walking, standing, jumping

	CharacterController cc;

	//Know when to stop firing and moving camera
	//private GameObject UI;
	private InterfaceScript UIscript;
	private NotificationsManager Notifications;
	//private GameManager gameManager;

	//to know if we need to invert y axis on camera
	private GameObject mainMenu;
	//private MenuScript mainMenuScript;

	float crouchSpeed = 5f;
	float walkSpeed = 10.0f;
	float runSpeed = 20.0f;

	float mouseSensitivityX = 5.0f;
	float mouseSensitivityY = 5.0f;

	float vertRotation = 0;
	float axisRangeY = 60.0f;

	float forwardSpeed = 0f;
	float sideSpeed = 0f;
	float verticalVelocity = 0;
	float jumpSpeed = 10.0f;
	//public bool canMove;
	private bool talking = false;


	void Awake(){
		//UI = GameObject.Find ("UI");
		UIscript = GameObject.Find("GameManager").GetComponent<InterfaceScript>();
		Notifications = GameObject.Find("GameManager").GetComponent<NotificationsManager>();
		//gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

		//mainMenu = GameObject.Find ("MainMenu");
		//mainMenuScript = mainMenu.GetComponent<MenuScript>();
	}	

	// Use this for initialization
	void Start () {
		//GameManager.Instance.notifications
		Notifications.AddListener(this, "OnTalkToGoblin");
		Notifications.AddListener(this, "OnEndTalkToGoblin");
		cc = GetComponent<CharacterController>();
		talking = false;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(talking);

		//if the world isn't paused due to pausing or menu-ing...
		if ((!UIscript.CheckPause()) && (!talking))
		{	
			//x-axis player/camera rotation
			float rotationX = Input.GetAxis ("Mouse X") * mouseSensitivityX;
			transform.Rotate (0, rotationX, 0);

			//y-axis player/camera rotation
			/*if (mainMenuScript.yAxisInverted){
				vertRotation -= Input.GetAxis ("Mouse Y") * -mouseSensitivityY;
			}else{
				vertRotation -= Input.GetAxis ("Mouse Y") * mouseSensitivityY;
			}*/
			vertRotation -= Input.GetAxis("Mouse Y") * mouseSensitivityY;

			vertRotation = Mathf.Clamp(vertRotation, -axisRangeY, axisRangeY);
			Camera.main.transform.localRotation = Quaternion.Euler(vertRotation, 0, 0);

			//if the player is currently on the ground...
			if (cc.isGrounded)
			{
				//if player is running
				if (Input.GetButton("Left Shift"))
				{
					forwardSpeed = Input.GetAxis("Vertical") * runSpeed;
					sideSpeed = Input.GetAxis("Horizontal") * walkSpeed;
				}
				//if player is crouching
				else if(Input.GetButton("Left Ctrl")){
					//TODO - move camera down while this is true
					forwardSpeed = Input.GetAxis("Vertical") * crouchSpeed;
					sideSpeed = Input.GetAxis("Horizontal") * crouchSpeed;
				}
				//if player is walking normally, or standing still
				else
				{
					forwardSpeed = Input.GetAxis("Vertical") * walkSpeed;
					sideSpeed = Input.GetAxis("Horizontal") * walkSpeed;
				}
			}
			else
			{
				verticalVelocity += Physics.gravity.y * Time.deltaTime;
			}


			//if player tries to jump
			if (Input.GetButtonDown("Space") && cc.isGrounded)
			{
				verticalVelocity = jumpSpeed;
				//forwardSpeed = forwardSpeed / 2;
			}

			Vector3 speed = new Vector3(sideSpeed, verticalVelocity, forwardSpeed);

			speed = transform.rotation * speed;

			cc.Move(speed * Time.deltaTime);
		}
		

		

		
		//}
	}
	private void OnEndTalkToGoblin()
	{
		talking = false;
	}
	private void OnTalkToGoblin()
	{
		talking = true;
	}
}
