using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	public GUIText errorDialog;
	public float motionSmoothing = 2f; // adjust smoothing of camera motion
	public float touchZoomSpeed;
	public float touchPanSpeed;
	public float mouseZoomSpeed;
	public float mousePanSpeed;
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
		///////////////////////////
		// keyboard / windows input
		///////////////////////////

		// pan the camera on touch or mouse
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			standardPos.transform.RotateAround(driver.position, Vector3.up, mousePanSpeed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.RightArrow))
		{
			standardPos.transform.RotateAround(driver.position, Vector3.up, -mousePanSpeed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.UpArrow))
		{
			// normalize the direction vector
			Vector3 directionVector = (standardPos.transform.position - driver.position).normalized;

			// compare with vector3.up
			if (Vector3.Magnitude(Vector3.up - directionVector) < 0.3f && directionVector.y > 0) return;

			Vector3 rotationAxis = GetNormal(driver.position, driver.position + Vector3.up, standardPos.transform.position);
			standardPos.transform.RotateAround(driver.position, rotationAxis, -mousePanSpeed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.DownArrow))
		{
			if (driver.position.y + 1.0f > standardPos.transform.position.y) return;

			Vector3 rotationAxis = GetNormal(driver.position, driver.position + Vector3.up, standardPos.transform.position);
			standardPos.transform.RotateAround(driver.position, rotationAxis, mousePanSpeed * Time.deltaTime);
		}

		// zoom the camera
		if (Input.GetAxis("Mouse ScrollWheel") != 0.0f)
		{
			standardPos.transform.position += standardPos.forward * Input.GetAxis("Mouse ScrollWheel") * mouseZoomSpeed * Time.deltaTime;
		}

		///////////////
		// mobile input
		///////////////

		// pan the camera on touch or mouse
		if (Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Moved) 
		{
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			
			// set a minimum rotate speed or the lerp will mess it up
			standardPos.transform.RotateAround(driver.position, Vector3.up, Mathf.Clamp(touchDeltaPosition.x * touchPanSpeed * Time.deltaTime,
			                                                                            -maxRotateSpeed, maxRotateSpeed));

			if (touchDeltaPosition.y != 0)
			{
				Vector3 directionVector = (standardPos.transform.position - driver.position).normalized;

				if (touchDeltaPosition.y > 0 && !(Vector3.Magnitude(Vector3.up - directionVector) < 0.3f && directionVector.y > 0) ||
				    touchDeltaPosition.y < 0 && !(driver.position.y + 1.0f > standardPos.transform.position.y))
				{
					Vector3 rotationAxis = GetNormal(driver.position, driver.position + Vector3.up, standardPos.transform.position);
					standardPos.transform.RotateAround(driver.position, rotationAxis, Mathf.Clamp(-touchDeltaPosition.y * touchPanSpeed * Time.deltaTime,
					                                                                              -maxRotateSpeed, maxRotateSpeed));
				}
			}
		}

		// make this scale with screen resolution in the future
		// zoom the camera in and out on two finger touch or scroll wheel
		if (Input.touchCount == 2) 
		{
			Touch touch0 = Input.GetTouch(0);
			Touch touch1 = Input.GetTouch(1);

			Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
			Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

			float prevDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
			float touchDeltaMag = (touch0.position - touch1.position).magnitude;

			float deltaMagDiff = touchDeltaMag - prevDeltaMag;

			standardPos.transform.position += standardPos.forward * deltaMagDiff * touchZoomSpeed * Time.deltaTime;
		}
	}

	// computes a positive valued normal
	Vector3 GetNormal(Vector3 a, Vector3 b, Vector3 c)
	{
		Vector3 side1 = b - a;
		Vector3 side2 = c - a;
		return Vector3.Cross (side1, side2).normalized;
	}
}
