using UnityEngine;
using System.Collections;

public class DragonMovement : MonoBehaviour {

	private float dragonSpeed = 35.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up * Time.deltaTime * dragonSpeed, Space.World);
	
	}
}
