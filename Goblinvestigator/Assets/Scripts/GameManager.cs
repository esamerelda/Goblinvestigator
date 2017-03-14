using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GameSetup))]
[RequireComponent (typeof (NotificationsManager))]
[RequireComponent(typeof(InterfaceScript))]
public class GameManager : MonoBehaviour {


	private static GameManager instance = null;
	public static GameManager Instance
	{
		get
		{
			if (instance == null) instance = new GameObject("GameManager").AddComponent<GameManager>();
			return instance;
		}
	}

	private bool inputAllowed;
	public bool InputAllowed
	{
		get
		{
			return inputAllowed;
		}
		set
		{
			inputAllowed = value;
		}
	}

	private static GameSetup gameSetup = null;
	public static GameSetup GameSetup
	{
		get
		{
			if (gameSetup == null) gameSetup = Instance.GetComponent<GameSetup>();
			return gameSetup;
		}
	}

	private static InterfaceScript interfaceScript = null;
	public static InterfaceScript UiScript
	{
		get
		{
			if (interfaceScript == null) interfaceScript = Instance.GetComponent<InterfaceScript>();
			return interfaceScript;
		}
	}
	private static NotificationsManager notifications = null;
	public static NotificationsManager Notifications
	{
		get
		{
			if (notifications == null) notifications = Instance.GetComponent<NotificationsManager>();
			return notifications;
		}
	}

	//internal ref to single active instance of object for singleton behavior
	

	//internal ref to notifications object
	

	void Awake()
	{
		//check if there is an existing instance of this object
		/*if((instance) && (instance.GetInstanceID() != GetInstanceID()))
		{
			DestroyImmediate(gameObject);  //delete duplicate
		}
		else
		{
			instance = this; //make this gameobj. the only instance
			DontDestroyOnLoad(gameObject);
		}*/

		inputAllowed = true;
	}

	void Start()
	{
		//Notifications.AddListener(this, "OnTalkToGoblin");
		//Notifications.AddListener(this, "OnEndTalkToGoblin");
	}

	private void OnTalkToGoblin()
	{
		//PauseGame();
	}

	private void OnEndTalkToGoblin()
	{
		//UnPauseGame();
	}

	private void PauseGame()
	{
		//InputAllowed = false;
	}

	private void UnPauseGame()
	{
		//InputAllowed = true;
	}

}
