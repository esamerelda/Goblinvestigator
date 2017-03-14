using UnityEngine;
using System.Collections;

public class ShowGizmos : MonoBehaviour {

	public float reachDist = 1.0f;

	void OnDrawGizmos()
	{
		GameObject[] array_walkingPoints = GameObject.FindGameObjectsWithTag("WalkingPoint");
		foreach (GameObject point in array_walkingPoints)
		{
			Gizmos.DrawSphere(point.transform.position, reachDist);
		}
	}
}
