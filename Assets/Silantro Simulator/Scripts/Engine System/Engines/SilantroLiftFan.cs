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
//
//
using System.IO;
using System.Text;
//
//[AddComponentMenu("Oyedoyin/Engine System/Lift Fan")]
public class SilantroLiftFan : MonoBehaviour {
	//Fan State
	public enum FanState
	{
		Off,
		Clutching,
		Active
	}
	//public LayerMask surfaceMask;
	//Control switches
	[HideInInspector]public bool start;
	[HideInInspector]public bool stop;
	private bool starting;
	//
	[HideInInspector]public float powerExtract;
	[HideInInspector]public SilantroTurboFan attachedEngine;
	//Fan Properties to calculate Thrust
	[HideInInspector]public float fanDiameter = 1f;
	[HideInInspector]public float fanAcceleration = 0.2f;
	//Efficiency affected by cross-wind and aircraft speed
	[HideInInspector]public float fanEfficiency = 76f;
	//
	[HideInInspector]public bool fanOn;
	[HideInInspector]public float fanPower;
	//
	[HideInInspector]public FanState CurrentFanState = FanState.Off;
	[HideInInspector]public float FanPower;
	private float FanIdleRPM = 10f;
	private float FanMaximumRPM = 100f;
	[HideInInspector]public float currentFanRPM;
	[HideInInspector]public float fanShaftPower;
	//
	//
	[HideInInspector]public bool isAccelerating;
	//
	[HideInInspector]public float DesiredRPM;
	[HideInInspector]public SilantroBladefoil bladefoil;
	[HideInInspector]public float advanceRatio;
	[HideInInspector]public float thrustCoefficient;
	[HideInInspector]public float powerCoefficient;
	[HideInInspector]public float bladeEfficency;
	[HideInInspector]public float CurrentRPM;
	//
	[HideInInspector]public Rigidbody Parent;
	[HideInInspector]public Transform Thruster;
	[HideInInspector]public Transform fan;
	//
	public SilantroFreyr freyr;
	//
	public enum RotationAxis
	{
		X,
		Y,
		Z
	}
	[HideInInspector]public RotationAxis rotationAxis = RotationAxis.X;
	//
	//
	public enum RotationDirection
	{
		CW,
		CCW
	}
	[HideInInspector]public RotationDirection rotationDirection = RotationDirection.CCW;
	//
	//
	private AudioSource fanStart;
	private AudioSource fanRun;
	private AudioSource fanShutdown;
	//
	//SOUND SETTINGS
	private float idlePitch = 0.5f;
	private float maximumPitch = 1.2f;
	private float fanSoundVolume = 1.5f;
	//
	[HideInInspector]public AudioClip fanStartClip;
	[HideInInspector]public AudioClip fanRunningClip;
	[HideInInspector]public AudioClip fanShutdownClip;
	//
	//
	[HideInInspector]public float airDensity = 1.225f;
	[HideInInspector]public float FanThrust;[HideInInspector]public string label = "Available Power";
	//
	SilantroControls controlBoard;
	//
	//
	void Awake () {
		controlBoard = GameObject.FindGameObjectWithTag ("GameController").GetComponent<SilantroControls> ();
		if (controlBoard == null) {
			Debug.LogError ("Control Board is missing, Place Control Board in scene and restart!");
		}

	}
	//
	public IEnumerator ReturnIgnition()
	{
		yield return new WaitForSeconds (0.5f);
		start = false;
		stop = false;
	}
	public void OnOff()
	{
		if (CurrentFanState == FanState.Active) {
			stop = true;
		} else if (CurrentFanState == FanState.Off) {
			start = true;
		}
	}

	//
	void Start()
	{
		attachedEngine.connectedFan = this.GetComponent<SilantroLiftFan> ();
		//
		Parent = attachedEngine.Parent;
		if (Parent == null) {
			Debug.Log ("Engine cannot be Operated without a Rigidbody parent!!, add Rigidbody to an empty gameobject if engine is for Test");
		}
		if (attachedEngine == null) {
			Debug.Log ("Engine requires an active Turbofan engine to function");
		}
		thrustCoefficient = bladefoil.StaticThrustCurve.Evaluate (FanMaximumRPM);
		powerCoefficient = bladefoil.StaticPowerCurve.Evaluate (FanMaximumRPM);
		//
		fanEfficiency = powerCoefficient/thrustCoefficient;
		//CALCULATE MAXIMUM RPM POSSIBLE
		FanMaximumRPM = attachedEngine.HighPressureFanRPM* (powerExtract/100f);
		FanIdleRPM = FanMaximumRPM * 0.1f;
		//

		//
		if (!Thruster) {
			GameObject thruter = new GameObject ();
			Thruster = thruter.transform;
		}
		//SETUP SOUND PARTS
		SetupSoundSystem();
	}
	//
	void SetupSoundSystem()
	{
		if (null != fanStartClip)
		{
			fanStart = Thruster.gameObject.AddComponent<AudioSource>();
			fanStart.clip = fanStartClip;
			fanStart.loop = false;
			fanStart.dopplerLevel = 0f;
			fanStart.spatialBlend = 1f;
			fanStart.rolloffMode = AudioRolloffMode.Custom;
			fanStart.
			maxDistance = 650f;
		}
		if (null != fanRunningClip)
		{
			fanRun = Thruster.gameObject.AddComponent<AudioSource>();
			fanRun.clip =  fanRunningClip;
			fanRun.loop = true;
			fanRun.Play();
			fanSoundVolume = fanRun.volume * 1.3f;
			fanRun.spatialBlend = 1f;
			fanRun.dopplerLevel = 0f;
			fanRun.rolloffMode = AudioRolloffMode.Custom;
			fanRun.maxDistance = 600f;
		}
		if (null != fanShutdownClip)
		{
			fanShutdown = Thruster.gameObject.AddComponent<AudioSource>();
			fanShutdown.clip = fanShutdownClip;
			fanShutdown.loop = false;
			fanShutdown.dopplerLevel = 0f;
			fanShutdown.spatialBlend = 1f;
			fanShutdown.rolloffMode = AudioRolloffMode.Custom;
			fanShutdown.maxDistance = 650f;
		}
	}
	//
	public void Update()
	{
		//
		if (fanRun != null) 
		{
			float rpmControl = (currentFanRPM - FanIdleRPM) / (FanMaximumRPM - FanIdleRPM);
			float pitchControl = idlePitch + (maximumPitch - idlePitch) * rpmControl;
			//
			pitchControl = Mathf.Clamp(pitchControl, 0f, maximumPitch);
			//
			//CONTROL FAN RUN PITCH
			fanRun.pitch = pitchControl * fanPower;
			fanRun.volume = fanSoundVolume;
			//
			if (currentFanRPM < FanIdleRPM) 
			{
				fanRun.volume = fanSoundVolume * pitchControl;
				if (currentFanRPM < FanIdleRPM * 0.1f) {
					fanRun.volume = 0f;
				}
			} 
			else
			{
				fanRun.volume = fanSoundVolume * pitchControl;
			}
		}
		//
		FanPowering();
		//
		if (fanPower > 0f) {
			FanCalculation ();
		}
		//
		if (Parent) {
			switch (CurrentFanState) {
			case FanState.Off:
				ShutDown ();
				break;
			case FanState.Clutching:
				Clutching ();
				break;
			case FanState.Active:
				Running ();
				break;
			}

		}
		//RPM REV
		if (fanOn) {
			CurrentRPM =   Mathf.Lerp (currentFanRPM, DesiredRPM,(fanAcceleration*4f) * Time.deltaTime * fanPower);
		} else {
			CurrentRPM =  Mathf.Lerp (currentFanRPM, 0.0f, (fanAcceleration*4f) * Time.deltaTime);
		}
	}
	//
	//SHUTDOWN
	private void ShutDown()
	{
		if (fanStart.isPlaying) {
			fanStart.Stop ();
			start = false;
		}
		if (start && attachedEngine != null && attachedEngine.EngineOn == true) {
			fanOn = true;
			fanStart.Play ();
			attachedEngine.HighPressureFanRPM = Mathf.Lerp (attachedEngine.HighPressureFanRPM, FanMaximumRPM, 1.5f);
			CurrentFanState = FanState.Clutching;
			starting = true;
			StartCoroutine (ReturnIgnition ());
		}
		DesiredRPM = 0f;
	}
	//START FAN
	private void Clutching()
	{
		if (starting) {
			if (!fanStart.isPlaying) {
				CurrentFanState = FanState.Active;
				starting = false;
				Running();
			}
		}
		else
		{
			fanStart.Stop();
			CurrentFanState = FanState.Off;
		}
		DesiredRPM = FanIdleRPM;
	}
	//RUN FAN
	//
	private void Running()
	{
		if (fanStart.isPlaying) {
			fanStart.Stop ();
		}
		DesiredRPM = FanIdleRPM + (FanMaximumRPM - FanIdleRPM) * attachedEngine.FuelInput;
		if (stop)
		{
			CurrentFanState = FanState.Off;
			attachedEngine.HighPressureFanRPM = Mathf.Lerp(attachedEngine.HighPressureFanRPM,attachedEngine.HPStorage,1.5f);
			fanOn = false;
			fanShutdown.Play();
			StartCoroutine(ReturnIgnition());
		}
	}
	//
	//POWER UP FAN
	private void FanPowering()
	{
		if (fanOn) {
			if (fanPower < 1f && !isAccelerating) {
				fanPower += Time.deltaTime * fanAcceleration;
			}

		} else if (fanPower > 0f) {
			fanPower -= Time.deltaTime * fanAcceleration;

		} else {
			fanPower = 0f;
		}
	}
	//

	//CALCULATE FAN THRUST
	public void FanCalculation()
	{
		FanPower = fanPower * 100f;
		//
		thrustCoefficient = bladefoil.StaticThrustCurve.Evaluate (currentFanRPM);
		powerCoefficient = bladefoil.StaticPowerCurve.Evaluate (currentFanRPM);
		fanEfficiency = powerCoefficient / thrustCoefficient;		//
		//Calculate FAN INTAKE AREA
		float fanIntakeArea = (3.142f * fanDiameter *fanDiameter)/4f;
		//
		float radialSpeed = (2*3.142f*currentFanRPM)/60f;
		float tipSpeed = (radialSpeed * fanDiameter / 2f);
		//CALCULATE FAN THRUST
		FanThrust = thrustCoefficient *airDensity*fanIntakeArea *tipSpeed*tipSpeed;
		//
		//SIMULATE TORQUE EXTRACTION FROM ENGINE
		//
		float dynamicPower = ((FanThrust/4.45f)/9.35f);float conversion = Mathf.Pow(dynamicPower,(3f/2f));
		fanShaftPower = conversion / (fanDiameter * 3.28f);
	}
	//
	//
	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		//
		Handles.color = Color.red;
		if(Thruster != null){
			Handles.DrawWireDisc (Thruster.transform.position, Parent.transform.up,  0.2f);
		}

		Handles.color = Color.blue;
		if(fan != null && Parent!=null){
			Handles.DrawWireDisc (fan.transform.position, Parent.transform.up,(fanDiameter/2f));
		}
		//
		Handles.color = Color.cyan;
		if(Thruster != null && fan != null ){
			Handles.DrawLine (fan.transform.position, Thruster.position);
		}
	}
	//
	#endif
	//
	private void FixedUpdate()
	{
		//ApplyLiftForce();
		//
		if (fan) {
			currentFanRPM = CurrentRPM;
			//

			if (rotationDirection ==RotationDirection.CCW) {
				if (rotationAxis == RotationAxis.X) {
					fan.Rotate (new Vector3 (currentFanRPM *0.1f* Time.deltaTime, 0, 0));
				}
				if (rotationAxis == RotationAxis.Y) {
					fan.Rotate (new Vector3 (0, currentFanRPM  *0.1f* Time.deltaTime, 0));
				}
				if (rotationAxis == RotationAxis.Z) {
					fan.Rotate (new Vector3 (0, 0, currentFanRPM  *0.1f* Time.deltaTime));
				}
			}
			//
			if (rotationDirection == RotationDirection.CW) {
				if (rotationAxis == RotationAxis.X) {
					fan.Rotate (new Vector3 (-1f *currentFanRPM  *0.1f* Time.deltaTime, 0, 0));
				}
				if (rotationAxis == RotationAxis.Y) {
					fan.Rotate (new Vector3 (0, -1f *currentFanRPM  *0.1f* Time.deltaTime, 0));
				}
				if (rotationAxis == RotationAxis.Z) {
					fan.Rotate (new Vector3 (0, 0, -1f *currentFanRPM  *0.1f* Time.deltaTime));
				}
			}
		}
		///
		//
		if (CurrentRPM <= 0f)
		{
			CurrentRPM = 0f;
		}
		//
	}
	//

}
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroLiftFan))]
public class LiftfanEditor: Editor
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
		SilantroLiftFan fan = (SilantroLiftFan)target;
		//
		GUILayout.Space(10f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Power Configuration", MessageType.None);
		GUI.color = backgroundColor;
		//
		fan.attachedEngine = EditorGUILayout.ObjectField ("Connected Engine", fan.attachedEngine, typeof(SilantroTurboFan), true) as SilantroTurboFan;
		GUILayout.Space(3f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Power Extraction Settings", MessageType.None);
		GUI.color = backgroundColor;
		fan.powerExtract = EditorGUILayout.Slider ("Extraction",fan.powerExtract,0,100);
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Shaft Power", fan.fanShaftPower.ToString ("0.00 Hp"));
		//
		//
		GUILayout.Space(15f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Fan Configuration", MessageType.None);
		GUI.color = backgroundColor;
		//
		GUILayout.Space(5f);
		fan.bladefoil = EditorGUILayout.ObjectField ("Blade Airfoil", fan.bladefoil, typeof(SilantroBladefoil), true)as SilantroBladefoil;
		GUILayout.Space(3f);fan.fanDiameter = EditorGUILayout.FloatField ("Fan Diameter", fan.fanDiameter);
		GUILayout.Space(10f);
		//EditorGUILayout.LabelField ("Advance Ratio", fan.advanceRatio.ToString ("0.00"));
		EditorGUILayout.LabelField ("Thrust Coefficient", fan.thrustCoefficient.ToString ("0.00"));
		EditorGUILayout.LabelField ("Power Coefficient", fan.powerCoefficient.ToString ("0.00"));
		EditorGUILayout.LabelField ("Blade Efficiency", fan.bladeEfficency.ToString ("0.0")+ " %");
		//
		//
		GUILayout.Space(25f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Connections", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		//fan.Parent = EditorGUILayout.ObjectField ("Engine Parent", fan.Parent, typeof(Rigidbody), true) as Rigidbody;
		//GUILayout.Space(2f);
		fan.fan = EditorGUILayout.ObjectField ("Intake Fan", fan.fan, typeof(Transform), true) as Transform;
		GUILayout.Space(3f);
		fan.rotationAxis = (SilantroLiftFan.RotationAxis)EditorGUILayout.EnumPopup("Rotation Axis",fan.rotationAxis);
		GUILayout.Space(3f);
		fan.rotationDirection = (SilantroLiftFan.RotationDirection)EditorGUILayout.EnumPopup("Rotation Direction",fan.rotationDirection);

		//
		GUILayout.Space(5f);
		fan.Thruster = EditorGUILayout.ObjectField ("Thruster", fan.Thruster, typeof(Transform), true) as Transform;
		//
		//
		GUILayout.Space(25f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Sound Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		fan.fanStartClip = EditorGUILayout.ObjectField ("Fan Start Sound", fan.fanStartClip, typeof(AudioClip), true) as AudioClip;
		fan.fanRunningClip = EditorGUILayout.ObjectField ("Fan Running Sound", fan.fanRunningClip, typeof(AudioClip), true) as AudioClip;
		fan.fanShutdownClip = EditorGUILayout.ObjectField ("Fan Shutdown Sound", fan.fanShutdownClip, typeof(AudioClip), true) as AudioClip;
		//

		fan.start = EditorGUILayout.Toggle ("Start", fan.start);
		//
		//
		GUILayout.Space(25f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Engine Display", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		//
		EditorGUILayout.LabelField ("Fan State",fan.CurrentFanState.ToString());
		EditorGUILayout.LabelField ("Fan Power",fan.FanPower.ToString("0.00") + " %");
		//
		EditorGUILayout.LabelField ("Core Speed",fan.currentFanRPM.ToString("0.0")+ " RPM");
		GUILayout.Space(3f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Engine Output", MessageType.None);
		GUI.color = backgroundColor;
		EditorGUILayout.LabelField ("Fan Thrust",fan.FanThrust.ToString("0.0")+ " N");
		//EditorGUILayout.LabelField(shaft.
		//
		//
		if (GUI.changed) {
			EditorUtility.SetDirty (fan);
			EditorSceneManager.MarkSceneDirty (fan.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
	}
}
#endif