using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

	private Animator anim;
	private void Awake()
	{
		anim = GetComponent<Animator> ();
	}

	private void OnEnable()
	{
		anim.SetTrigger ("Spawn");
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") 
		{
			gameManager.Instance.GetCoin ();
			anim.SetTrigger ("Collected");
		}
	}
}
