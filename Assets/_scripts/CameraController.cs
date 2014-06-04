using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	public GUIText errorDialog;
	public float motionSmoothing = 2f; // adjust smoothing of camera motion
	public float zoomSpeed = 0.05f;
	public float panSpeed = 20f;
	public float maxRotateSpeed = 10.0f;

	Transform standardPos; // usual position of camera
	Transform driver; // position of the player
	private Vector3 offset;
	private Vector2 deltaMousePos;

	void Start ()
	{
		// initialize references
		if (GameObject.Find ("CamPos"))
			standardPos = GameObject.Find ("CamPos").transform;

		if (GameObject.Find ("Driver"))
			driver = GameObject.Find ("Driver").transform;
	}

	void FixedUpdate ()
	{
		// Point the camera at the standard position and direction
		transform.position = Vector3.Lerp (transform.position, standardPos.position, Time.deltaTime * motionSmoothing);
		transform.forward = Vector3.Lerp (transform.forward, standardPos.forward, Time.deltaTime * motionSmoothing);
	}

	void Update () 
	{
		// pan the camera
		if (Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Moved) 
		{
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			// set a minimum rotate speed or the lerp will mess it up
			standardPos.transform.RotateAround(driver.position, Vector3.up, Mathf.Clamp(touchDeltaPosition.x * panSpeed * Time.deltaTime,
			                                                                            -maxRotateSpeed, maxRotateSpeed));
		}

		// zoom the camera in and out
		if (Input.touchCount == 2) 
		{
			Touch touch0 = Input.GetTouch(0);
			Touch touch1 = Input.GetTouch(1);

			Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
			Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

			float prevDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
			float touchDeltaMag = (touch0.position - touch1.position).magnitude;

			float deltaMagDiff = touchDeltaMag - prevDeltaMag;

			standardPos.transform.position += standardPos.forward * deltaMagDiff * zoomSpeed * Time.deltaTime;
		}
	}
}
