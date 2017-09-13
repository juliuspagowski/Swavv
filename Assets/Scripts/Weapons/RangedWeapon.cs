using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
	public uint CurrentAmmo;
	public uint MagazineCapacity;
	public uint AmmoConsumedPerShot;
	public uint ShotsFiredPerTriggerPull;
	// Current reserve ammo.
	public int TotalAmmo;
	// How much reserve ammo the weapon should spawn with. Leave at 0 if the weapon should have infinite ammo.
	public uint MAXTOTALAMMO;

	public bool LoseAmmoOnReload;
	public float ProjectileSpeed;
	public GameObject SpawnLocation;
	public GameObject ProjectileToFire;


	protected override void Start()
	{
		if (MAXTOTALAMMO != 0)
		{
			TotalAmmo = (int)MAXTOTALAMMO;
		}
		base.Start();
	}

	protected override void Update()
	{
		base.Update();
	}

	public override bool Fire()
	{
		if (base.Fire())
		{
			//Debug.Log("We are in the beam");
			for (int i = 0; i < ShotsFiredPerTriggerPull; i++)
			{
				if (CurrentAmmo > 0)
				{
					GameObject T = Instantiate(ProjectileToFire, SpawnLocation.transform.position, SpawnLocation.transform.rotation);
					if (T.GetComponent<Projectile>())
					{
						T.GetComponent<Projectile>().Damage = Damage;
						T.GetComponent<Rigidbody>().AddForce(gameObject.transform.right * SpawnLocation.transform.localScale.x * ProjectileSpeed, ForceMode.Force);
					}

					int NewAmmoCount = (int)CurrentAmmo - (int)AmmoConsumedPerShot;
					if (NewAmmoCount < 0)
					{
						NewAmmoCount = 0;
					}
					CurrentAmmo = (uint)NewAmmoCount;
				}
			}

			return true;
		}
		return false;
	}

	public override bool CanFire()
	{
		if (HasAmmoInMagazine())
		{
			return base.CanFire();
		}
		return false;
	}

	public virtual bool HasAmmoInMagazine()
	{
		if (CurrentAmmo > 0)
		{
			return true;
		}
		return false;
	}

	public virtual void Reload()
	{
		if (MAXTOTALAMMO != 0)
		{
			if (TotalAmmo > 0 && CurrentAmmo < MagazineCapacity)
			{
				int AmmoToReload = (int)MagazineCapacity;

				if (!LoseAmmoOnReload)
				{
					AmmoToReload -= (int)CurrentAmmo;
				}
				else
				{
					CurrentAmmo = 0;
				}

				TotalAmmo -= AmmoToReload;
				if (TotalAmmo <= 0)
				{
					AmmoToReload += TotalAmmo;
					TotalAmmo += -TotalAmmo;
				}
				Debug.Log("Reloading " + AmmoToReload + " bullets");

				CurrentAmmo += (uint)AmmoToReload;
				return;
			}
			return;
		}
		CurrentAmmo = MagazineCapacity;


	}
}
