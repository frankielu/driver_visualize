using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	public GameObject player;
	public float perspectiveZoomSpeed = 0.4f;
	public float orthoZoomSpeed = 0.4f;
	public float panSpeed = 200;
	private Vector3 offset;

	void Update () 
	{
		// pan the camera
		if (Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Moved) 
		{
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			transform.RotateAround(player.transform.position, Vector3.up, touchDeltaPosition.x * panSpeed * Time.deltaTime);
//			transform.RotateAround(player.transform.position, Vector3.right, touchDeltaPosition.y * panSpeed * Time.deltaTime);
//			transform.LookAt(player.transform.position + new Vector3(0.0f, 1.0f, 0.0f));
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

			float deltaMagDiff = prevDeltaMag - touchDeltaMag;

			if (camera.isOrthoGraphic)
			{
				camera.orthographicSize += deltaMagDiff * orthoZoomSpeed;
				camera.orthographicSize = Mathf.Max(camera.orthographicSize, 0.1f);
			}
			else
			{
				camera.fieldOfView += deltaMagDiff * perspectiveZoomSpeed;
				camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, 15, 140);
			}
		}
	}
}
