using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class SilantroElectricMotor : MonoBehaviour {
	//STANDARD
	[HideInInspector]public float ratedRPM;
	[HideInInspector]public float ratedVoltage;
	[HideInInspector]public float ratedCurrent;
	//
	[HideInInspector]public float coreRPM;
	float idleRPM;
	float targetRPM;
	//INPUT
	[HideInInspector]public float inputCurrent;
	[HideInInspector]public float inputVoltage;
	[HideInInspector]public float airDensity = 1.225f;
	//
	float num5;
	[HideInInspector]public float powerRating;
	[HideInInspector]public float weight;
	//
	//ENGINE CONFIGURATION
	[HideInInspector]public bool EngineOn;
	[HideInInspector]public float engineAcceleration = 0.2f;
	[HideInInspector]public float EnginePower;
	[HideInInspector]public float enginePower;
	[HideInInspector]public bool isAccelerating;
	[HideInInspector]public bool isDestructible;
	//
	[HideInInspector]public SilantroBatteryPack batteryPack;
	//
	//ENGINE STATE
	public enum EngineState
	{
		Off,
		Starting,
		Running
	}
	//CONTROL KEYCODES
	string startEngine;
	string stopEngine;
	string throttleControl;
	//
	[HideInInspector]public EngineState CurrentEngineState;
	//
	[HideInInspector]public float powerInput = 0.2f;
	//
	[HideInInspector]public float efficiency;
	[HideInInspector]public float torque;
	[HideInInspector]public float horsePower;
	//
	//public SilantroBatteryPack powerSource;
	[HideInInspector]public SilantroInstrumentation instrumentation;
	//
	//ENGINE SOUND SETTINGS
	[HideInInspector]public AudioClip runningSound;
	//
	[HideInInspector]private AudioSource EngineRun;
	//
	[HideInInspector]public float idlePitch = 0.5f;
	[HideInInspector]public float maximumPitch = 1f;
	[HideInInspector]float engineSoundVolume = 2f;
	SilantroControls controlBoard;
	//
	[HideInInspector]public Rigidbody Parent;
	[HideInInspector]public Transform Thruster;
	//
	[HideInInspector]public bool start;
	[HideInInspector]public bool stop;
	[HideInInspector]public bool starting;
	//
	[HideInInspector]public float voltageFactor;
	[HideInInspector]public bool isControllable = true;
	//
	void Awake () {
		controlBoard = GameObject.FindGameObjectWithTag ("GameController").GetComponent<SilantroControls> ();
		if (controlBoard == null) {
			Debug.LogError ("Control Board is missing, Place Control Board in scene and restart!");
		}
		//
		startEngine= controlBoard.engineStart;
		stopEngine = controlBoard.engineShutdown;
		throttleControl = controlBoard.engineThrottleControl;
		//
		batteryPack.Motor = this.gameObject.GetComponent<SilantroElectricMotor>();
	}
	//
	void Start()
	{
		//
		idleRPM = ratedRPM*0.1f;
		//
		if (Parent == null) {
			Debug.Log ("Engine cannot be Operated without a Rigidbody parent!!, add Rigidbody to an empty gameobject if engine is for Test");
		}
		if (batteryPack == null) {
			Debug.Log ("No Battery Pack is attached to this Engine!!, Note: Engine will not function correctly");
		} 
		if (instrumentation == null) {
			Debug.LogError ("Instrumentation is missing!! If Engine is just for Test, add Instrumentation Prefab to the Scene");
		} 
		//
		if (null != runningSound)
		{
			EngineRun = Thruster.gameObject.AddComponent<AudioSource>();
			EngineRun.clip = runningSound;
			EngineRun.loop = true;
			EngineRun.Play();
			engineSoundVolume = EngineRun.volume * 2f;
			EngineRun.spatialBlend = 1f;
			EngineRun.dopplerLevel = 0f;
			EngineRun.rolloffMode = AudioRolloffMode.Custom;
			EngineRun.maxDistance = 600f;
		}
		//
		//
		EngineOn = false;
		start = false;
		starting = false;
		stop = false;
		//
		inputCurrent = ratedCurrent*voltageFactor;
	}
	//
	void FixedUpdate()
	{
		if (coreRPM <= 0) {
			coreRPM = 0f;
		}
	}
	//
	void UseBattery()
	{
		if (batteryPack) {
			batteryPack.current = ratedCurrent*powerInput;
		}
	}
	//ACCELERATE AND DECELERATE ENGINE
	private void EngineActive()
	{
		if (EngineOn)
		{
			if (enginePower < 1f && !isAccelerating)
			{
				enginePower += Time.deltaTime * engineAcceleration;
			}
		}
		else if (enginePower > 0f)
		{
			enginePower -= Time.deltaTime * engineAcceleration;
		}
		else
		{
			enginePower = 0f;
		}
	}
	///
	//
	public IEnumerator ReturnIgnition()
	{
		yield return new WaitForSeconds (0.5f);
		start = false;
		stop = false;
	}

	//
	//
	private void RunEngine()
	{
		//
		powerInput = Mathf.Clamp(powerInput,0f,1f);
		targetRPM = idleRPM + (ratedRPM - idleRPM) * powerInput;
		//
		if (stop)
		{
			CurrentEngineState = EngineState.Off;
			EngineOn = false;
			horsePower = 0;
			powerInput = 0f;
			StartCoroutine(ReturnIgnition());
		}
	}
	//
	//
	private void ShutdownEngine()
	{
		if (start)
		{
			EngineOn = true;
			//EngineStart.Play();
			CurrentEngineState = EngineState.Running;
			starting = true;
			StartCoroutine(ReturnIgnition());
		}
		targetRPM = 0f;
	}
	//
	private void StartEngine()
	{
		if (starting)
		{
				CurrentEngineState = EngineState.Running;
				starting = false;
				//exhaustModule.enabled = true;
				RunEngine();
		}
		else
		{
			CurrentEngineState = EngineState.Off;
		}
		//
		targetRPM = idleRPM;
	}
	//
	void CalculatePower()
	{
		if (instrumentation != null) {
			airDensity = instrumentation.airDensity;
		}
		//
		inputVoltage = voltageFactor *batteryPack.voltage;
		powerRating = inputCurrent * inputVoltage;//Power Rating
		//
		torque = (powerRating*efficiency*60f)/(coreRPM*2f*3.142f*100f);
		//
		horsePower =(coreRPM/ratedRPM)* (torque*coreRPM)/5252f;
		//
		UseBattery();
	}
	//
	///STOP ENGINE IF DESTROYED
	public void DestroyEngine()
	{

		EngineOn = false;
		horsePower = 0f;
	}
	//
	//
	public void Update()
	{
		if (isControllable) {
			//CONTROLS
			if (Input.GetButtonDown (startEngine) && batteryPack != null) {
				start = true;
			}
			if (Input.GetButtonDown (stopEngine)) {
				stop = true;
			}
			//JOYSTICK FUEL CONTROL
			powerInput = Input.GetAxis (throttleControl);
			powerInput = Mathf.Clamp (powerInput, 0f, 100f);
			//
			EngineActive ();
			if (enginePower > 0f) {
				CalculatePower ();
			}
			//
			//
			if (Parent) {
				switch (CurrentEngineState) {
				case EngineState.Off:
					ShutdownEngine ();
					break;
				case EngineState.Starting:
					StartEngine ();
					break;
				case EngineState.Running:
					RunEngine ();
					break;
				}
			}
			//
			if (EngineOn) {
				coreRPM = Mathf.Lerp (coreRPM, targetRPM, 0.5f * Time.deltaTime * (enginePower));
			} else {
				coreRPM = Mathf.Lerp (coreRPM, 0.0f, 0.5f * Time.deltaTime);
			}
			//
			if (null != EngineRun) {
				if (Parent != null) {
					//PERFORM MINOR ENGINE CALCULATIONS TO CONTROL SOUND PITCH
					float magnitude = Parent.velocity.magnitude;
					float num2 = magnitude * 1.94384444f;
					float num3 = coreRPM + num2 * 10f;
					float num4 = (num3 - idleRPM) / (ratedRPM - idleRPM);
					num5 = idlePitch + (maximumPitch - idlePitch) * num4;
					num5 = Mathf.Clamp (num5, 0f, maximumPitch);
					//
				}
				EngineRun.pitch = num5 * enginePower;
				//}
				//
				EngineRun.volume = engineSoundVolume;
				if (coreRPM < idleRPM) {
					EngineRun.volume = engineSoundVolume * num5;
					if (coreRPM < idleRPM * 0.1f) {
						EngineRun.volume = 0f;
					}
				} else {
					EngineRun.volume = engineSoundVolume * num5;
				}
			}
		}
	}
}
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroElectricMotor))]
public class ElectricMotorEditor: Editor
{
	Color backgroundColor;
	Color silantroColor = Color.cyan;
	[HideInInspector]public int toolbarTab;
	[HideInInspector]public string currentTab;
	//
	//

	public override void OnInspectorGUI()
	{
		backgroundColor = GUI.backgroundColor;
		//
		DrawDefaultInspector ();	serializedObject.Update();
		//
		//SilantroTurboJet kk;
		SilantroElectricMotor motor = (SilantroElectricMotor)target;
		//

		GUILayout.Space(10f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Motor Specifications", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		motor.ratedVoltage = EditorGUILayout.FloatField ("Rated Voltage", motor.ratedVoltage);
		GUILayout.Space(3f);
		motor.ratedCurrent = EditorGUILayout.FloatField ("Rated Current", motor.ratedCurrent);
		GUILayout.Space(3f);
		motor.efficiency = EditorGUILayout.Slider ("Motor Efficiency", motor.efficiency, 0f, 100f);
		GUILayout.Space(8f);
		motor.ratedRPM = EditorGUILayout.FloatField ("Rated RPM", motor.ratedRPM);
		GUILayout.Space(3f);
		motor.weight = EditorGUILayout.FloatField ("Weight", motor.weight);
		//
		GUILayout.Space(25f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Power Settings", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		motor.batteryPack = EditorGUILayout.ObjectField ("Battery Pack", motor.batteryPack, typeof(SilantroBatteryPack), true) as SilantroBatteryPack;
		GUILayout.Space(8f);
		EditorGUILayout.LabelField ("Input Voltage", motor.inputVoltage.ToString ("0.0") + " Volts");
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Input Current", motor.inputCurrent.ToString ("0.0") + " Amps");
		//
		GUILayout.Space(25f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Connections", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		motor.Parent = EditorGUILayout.ObjectField ("Motor Parent", motor.Parent, typeof(Rigidbody), true) as Rigidbody;
		GUILayout.Space(2f);
		motor.Thruster = EditorGUILayout.ObjectField ("Thruster", motor.Thruster, typeof(Transform), true) as Transform;
		//
		GUILayout.Space(25f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Sound Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		motor.runningSound = EditorGUILayout.ObjectField ("Running Sound", motor.runningSound, typeof(AudioClip), true) as AudioClip;
		GUILayout.Space(10f);
		motor.idlePitch = EditorGUILayout.FloatField ("Motor Idle Pitch", motor.idlePitch);
		motor.maximumPitch = EditorGUILayout.FloatField ("Motor Maximum Pitch", motor.maximumPitch);

		//
		GUILayout.Space(25f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Performance", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Motor State", motor.CurrentEngineState.ToString ());
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Motor Power", (motor.powerRating/1000).ToString ("0.00") + " kW");
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Core Speed", motor.coreRPM.ToString ("0.0") + " RPM");
		//
		//
		GUILayout.Space(10f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Output", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Shaft Horsepower", motor.horsePower.ToString ("0.0") + " Hp");
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