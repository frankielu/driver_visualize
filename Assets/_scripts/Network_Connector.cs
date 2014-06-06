using UnityEngine;
using System.Collections;

using System;
using System.Linq;
using System.IO;

public class Network_Connector : MonoBehaviour 
{
	public GUIText errorDialog;
	public static bool networkEnabled = false;
	public static float[] lastDataReceived = new float[0];

	private static string _IPAddress = "128.54.38.18:8000";
	// add input validation to lower http timeout rates
	private static string url;

	public static string IPAddress
	{
		get { return _IPAddress; }
		set 
		{ 
			_IPAddress = value.Trim();
			url = "http://" + _IPAddress + "/singleData.txt";
		}
	}

	void FixedUpdate()
	{
		if (networkEnabled != false) readTextFile(url);
	}

	void readTextFile(string url)
	{
		WWW www = new WWW (url);
		StartCoroutine (WaitForRequest (www));
	}

	// get request
	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;

		// check for errors
		if (www.error == null)
		{
			lastDataReceived = www.text.Split(',').Select(x => float.Parse(x)).ToArray();
			errorDialog.text = String.Empty;
		}
		else
		{
			Debug.Log("WWW Error: " + www.error);
			errorDialog.text = "Error Retriving Data";
		}
	}
}