using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
//
public class SilantroCockpit : MonoBehaviour {
	//
	[HideInInspector]public bool pilotOnboard = false;
	//
	[HideInInspector]public SilantroHydraulicSystem canopyHydraulics;
	[HideInInspector]public GameObject player;
	[HideInInspector]public GameObject pilot;
	//
	//
	public enum ControlType
	{
		ThirdPerson,
		FirstPerson
	}
	[HideInInspector]public ControlType controlType = ControlType.ThirdPerson;
	//
	[HideInInspector]public Transform getOutPosition;
	[HideInInspector]public SilantroController controller;
	[HideInInspector]public SilantroData dataBoard;
	//
	[HideInInspector]public GameObject ladder;
	//
	private bool  opened = false;
	//private float waitTime = 1f;
	private bool  temp = false;
	//
	float openTime;
	float closeTime;
	//
	void Start()
	{
		if(!getOutPosition){
			GameObject getOutPos = new GameObject("Get Out Position");
			getOutPos.transform.SetParent(transform);
			getOutPos.transform.localPosition = new Vector3(-3f, 0f, 0f);
			getOutPos.transform.localRotation = Quaternion.identity;
			getOutPosition = getOutPos.transform;
		}
		//
		if (canopyHydraulics != null) {
			openTime = 4f;
			closeTime = 4f;
		} else {
			openTime = 1.5f;
			closeTime = 1.3f;
		}
		if (dataBoard != null && dataBoard.panel != null) {
			dataBoard.panel.SetActive (false);
		}
	}
	//
	//MANNED CONTROL
	void Update()
	{
		if (!temp && controller.controlType == SilantroController.ControlType.Internal && pilotOnboard && Input.GetKeyDown (KeyCode.F)) {
		//
			//EXIT CONDITION
			if (controller.gearHelper.wheelSystem [0].collider.isGrounded && controller.datalog.currentSpeed < 10) {
				Exit ();
				opened = false;
				temp = false;
			}
		}
	}
	//
	//START ENTRY PROCEDURE
	public void Enter()
	{
		if (!opened && !temp) {
			if (!pilotOnboard) {
				opened = true;
				temp = true;
				if (canopyHydraulics != null) {
					canopyHydraulics.open = true;
				}

				pilotOnboard = true;
				StartCoroutine (WaitForEnter ());
				//
			}
		}
	}
	//
	IEnumerator WaitForEnter()
	{
		yield return new WaitForSeconds (openTime);
		//SETUP ENTER STATE
		player.SetActive (false);
		pilot.SetActive (true);
		player.transform.SetParent(transform);
		player.transform.localPosition = Vector3.zero;
		player.transform.localRotation = Quaternion.identity;
		StartCoroutine (EnableControls ());
		//
		if (controlType == ControlType.FirstPerson) {
			controller.camisole.enabled = true;
		}
		//SETUP DATA DISPLAY
		dataBoard.cog = controller.datalog;
		dataBoard.controller = controller;
		dataBoard.enabled = true;
		if (dataBoard != null && dataBoard.panel != null) {
			dataBoard.panel.SetActive (true);
		}
		//
		if (ladder != null) {
			ladder.SetActive (false);
		}
	}
	//
	//ENABLE AIRCRAFT CONTROLS
	IEnumerator EnableControls()
	{
		yield return new WaitForSeconds (openTime);
		//ENABLE CONTROLS
		if (canopyHydraulics != null) {
			canopyHydraulics.close = true;
		}
		controller.EnableControls ();
		temp = false;
	}
	//
	public void Eject()
	{
		pilotOnboard = false;
	}
	//
	//START EXIT PROCEDURE
	public void Exit()
	{
		if (pilotOnboard) {
			if (canopyHydraulics != null) {
				canopyHydraulics.open = true;
			}
			//
			pilotOnboard = false;
			StartCoroutine (WaitForExit ());
		}
	}
	//
	IEnumerator WaitForExit()
	{
		yield return new WaitForSeconds (closeTime);
		pilot.SetActive (false);
		player.transform.SetParent (null);
		player.transform.position = getOutPosition.position;
		player.transform.rotation = getOutPosition.rotation;
		player.transform.rotation = Quaternion.Euler (0f, player.transform.eulerAngles.y, 0f);
		player.SetActive (true);
		//
		if (controlType == ControlType.FirstPerson) {
			controller.camisole.enabled = false;
		}
		//
		if (canopyHydraulics != null) {
			canopyHydraulics.close = true;
		}
		//SETUP DATA DISPLAY
		dataBoard.cog = null;
		dataBoard.controller = null;
		dataBoard.enabled = false;
		if (dataBoard != null && dataBoard.panel != null) {
			dataBoard.panel.SetActive (false);
		}
		//
		controller.DisableControls ();
	}
	//
}
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroCockpit))]
public class CockpitEditor: Editor
{
	Color backgroundColor;
	Color silantroColor = Color.cyan;

	public override void OnInspectorGUI()
	{
		backgroundColor = GUI.backgroundColor;
		//
		DrawDefaultInspector ();serializedObject.Update ();
		//
		SilantroCockpit cockpit = (SilantroCockpit)target;
		//
		GUILayout.Space(2f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("System Configuration", MessageType.None);
		GUI.color = backgroundColor;
		//
		GUILayout.Space(3f);
		cockpit.controller = EditorGUILayout.ObjectField ("Aircraft", cockpit.controller, typeof(SilantroController), true) as SilantroController;
		//
		GUILayout.Space(3f);
		cockpit.player = EditorGUILayout.ObjectField ("Player", cockpit.player, typeof(GameObject), true) as GameObject;
		GUILayout.Space(3f);
		//
		cockpit.pilot = EditorGUILayout.ObjectField ("Pilot", cockpit.pilot, typeof(GameObject), true) as GameObject;
		GUILayout.Space(3f);
		cockpit.getOutPosition = EditorGUILayout.ObjectField ("Exit Location", cockpit.getOutPosition, typeof(Transform), true) as Transform;
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Pilot OnBoard", cockpit.pilotOnboard.ToString ());
		//
		GUILayout.Space(10f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Extras", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		cockpit.canopyHydraulics = EditorGUILayout.ObjectField ("Canopy Hydraulics", cockpit.canopyHydraulics, typeof(SilantroHydraulicSystem), true) as SilantroHydraulicSystem;
		GUILayout.Space(3f);
		cockpit.dataBoard = EditorGUILayout.ObjectField ("Data Display", cockpit.dataBoard, typeof(SilantroData), true) as SilantroData;
		GUILayout.Space(3f);
		cockpit.ladder = EditorGUILayout.ObjectField ("Entry Ladder", cockpit.ladder, typeof(GameObject), true) as GameObject;
		//
		if (GUI.changed) {
			EditorUtility.SetDirty (cockpit);
			EditorSceneManager.MarkSceneDirty (cockpit.gameObject.scene);
		}
	}
}
#endif