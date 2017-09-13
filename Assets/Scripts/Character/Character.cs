using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(CapsuleCollider))]
public class Character : MonoBehaviour {

	protected Rigidbody RigidBody;
	public int InventorySize;
	public float Health;
	public float MaxHealth;
	public float MoveSpeed;
	public float JumpHeight;
	public Transform GroundCheck;
	public LayerMask IsOnGround;            // LayerMask detects whether or not it is over an object on layer 9(ground) to set whether isGrounded is true/false
	public bool IsGrounded;
	public Transform WeaponSocket;
	protected Weapon CurrentlyEquippedWeapon;
	protected Weapon[] WeaponInventory = new Weapon[5];
	protected bool IsFacingLeft; 

	protected virtual void Start ()
	{
		RigidBody = GetComponent<Rigidbody>();
		CurrentlyEquippedWeapon = GetComponentInChildren<Weapon>();
		WeaponSocket = transform.Find("GunSocket");
		Health = MaxHealth;
		//RigidBody.
		if (transform.localScale.x >= 0.0f)
		{
			IsFacingLeft = false;
		}
		else
		{
			IsFacingLeft = true;
		}
	}

	protected virtual void Update ()
	{
		IsGrounded = Physics.Linecast(GroundCheck.transform.position, GroundCheck.transform.position + new Vector3(0.0f, -0.15f, 0.0f), IsOnGround);
	}

	public virtual void AddInputVector(Vector3 InInputVector)
	{
		RigidBody.AddForce(InInputVector, ForceMode.Force);
	}

	public virtual void SetVelocity(Vector3 InVelocityVector)
	{
		
	}

	public virtual void AddImpulseVector(Vector3 InImpulseVector)
	{
		RigidBody.AddForce(InImpulseVector, ForceMode.Impulse);
	}

	public virtual float TakeDamage(float Damage, DamageType TypeOfDamage, GameObject DamageCauser)
	{
		float NewHealth = Health - Damage;
		if (NewHealth >= 0.0f)
		{
			NewHealth = 0.0f;
			OnFatalDamageTaken();
		}


		return NewHealth;
	}

	public virtual void OnFatalDamageTaken()
	{

	}

	public virtual void InvertX()
	{
		IsFacingLeft = !IsFacingLeft;

		Vector3 invert = GetComponentInChildren<SpriteRenderer>().transform.localScale;
		invert.x *= -1;

		GetComponentInChildren<SpriteRenderer>().transform.localScale = invert;

		Vector3 WeaponPosition = WeaponSocket.transform.localPosition;
		WeaponSocket.transform.localPosition = new Vector3(-1 * WeaponPosition.x, WeaponPosition.y, WeaponPosition.z);
		if (CurrentlyEquippedWeapon.GetType() == typeof(RangedWeapon))
		{
			RangedWeapon RecastWeapon = (RangedWeapon)CurrentlyEquippedWeapon;
			RecastWeapon.SpawnLocation.transform.localScale = transform.localScale;
			
		}
	}

	public virtual Weapon GetCurrentlyEquippedWeapon()
	{
		if (CurrentlyEquippedWeapon != null)
		{
			return CurrentlyEquippedWeapon;
		}
		return null;
	}

	public virtual Weapon[] GetWeaponInventory()
	{
		return WeaponInventory;
	}

	public virtual bool GetIsFacingLeft()
	{
		return IsFacingLeft;
	}

}
