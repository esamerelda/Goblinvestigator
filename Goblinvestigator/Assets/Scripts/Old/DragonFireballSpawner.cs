using UnityEngine;
using System.Collections;

public class DragonFireballSpawner : MonoBehaviour {

	public GameObject fireballPrefab;

	private float fireballFrequency = 1.0f;
	private float fireballSpeed = 55.0f;
	private float fireballStartTime = 1.0f;

	private GameObject player;
	
	void Awake()
	{
		player = GameObject.Find("Player Capsule");
	}

	// Use this for initialization
	void Start () {
		InvokeRepeating("SpawnFireball", fireballStartTime, fireballFrequency);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void SpawnFireball()
	{
		//Rigidbody fireballInstance = Instantiate(fireballPrefab, transform.position, transform.rotation) as Rigidbody;
		//fireballInstance.AddRelativeForce((player.gameObject.transform.position - fireballInstance.transform.position) * fireballSpeed);

		GameObject fireball = Instantiate(fireballPrefab, transform.position, transform.rotation) as GameObject;
		//fireball.GetComponent<Rigidbody>().AddRelativeForce((player.transform.position - fireball.transform.position) * fireballSpeed);
		fireball.GetComponent<Rigidbody>().AddForce((player.transform.position - fireball.transform.position) * fireballSpeed);
	}
}
