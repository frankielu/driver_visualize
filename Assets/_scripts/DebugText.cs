using UnityEngine;
using System.Collections;

public class DebugText : MonoBehaviour 
{
	public GameObject yawSkeleton;

	void Start () 
	{
		updateRotationCoordinates ();
	}

	void LateUpdate () 
	{
		updateRotationCoordinates ();
	}

	void updateRotationCoordinates ()
	{
		Vector3 coordinates = yawSkeleton.transform.eulerAngles;
		gameObject.guiText.text = "Rotation Coordinates - " + coordinates.ToString ();
	}
}
