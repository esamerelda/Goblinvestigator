using UnityEngine;
using System.Collections;
using System.Linq;

public class Victim_Data : MonoBehaviour {

	[SerializeField]
	private string inspectionText = "Not filled";
	public string InspectionText
	{
		get { return inspectionText; }
		set { inspectionText = value; }
	}

	//private Material[] array_mats;
	//private int skindex;

	void Awake()
	{
		//array_mats = Resources.LoadAll("Materials", typeof(Material)).Cast<Material>().ToArray();
	}

	void Start()
	{
		//skindex = 0;
	}

	void Update()
	{
		//int skindex = Random.Range(0, array_goblinMats.Length);
		//goblin.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = array_goblinMats[skindex];
		/*gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = array_mats[skindex];
		skindex++;
		if (skindex > array_mats.Length - 1)
		{
			skindex = 0;
		}
		Debug.Log(skindex);*/
	}
}
