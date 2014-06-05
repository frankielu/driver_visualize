using UnityEngine;
using System.Collections;

using System;
using System.Linq;
using System.IO;

public class Network_Connector : MonoBehaviour 
{
	public GUIText errorDialog;
	public static bool networkEnabled = false;
	public static float[] lastDataReceived = new float[7] {0.0f,0.0f,0.0f,0.0f,1.0f,1.0f,0.0f};

	private static string _IPAddress = "137.110.68.254:8000";
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