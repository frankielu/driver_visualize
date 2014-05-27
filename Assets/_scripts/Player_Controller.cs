using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Linq;

[RequireComponent (typeof (Animator))]
[RequireComponent (typeof (CapsuleCollider))]
[RequireComponent (typeof (Rigidbody))]
public class Player_Controller : MonoBehaviour 
{
	[System.NonSerialized]
	public float lookWeight;

	[System.NonSerialized]
	public Transform enemy;

	public GUIText errorDialog;
	public TextAsset textDataFile;
	public GameObject player_Head;
	
	private Vector3 originalPosition;
	private Quaternion originalRotation;
	private bool startAnimation = false;
	private bool changeFrame = false; // controls whether the next set of data points should be pushed
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

		originalPosition = player_Head.transform.position;
		originalRotation = player_Head.transform.rotation;
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

			// iterate every other count
			if (changeFrame == true)
			{
				count = count + 1;
				changeFrame = false;
			}
			else changeFrame = true;
		}
		else
		{
			player_Head.transform.position = originalPosition;
			player_Head.transform.rotation = originalRotation;
			count = 1;
			changeFrame = false;
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
		player_Head.transform.rotation = Quaternion.Lerp (player_Head.transform.rotation, Quaternion.Euler(rollMovement, -yawMovement, -pitchMovement), 
		                                                  0.5f);
		                                       
//		player_Head.transform.rotation = Quaternion.Euler (rollMovement, -yawMovement, -pitchMovement);
	}

}
