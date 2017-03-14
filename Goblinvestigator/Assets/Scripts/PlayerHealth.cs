using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
    public static int health = 100;
	private InterfaceScript uiScript;
	public GameObject player;
    public Slider healthBar;

	private int damageFromDragon = 30;
	private int damageFromFireball = 15;

	public AudioClip dragonCollisionSound;
	public AudioClip fireHitSound;
	private SoundManager sound;


	void Awake()
	{
		//sound = GameObject.Find("SoundManager").GetComponent<SoundManager>();
		uiScript = GameObject.Find("UI").GetComponent<InterfaceScript>();
	}

	// Use this for initialization
	void Start () {
		//increases health every second.  (Function, how long before it starts, how often it repeats.)
        //InvokeRepeating("IncreaseHealth", 1, 1);
	}
	
    void IncreaseHealth()
    {
        health = health + 1;
		UpdateHealth();
    }

	public void TakeDamage(int damage)
	{
		health = health - damage;
		UpdateHealth();
		CheckForDeath();
	}

	// Update is called once per frame
	void Update () {
	
	}

	void CheckForDeath()
	{
		if (health <= 0)
		{
			//die
			uiScript.LoseGame();
		}
	}

	void OnCollisionEnter(Collision other)
	{
		//dragon
		if (other.gameObject.tag == "Monster")
		{
			//Debug.Log("You collided with the dragon");
			//sound.PlaySound(dragonCollisionSound);
			TakeDamage(damageFromDragon);
		}

		//fireballs
		if (other.gameObject.tag == "Fireball")
		{
			Debug.Log("You have been hit with a fireball");
			//sound.PlaySound(fireHitSound);
			TakeDamage(damageFromFireball);
		}
	}

	private void UpdateHealth()
	{
		healthBar.value = health;
	}
}
