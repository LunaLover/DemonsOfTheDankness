using UnityEngine;
using System.Collections;

public class DamageScript : MonoBehaviour 
{
	void OnTriggerEnter2D(Collider2D col)
	{
		//Checks to make sure we aren't hitting anything but the enemy.
		if(col.transform.root != transform.root && col.tag != "Ground" && !col.isTrigger)
		{
			if(!col.transform.GetComponent<PlayerController>().damage)
			{
				col.transform.GetComponent<PlayerController>().damage = true;
				col.transform.root.GetComponentInChildren<Animator>().SetTrigger ("Damage");
			}
		}
	}
}
