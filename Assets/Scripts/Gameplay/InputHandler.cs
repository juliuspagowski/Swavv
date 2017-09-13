using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

	Character PossessedCharacter;

	// Use this for initialization
	void Start ()
	{
		PossessedCharacter = GameObject.Find("Character").GetComponent<Character>();	
	}

	void FixedUpdate()
	{
		float xInput = Input.GetAxisRaw("Horizontal");
		if (PossessedCharacter)
		{
			
			//Vector3 InputVector = new Vector3(xInput, 0.0f, 0.0f);

			Vector3 PossessedCharacterRight = PossessedCharacter.transform.right;
			PossessedCharacterRight.y = 0f;
			PossessedCharacterRight = PossessedCharacterRight.normalized;

			if (PossessedCharacter.IsGrounded)
			{
				// If sprinting, use the SprintSpeed value for movement instead of the default movespeed.
				if (Input.GetButton("Sprint"))
				{
					PossessedCharacter.AddInputVector(PossessedCharacterRight * xInput * (PossessedCharacter as PlayerCharacter).SprintSpeed);
				}
				else
				{
					PossessedCharacter.AddInputVector(PossessedCharacterRight * xInput * PossessedCharacter.MoveSpeed);
				}
				
				if ((xInput > 0 && PossessedCharacter.GetIsFacingLeft()) || (xInput < 0 && !PossessedCharacter.GetIsFacingLeft()))
				{
					PossessedCharacter.InvertX();
				}
			}
		}
	}

	// Update is called once per frame
	void Update ()
	{
		Vector3 MousePosition = Input.mousePosition;
		if (PossessedCharacter)
		{
			if (Input.GetButtonDown("Jump") && PossessedCharacter.IsGrounded)
			{
				PossessedCharacter.AddImpulseVector(Vector2.up * PossessedCharacter.JumpHeight);
				//Debug.Log("Jump");
			}
			//if (Input.GetButtonDown("Jump") && PossessedCharacter.IsGrounded)
			//{
			//	PossessedCharacter.AddImpulseVector(Vector2.up * PossessedCharacter.JumpHeight);
			//	//Debug.Log("Jump");
			//}

			//Vector3 InputVector = new Vector3(xInput, 0.0f, 0.0f);


			if (Input.GetButtonDown("Fire1") && PossessedCharacter.GetCurrentlyEquippedWeapon() != null)
			{
				PossessedCharacter.GetCurrentlyEquippedWeapon().Fire();
			}

			if (Input.GetButtonDown("Reload") && PossessedCharacter.GetCurrentlyEquippedWeapon() != null)
			{
				if (PossessedCharacter.GetCurrentlyEquippedWeapon().GetType() == typeof(RangedWeapon))
				{
					(PossessedCharacter.GetCurrentlyEquippedWeapon() as RangedWeapon).Reload();
				}

			}
			if (PossessedCharacter.GetCurrentlyEquippedWeapon().GetType() == typeof(RangedWeapon))
			{
				Transform WeaponTransform = PossessedCharacter.GetCurrentlyEquippedWeapon().transform;
				switch ((PossessedCharacter as PlayerCharacter).CurrentAimingState)
				{
					case PlayerCharacter.AimingState.foreground:
						{ 
							Vector3 Position = Camera.main.WorldToScreenPoint(WeaponTransform.position);
							Vector3 Direction = Input.mousePosition - Position;
							float Angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;


							WeaponTransform.rotation = Quaternion.AngleAxis(Angle, Vector3.forward);
						}
						break;

					case PlayerCharacter.AimingState.background:
						{
							RaycastHit HitResult;
							Ray RayToCast = Camera.main.ScreenPointToRay(Input.mousePosition);
							if (Physics.Raycast(RayToCast, out HitResult, 100.0f))
							{
								//Debug.DrawLine(RayToCast.origin, HitResult.point, Color.red, 5f);

								Vector3 Position = WeaponTransform.position;
								Vector3 Direction = (HitResult.point - Position).normalized;
								Quaternion LookRotation;

								
								LookRotation = Quaternion.FromToRotation(Vector3.right, Direction);
								

								WeaponTransform.rotation = LookRotation;
							}
						}
						break;
					default:
						break;
				}
				

				if (Input.GetButton("Fire2") && PossessedCharacter.GetCurrentlyEquippedWeapon() != null)
				{
					(PossessedCharacter as PlayerCharacter).CurrentAimingState = PlayerCharacter.AimingState.background;
				}
				else
				{
					(PossessedCharacter as PlayerCharacter).CurrentAimingState = PlayerCharacter.AimingState.foreground;
				}
			}
			


		}

	}
}
