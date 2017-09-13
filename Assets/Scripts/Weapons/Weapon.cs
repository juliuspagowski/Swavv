using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	public float RateOfFire;
	public float Damage;
	protected float TimeSinceLastFire;

	protected virtual void Start ()
	{
		if (Damage == 0.0f)
		{
			Debug.Log(gameObject.name + " has a damage value of 0, is this intentional?");
		}
		if (RateOfFire == 0.0f)
		{
			Debug.Log(gameObject.name + " has a rate of fire of 0, is this intentional?");
		}
		TimeSinceLastFire = RateOfFire;

	}

	protected virtual void Update ()
	{
		if (TimeSinceLastFire <= RateOfFire)
		{
			TimeSinceLastFire += Time.deltaTime;
		}
	}

	public virtual bool Fire()
	{
		if (CanFire())
		{
			TimeSinceLastFire = 0.0f;
			return true;
		}
		return false;
	}

	public virtual bool CanFire()
	{
		if (TimeSinceLastFire >= RateOfFire)
		{
			return true;
		}
		return false;
	}
}
