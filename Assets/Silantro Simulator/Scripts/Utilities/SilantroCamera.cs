using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
//

[RequireComponent(typeof(Camera))]
public class SilantroCamera : MonoBehaviour {
	//
	public enum CameraType
	{
		Orbit,
	//	Velocity
	}
	[HideInInspector]public CameraType sweepDirection = CameraType.Orbit;
	//
	[HideInInspector]public float CameraDistance = 10.0f;
	[HideInInspector]public float CameraHeight = 2.0f;
	//
	private bool FirstClick = false;
	private Vector3 MouseStart;
	private float CameraAngle = 180.0f;
	//
	[HideInInspector]public GameObject FocusPoint;
	[HideInInspector]public bool CameraActive = true;
	//
	// Use this for initialization
	void Awake () 
	{
		gameObject.GetComponent<Camera>().enabled = false;
		if (FocusPoint == null) {
			FocusPoint = transform.root.gameObject;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if ((CameraActive) && (null != FocusPoint)) {
			if (sweepDirection == CameraType.Orbit) {
				if (Input.GetMouseButton (0)) {
					if (FirstClick) {
						MouseStart = Input.mousePosition;
						FirstClick = false;
					}
					CameraAngle += (Input.mousePosition - MouseStart).x * Time.deltaTime;
				} else {
					FirstClick = true;
				}

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
	}

}
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroCamera))]
public class CameraEditor: Editor
{
	Color backgroundColor;
	public override void OnInspectorGUI()
	{
		backgroundColor = GUI.backgroundColor;
		//
		DrawDefaultInspector ();
		SilantroCamera camera = (SilantroCamera)target;
		//
		serializedObject.Update();
		//
		GUILayout.Space(10f);
		GUI.color = Color.yellow;
		EditorGUILayout.HelpBox ("Camera Type", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		camera.sweepDirection = (SilantroCamera.CameraType)EditorGUILayout.EnumPopup ("Type", camera.sweepDirection);
		//
		GUILayout.Space(20f);
		GUI.color = Color.yellow;
		EditorGUILayout.HelpBox ("Camera Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		camera.FocusPoint = EditorGUILayout.ObjectField ("Focus Point", camera.FocusPoint, typeof(GameObject), true) as GameObject;
		GUILayout.Space(10f);
		camera.CameraDistance = EditorGUILayout.FloatField ("Camera Distance", camera.CameraDistance);
		GUILayout.Space(3f);
		camera.CameraHeight = EditorGUILayout.FloatField ("Camera Height", camera.CameraHeight);
		//
		GUILayout.Space(3f);
		camera.CameraActive = EditorGUILayout.Toggle ("Camera Active", camera.CameraActive);
		//
		if (GUI.changed) {
			EditorUtility.SetDirty (camera);
			EditorSceneManager.MarkSceneDirty (camera.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
	}
}
#endif