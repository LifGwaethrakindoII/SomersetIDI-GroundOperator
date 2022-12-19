using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
//
public class SilantroRocketMotor : MonoBehaviour {
	//
	public enum FuelType
	{
		Liquid,
		Solid
	}
	[HideInInspector]public FuelType fuelType = FuelType.Liquid;
	public enum LiquidFuelType
	{
		RP1,
		RP2,
		Hydrogen,
		MMH,
		UDMHComposite
	}
	[HideInInspector]public LiquidFuelType liquidfuelType = LiquidFuelType.RP1;
	//
	public enum SolidFuelType
	{
		PBSR,//236
		ThiokolAmmoniumPerchlorate,//202
		//RubberAmmoniumPerchlorate,
		PolyurethaneAmmoniumPerchlorate,//241
		NitropolymerAmmoniumPerchlorate,//222
		DoublebaeAmmonumNitrate,//250
		NitropolymerAmmoniumNitrate//215
	}
	[HideInInspector]public AnimationCurve thrustCurve;
	[HideInInspector]public float burnTime;
	//FUEL PROPERTIES
	[HideInInspector]public float specificImpulse;
	[HideInInspector]public float heatingValue;
	[HideInInspector]public float density;
	[HideInInspector]public float combustionTemperature;
	[HideInInspector]public float molarMass;
	//
	[HideInInspector]public float chamberPressure;
	//
	//Performance
	[HideInInspector]public float Thrust;
	[HideInInspector]public float flowRate;
	[HideInInspector]public float fuelFlowRate;
	[HideInInspector]public float currentSpeed;
	float areaFactor;
	[HideInInspector]public float exhaustTemperature; 
	[HideInInspector]public float effectiveExhautVelocity;
	[HideInInspector]public float theoreticalExhaustVelocity;
	[HideInInspector]public float meanExhaustVelocity;
	//
	//Pressures
	[HideInInspector]public float atmosphericPressure;
	[HideInInspector]public float nozzlePressure;
	//
	[HideInInspector]public float nozzleArea;
	[HideInInspector]public float throatArea;
	float coefficientFactor;
	[HideInInspector]public float thrustCoefficient;
	//
	//EXHAUST EFFECTS
	[HideInInspector]public ParticleSystem exhaustSmoke;
	[HideInInspector]ParticleSystem.EmissionModule smokeModule;
	[HideInInspector]public ParticleSystem exhaustFlame;
	[HideInInspector]ParticleSystem.EmissionModule flameModule;
	[HideInInspector]public float maximumSmokeEmissionValue = 50f;
	[HideInInspector]public float maximumFlameEmissionValue = 50f;
	//
	[HideInInspector]public AudioClip motorSound;
	AudioSource boosterSound;
	[HideInInspector]public float maximumPitch =1.2f;
	//
	[HideInInspector]public Rigidbody Rocket;
	[HideInInspector]public SilantroInstrumentation control;
	[HideInInspector]public SilantroRocket booster;
	[HideInInspector]public float runTime;
	public bool active;
	float fuelFactor;
	//
	public void InitialCalculations()
	{
		if (exhaustSmoke != null) {
			smokeModule = exhaustSmoke.emission;
			smokeModule.rateOverTime = 0.0f;
		}
		if (exhaustFlame != null) {
			flameModule = exhaustFlame.emission;
			flameModule.rateOverTime = 0.0f;
		}
		//SETUP SOUND
		GameObject soundPoint = new GameObject("Booster Sound");
		soundPoint.transform.parent = this.transform;
		soundPoint.transform.localPosition = new Vector3 (0, 0, 0);
		boosterSound = soundPoint.AddComponent<AudioSource>();
		boosterSound.dopplerLevel = 0f;
		boosterSound.spatialBlend = 1f;
		boosterSound.rolloffMode = AudioRolloffMode.Custom;
		boosterSound.maxDistance = 650f;
		boosterSound.clip = motorSound;
		boosterSound.loop = true;boosterSound.Play ();boosterSound.volume = 0.0f;
		//
		areaFactor = (0.0088f* (chamberPressure)) + 0.96f;
		throatArea = nozzleArea / areaFactor;
		//
		float exhaustFactor = (-0.0004f*(chamberPressure))+0.7488f;
		exhaustTemperature = combustionTemperature * exhaustFactor;
		//
		effectiveExhautVelocity = 9.801f*specificImpulse;
		float velocityFactor = (97.608f * exhaustTemperature) / molarMass;
		theoreticalExhaustVelocity = Mathf.Pow (velocityFactor, 0.5f);
		//
		meanExhaustVelocity = (theoreticalExhaustVelocity+effectiveExhautVelocity)/2f;
		//
		float flowFactor1 = ((2.6066f*molarMass)/(8.134f*combustionTemperature));
		float flowFactor2 = Mathf.Pow (flowFactor1, 0.5f);
		flowRate = chamberPressure * throatArea * flowFactor2;
		//
		fuelFlowRate = flowRate;
		//
		nozzlePressure = 0.564f*chamberPressure;
		//
		//SOLID BOOSTER
		//Calculations based on assumption that gamma y=1.4
		float a = (nozzlePressure / chamberPressure);
		float b = Mathf.Pow (a, 0.3857f);
		float c = 7.7f * b * (1 - b)*0.1937f;
		float f = nozzleArea / throatArea;
		//
		coefficientFactor = c*f;
		//
		//SETUP CURVE
		float a1 = 0.15f*burnTime;
		float b1 = 0.30f*burnTime;
		float c1 = 0.60f*burnTime;
		float d1 = 0.80f*burnTime;
		float e1 = 1.0f*burnTime;
		Keyframe a2 = new Keyframe (0, 0);
		Keyframe a3 = new Keyframe (a1, 90f);
		Keyframe b2 = new Keyframe (b1, 100f);
		Keyframe c2 = new Keyframe (c1, 100f);
		Keyframe d2 = new Keyframe (d1, 100f);
		Keyframe e2 = new Keyframe (e1, 0f);
		//
		thrustCurve.AddKey(a2);
		thrustCurve.AddKey(a3);
		thrustCurve.AddKey(b2);
		thrustCurve.AddKey(c2);
		thrustCurve.AddKey(d2);
		thrustCurve.AddKey(e2);
	}
	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		//
		if(burnTime >0){
		//SETUP CURVE
		//
		thrustCurve = new AnimationCurve();
		//
		float a1 = 0.15f*burnTime;
		float b1 = 0.30f*burnTime;
		float c1 = 0.60f*burnTime;
		float d1 = 0.80f*burnTime;
		float e1 = 1.0f*burnTime;
			if (booster == null) {
				booster = GetComponentInParent<SilantroRocket> ();
			}
			if(booster.boosterType == SilantroRocket.BoosterType.Booster)
			{
				Keyframe a2 = new Keyframe (0, 0);
				Keyframe a3 = new Keyframe (a1, 90f);
				Keyframe b2 = new Keyframe (b1, 100f);
				Keyframe c2 = new Keyframe (c1, 100f);
				Keyframe d2 = new Keyframe (d1, 100f);
				Keyframe e2 = new Keyframe (e1, 0f);
				//
				thrustCurve.AddKey(a2);
				thrustCurve.AddKey(a3);
				thrustCurve.AddKey(b2);
				thrustCurve.AddKey(c2);
				thrustCurve.AddKey(d2);
				thrustCurve.AddKey(e2);
			}
			if (booster.boosterType == SilantroRocket.BoosterType.Weapon) {
				Keyframe a2 = new Keyframe (0, 100);
				Keyframe a3 = new Keyframe (a1, 100f);
				Keyframe b2 = new Keyframe (b1, 100f);
				Keyframe c2 = new Keyframe (c1, 90f);
				Keyframe d2 = new Keyframe (d1, 70f);
				Keyframe e2 = new Keyframe (e1, 0f);
				//
				thrustCurve.AddKey(a2);
				thrustCurve.AddKey(a3);
				thrustCurve.AddKey(b2);
				thrustCurve.AddKey(c2);
				thrustCurve.AddKey(d2);
				thrustCurve.AddKey(e2);
			}
		
		
	 }
	}
	#endif
	// Update is called once per frame
	void FixedUpdate () {
		//
		if (active) {
			runTime += Time.deltaTime;
			EngineCalculations ();
			//
			Vector3 force = this.transform.forward * Thrust;
			//
			if (booster.boosterType == SilantroRocket.BoosterType.Weapon) {
				Rocket.AddForce (force, ForceMode.Force);
			}
			if (booster.boosterType == SilantroRocket.BoosterType.Booster) {
				booster.Aircraft.AddForce (force, ForceMode.Force);
			}
			//
			currentSpeed = control.currentSpeed;
		}
		//
	}
	//
	//START MOTOR
	public void Fire()
	{
		active = true;
		runTime = 0.0f;

	}
	//DO MATHEMATICAL CALCULATIONS
	void EngineCalculations()
	{
		
		//
		atmosphericPressure = control.ambientPressure;
		//Calculate Engine Thrust
		if (fuelType == FuelType.Liquid) { 
			
			Thrust = (fuelFlowRate * meanExhaustVelocity )+ ((nozzlePressure*6894.757f) - (atmosphericPressure*1000f)) * nozzleArea ;
		}
		if (fuelType == FuelType.Solid) {
			//
			fuelFactor = thrustCurve.Evaluate(runTime);
			//Calculations based on assumption that gamma y=1.4
			float d = ((chamberPressure*6894.757f) - (atmosphericPressure*1000f)) / (chamberPressure*6894.757f);
			thrustCoefficient = coefficientFactor * d;
			Thrust = (chamberPressure*6894.757f) * throatArea * thrustCoefficient*(fuelFactor/100f);
			//
			float soundVolume = maximumPitch *(fuelFactor/100f);
			boosterSound.volume = soundVolume;
			//
			if (exhaustFlame) {
				flameModule.rateOverTime = maximumFlameEmissionValue *(fuelFactor/100f);
			}
			if (exhaustSmoke) {
				smokeModule.rateOverTime = maximumSmokeEmissionValue *(fuelFactor/100f);
			}
			//
			if (runTime > burnTime) {
				active = false;
			}
		}
		//

	}
}
//
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroRocketMotor))]
[CanEditMultipleObjects]
public class RocketMotorEditor: Editor
{
	Color backgroundColor;
	Color silantroColor = Color.cyan;
	//
	public override void OnInspectorGUI()
	{
		backgroundColor = GUI.backgroundColor;
		//
		DrawDefaultInspector ();
		//
		SilantroRocketMotor motor = (SilantroRocketMotor)target;
		//
		GUILayout.Space(2f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Motor Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Throat Area", (motor.throatArea*10000).ToString ("0.00") + " cm2");
		GUILayout.Space(2f);
		motor.chamberPressure = EditorGUILayout.FloatField ("Chamber Pressure", motor.chamberPressure);
		//
		GUILayout.Space(20f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Fuel Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		motor.fuelType = (SilantroRocketMotor.FuelType)EditorGUILayout.EnumPopup ("Fuel Type", motor.fuelType);
		GUILayout.Space(3f);
		//
		if (motor.fuelType == SilantroRocketMotor.FuelType.Liquid) {
			//
			motor.liquidfuelType = (SilantroRocketMotor.LiquidFuelType)EditorGUILayout.EnumPopup("Liquid Fuel Type",motor.liquidfuelType);
			//
			if (motor.liquidfuelType == SilantroRocketMotor.LiquidFuelType.RP1) {
				motor.specificImpulse = 358f;
				motor.heatingValue = 47f;
				motor.density = 0.91f;
				motor.molarMass = 27.6f;
				motor.combustionTemperature = 3670f;
			} else if (motor.liquidfuelType == SilantroRocketMotor.LiquidFuelType.Hydrogen) {
				motor.specificImpulse = 391f;
				motor.heatingValue = 141.7f;
				motor.density = 0.090f;
				motor.molarMass = 28.2f;
				motor.combustionTemperature = 2985f;
			} else if (motor.liquidfuelType == SilantroRocketMotor.LiquidFuelType.MMH) {
				motor.specificImpulse = 292f;
				motor.heatingValue = 47.8f;
				motor.density = 1.021f;
				motor.combustionTemperature = 910f;
			} else if (motor.liquidfuelType == SilantroRocketMotor.LiquidFuelType.RP2) {
				motor.specificImpulse = 373.5f;
				motor.heatingValue = 43.2f;
				motor.density = 0.821f;
				motor.combustionTemperature = 3459f;
			} 
			//
			GUILayout.Space(3f);
			EditorGUILayout.LabelField ("Density",motor.density.ToString("0.000")+" kg/m3");
			GUILayout.Space(1f);
			EditorGUILayout.LabelField ("Specific Impulse",motor.specificImpulse.ToString("0")+" s");
			GUILayout.Space(1f);
			EditorGUILayout.LabelField ("Heating Value",motor.heatingValue.ToString("0.0")+" MJ/kg");
			GUILayout.Space(1f);
			EditorGUILayout.LabelField ("Combustion Temperature",motor.combustionTemperature.ToString("0")+" °K");
			//
		}
		if (motor.fuelType == SilantroRocketMotor.FuelType.Solid) {
			GUILayout.Space(3f);
			motor.burnTime = EditorGUILayout.FloatField ("Burn Time", motor.burnTime);
			GUILayout.Space(2f);
			EditorGUILayout.CurveField ("Thrust Curve", motor.thrustCurve);
		}
		//
		GUILayout.Space(20f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Rocket Effects", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		motor.exhaustSmoke = EditorGUILayout.ObjectField ("Exhaust Smoke", motor.exhaustSmoke, typeof(ParticleSystem), true) as ParticleSystem;
		GUILayout.Space (2f);
		motor.maximumSmokeEmissionValue = EditorGUILayout.FloatField ("Maximum Emission", motor.maximumSmokeEmissionValue);
		GUILayout.Space(5f);
		motor.exhaustFlame = EditorGUILayout.ObjectField ("Exhaust Flame", motor.exhaustFlame, typeof(ParticleSystem), true) as ParticleSystem;
		GUILayout.Space (2f);
		motor.maximumFlameEmissionValue = EditorGUILayout.FloatField ("Maximum Emission", motor.maximumFlameEmissionValue);
		//
		GUILayout.Space(20f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Sound Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		motor.motorSound = EditorGUILayout.ObjectField ("Booster Sound", motor.motorSound, typeof(AudioClip), true) as AudioClip;
		GUILayout.Space(2f);
		motor.maximumPitch = EditorGUILayout.FloatField ("Maximum Pitch", motor.maximumPitch);
		//
		GUILayout.Space(20f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Output", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Thrust Generated", motor.Thrust.ToString ("0.00") + " N");
		//
		//
		if (GUI.changed) {
			EditorUtility.SetDirty (motor);
			EditorSceneManager.MarkSceneDirty (motor.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
	}
}
#endif