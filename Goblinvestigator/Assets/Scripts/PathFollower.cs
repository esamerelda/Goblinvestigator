using UnityEngine;
using System.Collections;

public class PathFollower : MonoBehaviour {

	public Transform[] path;
	public float speed = 5.0f;
	public float reachDist = 1.0f;
	public int currentPoint = 0;
	
	
	// Update is called once per frame
	void Update () {
        
        Vector3 dir = path[currentPoint].position - transform.position;

        transform.position += dir * Time.deltaTime * speed;
        if(dir.magnitude <= reachDist)
        {
            currentPoint++;
			if (currentPoint >= path.Length)
			{
				currentPoint = 0;
			}
			//transform.Rotate(Vector3.up * (40));
			transform.LookAt(path[currentPoint].position);

        }
        
	}

    void OnDrawGizmos()
    {
		GameObject[] array_walkingPoints = GameObject.FindGameObjectsWithTag("WalkingPoint");
		foreach (GameObject point in array_walkingPoints)
		{
			Gizmos.DrawSphere(point.transform.position, reachDist);
		}
        if(path.Length > 0)
        {
            for(int i = 0; i < path.Length; i++)
            {
                if(path[i] != null)
                {
                    Gizmos.DrawSphere(path[i].position, reachDist);
                    
                }
            }
        }
    }
}
