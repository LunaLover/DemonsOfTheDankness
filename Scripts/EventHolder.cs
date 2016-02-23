using UnityEngine;
using System.Collections;

public class EventHolder : MonoBehaviour 
{
	PlayerController pl;

	void Start ()
	{
		pl = transform.root.GetComponent<PlayerController> ();
	}

	public void ThrowProjectile()
	{
		pl.specialAttack = true;
	}
}
