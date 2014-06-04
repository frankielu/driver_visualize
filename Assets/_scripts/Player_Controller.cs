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
	public GameObject player_leftEye;
	public GameObject player_rightEye;
	public GameObject player_leftUEL; //Upper eyelid
	public GameObject player_leftLEL; //Lower eyelid
	public GameObject player_rightUEL; //Upper eyelid
	public GameObject player_rightLEL; //Lower eyelid
	public GameObject player_rightArm;
	public GameObject player_rightForeArm;
	public GameObject player_rightHandFI1;
	public GameObject player_rightHandFI2;
	public GameObject player_rightHandFI3;
	public GameObject player_rightHandFM1;
	public GameObject player_rightHandFM2;
	public GameObject player_rightHandFM3;
	public GameObject player_rightHandFR1;
	public GameObject player_rightHandFR2;
	public GameObject player_rightHandFR3;
	public GameObject player_rightHandFP1;
	public GameObject player_rightHandFP2;
	public GameObject player_rightHandFP3;
	
	private Vector3 originalPosition;
	private Vector3 originalLeftUELPos;
	private Vector3 originalLeftLELPos;
	private Vector3 originalRightUELPos;
	private Vector3 originalRightLELPos;
	private float REsize;
	private float LEsize;
	private float Llastoffset = 0;
	private float Rlastoffset = 0;
	private float Llastrotate = 0;
	private float Rlastrotate = 0;
	private Quaternion originalleftEyeRotation;
	private Quaternion originalrightEyeRotation;
	private Quaternion originalRotation;
	private Quaternion originalRARotation;
	private Quaternion originalRFARotation;
	private Quaternion originalRHFI1Rotation;
	private Quaternion originalRHFI2Rotation;
	private Quaternion originalRHFI3Rotation;
	private Quaternion originalRHFM1Rotation;
	private Quaternion originalRHFM2Rotation;
	private Quaternion originalRHFM3Rotation;
	private Quaternion originalRHFR1Rotation;
	private Quaternion originalRHFR2Rotation;
	private Quaternion originalRHFR3Rotation;
	private Quaternion originalRHFP1Rotation;
	private Quaternion originalRHFP2Rotation;
	private Quaternion originalRHFP3Rotation;
	private bool iseyeclose = false;
	private bool isrhonshift = false;
	private bool _headIsMoving;
	private float[][] movementData = null;

	private float pitchMovement;
	private float yawMovement;
	private float rollMovement;
	private float leftEyeArea = 1.0f;
	private float rightEyeArea = 1.0f;
	
	public bool headIsMoving
	{
		get { return _headIsMoving; }
		set
		{
			_headIsMoving = value;
			if (value == false)
			{
				player_Head.transform.position = originalPosition;
				player_Head.transform.rotation = originalRotation;
				player_leftUEL.transform.position = originalLeftUELPos;
				player_leftLEL.transform.position = originalLeftLELPos;
				player_rightUEL.transform.position = originalRightUELPos;
				player_rightLEL.transform.position = originalRightLELPos;
				player_rightEye.transform.rotation = originalrightEyeRotation;
				player_leftEye.transform.rotation = originalleftEyeRotation;
				player_rightArm.transform.rotation = originalRARotation;
				player_rightForeArm.transform.rotation = originalRFARotation;
				player_rightHandFI1.transform.rotation = originalRHFI1Rotation;
				player_rightHandFI2.transform.rotation = originalRHFI2Rotation;
				player_rightHandFI3.transform.rotation = originalRHFI3Rotation;
				player_rightHandFM1.transform.rotation = originalRHFM1Rotation;
				player_rightHandFM2.transform.rotation = originalRHFM2Rotation;
				player_rightHandFM3.transform.rotation = originalRHFM3Rotation;
				player_rightHandFR1.transform.rotation = originalRHFR1Rotation;
				player_rightHandFR2.transform.rotation = originalRHFR2Rotation;
				player_rightHandFR3.transform.rotation = originalRHFR3Rotation;
				player_rightHandFP1.transform.rotation = originalRHFP1Rotation;
				player_rightHandFP2.transform.rotation = originalRHFP2Rotation;
				player_rightHandFP3.transform.rotation = originalRHFP3Rotation;
			}
			else
			{
				originalPosition = player_Head.transform.position;
				originalRotation = player_Head.transform.rotation;
				originalLeftUELPos = player_leftUEL.transform.position;
				originalLeftLELPos = player_leftLEL.transform.position;
				originalRightUELPos = player_rightUEL.transform.position;
				originalRightLELPos = player_rightLEL.transform.position;
				LEsize = originalLeftUELPos.y - originalLeftLELPos.y;
				REsize = originalRightUELPos.y - originalRightLELPos.y;
				originalrightEyeRotation = player_rightEye.transform.rotation;
				originalleftEyeRotation = player_leftEye.transform.rotation;
				originalRARotation = player_rightArm.transform.rotation;
				originalRFARotation = player_rightForeArm.transform.rotation;
				originalRHFI1Rotation = player_rightHandFI1.transform.rotation;
				originalRHFI2Rotation = player_rightHandFI2.transform.rotation;
				originalRHFI3Rotation = player_rightHandFI3.transform.rotation;
				originalRHFM1Rotation = player_rightHandFM1.transform.rotation;
				originalRHFM2Rotation = player_rightHandFM2.transform.rotation;
				originalRHFM3Rotation = player_rightHandFM3.transform.rotation;
				originalRHFR1Rotation = player_rightHandFR1.transform.rotation;
				originalRHFR2Rotation = player_rightHandFR2.transform.rotation;
				originalRHFR3Rotation = player_rightHandFR3.transform.rotation;
				originalRHFP1Rotation = player_rightHandFP1.transform.rotation;
				originalRHFP2Rotation = player_rightHandFP2.transform.rotation;
				originalRHFP3Rotation = player_rightHandFP3.transform.rotation;

				rollMovement = originalRotation.eulerAngles.x;
				yawMovement = originalRotation.eulerAngles.y;
				pitchMovement = originalRotation.eulerAngles.z;
			}
		}
	}

	// Use this for initialization
	void Start () 
	{
		headIsMoving = true;
		headIsMoving = false;
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
	}

	void FixedUpdate () 
	{
		if (headIsMoving == true) 
		{
			float[] mostRecentData = UDP_Connector.lastDataReceived;

			// for quaternion
			pitchMovement = mostRecentData [1];
			yawMovement =  mostRecentData [2];
			rollMovement = mostRecentData [3];
			leftEyeArea = mostRecentData [4];
			rightEyeArea = mostRecentData [5];

			performRotation(pitchMovement, yawMovement, rollMovement);
			performEyeBlink(leftEyeArea, rightEyeArea);
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
		if(GUI.Button(new Rect(20,60,80,30), "Reset")) 
		{
			if (headIsMoving == true) headIsMoving = false;
			else headIsMoving = true;
		}

		if(GUI.Button(new Rect(20,30,80,30), "Wink")) 
		{
			if (iseyeclose == false)
			{
				eyeclose();
				iseyeclose = true;
			}
			else
			{
				eyeopen();
				iseyeclose = false;
			}
		}

		if(GUI.Button(new Rect(20,0,80,30), "Hand")) 
		{
			if (isrhonshift == false)
			{
				handonshift();
				isrhonshift = true;
			}
			else
			{
				handonwheel();
				isrhonshift = false;
			}
		}


	}

	void performRotation(float pitchMovement, float yawMovement, float rollMovement)
	{
		player_Head.transform.rotation = Quaternion.Lerp (player_Head.transform.rotation, Quaternion.Euler(rollMovement, -yawMovement, -pitchMovement), 
		                                                  0.5f);
		                                       
//		player_Head.transform.rotation = Quaternion.Euler (rollMovement, -yawMovement, -pitchMovement);
	}

	void performEyeBlink(float leftEyeArea, float rightEyeArea)
	{
		float nextLsize = LEsize * leftEyeArea;
		float nextRsize = REsize * rightEyeArea;
		float Loffset = (nextLsize - LEsize) / 2.0f;
		float Roffset = (nextRsize - REsize) / 2.0f;
		float Lrotate = (2*Loffset/LEsize)*30;
		float Rrotate = (2*Roffset/REsize)*30;
		if (Lrotate >= 0 ||(Llastrotate - Lrotate) < -20) { Lrotate = 0;};
		if (Rrotate >= 0 ||(Rlastrotate - Rrotate) < -20) { Rrotate = 0;};

		Vector3 nextleftUELpos = new Vector3(player_leftUEL.transform.position.x, player_leftUEL.transform.position.y - Llastoffset + 2*Loffset, player_leftUEL.transform.position.z);
		Vector3 nextleftLELpos = new Vector3(player_leftLEL.transform.position.x, player_leftLEL.transform.position.y, player_leftLEL.transform.position.z);
		Vector3 nextrightUELpos = new Vector3(player_rightUEL.transform.position.x, player_rightUEL.transform.position.y - Rlastoffset + 2*Roffset, player_rightUEL.transform.position.z);
		Vector3 nextrightLELpos = new Vector3(player_rightLEL.transform.position.x, player_rightLEL.transform.position.y, player_rightLEL.transform.position.z);
		player_leftEye.transform.Rotate (new Vector3(Llastrotate - Lrotate , 0.0f, 0.0f));
		player_rightEye.transform.Rotate (new Vector3(Rlastrotate - Rrotate , 0.0f, 0.0f));
		player_leftUEL.transform.position = nextleftUELpos;
		player_leftLEL.transform.position = nextleftLELpos;
		player_rightUEL.transform.position = nextrightUELpos;
		player_rightLEL.transform.position = nextrightLELpos;

		Llastoffset = 2*Loffset;
		Rlastoffset = 2*Roffset;
		Llastrotate = Lrotate;
		Rlastrotate = Rrotate;
		//errorDialog.text = lastoffset.ToString();

	}

	void eyeclose() 
	{
		player_leftUEL.transform.position = new Vector3(player_leftUEL.transform.position.x, player_leftUEL.transform.position.y - 0.015f, player_leftUEL.transform.position.z);
		player_leftLEL.transform.position = new Vector3(player_leftLEL.transform.position.x, player_leftLEL.transform.position.y + 0.0000f, player_leftLEL.transform.position.z);
	}

	void eyeopen()
	{
		player_leftUEL.transform.position = new Vector3(player_leftUEL.transform.position.x, player_leftUEL.transform.position.y + 0.015f, player_leftUEL.transform.position.z);
		player_leftLEL.transform.position = new Vector3(player_leftLEL.transform.position.x, player_leftLEL.transform.position.y - 0.0000f, player_leftLEL.transform.position.z);
	}

	void handonshift()
	{
		player_rightArm.transform.Rotate (new Vector3 (14.7f, -1.2f, -57.08f));
		player_rightForeArm.transform.Rotate (new Vector3 (33.3f, 18.6f, 43.6f));
		player_rightHandFI1.transform.Rotate (new Vector3(0.0f, 0.0f, 30.51f));
		player_rightHandFI2.transform.Rotate (new Vector3(0.0f, 0.0f, 88.4f));
		player_rightHandFI3.transform.Rotate (new Vector3(0.0f, 0.0f, 38.38f));
		player_rightHandFM1.transform.Rotate (new Vector3(0.0f, 0.0f, 35.91f));
		player_rightHandFM2.transform.Rotate (new Vector3(0.0f, 0.0f, 63.0f));
		player_rightHandFM3.transform.Rotate (new Vector3(0.0f, 0.0f, 47.18f));
		player_rightHandFR1.transform.Rotate (new Vector3(0.0f, 0.0f, 35.91f));
		player_rightHandFR2.transform.Rotate (new Vector3(0.0f, 0.0f, 63.0f));
		player_rightHandFR3.transform.Rotate (new Vector3(0.0f, 0.0f, 47.18f));
		player_rightHandFP1.transform.Rotate (new Vector3(0.0f, 0.0f, 35.91f));
		player_rightHandFP2.transform.Rotate (new Vector3(0.0f, 0.0f, 63.0f));
		player_rightHandFP3.transform.Rotate (new Vector3(0.0f, 0.0f, 47.18f));

	}

	void handonwheel()
	{
		player_rightArm.transform.rotation = originalRARotation;
		player_rightForeArm.transform.rotation = originalRFARotation;
		player_rightHandFI1.transform.rotation = originalRHFI1Rotation;
		player_rightHandFI2.transform.rotation = originalRHFI2Rotation;
		player_rightHandFI3.transform.rotation = originalRHFI3Rotation;
		player_rightHandFM1.transform.rotation = originalRHFM1Rotation;
		player_rightHandFM2.transform.rotation = originalRHFM2Rotation;
		player_rightHandFM3.transform.rotation = originalRHFM3Rotation;
		player_rightHandFR1.transform.rotation = originalRHFR1Rotation;
		player_rightHandFR2.transform.rotation = originalRHFR2Rotation;
		player_rightHandFR3.transform.rotation = originalRHFR3Rotation;
		player_rightHandFP1.transform.rotation = originalRHFP1Rotation;
		player_rightHandFP2.transform.rotation = originalRHFP2Rotation;
		player_rightHandFP3.transform.rotation = originalRHFP3Rotation;
		
	}
	
	
}
