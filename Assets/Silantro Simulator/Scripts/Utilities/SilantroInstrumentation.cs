//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//using System.Collections;
using System.Collections.Generic;
using UnityEngine;using System;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
//[AddComponentMenu("Oyedoyin/Utilties/Instrumentation")]
public class SilantroInstrumentation : MonoBehaviour {
	//
	[HideInInspector]public Rigidbody airplane;
	//
	public enum AircraftType
	{
		Jet,
		Propeller,
		Rocket
	}
	[HideInInspector]public AircraftType aircraftType;
	//
	[HideInInspector]public float currentSpeed;
	[HideInInspector]public float groundSpeed;
	[HideInInspector]public float machSpeed;
	[HideInInspector]public float currentAltitude;
	[HideInInspector]public float headingDirection;
	[HideInInspector]public float verticalSpeed;
	[HideInInspector]public float airDensity;
	[HideInInspector]public float ambientTemperature;
	[HideInInspector]public float ambientPressure;

	 float XbankAngle;
	 float YbankAngle;
	 float ZbankAngle;
	//
	[HideInInspector]public float xforce;
	float yforce;
	[HideInInspector]public float zforce;
	//
	[HideInInspector]public float gForce;
	// Use this for initialization
	[HideInInspector]public bool Supersonic = false;
	[HideInInspector]public ParticleSystem condenationEffect;
	[HideInInspector]public AudioClip sonicBoom;
	[HideInInspector]public AudioSource boom;
	ParticleSystem.EmissionModule condensation;
	bool sonicing;
	bool played = false;
	float baseDensity;
	SilantroSapphire weather;
	[HideInInspector]public SilantroController controller;
	float sourceTemperature;
	// Update is called once per frame
	//
	void Awake()
	{
		GameObject lighter = GameObject.FindGameObjectWithTag ("Weather");
		if (lighter != null) {
			weather = lighter.GetComponent<SilantroSapphire> ();
		} else {
			Debug.Log ("Consider adding the weather controller for more realism!");
		}
	}
	//
	void Start()
	{
		//
		if (aircraftType == AircraftType.Jet && condenationEffect !=null) {
			condenationEffect.Stop ();
		}
		currentAltitude = airplane.gameObject.transform.position.y * 3.28f;
		float altiKmeter = currentAltitude / 3280.84f;
		//
		float a =  0.0025f * Mathf.Pow(altiKmeter,2f);
		float b = 0.106f * altiKmeter;
		//
		airDensity = a -b +1.2147f;

		baseDensity = airDensity;
	}
	//
	void FixedUpdate () {
		if (airplane != null) {
			currentAltitude = airplane.gameObject.transform.position.y * 3.28f;
		}
		//
		CalculateDensity (currentAltitude);
		CalculateData (currentAltitude);
		//
		//Calculate Speed
		currentSpeed = airplane.velocity.magnitude * 1.944f;
		//
		verticalSpeed = airplane.velocity.y * 3.28f * 60f;
	}
	//
	void CalculateDensity(float altitude)
	{
		float kelvinTemperatrue = ambientTemperature+273.15f;
		airDensity = (ambientPressure*1000f) / (287.05f * kelvinTemperatrue);
	}
	//
	void CalculateData(float altitude)
	{
		//Calculate Temperature
		float a1 = 0.000000003f * altitude * altitude;
		float a2 = 0.0021f * altitude;
		//
		if (weather) {
			ambientTemperature = a1-a2+weather.localTemperature;//
		}
		else
		{
		ambientTemperature = a1-a2+15.443f;//
		}
		//Calculate Pressure
		float a = 0.0000004f * altitude * altitude;
		float b = (0.0351f*altitude);
		ambientPressure =( a - b + 1009.6f)/10f;
		//
		headingDirection = airplane.transform.eulerAngles.y;
		//
		float soundSpeed = Mathf.Pow((1.2f*287f*(273.15f+ambientTemperature)),0.5f);
		machSpeed = (currentSpeed / 1.944f) / soundSpeed;
		//
		if (aircraftType == AircraftType.Jet && Supersonic) {
			if (machSpeed >= 0.98f && !played) {
				Boom ();
			}
			//
			if (machSpeed < 0.98f && played) {
				played = false;
			}
		}
		//
		float densityRatio = airDensity/baseDensity;
		float m = Mathf.Pow (densityRatio, 0.5f);
		groundSpeed = currentSpeed / m;
		//
		//
		XbankAngle = transform.root.eulerAngles.x;
		if (XbankAngle > 180) {
			XbankAngle -= 360;
	}
		YbankAngle = transform.root.eulerAngles.y;if (YbankAngle > 180) {
			YbankAngle -= 360;
		}//yAngle = (yAngle>180)?yAngle-360:yAngle;
		ZbankAngle = transform.root.eulerAngles.z;//if (ZbankAngle > 180) {
		//	ZbankAngle -= 360;
		//}//zAn
		//
		//
		xforce = 1/(Mathf.Cos(XbankAngle*0.0174556f));
		yforce =1/(Mathf.Cos(YbankAngle*0.0174556f));
		zforce =1/(Mathf.Cos(ZbankAngle*0.0174556f));
		//
		gForce = (xforce+zforce)/2f;
		//
		if (XbankAngle > 0) {
			xforce *= -1;
		}
	
	}

	//
	void Boom()
	{
		boom.PlayOneShot (sonicBoom);
		condenationEffect.Emit (250);
		played = true;
	}
	//
}
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroInstrumentation))]
public class InstrumentationEditor: Editor
{
	Color backgroundColor;
	//
	public override void OnInspectorGUI()
	{
		backgroundColor = GUI.backgroundColor;
		//
		DrawDefaultInspector ();SilantroInstrumentation instrument = (SilantroInstrumentation)target;
		//
		serializedObject.Update();
		//
		GUILayout.Space(5f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Connection", MessageType.None);
		GUI.color = backgroundColor;GUILayout.Space(2f);
		GUILayout.Space(3f);
		instrument.airplane = EditorGUILayout.ObjectField ("Aircraft", instrument.airplane, typeof(Rigidbody), true) as Rigidbody;
		GUILayout.Space(3f);
		instrument.aircraftType = (SilantroInstrumentation.AircraftType)EditorGUILayout.EnumPopup("Aircraft Type",instrument.aircraftType);
		//
		GUILayout.Space(2f);
		GUI.color = Color.yellow;
		EditorGUILayout.HelpBox ("Performance Data", MessageType.None);
		GUI.color = backgroundColor;GUILayout.Space(2f);
		GUILayout.Space(5f);
		EditorGUILayout.LabelField ("Air Speed", instrument.currentSpeed.ToString ("0.0") + " knots");
		EditorGUILayout.LabelField ("Ground Speed", instrument.groundSpeed.ToString ("0.0") + " knots");
		EditorGUILayout.LabelField ("Mach", instrument.machSpeed.ToString ("0.00"));
		EditorGUILayout.LabelField ("G-Force", instrument.gForce.ToString ("0.0"));
		EditorGUILayout.LabelField ("Altitude", instrument.currentAltitude.ToString ("0.0") + " feet");
		EditorGUILayout.LabelField ("Vertical Speed", instrument.verticalSpeed.ToString ("0.0") + " ft/min");
		EditorGUILayout.LabelField ("Heading Direction", instrument.headingDirection.ToString ("0.0") + " °");
		//
		GUILayout.Space(3f);
		GUI.color = Color.yellow;
		EditorGUILayout.HelpBox ("Ambient Data", MessageType.None);
		GUI.color = backgroundColor;GUILayout.Space(2f);
		GUILayout.Space(5f);
		EditorGUILayout.LabelField ("Air Density", instrument.airDensity.ToString ("0.000")+ " kg/m3");
		EditorGUILayout.LabelField ("Temperature", instrument.ambientTemperature.ToString ("0.0")+" °C");
		EditorGUILayout.LabelField ("Pressure", instrument.ambientPressure.ToString ("0.0")+ " kPa");
		//
		GUILayout.Space(2f);
		if (instrument.aircraftType == SilantroInstrumentation.AircraftType.Jet) {
			instrument.Supersonic = EditorGUILayout.Toggle ("Supersonic", instrument.Supersonic);
			//
			if (instrument.Supersonic) {
				GUILayout.Space(5f);
				//
				GUI.color = Color.yellow;
				EditorGUILayout.HelpBox ("Supersonic Effects", MessageType.None);
				GUI.color = backgroundColor;GUILayout.Space(2f);
				//
				instrument.condenationEffect = EditorGUILayout.ObjectField ("Condensation Effect", instrument.condenationEffect, typeof(ParticleSystem), true) as ParticleSystem;
				GUILayout.Space(4f);
				instrument.sonicBoom = EditorGUILayout.ObjectField ("Sonic Boom Sound", instrument.sonicBoom, typeof(AudioClip),true) as AudioClip;
			}
		}
		//
		//
		if (GUI.changed) {
			EditorUtility.SetDirty (instrument);
			EditorSceneManager.MarkSceneDirty (instrument.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
	}

}
#endif