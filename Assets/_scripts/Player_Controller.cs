using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Linq;

public class Player_Controller : MonoBehaviour 
{
	public GUIText errorDialog;
	public TextAsset textDataFile;

	private Vector3 originalPosition;
	private Quaternion originalRotation;
	private bool startAnimation = false;
	private int count = 1;
	private float[][] movementData = null;

	// Use this for initialization
	void Start () 
	{
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		errorDialog.text = "No errors.";

		// load data file
		try
		{
			string[] splitLines = textDataFile.text.Split(new string[] { "\n" }, StringSplitOptions.None);
			string[][] linesDelimited = splitLines.Select(line => line.Split(',').ToArray()).ToArray();

			movementData = linesDelimited.Select(line => line.Select(x => float.Parse(x)).ToArray()).ToArray();
		}
		catch
		{
			errorDialog.text = "Unable to read text file!";
			return;
		}

		performRotation (movementData[0][1], movementData[0][2], movementData[0][3]);

		originalPosition = transform.position;
		originalRotation = transform.rotation;
	}

	void FixedUpdate () 
	{
		if (startAnimation == true) 
		{
			// for quaternion
			float pitchMovement = movementData [count] [1];
			float yawMovement = movementData [count] [2];
			float rollMovement = movementData [count] [3];

			performRotation(pitchMovement, yawMovement, rollMovement);

			errorDialog.text = transform.localPosition.ToString("F");
			count = count + 1;
		}
		else
		{
			transform.position = originalPosition;
			transform.rotation = originalRotation;
			count = 1;
		}
	}

	void LateUpdate ()
	{
		// the back button of an android device quits the game
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	void OnGUI()
	{
		if(GUI.Button(new Rect(20,60,80,50), "Reset")) 
		{
			if (startAnimation == true) startAnimation = false;
			else startAnimation = true;
		}
	}

	void performRotation(float pitchMovement, float yawMovement, float rollMovement)
	{
		transform.rotation = Quaternion.Euler (rollMovement, -yawMovement, -pitchMovement);
	}

}
