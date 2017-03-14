using UnityEngine;
using System.Collections;

public class Container_Data : MonoBehaviour {


	private bool searched = false;
	public bool Searched
	{
		//both get & set called by playerInteraction
		get
		{
			return searched;
		}
		set
		{
			searched = value;
		}
	}

	//[SerializeField]
	private string containerText = "You found nothing useful in here.";
	public string ContainerText
	{
		get
		{
			return containerText;
		}
		set
		{
			//set by GameSetup
			containerText = value;
		}
	}
}
