using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * WARNING: This code is provided as-is and is not guaranteed to work. You are
 * liable for any damages caused by this code. Credit is not required and this
 * code may be freely distributed with no license.
 *
 * Instructions
 *
 * Attach this script to any GameObject, and ensure that the object persists between
 * scenes. THIS SCRIPT SHOULD NOT BE MODIFIED (unless the compiler tells you otherwise)
 * to ensure that the kongregateUnitySupport object does not break.
 *
 * Usage
 *
 * To submit a stat simply call from anywhere:
 *
 *     KongregateAPI.Submit("MyStatisticName", VariableName);
 *
 * where "MyStatisticName" is a string literal of the same value you gave to Kongregate
 * while setting up your project and VariableName is a variable instance within your
 * code holding some arbitrary value.
 *
 * Do not submit stats often. Only submit stats during discrete events such as the player
 * dying, level completion, or boss deaths. As a rule of thumb you should avoid submitting
 * a stat every frame.
 *
 * Connection status logging passes the gameObject to the logger. This parameter should
 * be removed once you are ready to publish your game.
 */

/// <summary>
/// Provides quick access to Kongregate's API system, allowing the submission of stats. It is best to handle setup of this class
/// as soon as possible in your application.
/// </summary>
public class KongregateAPI : MonoBehaviour
{
	/// <summary>
	/// Static KongregateAPI instance.
	/// </summary>
	private static KongregateAPI Instance;

	/// <summary>
	/// Are we connected to Kongregate's API?
	/// </summary>
	public bool Connected { get; private set; }
	/// <summary>
	/// Are we connected to Kongregate's API?
	/// </summary>
	public static bool GetConnected() { return Instance != null ? Instance.Connected : false; }

	/// <summary>
	/// The user's unique identifier.
	/// </summary>
	public int UserId { get; private set; }

	/// <summary>
	/// The user's unique identifier.
	/// </summary>
	public static int GetUserId() { return Instance != null ? Instance.UserId : -1; }

	/// <summary>
	/// The user's Kongregate username.
	/// </summary>
	public string Username { get; private set; }
	/// <summary>
	/// The user's Kongregate username.
	/// </summary>
	public static string GetUsername() { return Instance != null ? Instance.Username : string.Empty; }

	/// <summary>
	/// The game's authentication token.
	/// </summary>
	public string GameAuthToken { get; private set; }

	/// <summary>
	/// The game's authentication token.
	/// </summary>
	public static string GetGameAuthToken() { return Instance != null ? Instance.GameAuthToken : string.Empty; }

	void Start()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Debug.LogWarning("You have created more than one KongregateAPI instance.");
			Destroy(gameObject);
			return;
		}

		Object.DontDestroyOnLoad(gameObject);
		gameObject.name = "KongregateAPI";

		Application.ExternalEval(
			@"if(typeof(kongregateUnitySupport) != 'undefined') {" +
				"kongregateUnitySupport.initAPI('KongregateAPI', 'OnKongregateAPILoaded');" +
			"}"
		);

		Debug.Log("Attempting to connect to Kongregate API...", gameObject);
	}

	/// <summary>
	/// Submit a value to the server.
	/// </summary>
	/// <param name="statisticName">The name of the statistic. This is the name provided in the "Statistic name" section when you fill in the API when uploading your game. See http://developers.kongregate.com/docs/kongregate-apis/stats for more details.</param>
	/// <param name="value">The value to submit (score, kills, deaths, etc...).</param>
	public static void Submit(string statisticName, int value)
	{
		if (Instance != null && Instance.Connected)
		{
			Application.ExternalCall("kongregate.stats.submit", statisticName, value);
		}
		else
		{
			Debug.LogWarning("You are attempting to submit a statistic without being connected to Kongregate's API. Connect first (by creating a GameObject and attaching this script to it), then submit.");
		}
	}

	public void OnKongregateAPILoaded(string userInfoString)
	{
		if (!Instance.Connected)
		{
			Debug.Log("Connection to Kongregate API successful.", gameObject);

			string[] parameters = userInfoString.Split('|');

			Instance.Connected = true;
			Instance.UserId = System.Convert.ToInt32(parameters[0]);
			Instance.Username = parameters[1];
			Instance.GameAuthToken = parameters[2];
		}
		else
		{
			Debug.LogWarning("Invalid call to KongregateAPI.OnKongregateAPILoaded(). Do not call this method manually.", gameObject);
		}
	}
}
