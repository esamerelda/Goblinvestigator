using UnityEngine;
using UnityEngine.UI;

public class MonsterHealth : MonoBehaviour {
    public static int health = 100;
    public GameObject Monster;
    public Slider monsterHealthBar;

    // Use this for initialization
    void Start () {
        InvokeRepeating("ReduceHealth", 1, 1);
    }

    void ReduceHealth()
    {
        health = health+2;
        monsterHealthBar.value = health;
        if(health <= 0)
        {

        }
    }
    // Update is called once per frame
    void Update () {
		
    }
}
