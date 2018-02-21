using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GivePlayerSword : MonoBehaviour {

	public GameObject SWORD_IN_STONE;
	public GameObject SWORD_ON_PLAYER;

	private bool _actionCompleted = false;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && !_actionCompleted)
		{
			_actionCompleted = true;
			/* Hide the sword in the stone */
			SWORD_IN_STONE.SetActive(false);
			/* Give the sword to the player */
			SWORD_ON_PLAYER.SetActive(true);
		}
	}
}
