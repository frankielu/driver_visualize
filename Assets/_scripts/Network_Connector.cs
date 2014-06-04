using UnityEngine;
using System.Collections;

public class Network_Connector : MonoBehaviour {
	
	void Start () 
	{
		string url = "0.0.0.0:8000";
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
			Debug.Log("WWW Working: " + www.text);
		}
		else
		{
			Debug.Log("WWW Error: " + www.error);
		}
	}
}