using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour {

    public GameObject textbox;
    public Text theText;

    public TextAsset textFile;
    public string[] textLines;

    public int currentLine;
    public int endAtLine;

    public PlayerMovement player;

    public bool isActive;
    public bool stopMovement;
    // Use this for initialization
    void Start()
    {
       // DisableTextBox();
        player = FindObjectOfType<PlayerMovement>();
        if (textFile != null)
        {
            textLines = (textFile.text.Split('\n'));
        }
        if(endAtLine == 0)
        {
            endAtLine = textLines.Length - 1;
        }
        if (isActive)
        {
            EnableTextBox();
        }else
        {
            DisableTextBox();
        }
  
    }

    void Update()
    {
        if (!isActive)
        {
            return;
        }
        theText.text = textLines[currentLine];

        if (/*Input.GetKeyDown(KeyCode.Space)*/Input.GetMouseButtonDown(0))
        {
            currentLine += 1;
        }
        if(currentLine > endAtLine)
        {
            DisableTextBox();
        }
    }
    public void EnableTextBox()
    {
        textbox.SetActive(true);
        theText.enabled = true;
        isActive = true;
        stopMovement = true;
        if (stopMovement)
        {
            //player.canMove = false;
        }
    }
    public void DisableTextBox()
    {
        textbox.SetActive(false);
        theText.enabled = false;
        isActive = false;

        //player.canMove = true;
    }
    public void ReloadScript(TextAsset theText)
    {
        if(theText != null)
        {
            textLines = new string[1];
            textLines = (theText.text.Split('\n'));
        }
    }
}
