using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
//

public class SilantroSapphire : MonoBehaviour {

	[HideInInspector]public Light sun;
	[HideInInspector]public float fullDaySeconds = 100f;
	[Range(0,1)]
	[HideInInspector]public float currentTime = 0;
	[HideInInspector]public float multiplier = 1f;
	// Use this for initialization
	 int hour;
	 int minute;
	[HideInInspector]public string CurrentTime;
	[HideInInspector]public float sunMaximumIntensity = 1.3f;
	[HideInInspector]public float localTemperature;
	[HideInInspector]public SilantroTemperature temperaturePreset;
	//
	[HideInInspector]public float maximumSolarIntensity = 1000f;
	[HideInInspector]public float currentSolarIntensity;
	//
	[HideInInspector]public float sunAngle;
	[HideInInspector]public float sunClimateAngle = 107;
	[HideInInspector]float superlation;
	//
	//
	void Update () {
		//
		UpdateSun ();
		//
		currentTime += (Time.deltaTime/fullDaySeconds)*multiplier;
		//
		if (currentTime >= 1) {
			currentTime = 0;
		}
		//
		//CALCULATE LOCAL TEMPERATURE
		float localTime = 24f*currentTime;
		localTemperature = temperaturePreset.temperature.Evaluate (localTime);
	}
	//
	void UpdateSun ()
	{
		string definition;
		//
		float currentHour;
		float currentMinute;
		//
		currentHour = (24f*currentTime);
		currentMinute = 60*(currentHour - Mathf.Floor (currentHour));
		//
		if (currentHour >= 13) {
			currentHour = currentHour - 12;
			definition = " PM";
		} else {
			definition = " AM";
		}
		//
		hour = (int)currentHour;
		minute = (int)currentMinute;
		CurrentTime = hour.ToString () + ":" + minute.ToString ("00") + definition;
		//
		sun.transform.eulerAngles = new Vector3(currentTime * 360 - 90, sunClimateAngle, 180);
		//sun.transform.localRotation = Quaternion.Euler ((currentTime * 360f) - 90, 107, 0);
		sunAngle = sun.transform.localRotation.eulerAngles.x;
		if (sunAngle > 0 && sunAngle < 180) {
			float sunRadians = sunAngle * Mathf.Deg2Rad;
			superlation = 1-Mathf.Cos (sunRadians);
		} else {
			superlation = 0;
		}
		//
		float intensityMultiplier = 1;
		if (currentTime <= 0.23f || currentTime >= 0.75f) {
			intensityMultiplier = 0;
		} else if (currentTime <= 0.25f) {
			intensityMultiplier = Mathf.Clamp01 ((currentTime - 0.23f) * (1 / 0.02f));
		}
		else if (currentTime >= 0.73f) {
			intensityMultiplier = Mathf.Clamp01 (1 - ((currentTime - 0.73f) * (1 / 0.02f)));
		}
		//
		sun.intensity = sunMaximumIntensity * intensityMultiplier;
		currentSolarIntensity = maximumSolarIntensity * intensityMultiplier*superlation;
		//
	}


}
//
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroSapphire))]
public class WeatherEditor: Editor
{
	Color backgroundColor;
	Color silantroColor;
	//
	public override void OnInspectorGUI()
	{
		backgroundColor = GUI.backgroundColor;
		//
		DrawDefaultInspector ();
		SilantroSapphire weather = (SilantroSapphire)target;
		//
		serializedObject.Update();
		//
		GUILayout.Space(10f);
		GUI.color = Color.yellow;
		EditorGUILayout.HelpBox ("Time Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		weather.fullDaySeconds = EditorGUILayout.FloatField ("Full Day RealTime", weather.fullDaySeconds);
		GUILayout.Space(6f);
		EditorGUILayout.LabelField ("Current Time", weather.CurrentTime);
		//
		GUILayout.Space(20f);
		//GUILayout.Space(10f);
		GUI.color = Color.yellow;
		EditorGUILayout.HelpBox ("Solar Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		weather.sun = EditorGUILayout.ObjectField ("Sun", weather.sun, typeof(Light), true) as Light;
		GUILayout.Space(8f);
		weather.sunMaximumIntensity = EditorGUILayout.FloatField ("Maximum Intensity", weather.sunMaximumIntensity);
		GUILayout.Space(3f);
		weather.currentSolarIntensity = EditorGUILayout.FloatField ("Radiation Intensity", weather.maximumSolarIntensity);
		GUILayout.Space(3f);
		weather.sunClimateAngle = EditorGUILayout.FloatField ("Solar Angle", weather.sunClimateAngle);
		//
		GUILayout.Space(20f);
		//GUILayout.Space(10f);
		GUI.color = Color.yellow;
		EditorGUILayout.HelpBox ("Weather Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		weather.temperaturePreset = EditorGUILayout.ObjectField ("Temperature Preset", weather.temperaturePreset, typeof(SilantroTemperature), true) as SilantroTemperature;
		GUILayout.Space(6f);
		EditorGUILayout.LabelField ("Local Temperature", weather.localTemperature.ToString ("0.0") + " C");
	//
		//
		if (GUI.changed) {
			EditorUtility.SetDirty (weather);
			EditorSceneManager.MarkSceneDirty (weather.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
	}

}
#endif