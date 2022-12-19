using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
//
[RequireComponent(typeof(Camera))]
public class DemoCamera : MonoBehaviour {
	//
	public float CameraDistance = 10.0f;
	public float CameraHeight = 2.0f;
	//
	private float CameraAngle = 180.0f;
	//
	public GameObject FocusPoint;
	public bool CameraActive = true;
	// Use this for initialization
	void Awake () 
	{
		gameObject.GetComponent<Camera>().enabled = false;
		if (FocusPoint == null) {
			FocusPoint = transform.root.gameObject;
		}
	}
	//
	// Update is called once per frame
	void Update () {
		Vector3 zAxis = FocusPoint.transform.forward;
		zAxis.y = 0.0f;
		zAxis.Normalize ();
		zAxis = Quaternion.Euler (0, CameraAngle, 0) * zAxis;

		Vector3 cameraPosition = FocusPoint.transform.position;
		cameraPosition += zAxis * CameraDistance;
		cameraPosition += new Vector3 (0.0f, 1.0f, 0.0f) * CameraHeight;

		Vector3 cameraTarget = FocusPoint.transform.position;

		//Apply to main camera.
		Camera.main.transform.position = cameraPosition;
		Camera.main.transform.LookAt (cameraTarget);

		Camera.main.fieldOfView = gameObject.GetComponent<Camera> ().fieldOfView;
		Camera.main.nearClipPlane = gameObject.GetComponent<Camera> ().nearClipPlane;
		Camera.main.farClipPlane = gameObject.GetComponent<Camera> ().farClipPlane;
	}
}
