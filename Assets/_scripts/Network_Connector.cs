/// <summary>
/// Network_Connector.cs
/// 
/// Provides a connection to a server via a web client and retrieves data by sending
/// out HTTP requests. We will use this to retrieve vehicle data.
/// </summary>

using UnityEngine;
using System.Collections;

using System;
using System.Linq;
using System.IO;

public class Network_Connector : MonoBehaviour 
{
	public GUIText errorDialog;
	public static bool isNetworkEnabled = false;
	public static float[] lastDataReceived = new float[0];

	private static string _IPAddress = "169.228.152.107:8000"; // default ip to open socket with
	// xxx - add input validation to lower http timeout rates
	private static string url;
	
	public static string IPAddress
	{
		get { return _IPAddress; }
		set 
		{ 
			// if the ip changes, change the url to reflect it
			_IPAddress = value.Trim();
			url = "http://" + _IPAddress + "/singleData.txt";
		}
	}

	void FixedUpdate()
	{
		// if there is a valid connection, start reading data from a text file
		if (isNetworkEnabled != false) 
		{
			WWW www = new WWW (url);
			StartCoroutine (WaitForRequest (www));
		}
	}

	// get request
	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;

		if (www.error == null) // no errors
		{
			lastDataReceived = www.text.Split(',').Select(x => float.Parse(x)).ToArray();
			errorDialog.text = String.Empty;
		}
		else // there is an error
		{
			Debug.Log("WWW Error: " + www.error);
			errorDialog.text = "Error Retriving Data";
		}
	}
}