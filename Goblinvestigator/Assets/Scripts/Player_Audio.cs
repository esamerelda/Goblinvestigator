using UnityEngine;
using System.Collections;

public class Player_Audio : MonoBehaviour {

	private AudioSource source;

	private void PlaySound(AudioClip sound)
	{
		source.PlayOneShot(sound);
	}
}
