using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
//
public class SilantroPilot : MonoBehaviour {
	//
	[HideInInspector]public float maxRayDistance= 2f;
	[HideInInspector]public Transform head;
	[HideInInspector]public GameObject notice;
	//
	public enum ControlType
	{
		ThirdPerson,
		FirstPerson
	}
	[HideInInspector]public ControlType controlType = ControlType.ThirdPerson;
	//
//	public SilantroData dataBoard;
	//
	// Update is called once per frame
	void Update () {
		Vector3 direction= transform.TransformDirection(Vector3.forward);
		RaycastHit hit;
		//
		if (Physics.Raycast (head.position, direction, out hit, maxRayDistance)) {
			//
			SilantroController controller = hit.transform.gameObject.GetComponent<SilantroController> ();

			if (controller != null) {
				if (controller.cockpitControl.pilotOnboard) {
					notice.SetActive (false);
				} else {
					notice.SetActive (true);
				}
				if (Input.GetKeyDown (KeyCode.F)) {
					//
					//SEND PLAYER INFORMATION
					controller.cockpitControl.player = this.gameObject;
					//
					if (controlType == ControlType.FirstPerson) {
						controller.cockpitControl.controlType = SilantroCockpit.ControlType.FirstPerson;
					}
					if (controlType == ControlType.ThirdPerson) {
						controller.cockpitControl.controlType = SilantroCockpit.ControlType.ThirdPerson;
					}
					controller.cockpitControl.Enter ();
					//notice.SetActive (false);
				}
			}
			//
			else {
				notice.SetActive (false);
			}
		} else {
			notice.SetActive (false);
		}
	}
	//
	void OnDrawGizmos()
	{
		if (head != null) {
			Gizmos.color = Color.red;
			Gizmos.DrawRay (head.position, transform.forward * maxRayDistance);
		}
	}
}
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroPilot))]
public class PilotEditor: Editor
{
	Color backgroundColor;
	Color silantroColor = Color.cyan;

	public override void OnInspectorGUI()
	{
		backgroundColor = GUI.backgroundColor;
		//
		DrawDefaultInspector ();serializedObject.Update ();
		//
		SilantroPilot pilot = (SilantroPilot)target;
		//
		GUILayout.Space(2f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Entry Configuration", MessageType.None);
		GUI.color = backgroundColor;
		//
		GUILayout.Space(3f);
		pilot.controlType = (SilantroPilot.ControlType)EditorGUILayout.EnumPopup("Player Type",pilot.controlType);
		GUILayout.Space(7f);
		//
		//
		pilot.maxRayDistance = EditorGUILayout.FloatField ("Entry Distance", pilot.maxRayDistance);
		GUILayout.Space (3f);
		pilot.head = EditorGUILayout.ObjectField ("Pilot Head", pilot.head, typeof(Transform), true) as Transform;
		GUILayout.Space (3f);
		pilot.notice = EditorGUILayout.ObjectField ("Entry Notice", pilot.notice, typeof(GameObject), true) as GameObject;
		//
		if (GUI.changed) {
			EditorUtility.SetDirty (pilot);
			EditorSceneManager.MarkSceneDirty (pilot.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
	}
}
#endif