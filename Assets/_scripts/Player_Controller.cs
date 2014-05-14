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

		originalPosition = transform.position;
		originalRotation = transform.rotation;

//		bodypart_Head.transform.eulerAngles = new Vector3 (bodypart_Head.transform.eulerAngles.x, 
//		                                                 bodypart_Head.transform.eulerAngles.y + movementData [0] [2],
//		                                                 bodypart_Head.transform.eulerAngles.z);

		// need to set initial condition for pitch and roll as well
	}

	void FixedUpdate () 
	{
		if (startAnimation == true) 
		{
			// these values need to be multiplied by a multiplier depending on how degrees are measured by the sensor
			float yawMovement = -1.0f * (movementData [count] [2] - movementData [count - 1] [2]);
			float pitchMovement = 1.0f * (movementData [count] [1] - movementData [count - 1] [1]);
			float rollMovement = -1.0f * (movementData [count] [3] - movementData [count - 1] [3]);

//			// offset is currently (0.0, 0.4, 0.0)
//			bodypart_Head.transform.RotateAround (originalPosition, Vector3.up, yawMovement);
//			bodypart_Head.transform.RotateAround (new Vector3 (0.0f, 1.55f, 0.0f),
//                              new Vector3 (1.0f, 0.0f, 0.0f),
//                              pitchMovement * 0.6f);
//			bodypart_Head.transform.RotateAround (new Vector3 (0.0f, 1.45f, 0.0f),
//                             new Vector3 (0.0f, 0.0f, 1.0f),
//                             rollMovement * 0.5f);

			transform.Rotate(10.0f, 0.0f, 0.0f, Space.Self);

			errorDialog.text = transform.localPosition.ToString();

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

}
