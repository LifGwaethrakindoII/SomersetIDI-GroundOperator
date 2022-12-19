using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class SilantroBatteryPack : MonoBehaviour {
	//
	[HideInInspector]public SilantroBattery[] seriesBatteries;
	[HideInInspector]public float voltage;
	[HideInInspector]public float current;
	[HideInInspector]public float capacity;
	[HideInInspector]public float availablePower;
	[HideInInspector]public SilantroElectricMotor Motor;
	// Use this for initialization

	void Start () {
		seriesBatteries = GetComponentsInChildren<SilantroBattery> ();
		voltage = 0;
		capacity = 0;
		//
		foreach(SilantroBattery battery in seriesBatteries)
		{
			battery.outputCurrent = current / seriesBatteries.Length;
			voltage += battery.outputVoltage;
			capacity += battery.capacity;
		}
		//
		availablePower = capacity*voltage;
		if (Motor) {
			//Debug.Log (voltage + "" + Motor.ratedVoltage);
			Motor.voltageFactor = (voltage / Motor.ratedVoltage);
		}
	}
	
	// Update is called once per frame
	void Update () {
		CalculateModules ();
	}
	//
	//
	void CalculateModules () {
		//
		voltage = 0;
		capacity = 0;
		//
		foreach(SilantroBattery battery in seriesBatteries)
		{
			battery.outputCurrent = current / seriesBatteries.Length;
			voltage += battery.outputVoltage;
			capacity += battery.capacity;
		}
		//
		availablePower = capacity*voltage;
	}
	//
	void OnDrawGizmos()
	{
		if (seriesBatteries.Length <= 0) {
			seriesBatteries = GetComponentsInChildren<SilantroBattery> ();
			CalculateModules ();
		}
	}

}
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroBatteryPack))]
public class PackEditor: Editor
{
	Color backgroundColor;
	Color silantroColor = Color.yellow;
	//

	public override void OnInspectorGUI()
	{
		backgroundColor = GUI.backgroundColor;
		//
		DrawDefaultInspector ();serializedObject.Update ();
		//
		SilantroBatteryPack pack = (SilantroBatteryPack)target;
		//
		GUILayout.Space(3f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Available Batteries", MessageType.None);
		GUI.color = backgroundColor;
		//
		//
		GUIContent seriesLabel = new GUIContent ("Battery Count");
		GUILayout.Space(5f);
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.BeginVertical ();
		//

		SerializedProperty series = this.serializedObject.FindProperty("seriesBatteries");
		EditorGUILayout.PropertyField (series.FindPropertyRelative ("Array.size"),seriesLabel);
		GUILayout.Space(5f);
		for (int i = 0; i < series.arraySize; i++) {
			GUIContent label = new GUIContent("Battery " +(i+1).ToString ());
			EditorGUILayout.PropertyField (series.GetArrayElementAtIndex (i), label);
		}
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.EndVertical ();
		//
		//
		GUILayout.Space(20f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Pack Output", MessageType.None);
		GUI.color = backgroundColor;
		//
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Terminal Voltage", pack.voltage.ToString ("0.0") + " Volts");
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Total Capacity", pack.capacity.ToString ("0.0") + " Ah");
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Effective Power",( pack.availablePower/1000).ToString ("0.0") + " kWh");

		//
		//
		if (GUI.changed) {
			EditorUtility.SetDirty (pack);
			EditorSceneManager.MarkSceneDirty (pack.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
	}
}
#endif