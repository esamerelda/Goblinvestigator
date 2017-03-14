using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class InterfaceScript : MonoBehaviour {

	private bool gameStarted;
	private bool paused;

	public Button b_titlePlay;

	public Canvas controlScreen;
	public Canvas creditScreen;
	public Canvas dialogueScreen;
	public Canvas introScreen;
	public Canvas journalScreen;
	public Canvas loseScreen;
	public Canvas pauseScreen;
	public Canvas quitScreen;
	public Canvas titleScreen;
	public Canvas winScreen;

	//these canvases aren't really "screens"
	public Canvas crosshairs;
	public Canvas infoBox;

	private Dictionary<string, string> dialogueResponses_2 = new Dictionary<string, string>();
	private Dictionary<string, string> dialogueResponses_3 = new Dictionary<string, string>();

	private Dictionary<string, GoblinData> goblins = new Dictionary<string, GoblinData>();
	public Dictionary<string, GoblinData> Goblins {
		set
		{
			goblins = value;
			//once we have a reference to each goblin in the scene, we can create the Journal Screen via code:
			foreach (KeyValuePair<string, GoblinData> g in goblins)
			{
				GameObject newPanel = Instantiate(prefab_suspectPanel);
				newPanel.transform.SetParent(JournalScreen_Parent, false);
				newPanel.transform.Find("Name").GetComponent<Text>().text = g.Key;

				//for testing journal ui with more goblins in the scene
				/*GameObject newPanel2 = Instantiate(prefab_suspectPanel);
				newPanel2.transform.SetParent(JournalScreen_Parent, false);
				newPanel2.transform.Find("Name").GetComponent<Text>().text = g.Key;*/
			}
			GameObject newButton = Instantiate(prefab_backButton);
			newButton.transform.SetParent(JournalScreen_Parent, false);
			newButton.GetComponent<Button>().onClick.AddListener(() => { GameObject.Find("GameManager").GetComponent<InterfaceScript>().DisableScreen(journalScreen); });
		}
	}

	public Dropdown Dropdown_2;
	public Dropdown Dropdown_3;

	private List<Canvas> list_screens = new List<Canvas>();

	private enum PlayerState { none, talking}
	private PlayerState playerState;

	public GameManager gameManager;

	public GameObject panel2;
	public GameObject panel3;
	public GameObject prefab_backButton;
	public GameObject prefab_suspectPanel;
	//public VerticalLayoutGroup js_vlg;

    public int gamestate = 1;

	private NotificationsManager Notifications = null;

	


	private List<string> list_dialogueDropdownChoices = new List<string>();
	public List<string> List_DialogueDropdownChoices
	{
		//set by PlayerInteraction
		set
		{
			list_dialogueDropdownChoices = value;
			Dropdown_2.options.Clear();
			Dropdown_3.options.Clear();
			int gobNum = 0;
			foreach (string s in list_dialogueDropdownChoices)
			{
				if (s != goblinA_Name)		//prevent from asking goblin A about themself
				{
					Dropdown_2.options.Add(new Dropdown.OptionData() { text = s });
					Dropdown_3.options.Add(new Dropdown.OptionData() { text = s });
					gobNum++;
				}
			}
			if (gobNum > 0)		//if player has spoken to goblins other than goblin A, enable 2nd and 3rd dialogue options
			{
				panel2.SetActive(true);
				Dropdown_2.value++;         //prevents player for clicking non-existant option
				panel3.SetActive(true);
				Dropdown_3.value++;			//prevents player for clicking non-existant option
			}
			else
			{
				panel2.SetActive(false);
				panel3.SetActive(false);
			}
		}
	}

	private string dialogueChoice_Victim;
	public string DialogueChoice_Victim
	{
		//set by GameSetup
		set
		{
			dialogueChoice_Victim = "What do you think of " + value + "?";
			Text_DialogueChoice_1.text = dialogueChoice_Victim;             //set UI text for Option 1 to display victim's name
		}
	}

	private string dialogueChoice_thinkOfVictim;
	public string DialogueChoice_thinkOfVictim
	{
		//set by GameSetup
		set
		{
			dialogueChoice_thinkOfVictim = " think of " + value + "?";
			Text_DialogueChoice_3.text = dialogueChoice_thinkOfVictim;      //set UI text for Option 3 to display victim's name
		}
	}


	private string dialogueTextOutput;
	public string DialogueTextOutput
	{
		get
		{
			return goblinNotes;
		}
		set
		{
			dialogueTextOutput = value;
			//Text_DialogueOutput.text = Text_DialogueOutput.text + "\n" + dialogueTextOutput;		//adds new stuff top to bottom
			goblinNotes = dialogueTextOutput + "\n" + Text_DialogueOutput.text;     //adds new stoff bottom to top
			Text_DialogueOutput.text = goblinNotes;
		}
	}


	private string goblinA_Name;
	public string GoblinA_Name
	{
		//set by PlayerInteraction
		set
		{
			goblinA_Name = value;
			Text_GoblinA_Name.text = goblinA_Name;      //set the name of the goblin you're talking to in the UI
														//once we have the name, we can load the dictionaries with their responses to the questions
			dialogueResponses_2 = goblins[goblinA_Name].goblinDialogue2;
			dialogueResponses_3 = goblins[goblinA_Name].goblinDialogue3;
		}
	}


	private string goblinNotes;

	//only called by a goblin script if a goblin was killed by the player who is not the murderer
	private string goblinPlayerKilledName;
	public string GoblinPlayerKilledName
	{
		set
		{
			goblinPlayerKilledName = value;
			//set lose screen text
			Text_LoseReason.text = "You killed " + goblinPlayerKilledName + ", but " + murdererName + " was the murderer.";
		}
	}


	private string infoBoxText;
	public string InfoBoxText
	{
		//called by PlayerInteraction.  Controlls info box on the bottom.
		set
		{
			infoBoxText = value;
			if (infoBoxText == "")
			{
				infoBox.enabled = false;
			}
			else
			{
				if (!infoBox.enabled)
				{
					Text_InfoBox.text = infoBoxText;
					infoBox.enabled = true;
				}
			}
		}
	}

	private string murdererName;
	public string MurdererName
	{
		set
		{
			murdererName = value;
		}
	}

	private string victimName;
	public string VictimName
	{
		//set by GameSetup
		set
		{
			victimName = value;
			Text_Intro.text = Text_Intro.text + "\n\n The victim's name is " + victimName + ".";
		}
	}

	public RectTransform JournalScreen_Parent;

	public Text Text_DialogueChoice_1;
	public Text Text_DialogueChoice_3;
	public Text Text_DialogueOutput;			//area where text output goes
	public Text Text_GoblinA_Name;
	public Text Text_InfoBox;
	public Text Text_Intro;
	public Text Text_LoseReason;
	public Text Text_PlayButton;
	public Text Text_WinText;

	//public Text Title;


	void Awake()
	{
		Notifications = GameObject.Find("GameManager").GetComponent<NotificationsManager>();
		Notifications.AddListener(this, "OnGameSetupComplete");
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

		gameStarted = false;
		playerState = PlayerState.none;

		//add screens to list at the same time
		list_screens.Clear();
		list_screens.Add(controlScreen);
		list_screens.Add(creditScreen);
		list_screens.Add(dialogueScreen);
		list_screens.Add(infoBox);
		list_screens.Add(introScreen);
		list_screens.Add(journalScreen);
		list_screens.Add(loseScreen);
		list_screens.Add(pauseScreen);
		list_screens.Add(quitScreen);
		list_screens.Add(titleScreen);
		list_screens.Add(winScreen);

		DisableAllScreensExcept(titleScreen);
		crosshairs.enabled = false;
		Text_WinText.text = "You have avenged " + victimName + "'s untimely death by executing " + murdererName + "! \n The party continues, and the goblins sing horrible songs of your investigatory prowess.";
		
	}


	void Start()
	{
		Notifications.AddListener(this, "OnTalkToGoblin");
		PauseTrue();
		
		
	}

	void Update()
	{
		//if game has started and player presses Pause button (set to Esc in Edit > Project Settings > Input)
		//looks complicated, but makes it so all this code isn't being run every single frame.
		if (gameStarted && Input.GetButtonDown("Journal") && (playerState != PlayerState.talking))
		{
			if (!journalScreen.enabled)
			{
				EnableScreen(journalScreen);
			}
			else
			{
				DisableAllScreens();
				PauseFalse();
			}
		}


		if (gameStarted && Input.GetButtonDown("Pause") && (playerState != PlayerState.talking))
		{
            //toggle pause
			paused = !paused;
            

			//if pause = true
			if (paused)
			{
				//pause the game and enable the pause screen
				PauseTrue();
				EnableScreen(pauseScreen);
			}
			else
			{
				PauseFalse();
				DisableAllScreens();
			}
		}
	}


	//called by PlayerController to freeze player movements when game is paused
	public bool CheckPause()
	{
		return paused;
	}


	public void Dialogue1()
	{
		DialogueTextOutput = goblins[goblinA_Name].victimDialogue;
	}


	public void Dialogue2()
	{
		string goblinB_name = Dropdown_2.captionText.text;			//get name of Goblin B you are asking Goblin A about
		DialogueTextOutput = dialogueResponses_2[goblinB_name];		//get appropriate response
	}


	public void Dialogue3()
	{
		string goblinB_name = Dropdown_3.captionText.text;          //get name of Goblin B you are asking Goblin A about
		DialogueTextOutput = dialogueResponses_3[goblinB_name];     //get appropriate response
	}


	public void DisableAllScreens()
	{
		foreach(Canvas screen in list_screens)
		{
			screen.enabled = false;
		}
	}


	public void DisableAllScreensExcept(Canvas exception)
	{
		foreach (Canvas screen in list_screens)
		{
			if (screen != exception)
			{
				screen.enabled = false;
			}
			else
			{
				screen.enabled = true;
			}
		}
	}


	public void DisableInfoBox()					//needs own function in order to be interactive from other scripts
	{
		infoBox.enabled = false;
	}


	public void DisablePopup(Canvas screen)
	{
		screen.enabled = false;
	}


	public void DisableScreen(Canvas screen)
	{
		screen.enabled = false;
		PauseFalse();
	}


	public void EnablePopup(Canvas screen)
	{
		screen.enabled = true;
	}

	
	public void EnableScreen(Canvas screen)
	{
		PauseTrue();
		DisableAllScreensExcept(screen);
	}


	public void LoseGame()
	{
		Debug.Log("You Lose!");
		EnableScreen(loseScreen);
	}


	public void OnEndTalkToGoblin()
	{
		
		DisableScreen(dialogueScreen);

		Notifications.PostNotification(this, "OnEndTalkToGoblin");
		//PauseFalse();
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		playerState = PlayerState.none;

	}


	public void OnGameSetupComplete()
	{
		b_titlePlay.interactable = true;
		Text_PlayButton.text = "Play!";
		//Debug.Log("WTF");
	}


	private void OnTalkToGoblin()
	{
		DisablePopup(infoBox);
		//Set up dialogue screen
		playerState = PlayerState.talking;
		Text_DialogueOutput.text = "";			//TODO - replace "" with variable containing previously generated dialogue
		dialogueScreen.enabled = true;          //actually enable the screen
												//PauseTrue();							//pause the game
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}


	private void PauseFalse()
	{
		Time.timeScale = 1;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		paused = false;
		crosshairs.enabled = true;
	}


	private void PauseTrue()
	{
		Time.timeScale = 0;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		paused = true;
		crosshairs.enabled = false;
	}


	public void RestartGame()
	{
		DisableAllScreens();
		//paused = false;
		//PauseFalse();
		//gameStarted = true;
		//gamestate = 1;
		SceneManager.LoadScene("MainLevel");
		
	}


	public void StartGame()
	{
		DisableAllScreens();
        PauseFalse();
        gameStarted = true;
		crosshairs.enabled = true;
    }


	public void QuitGame()
	{
		Debug.Log("GAME QUIT");
		Application.Quit();
	}


	public void WinGame()
	{
		Debug.Log("You Win!");
		EnableScreen(winScreen);
    }
}
