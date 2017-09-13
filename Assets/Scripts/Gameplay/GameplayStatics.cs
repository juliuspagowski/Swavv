using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayStatics : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public static void ApplyDamage(GameObject DamagedCharacter, float Damage, DamageType TypeOfDamage, GameObject DamageCauser)
	{
		if (DamagedCharacter.GetComponent<Character>())
		{
			DamagedCharacter.GetComponent<Character>().TakeDamage(Damage, TypeOfDamage, DamageCauser);
		}
		else
		{
			Debug.Log("Attempted to damage a " + DamagedCharacter.name + " but it isn't a damagable character! Invoked by " + DamageCauser);
		}
	}
}
