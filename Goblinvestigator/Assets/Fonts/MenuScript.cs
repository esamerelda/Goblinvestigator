using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    public Canvas quitMenu;
    public Button startText;
    public Button exitText;
    public Button controlText;
    public Canvas controlMenu;
	public Canvas startMenu;


	void Awake()
	{
        quitMenu = quitMenu.GetComponent<Canvas>();
		controlMenu = controlMenu.GetComponent<Canvas>();
		startMenu = startMenu.GetComponent<Canvas>();
		startText = startText.GetComponent<Button>();
		exitText = exitText.GetComponent<Button>();
		controlText = controlText.GetComponent<Button>();

		controlMenu.enabled = false;
		quitMenu.enabled = false;
	}

	// Use this for initialization
	void Start () {
        quitMenu = quitMenu.GetComponent<Canvas>();
        controlMenu = controlMenu.GetComponent<Canvas>();
		startMenu = startMenu.GetComponent<Canvas>();
        startText = startText.GetComponent<Button>();
        exitText = exitText.GetComponent<Button>();
        controlText = controlText.GetComponent<Button>();
        //quitMenu.enabled = false;
        //controlMenu.enabled = false;
	}
    public void BackPress()
    {
        quitMenu.enabled = false;
        controlMenu.enabled = false;
        startText.enabled = true;
        exitText.enabled = true;
        controlText.enabled = true;
    }
	public void ExitPress()
    {
		Debug.Log("EXIT");
        quitMenu.enabled = true;
        controlMenu.enabled = false;
        startText.enabled = false;
        exitText.enabled = false;
        controlText.enabled = false;

    }
    public void ControlPress()
    {
        quitMenu.enabled = false;
        controlMenu.enabled = true;
        startText.enabled = false;
        exitText.enabled = false;
        controlText.enabled = false;
    }
    public void NoPress()
    {
        controlMenu.enabled = false;
        quitMenu.enabled = false;
        startText.enabled = true;
        exitText.enabled = true;
        controlText.enabled = true;

		//if (gm.CheckForGameStart())
		//{
		//	gm.UnPauseGame();
		//}
    }
    public void StartLevel()
    {
		//SceneManager.LoadScene("GameScene");
		startMenu.enabled = false;
		controlMenu.enabled = false;
		quitMenu.enabled = false;
		//gm.UnPauseGame();
		//gm.StartGame();


    }
    public void ExitGame()
    {
        Application.Quit();
    }
	// Update is called once per frame
	//void Update () {
	
	//}
}
