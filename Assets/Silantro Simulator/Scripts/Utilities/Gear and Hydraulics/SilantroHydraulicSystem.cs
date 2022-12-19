//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
//[AddComponentMenu("Oyedoyin/Components/Hydraulics")]
public class SilantroHydraulicSystem : MonoBehaviour {
	//
	[Header("Hydraulics")]
	public Door[] components;
	//
	[System.Serializable]
	public class Door
	{
		public string Identifier;
		public Transform doorElement;
		//
		[Header("Rotation Angles")]
		public float xAxis;
		public float yAxis;
		public float zAxis;
		[HideInInspector]public Quaternion initialPosition;
		[HideInInspector]public Quaternion finalPosition;
		[HideInInspector]public Quaternion currentRotation;
		//
		[Header("Drag  Settings")]
		public float DragCoefficient;
	}
	//
	[HideInInspector]public float currentRotation;
	//
	[HideInInspector]public bool open;
	bool activated;
	public enum CurrentState
	{
		Open,
		Closed
	}
	[HideInInspector]public CurrentState currentState = CurrentState.Closed;
	//
	public enum StartState
	{
		Open,
		Closed
	}
	[HideInInspector]public StartState startState = StartState.Closed;
	[HideInInspector]public bool started;

	[HideInInspector]public bool close ;
	//
	[HideInInspector]public float currentDragPercentage = 0;
	[HideInInspector]public bool generatesDragWhenOpened = false;
	[HideInInspector]public float dragAmount;
	[HideInInspector]public float currentDrag;
	//
	[HideInInspector]public float openTime = 5f;
	[HideInInspector]public float closeTime = 5f;
	[HideInInspector]public float rotateSpeed = 10f;
	//
	[HideInInspector]public AudioClip openSound;
	[HideInInspector]public AudioClip closeSound;
	//
	[HideInInspector]public bool playSound = true;
	[HideInInspector]public int toolbarTab;
	[HideInInspector]public string currentTab;
	[HideInInspector]public bool isControllable = true;
	//
	AudioSource doorSound;
// Use this for initialization
	void Start () {
		// 
		//
		GameObject soundPoint = new GameObject();
		soundPoint.transform.parent = this.transform;
		soundPoint.transform.localPosition = new Vector3 (0, 0, 0);
		soundPoint.name = this.name +" Sound Point";
		//
		doorSound = soundPoint.AddComponent<AudioSource>();
	
		//
		//SET CURRENT DRAG PERCENTAGE
		if (startState == StartState.Closed) {
			currentDragPercentage = 0f;
			currentState = CurrentState.Closed;
			//SET DOOR ROTATION PROPERTIES
			foreach (Door door in components) {
				door.finalPosition = door.doorElement.localRotation;
				door.initialPosition = Quaternion.Euler (door.xAxis, door.yAxis, door.zAxis);
			}
			//
		} else if (startState == StartState.Open) {
			currentDragPercentage = 100f;
			currentState = CurrentState.Open;
			//SET DOOR ROTATION PROPERTIES
			foreach (Door door in components) {
				door.initialPosition = door.doorElement.localRotation;
				door.finalPosition = Quaternion.Euler (door.xAxis, door.yAxis, door.zAxis);
			}
			//
		}
		//
		started = true;
	}
	
	// Update is called once per frame
	void Update () {
		//
		//CALCULATE DRAG
		if (generatesDragWhenOpened) {
			CalculateDrag ();
		}
		//
		if (open && currentState == CurrentState.Closed) {
			//
			foreach (Door door in components) {
				door.doorElement.localRotation = Quaternion.RotateTowards (door.doorElement.localRotation, door.initialPosition, Time.deltaTime * rotateSpeed);
				door.currentRotation = door.doorElement.localRotation;
				//currentDragPercentage = Mathf.Lerp (currentDragPercentage, 100f,  Time.fixedDeltaTime * 1f);
			}
			if (!activated) {
				StartCoroutine (Open ());
				StartCoroutine (Increase ());
				activated = true;
				if (openSound != null && playSound) {
					doorSound.PlayOneShot (openSound);
				}
			}
		}
		if (close && currentState == CurrentState.Open ) {

			foreach (Door door in components) {
				door.doorElement.localRotation = Quaternion.RotateTowards (door.doorElement.localRotation, door.finalPosition, Time.deltaTime * rotateSpeed);
				door.currentRotation = door.doorElement.localRotation;		
				//currentDragPercentage = Mathf.Lerp (currentDragPercentage, 0f,  Time.fixedDeltaTime * 1f);
			}
			if (!activated) {
				StartCoroutine (Close ());
				StartCoroutine (Decrease ());
				activated = true;
				if (closeSound != null && playSound) {
					doorSound.PlayOneShot (closeSound);
				}
			}
		
		}
		//
	}
	//
	void CalculateDrag()
	{
		dragAmount = 0;
		foreach (Door door in components) {
			float sampleDrag = door.DragCoefficient * currentDragPercentage / 100f;
			dragAmount += sampleDrag;
			if (dragAmount < 0.0) {
				dragAmount = 0;
			}
		}
	}
	//
	IEnumerator Open()
	{
		yield return new WaitForSeconds (openTime);
		CloseSwitches ();
		currentState = CurrentState.Open;
		activated = false;

	}
	IEnumerator Close()
	{
		yield return new WaitForSeconds (closeTime);
		currentState = CurrentState.Closed;
		CloseSwitches ();
		activated = false;

	}
	void CloseSwitches()
	{
		open =  false;
		close = false;
	}
	//
	public IEnumerator Decrease()
	{
		float time = (openTime + closeTime) / 2f;
		float timeSlice = 100f / time;
		while (currentDragPercentage >= 0) {

			currentDragPercentage -= timeSlice;
			yield return new WaitForSeconds (1);
			if (currentDragPercentage <= 0)
				break;
		}
		if (currentDragPercentage < 0) {
			currentDragPercentage = 0;
		}
		yield return null;
	}
	//
	public IEnumerator Increase()
	{
		float time = (openTime + closeTime) / 2f;
		float timeSlice = 100f / time;
		while (currentDragPercentage >= 0) {

			currentDragPercentage += timeSlice;
			yield return new WaitForSeconds (1);
			if (currentDragPercentage >= 100)
				break;
		}
		if (currentDragPercentage > 100) {
			currentDragPercentage = 100;
		}
		yield return null;
	}

}
//
//
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroHydraulicSystem))]
public class HydraulicsEditor: Editor
{
	Color backgroundColor;
	Color silantroColor = Color.cyan;
	//
	//
	public override void OnInspectorGUI()
	{
		backgroundColor = GUI.backgroundColor;
		//
		DrawDefaultInspector ();
		//
		serializedObject.Update ();
		//
		SilantroHydraulicSystem hydraulic = (SilantroHydraulicSystem)target;
		//
		//
		GUILayout.Space (10f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Controls", MessageType.None);
		GUI.color = backgroundColor;
		if (!hydraulic.started) {
			GUILayout.Space (3f);
			hydraulic.startState = (SilantroHydraulicSystem.StartState)EditorGUILayout.EnumPopup ("Start State", hydraulic.startState);
		}
		GUILayout.Space (2f);
		EditorGUILayout.LabelField ("Current State", hydraulic.currentState.ToString ());
		GUILayout.Space (2f);
		hydraulic.open = EditorGUILayout.Toggle ("Engage", hydraulic.open);
		GUILayout.Space (2f);
		hydraulic.close = EditorGUILayout.Toggle ("Disengage", hydraulic.close);
		//
		GUILayout.Space (15f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Actuator Settings", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space (3f);
		EditorGUILayout.LabelField ("Actuation Level", hydraulic.currentDragPercentage.ToString ("0") + " %");
		GUILayout.Space (3f);
		hydraulic.openTime = EditorGUILayout.FloatField ("Open Time", hydraulic.openTime);
		GUILayout.Space (2f);
		hydraulic.closeTime = EditorGUILayout.FloatField ("Close Time", hydraulic.closeTime);
		GUILayout.Space (2f);
		hydraulic.rotateSpeed = EditorGUILayout.FloatField ("Rotate Speed", hydraulic.rotateSpeed);
		//
		GUILayout.Space (15f);
		hydraulic.generatesDragWhenOpened = EditorGUILayout.Toggle ("Generates Drag", hydraulic.generatesDragWhenOpened);
		if (hydraulic.generatesDragWhenOpened) {
			GUILayout.Space (3f);
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("Drag Settings", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Total Drag Coefficient", hydraulic.dragAmount.ToString ("0.0"));
			GUILayout.Space (2f);
			EditorGUILayout.LabelField ("Current Drag Force", hydraulic.currentDrag.ToString ("0.0") + " N");
		}
		//
		GUILayout.Space (15f);
		hydraulic.playSound = EditorGUILayout.Toggle ("Play Sounds", hydraulic.playSound);
		if (hydraulic.playSound) {
			GUILayout.Space (2f);
			EditorGUILayout.HelpBox ("Sound Settings", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (3f);
			hydraulic.openSound = EditorGUILayout.ObjectField ("Open Sound", hydraulic.openSound, typeof(AudioClip), true) as AudioClip;
			GUILayout.Space (2f);
			hydraulic.closeSound = EditorGUILayout.ObjectField ("Close Sound", hydraulic.closeSound, typeof(AudioClip), true) as AudioClip;
		}
		//
		if (GUI.changed) {
			EditorUtility.SetDirty (hydraulic);
			EditorSceneManager.MarkSceneDirty (hydraulic.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
	}
}
#endif