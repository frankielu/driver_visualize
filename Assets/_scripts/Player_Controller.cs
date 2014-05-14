using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Linq;

public class Player_Controller : MonoBehaviour 
{
	public GameObject bodypart_Head;
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

		performRotation (-movementData [0] [3], movementData [0] [1], -movementData [0] [2]);

		originalPosition = bodypart_Head.transform.position;
		originalRotation = bodypart_Head.transform.rotation;
	}

	void FixedUpdate () 
	{
		if (startAnimation == true) 
		{
			// these values need to be multiplied by a multiplier depending on how degrees are measured by the sensor
			float yawMovement = -1.0f * (movementData [count] [2] - movementData [count - 1] [2]);
			float pitchMovement = 1.0f * (movementData [count] [1] - movementData [count - 1] [1]);
			float rollMovement = -1.0f * (movementData [count] [3] - movementData [count - 1] [3]);

			performRotation(rollMovement, pitchMovement, yawMovement);

			errorDialog.text = bodypart_Head.transform.position.ToString("F");
			count = count + 1;
		}
		else
		{
			bodypart_Head.transform.position = originalPosition;
			bodypart_Head.transform.rotation = originalRotation;
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

	void performRotation(float rollMovement, float pitchMovement, float yawMovement)
	{
		bodypart_Head.transform.RotateAround (new Vector3 (0.0f, 1.59f, 0.0f),
		                                      Vector3.up + bodypart_Head.transform.position,
		                                      yawMovement);
		bodypart_Head.transform.RotateAround (new Vector3 (0.0f, 1.59f, 0.0f),
		                                      Vector3.right + bodypart_Head.transform.position,
		                                      pitchMovement);
		bodypart_Head.transform.RotateAround (new Vector3 (0.0f, 1.59f, 0.0f),
		                                      Vector3.forward + bodypart_Head.transform.position,
		                                      rollMovement);
	}

}
