using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

	// This script just makes things face the camera at all times

	public Camera cam;

	void Update()
	{
		//transform.LookAt(cam.transform.position);
		transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
	}
}
