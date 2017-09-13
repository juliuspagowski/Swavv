using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character {

	public enum AimingState
	{
		foreground,
		background
	}
	public AimingState CurrentAimingState;
	public float SprintSpeed;

	// Use this for initialization
	protected override void Start ()
	{
		base.Start();
	}

	// Update is called once per frame
	protected override void Update ()
	{
		base.Update();

		//switch (CurrentAimingState)
		//{
		//	case AimingState.foreground:
		//		break;
		//	case AimingState.background:
		//		break;
		//	default:
		//		break;
		//}
	}

	public override void InvertX()
	{
		base.InvertX();
	}
}
