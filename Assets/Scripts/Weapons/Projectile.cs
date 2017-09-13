using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Projectile : MonoBehaviour {

	public float Damage;
	private float lifeTime = 7f;
	private float timeToDie;

	void Start()
	{
		timeToDie = Time.time + lifeTime;
	}

	void Update()
	{
		if (Time.time > timeToDie)
		{
			Destroy(gameObject);
		}
			
	}

	void OnCollisionEnter(Collision c)
	{
		if (c.gameObject.tag == "Player")
		{
			Destroy(gameObject);
		}

		GameplayStatics.ApplyDamage(c.gameObject, Damage, new DamageType(), gameObject);
		Destroy(gameObject);

	}

	public virtual bool GetUseGravity()
	{
		return GetComponent<Rigidbody>().useGravity;
	}

	public virtual void SetGravityScale(bool NewGravitySetting)
	{
		GetComponent<Rigidbody>().useGravity = NewGravitySetting;
	}
}
