using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DragonHealth : MonoBehaviour {

	public static int health = 100;
	//public GameObject dragon;
	public Slider healthBar;


	private int damageFromWeapons = 20;

	private InterfaceScript uiScript;

	public AudioClip dragonHurtSound;
	private SoundManager sound;

	void Awake()
	{
		sound = GameObject.Find("SoundManager").GetComponent<SoundManager>();
		uiScript = GameObject.Find("UI").GetComponent<InterfaceScript>();
	}

	// Use this for initialization
	void Start () {
		UpdateHealth();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void CheckForDeath()
	{
		if (health <= 0)
		{
			//die
			//Destroy(transform.parent.gameObject);
			uiScript.WinGame();
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Item")
		{
			//Debug.Log("Dragon hit with weapon");
			TakeDamage(damageFromWeapons);
			sound.PlaySound(dragonHurtSound);
			GameObject item = other.gameObject;
			Rigidbody itemRb = item.GetComponent<Rigidbody>();
			item.transform.parent = transform;
			itemRb.isKinematic = true;
		}
	}

	private void TakeDamage(int damage)
	{
		health = health - damage;
		UpdateHealth();
		CheckForDeath();
	}

	private void UpdateHealth()
	{
		healthBar.value = health;
	}
}
