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
	#region Variable Declarations

	[System.NonSerialized]
	public Transform enemy;

	public Texture ucsdLogo;
	public GUIText errorDialog;
	
	public GameObject player_Head;
	public GameObject player_leftHand;
	public GameObject player_rightHand;
	public GameObject player_leftEye;
	public GameObject player_rightEye;
	public GameObject player_leftUEL; //Upper eyelid
	public GameObject player_leftLEL; //Lower eyelid
	public GameObject player_rightUEL; //Upper eyelid
	public GameObject player_rightLEL; //Lower eyelid
	public GameObject player_rightArm;
	public GameObject player_leftArm;
	public GameObject player_rightForeArm;
	public GameObject player_leftForeArm;
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
	public GameObject player_leftHandFI1;
	public GameObject player_leftHandFI2;
	public GameObject player_leftHandFI3;
	public GameObject player_leftHandFM1;
	public GameObject player_leftHandFM2;
	public GameObject player_leftHandFM3;
	public GameObject player_leftHandFR1;
	public GameObject player_leftHandFR2;
	public GameObject player_leftHandFR3;
	public GameObject player_leftHandFP1;
	public GameObject player_leftHandFP2;
	public GameObject player_leftHandFP3;

	// note: refactor names
	private Vector3 originalHeadPosition;
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

	// face original rotations
	private Quaternion originalleftEyeRotation;
	private Quaternion originalrightEyeRotation;
	private Quaternion originalHeadRotation;

	//right hand original rotations
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

	//left hand original rotations
	private Quaternion originalLARotation;
	private Quaternion originalLFARotation;
	private Quaternion originalLHFI1Rotation;
	private Quaternion originalLHFI2Rotation;
	private Quaternion originalLHFI3Rotation;
	private Quaternion originalLHFM1Rotation;
	private Quaternion originalLHFM2Rotation;
	private Quaternion originalLHFM3Rotation;
	private Quaternion originalLHFR1Rotation;
	private Quaternion originalLHFR2Rotation;
	private Quaternion originalLHFR3Rotation;
	private Quaternion originalLHFP1Rotation;
	private Quaternion originalLHFP2Rotation;
	private Quaternion originalLHFP3Rotation;

	// switches and presets
	private bool _headIsMoving;
	private bool isrhclose = true;
	private bool islhclose = true;
	private bool islhbend = false;
	private bool isrhbend = false;
	private Vector3 preset = new Vector3 (-23.49268f, 1.148f, -0.15f);
//	private Vector3 shiftwc = new Vector3 (-23.2375f, 1.042182f, 0.443f);

	private float headPitchValue;
	private float headYawValue;
	private float headRollValue;
	private float leftHandXValue;
	private float leftHandYValue;
	private float rightHandXValue;
	private float rightHandYValue;

	private float leftEyeAreaValue = 1.0f;
	private float rightEyeAreaValue = 1.0f;

	#endregion
	
	public bool driverEnabled
	{
		get { return _headIsMoving; }
		set
		{
			_headIsMoving = value;
			if (value == false)
			{
				// head
				player_Head.transform.position = originalHeadPosition;
				player_Head.transform.rotation = originalHeadRotation;

				// eyes
				player_leftUEL.transform.position = originalLeftUELPos;
				player_leftLEL.transform.position = originalLeftLELPos;
				player_rightUEL.transform.position = originalRightUELPos;
				player_rightLEL.transform.position = originalRightLELPos;
				player_rightEye.transform.rotation = originalrightEyeRotation;
				player_leftEye.transform.rotation = originalleftEyeRotation;

				// right arm
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

				// left arm
				player_leftArm.transform.rotation = originalLARotation;
				player_leftForeArm.transform.rotation = originalLFARotation;
				player_leftHandFI1.transform.rotation = originalLHFI1Rotation;
				player_leftHandFI2.transform.rotation = originalLHFI2Rotation;
				player_leftHandFI3.transform.rotation = originalLHFI3Rotation;
				player_leftHandFM1.transform.rotation = originalLHFM1Rotation;
				player_leftHandFM2.transform.rotation = originalLHFM2Rotation;
				player_leftHandFM3.transform.rotation = originalLHFM3Rotation;
				player_leftHandFR1.transform.rotation = originalLHFR1Rotation;
				player_leftHandFR2.transform.rotation = originalLHFR2Rotation;
				player_leftHandFR3.transform.rotation = originalLHFR3Rotation;
				player_leftHandFP1.transform.rotation = originalLHFP1Rotation;
				player_leftHandFP2.transform.rotation = originalLHFP2Rotation;
				player_leftHandFP3.transform.rotation = originalLHFP3Rotation;

				Network_Connector.networkEnabled = false;
			}
			else
			{
				// head
				originalHeadPosition = player_Head.transform.position;
				originalHeadRotation = player_Head.transform.rotation;
				headRollValue = originalHeadRotation.eulerAngles.x;
				headYawValue = originalHeadRotation.eulerAngles.y;
				headPitchValue = originalHeadRotation.eulerAngles.z;

				// eyes
				originalLeftUELPos = player_leftUEL.transform.position;
				originalLeftLELPos = player_leftLEL.transform.position;
				originalRightUELPos = player_rightUEL.transform.position;
				originalRightLELPos = player_rightLEL.transform.position;
				LEsize = originalLeftUELPos.y - originalLeftLELPos.y;
				REsize = originalRightUELPos.y - originalRightLELPos.y;
				originalrightEyeRotation = player_rightEye.transform.rotation;
				originalleftEyeRotation = player_leftEye.transform.rotation;

				// right arm
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

				// left arm
				originalLARotation = player_leftArm.transform.rotation;
				originalLFARotation = player_leftForeArm.transform.rotation;
				originalLHFI1Rotation = player_leftHandFI1.transform.rotation;
				originalLHFI2Rotation = player_leftHandFI2.transform.rotation;
				originalLHFI3Rotation = player_leftHandFI3.transform.rotation;
				originalLHFM1Rotation = player_leftHandFM1.transform.rotation;
				originalLHFM2Rotation = player_leftHandFM2.transform.rotation;
				originalLHFM3Rotation = player_leftHandFM3.transform.rotation;
				originalLHFR1Rotation = player_leftHandFR1.transform.rotation;
				originalLHFR2Rotation = player_leftHandFR2.transform.rotation;
				originalLHFR3Rotation = player_leftHandFR3.transform.rotation;
				originalLHFP1Rotation = player_leftHandFP1.transform.rotation;
				originalLHFP2Rotation = player_leftHandFP2.transform.rotation;
				originalLHFP3Rotation = player_leftHandFP3.transform.rotation;

				Network_Connector.networkEnabled = true;
			}
		}
	}
	
	void Start () 
	{
		// work around for positioning for now
		driverEnabled = true;
		driverEnabled = false;

		Screen.orientation = ScreenOrientation.LandscapeLeft;
		errorDialog.text = String.Empty;
	}

	void FixedUpdate () 
	{
		if (driverEnabled == true) 
		{
			// get data from server
			float[] mostRecentData = Network_Connector.lastDataReceived;
			if (mostRecentData.Length != 11) return;

			headPitchValue = mostRecentData [1];
			headYawValue =  mostRecentData [2];
			headRollValue = mostRecentData [3];
			leftEyeAreaValue = mostRecentData [4];
			rightEyeAreaValue = mostRecentData [5];
			leftHandXValue = mostRecentData [7];
			leftHandYValue = mostRecentData [8];
			rightHandXValue = mostRecentData [9];
			rightHandYValue = mostRecentData [10];

			PerformRotation(headPitchValue, headYawValue, headRollValue);
			PerformEyeBlink(leftEyeAreaValue, rightEyeAreaValue);

			if (leftHandXValue < 0 || leftHandYValue < 0 || rightHandXValue < 0 || rightHandYValue < 0) return;
			PerformHandMovement(leftHandXValue, leftHandYValue, rightHandXValue, rightHandYValue);
		}
	}

	void LateUpdate ()
	{
		// the back button of an android device quits the game, esc for keyboard input
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	void OnGUI()
	{
		GUI.contentColor = driverEnabled == true ? Color.green : Color.red;

		// client can change ip address to match the server
		Network_Connector.IPAddress = GUI.TextField (new Rect (10f,10f,Screen.width*0.05f,Screen.height*0.03f), Network_Connector.IPAddress, 20);
		// add input validation in the future

		// ucsd logo, resolution is 1024 x 128
		GUI.DrawTexture (new Rect (Screen.width*0.55f, 0, Screen.width*0.45f, Screen.width*0.45f*0.125f), ucsdLogo, ScaleMode.ScaleAndCrop);

		// increase text size
		if(GUI.Button(new Rect(10f,15f+Screen.height*0.05f,Screen.width*0.05f,Screen.height*0.05f), "On/Off")) 
		{
			if (driverEnabled == true) driverEnabled = false;
			else driverEnabled = true;
		}
	}

	void PerformRotation(float pitchMovement, float yawMovement, float rollMovement)
	{
		player_Head.transform.localRotation = Quaternion.Lerp (player_Head.transform.localRotation, Quaternion.Euler(rollMovement, -yawMovement, -pitchMovement), 
		                                                  0.4f);                                 
	}

	void PerformEyeBlink(float leftEyeArea, float rightEyeArea)
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
	}

	void Rhandonlap()
	{
		Vector3 RAdirection = new Vector3 (-23.5593f, 1.012144f, -0.0541f);
		Vector3 RFAdirection = new Vector3 (-23.5593f, 0.87f, 0.3144125f);
		pointRAto (RAdirection);
		pointRFAto (RFAdirection);
		righthandopen ();
	}
	
	void Lhandonlap()
	{
		Vector3 LAdirection = new Vector3 (-24.2f, -0.075f, 1.0678f);
		Vector3 LFAdirection = new Vector3 (-23.85f, 0.588f, 1.86f);
		pointLAto (LAdirection);
		pointLFAto (LFAdirection);
		lefthandopen ();
	}

	// not needed but keep
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

	void PerformHandMovement(float lefthandx, float lefthandy, float righthandx, float righthandy)
	{
		Vector3 n = new Vector3(0.01f, -0.8f, 1.0f);
		n = n / n.magnitude;
		float Zo = 0.443f;
		float unitconvert = 0.005f;
		float[] originalLX = new float[2]{-23.90527f, 147.0f};
		float[] originalLY = new float[2]{1.416689f, 125.0f};
		float[] originalRX = new float[2]{-23.55484f, 314.0f};
		float[] originalRY = new float[2]{1.362127f, 150.0f};
		
		float offsetLX = (lefthandx - originalLX [1]) * unitconvert;
		float offsetLY = (lefthandy - originalLY [1]) * unitconvert;
		float offsetRX = (righthandx - originalRX [1])* unitconvert;
		float offsetRY = (righthandy - originalRY [1])* unitconvert;
		//float offsetLZ = Zo- n.x*offsetLX/n.z - n.y*offsetLY/n.z;
		//float offsetRZ = Zo - n.x*offsetRX/n.z - n.y*offsetRY/n.z;
		float LX = originalLX [0] + offsetLX-0.3f;
		float LY = originalLY [0] - offsetLY+0.3f;
		float RX = originalRX [0] + offsetRX-0.05f;
		float RY = originalRY [0] - offsetRY+0.05f;
		float LZ = Zo - n.x*LX/n.z - n.y*LY/n.z - 0.02f;
		float RZ = Zo - n.x*RX/n.z - n.y*RY/n.z - 0.02f;
		
		int lhzone = checkzone (lefthandx, lefthandy);
		int rhzone = checkzone (righthandx, righthandy);
		
		if (righthandx == 0 && righthandy == 0) 
		{
			Rhandonlap();
		}
		else if (rhzone == 1)
		{
			pointRAto (new Vector3(RX, RY, RZ));
			pointRFAto(new Vector3(RX, RY, RZ));
			player_rightForeArm.transform.Rotate (new Vector3(30f, 0f, 0f));
			righthandbend ();
			righthandopen ();
		}
		else if (rhzone == 2)
		{
			pointRAto (new Vector3(RX, RY, RZ));
			pointRFAto(new Vector3(RX, RY, RZ));
			player_rightForeArm.transform.Rotate (new Vector3(-60f, 0f, 0f));
			righthandunbend ();
			righthandclose ();
		}
		else if (rhzone == 3 || rhzone == 5)
		{
			pointRAto(new Vector3(RX, RY, RZ));
			pointRFAto(new Vector3(RX, RY, RZ));
			righthandunbend();
			righthandopen();
		}
		else if (rhzone == 4)
		{
			pointRAto(preset);
			pointRFAto(new Vector3(RX, RY, RZ));
			righthandunbend ();
			righthandclose();
		}
		
		
		if (lefthandx == 0 && lefthandy == 0)
		{
			Lhandonlap();
		}
		else if (lhzone == 1)
		{
			pointLAto(new Vector3(LX, LY, LZ));
			pointLFAto(new Vector3(LX, LY, LZ));
			player_leftForeArm.transform.Rotate (new Vector3(-60f, 0f, 0f));
			lefthandunbend();
		}
		else if (lhzone == 2)
		{
			pointLAto(new Vector3(LX, LY, LZ));
			pointLFAto(new Vector3(LX, LY, LZ));
			lefthandbend();
		}
		else if (lhzone == 3 || lhzone == 4 || lhzone == 5)
		{
			pointLAto(new Vector3(LX, LY, LZ));
			pointLFAto(new Vector3(LX, LY, LZ));
			lefthandunbend();
			//lefthandopen ();
		}
	}

	void lefthandbend()
	{
		if (islhbend == false)
		{
			player_leftHand.transform.Rotate (new Vector3(0f, 0f, -60f));
			lefthandopen ();
			islhbend = true;
		}
	}
	
	void lefthandunbend()
	{
		if (islhbend == true)
		{
			player_leftHand.transform.Rotate(new Vector3(0f, 0f, 60f));
			lefthandclose ();
			islhbend = false;
		}
	}
	
	void righthandbend()
	{
		if (isrhbend == false)
		{
			player_rightHand.transform.Rotate (new Vector3(0f, 0f, 70f));
			righthandopen ();
			isrhbend = true;
		}
	}
	
	void righthandunbend()
	{
		if (isrhbend == true)
		{
			player_rightHand.transform.Rotate(new Vector3(0f, 0f, -70f));
			righthandclose ();
			isrhbend = false;
		}
	}
	
	void righthandopen()
	{
		if (isrhclose == true)
		{
			Vector3 z = new Vector3(0f, 0f, 10f);
			player_rightHandFI1.transform.Rotate (new Vector3(0.0f, 0.0f, 35.91f)+z);
			player_rightHandFI2.transform.Rotate (new Vector3(0.0f, 0.0f, 63.0f)+z);
			player_rightHandFI3.transform.Rotate (new Vector3(0.0f, 0.0f, 47.18f)+z);
			player_rightHandFM1.transform.Rotate (new Vector3(0.0f, 0.0f, 35.91f)+z);
			player_rightHandFM2.transform.Rotate (new Vector3(0.0f, 0.0f, 63.0f)+z);
			player_rightHandFM3.transform.Rotate (new Vector3(0.0f, 0.0f, 47.18f)+z);
			player_rightHandFR1.transform.Rotate (new Vector3(0.0f, 0.0f, 35.91f)+z);
			player_rightHandFR2.transform.Rotate (new Vector3(0.0f, 0.0f, 63.0f)+z);
			player_rightHandFR3.transform.Rotate (new Vector3(0.0f, 0.0f, 47.18f)+z);
			player_rightHandFP1.transform.Rotate (new Vector3(0.0f, 0.0f, 35.91f)+z);
			player_rightHandFP2.transform.Rotate (new Vector3(0.0f, 0.0f, 63.0f)+z);
			player_rightHandFP3.transform.Rotate (new Vector3(0.0f, 0.0f, 47.18f)+z);
			isrhclose = false;
		}
	}
	
	void lefthandclose()
	{
		if (islhclose == false)
		{
			player_leftHandFI1.transform.Rotate (new Vector3(0.0f, 0.0f, 20.0f));
			player_leftHandFI2.transform.Rotate (new Vector3(0.0f, 0.0f, 50.0f));
			player_leftHandFI3.transform.Rotate (new Vector3(0.0f, 0.0f, 50.0f));
			player_leftHandFM1.transform.Rotate (new Vector3(0.0f, 0.0f, 20.0f));
			player_leftHandFM2.transform.Rotate (new Vector3(0.0f, 0.0f, 50.0f));
			player_leftHandFM3.transform.Rotate (new Vector3(0.0f, 0.0f, 50.0f));
			player_leftHandFR1.transform.Rotate (new Vector3(0.0f, 0.0f, 20.0f));
			player_leftHandFR2.transform.Rotate (new Vector3(0.0f, 0.0f, 50.0f));
			player_leftHandFR3.transform.Rotate (new Vector3(0.0f, 0.0f, 50.0f));
			player_leftHandFP1.transform.Rotate (new Vector3(0.0f, 0.0f, 20.0f));
			player_leftHandFP2.transform.Rotate (new Vector3(0.0f, 0.0f, 50.0f));
			player_leftHandFP3.transform.Rotate (new Vector3(0.0f, 0.0f, 50.0f));
			islhclose = true;
		}
	}

	void righthandclose()
	{
		if (isrhclose == false)
		{
			Vector3 z = new Vector3(0f, 0f, 10f);
			player_rightHandFI1.transform.Rotate (new Vector3(0.0f, 0.0f, -35.91f)-z);
			player_rightHandFI2.transform.Rotate (new Vector3(0.0f, 0.0f, -63.0f)-z);
			player_rightHandFI3.transform.Rotate (new Vector3(0.0f, 0.0f, -47.18f)-z);
			player_rightHandFM1.transform.Rotate (new Vector3(0.0f, 0.0f, -35.91f)-z);
			player_rightHandFM2.transform.Rotate (new Vector3(0.0f, 0.0f, -63.0f)-z);
			player_rightHandFM3.transform.Rotate (new Vector3(0.0f, 0.0f, -47.18f)-z);
			player_rightHandFR1.transform.Rotate (new Vector3(0.0f, 0.0f, -35.91f)-z);
			player_rightHandFR2.transform.Rotate (new Vector3(0.0f, 0.0f, -63.0f)-z);
			player_rightHandFR3.transform.Rotate (new Vector3(0.0f, 0.0f, -47.18f)-z);
			player_rightHandFP1.transform.Rotate (new Vector3(0.0f, 0.0f, -35.91f)-z);
			player_rightHandFP2.transform.Rotate (new Vector3(0.0f, 0.0f, -63.0f)-z);
			player_rightHandFP3.transform.Rotate (new Vector3(0.0f, 0.0f, -47.18f)-z);
			isrhclose = true;
		}
	}

	void lefthandopen()
	{
		if (islhclose == true)
		{
			player_leftHandFI1.transform.Rotate (new Vector3(0.0f, 0.0f, -20.0f));
			player_leftHandFI2.transform.Rotate (new Vector3(0.0f, 0.0f, -50.0f));
			player_leftHandFI3.transform.Rotate (new Vector3(0.0f, 0.0f, -50.0f));
			player_leftHandFM1.transform.Rotate (new Vector3(0.0f, 0.0f, -20.0f));
			player_leftHandFM2.transform.Rotate (new Vector3(0.0f, 0.0f, -50.0f));
			player_leftHandFM3.transform.Rotate (new Vector3(0.0f, 0.0f, -50.0f));
			player_leftHandFR1.transform.Rotate (new Vector3(0.0f, 0.0f, -20.0f));
			player_leftHandFR2.transform.Rotate (new Vector3(0.0f, 0.0f, -50.0f));
			player_leftHandFR3.transform.Rotate (new Vector3(0.0f, 0.0f, -50.0f));
			player_leftHandFP1.transform.Rotate (new Vector3(0.0f, 0.0f, -20.0f));
			player_leftHandFP2.transform.Rotate (new Vector3(0.0f, 0.0f, -50.0f));
			player_leftHandFP3.transform.Rotate (new Vector3(0.0f, 0.0f, -50.0f));
			islhclose = false;
		}
	}

	void pointRAto(Vector3 target)
	{
		
		player_rightArm.transform.LookAt (target);
		player_rightArm.transform.Rotate (new Vector3 (0, -90, 0));
	}
	
	void pointLAto(Vector3 target)
	{
		player_leftArm.transform.LookAt (target); 
		player_leftArm.transform.Rotate (new Vector3 (0, 90, 0));
	}
	
	void pointRFAto(Vector3 target)
	{
		player_rightForeArm.transform.LookAt (target);
		player_rightForeArm.transform.Rotate (new Vector3 (0, -90, 0));
	}
	
	void pointLFAto(Vector3 target)
	{
		player_leftForeArm.transform.LookAt (target);
		player_leftForeArm.transform.Rotate (new Vector3 (0, 90, 0));
	}

	int checkzone(float pix, float piy)
	{
		Vector4 wheel_left = new Vector4 (120f, 310f, 247f, 54f);
		Vector4 wheel_right = new Vector4 (247f, 310f, 387f, 54f);
		Vector4 gear = new Vector4 (380f, 480f, 540f, 275f);
		Vector4 radio = new Vector4 (380f, 275f, 570f, 78f);
		
		if (pix >= wheel_left.x && pix < wheel_left.z && piy >= wheel_left.w && piy < wheel_left.y) {return 1;}
		if (pix >= wheel_right.x && pix < wheel_right.z && piy >= wheel_right.w && piy < wheel_right.y) {return 2;}
		if (pix >= radio.x && pix < radio.z && piy >= radio.w && piy < radio.y) {return 3;}
		if (pix >= gear.x && pix < gear.z && piy >= gear.w && piy < gear.y) {return 4;}
		
		return 5;
	}
}
