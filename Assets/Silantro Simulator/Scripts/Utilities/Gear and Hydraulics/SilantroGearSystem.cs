using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
//
//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
	public class SilantroGearSystem : MonoBehaviour {
	//
	//WHEEL CONFIGURATION
	[Header("Wheel Configuration")]
	public List<WheelSystem> wheelSystem = new List<WheelSystem>();
	//
	[HideInInspector]public float maximumSteerAngle =20f;
	[HideInInspector]public float maximumSteerSpeed = 20f;
	[HideInInspector]float speed;
	[HideInInspector]public Transform frontWheelAxle;
	[HideInInspector]public SilantroLight[] lights;
	public enum RotationAxis
	{
		X,
		Y,
		Z
	}
	[HideInInspector]public RotationAxis rotationAxis = RotationAxis.X;
	Vector3 axisRotation;
	private Quaternion InitialModelRotation = Quaternion.identity;
	//
	[HideInInspector]public float brakeTorque= 5000f;
	[HideInInspector]public bool brakeActivated = true;
	//FOR DISPLAY
	string brake ;
	[HideInInspector]public float availablePushForce = 1000f;
	[HideInInspector]public SilantroController control;
	[HideInInspector]public float aircraftWeight;
	//
	[HideInInspector]public float steerAngle;
	string IncrementalBrake;
	[HideInInspector]public float brakeControl;
	[HideInInspector]public float ActualBrakeTorque;
	//
	[HideInInspector]public bool playSound;
	[HideInInspector]public AudioClip groundRoll;
	[HideInInspector]public AudioSource soundSource;
	//
	[HideInInspector]public SilantroControls controlBoard;
	[System.Serializable]
	public class WheelSystem
	{
		[Header("Connections")]
		public string Identifier;
		public WheelCollider collider;
		public Transform wheelModel;
		//
		[Header("Axis Rotation")]
		public bool AxisX = true;
		public bool AxisY;
		public bool AxisZ;
		//
		[Header("Controls")]
		public bool steerable;
		public bool attachedMotor;
	}
	//
	bool inTransition;
	[HideInInspector]public SilantroHydraulicSystem gearHydraulics;
	[HideInInspector]public SilantroHydraulicSystem doorHydraulics;
	[HideInInspector]public SilantroSequenceHydraulics[] hydraulics;
	public enum GearType
	{
		Combined,
		Seperate,
		Complex
	}
	[HideInInspector]public GearType gearType;
	//
	[HideInInspector]public WheelCollider LeftbalanceWheel;
	[HideInInspector]public WheelCollider RightbalanceWheel;
	[HideInInspector]public bool open;
	[HideInInspector]public bool gearOpened = true;
	//
	[HideInInspector]public bool isControllable = true;
	string gearActivate;
	[HideInInspector]public bool close ;
	[HideInInspector]public bool gearClosed = false;
	//
	Rigidbody aircraft;
	//
	void Awake () {
		//
		controlBoard = GameObject.FindGameObjectWithTag ("GameController").GetComponent<SilantroControls> ();
		if (controlBoard == null) {
			Debug.LogError ("Control Board is missing, Place Control Board in scene and restart!");
		}
		brake = controlBoard.BrakeHoldRelease;
		IncrementalBrake = controlBoard.IncrementalBrake;
		gearActivate = controlBoard.LandingGear;
	//
	}
	void Start()
	{
		//
		aircraft = GetComponentInParent<Rigidbody>();
		//
		brakeActivated = false;//
		foreach (WheelSystem system in wheelSystem) {
			if (system.attachedMotor) {
				system.collider.motorTorque =  10;
			}
		}
		if (rotationAxis == RotationAxis.X) {
			axisRotation = new Vector3 (1, 0, 0);
		} else if (rotationAxis == RotationAxis.Y) {
			axisRotation = new Vector3 (0, 1, 0);
		} else if (rotationAxis == RotationAxis.Z) {
			axisRotation = new Vector3 (0, 0, 1);
		}
		axisRotation.Normalize();

		if ( null != frontWheelAxle )
		{
			InitialModelRotation = frontWheelAxle.transform.localRotation;
		}
		brakeActivated = true;
		//
		if (playSound && groundRoll != null) {
			GameObject source = new GameObject ("Sound Source");
			source.transform.parent = this.transform;
			source.transform.localPosition = new Vector3 (0, 0, 0);
			soundSource = source.AddComponent<AudioSource> ();
			//
			soundSource.clip = groundRoll;
			soundSource.loop = true;
			soundSource.Play ();
			soundSource.volume = 0f;
			soundSource.spatialBlend = 1f;
			soundSource.dopplerLevel = 0f;
			soundSource.rolloffMode = AudioRolloffMode.Custom;
			soundSource.maxDistance = 180f;
		}
	}
	void Update()
	{
		//
		if (isControllable) {
			
			if (Input.GetButtonDown (brake)) {
				brakeActivated = !brakeActivated;
			}
			//
			if (Input.GetButtonDown (gearActivate) && !gearOpened && !inTransition && control.aircraft.transform.position.y > 5f) {
				open = true;
			}
			if (Input.GetButtonDown (gearActivate) && !gearClosed && !inTransition && control.aircraft.transform.position.y > 5f) {
				close = true;
			}
		}
			//
			if (open && !gearOpened) {
				if (gearType == GearType.Combined) {
					gearHydraulics.open = true;
					StartCoroutine (WaitToOpen ());
				} else if (gearType == GearType.Seperate) {
					doorHydraulics.open = true;
					StartCoroutine (WaitToOpen ());
					if (!inTransition) {
						StartCoroutine (WaitForDoorOpen ());
						inTransition = true;
					}
				} else if (gearType == GearType.Complex) {
					foreach (SilantroSequenceHydraulics hyd in hydraulics) {
						hyd.open = true;
						StartCoroutine (WaitToOpenComplex ());
					}
				}
			}
			if (close && !gearClosed) {

				if (gearType == GearType.Combined) {
					gearHydraulics.close = true;
					StartCoroutine (WaitToClose ());
				} else if (gearType == GearType.Seperate) {
					gearHydraulics.close = true;
					if (!inTransition) {
						StartCoroutine (WaitForGearClose ());
						inTransition = true;
					}
				} else if (gearType == GearType.Complex) {
					foreach (SilantroSequenceHydraulics hyd in hydraulics) {
						hyd.close = true;
						StartCoroutine (WaitToCloseComplex ());
					}
				}
			}
		//
		if (playSound && groundRoll != null) {
			if (aircraft != null) {
				//
				if(aircraft.velocity.magnitude > 5){
				soundSource.volume = Mathf.Lerp (soundSource.volume, 1f, 0.2f);
				}
				else {
					soundSource.volume = Mathf.Lerp (soundSource.volume, 0f, 0.2f);
				}
			} 
		}
	}
	//
	//COMPLEX HYDRAULICS
	IEnumerator WaitToCloseComplex()
	{
		yield return new WaitForSeconds (4f);
		gearClosed = true;TurnOffLights ();
		gearOpened = false;CloseGearSwitches ();
		inTransition = false;
	}
	IEnumerator WaitToOpenComplex()
	{
		yield return new WaitForSeconds (4f);
		gearClosed = false;TurnOnLights ();
		gearOpened = true;CloseGearSwitches ();
		inTransition = false;
	}
	//
	IEnumerator WaitToClose()
	{
		yield return new WaitForSeconds (gearHydraulics.closeTime);
		gearClosed = true;TurnOffLights ();
		gearOpened = false;CloseGearSwitches ();
		inTransition = false;
	}
	IEnumerator WaitToOpen()
	{
		yield return new WaitForSeconds (gearHydraulics.openTime);
		gearClosed = false;TurnOnLights ();
		gearOpened = true;CloseGearSwitches ();
		inTransition = false;
	}
	//CLOSE GEAR
	IEnumerator WaitForGearClose()
	{
		yield return new WaitForSeconds (gearHydraulics.closeTime);
		doorHydraulics.close = true;
		StartCoroutine (WaitForDoorClose ());
	}
	//
	IEnumerator WaitForDoorClose()
	{
		yield return new WaitForSeconds (doorHydraulics.closeTime);
		gearClosed = true;TurnOffLights ();
		gearOpened = false;CloseGearSwitches ();
		inTransition = false;
	}
	//OPEN GEAR
	//
	IEnumerator WaitForDoorOpen()
	{
		yield return new WaitForSeconds (gearHydraulics.closeTime);
		gearHydraulics.open = true;
		StartCoroutine (WaitForGearOpen ());
	}
	//
	IEnumerator WaitForGearOpen()
	{
		yield return new WaitForSeconds (gearHydraulics.openTime);
		gearClosed = false;TurnOnLights ();
		gearOpened = true;CloseGearSwitches ();
		inTransition = false;
	}
	void CloseGearSwitches()
	{
		open =  false;
		close = false;
		//
	}
	// Update is called once per frame
	void FixedUpdate () {
		//
		//SEND BRAKING DATA
		foreach (WheelSystem system in wheelSystem) {
			if (system.attachedMotor) {
				BrakingSystem (system);
			}
		}
		//
		steerAngle = -1 *maximumSteerAngle * Input.GetAxis (controlBoard.Rudder);
		//
		foreach (WheelSystem system in wheelSystem) {
			//SEND ROTATION DATA
			RotateWheel(system.wheelModel,system);
			//SEND ALIGNMENT DATA
			WheelAllignment(system,system.wheelModel);
			//SEND Balance DATA
			//WheelBalance();
			//
			if (system.steerable) {
				//ROTATE FRONT AXLE
				if ( null != frontWheelAxle )
				{
					frontWheelAxle.transform.localRotation = InitialModelRotation;
					frontWheelAxle.transform.Rotate( axisRotation, steerAngle );
				}
				if (transform.parent.gameObject.GetComponent<Rigidbody> ().velocity.magnitude < maximumSteerSpeed) {
					if (system.collider != null) {
						system.collider.steerAngle = steerAngle;
					}
				} else {
					if (system.collider != null) {
						system.collider.steerAngle = 0f;
					}
				}
			}
		}
	}
	//
	//TURN ON LANDING LIGHTS
	public void TurnOnLights()
	{
		foreach (SilantroLight light in lights) {
			if (light.lightType == SilantroLight.LightType.Landing) {
				light.TurnOn ();
			}
		}
	}
	//
	//TURN OFF LANDING LIGHTS
	public void TurnOffLights()
	{
		foreach (SilantroLight light in lights) {
			if (light.lightType == SilantroLight.LightType.Landing) {
				light.TurnOff ();
			}
		}
	}
	//
	//
	//ANTI ROLL BALANCE
	public void WheelBalance()
	{
		WheelHit balanceWheelHit;
		//
		float leftSide = 1f;
		float rightSide = 1f;
		//LEFT WHEEL
		bool leftWheelgrounded = LeftbalanceWheel.GetGroundHit(out balanceWheelHit);
		if (leftWheelgrounded) {
			//
			leftSide = -(LeftbalanceWheel.transform.InverseTransformPoint (balanceWheelHit.point).y - LeftbalanceWheel.radius) / LeftbalanceWheel.suspensionDistance;
		}
		//RIGHT WHEEL 
		bool rightWheelgrounded = RightbalanceWheel.GetGroundHit(out balanceWheelHit);
		if (rightWheelgrounded) {
			//
			rightSide = -(RightbalanceWheel.transform.InverseTransformPoint (balanceWheelHit.point).y - RightbalanceWheel.radius) / RightbalanceWheel.suspensionDistance;
		}
		//CALCULATE REQUIRED PRESS DOWN FORCE
		float balanceForce = (leftSide - rightSide) * control.currentWeight;
	//	Debug.Log (balanceForce);
		//
		if(leftWheelgrounded)
			control.aircraft.AddForceAtPosition(LeftbalanceWheel.transform.up*-balanceForce,LeftbalanceWheel.transform.position);
		if (rightWheelgrounded)
			control.aircraft.AddForceAtPosition (RightbalanceWheel.transform.up * balanceForce, RightbalanceWheel.transform.position);
	}
	//
	//ROTATE WHEEL
	void RotateWheel(Transform wheel,WheelSystem system)
	{
		if (system.collider != null) {
			speed = system.collider.rpm;
		}
		if (wheel != null) {
			//
			if (system.AxisX) {
				wheel.Rotate (new Vector3 (speed * Time.deltaTime, 0, 0));
			}
			if (system.AxisY) {
				wheel.Rotate (new Vector3 (0, speed * Time.deltaTime, 0));
			}
			if (system.AxisZ) {
				wheel.Rotate (new Vector3 (0, 0, speed * Time.deltaTime));
			}
		}
	}
	//
	//ALLIGN WHEEL TO COLLIDER
	void WheelAllignment(WheelSystem system,Transform wheel)
	{
		if (wheel != null) {
			RaycastHit hit;
			WheelHit CorrespondingGroundHit;
			if (system.collider != null) {
				Vector3 ColliderCenterPoint = system.collider.transform.TransformPoint (system.collider.center);
				system.collider.GetGroundHit (out CorrespondingGroundHit);

				if (Physics.Raycast (ColliderCenterPoint, -system.collider.transform.up, out hit, (system.collider.suspensionDistance + system.collider.radius) * transform.localScale.y)) {
					wheel.position = hit.point + (system.collider.transform.up * system.collider.radius) * transform.localScale.y;
					float extension = (-system.collider.transform.InverseTransformPoint (CorrespondingGroundHit.point).y - system.collider.radius) / system.collider.suspensionDistance;
					Debug.DrawLine (CorrespondingGroundHit.point, CorrespondingGroundHit.point + system.collider.transform.up, extension <= 0.0 ? Color.magenta : Color.white);
					Debug.DrawLine (CorrespondingGroundHit.point, CorrespondingGroundHit.point - system.collider.transform.forward * CorrespondingGroundHit.forwardSlip * 2f, Color.green);
					Debug.DrawLine (CorrespondingGroundHit.point, CorrespondingGroundHit.point - system.collider.transform.right * CorrespondingGroundHit.sidewaysSlip * 2f, Color.red);
				} else {
					wheel.transform.position = Vector3.Lerp (wheel.transform.position, ColliderCenterPoint - (system.collider.transform.up * system.collider.suspensionDistance) * transform.localScale.y, Time.deltaTime * 10f);
				}
			}
		}
	}
	//
	// BRAKE
	void BrakingSystem(WheelSystem wheel)
	{

		//PARKING BRAKE
		if (wheel != null && wheel.collider != null) {
			if (brakeActivated) {
				wheel.collider.brakeTorque = brakeTorque;
				wheel.collider.motorTorque = 0;
			} else {
				wheel.collider.motorTorque = 10f;
				wheel.collider.brakeTorque = 0f;
			}
		}
		//INCREMENTAL BRAKE
		if (Input.GetButton (IncrementalBrake)) {
			brakeControl = Mathf.Lerp (brakeControl, 1f, 0.01f);
		} else {
			brakeControl = Mathf.Lerp (brakeControl, 0f, 0.03f);
		}
		//
		if (brakeControl < 0) {
			brakeControl = 0;
		}
		//
		ActualBrakeTorque = brakeControl * brakeTorque;
		if (ActualBrakeTorque > 10f) {
			if(wheel.collider != null)
			wheel.collider.brakeTorque = ActualBrakeTorque;
		}
	}
}
//
//
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroGearSystem))]
public class GearEditor: Editor
{
	Color backgroundColor;
	Color silantroColor = Color.cyan;
	//
	public override void OnInspectorGUI()
	{
		backgroundColor = GUI.backgroundColor;
		//
		DrawDefaultInspector ();serializedObject.Update ();
		//
		SilantroGearSystem gear = (SilantroGearSystem)target;
		//
		GUILayout.Space(20f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Steering Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space (3f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Wheel Axle", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space (2f);
		gear.frontWheelAxle = EditorGUILayout.ObjectField ("Steering Wheel Axle", gear.frontWheelAxle, typeof(Transform), true) as Transform;
		GUILayout.Space (2f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Steer Settings", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space (2f);
		gear.maximumSteerSpeed =  EditorGUILayout.FloatField ("Maximum Steer Speed", gear.maximumSteerSpeed);
		GUILayout.Space (2f);
		gear.maximumSteerAngle = EditorGUILayout.FloatField ("Maximum Steer Angle", gear.maximumSteerAngle);
		GUILayout.Space (2f);
		EditorGUILayout.LabelField ("Current Angle", gear.steerAngle.ToString ("0.0") + " °");
		GUILayout.Space (2f);
		gear.rotationAxis = (SilantroGearSystem.RotationAxis)EditorGUILayout.EnumPopup ("Steer Axis", gear.rotationAxis);
		//
		GUILayout.Space(20f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Braking Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space (3f);
		gear.brakeTorque = EditorGUILayout.FloatField ("Brake Torque", gear.brakeTorque);
		GUILayout.Space (3f);
		EditorGUILayout.LabelField ("Parking Brake Engaged", gear.brakeActivated.ToString ());
		GUILayout.Space (3f);
		EditorGUILayout.LabelField ("Incremental Brake", (gear.brakeControl * 100f).ToString ("0.0") + " %");
		//
		//
		GUILayout.Space(20f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Sound Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space (3f);
		gear.playSound = EditorGUILayout.Toggle ("Play Sound", gear.playSound);
		if (gear.playSound) {
			GUILayout.Space (3f);
			gear.groundRoll = EditorGUILayout.ObjectField ("Ground Roll", gear.groundRoll, typeof(AudioClip), true) as AudioClip;
			GUILayout.Space (3f);
		}
		//
		GUILayout.Space(20f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Hydraulics Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space (3f);
		gear.gearType = (SilantroGearSystem.GearType)EditorGUILayout.EnumPopup ("Gear Type", gear.gearType);
		GUILayout.Space (5f);
		//
		if (gear.gearType == SilantroGearSystem.GearType.Combined) {
			
			gear.gearHydraulics = EditorGUILayout.ObjectField ("Gear Actuator", gear.gearHydraulics, typeof(SilantroHydraulicSystem), true) as SilantroHydraulicSystem;
		}
		if (gear.gearType == SilantroGearSystem.GearType.Seperate) {
			gear.gearHydraulics = EditorGUILayout.ObjectField ("Gear Actuator", gear.gearHydraulics, typeof(SilantroHydraulicSystem), true) as SilantroHydraulicSystem;
			GUILayout.Space (4f);
			gear.doorHydraulics = EditorGUILayout.ObjectField ("Door Actuator", gear.doorHydraulics, typeof(SilantroHydraulicSystem), true) as SilantroHydraulicSystem;
		}
		if (gear.gearType == SilantroGearSystem.GearType.Complex) {
			GUILayout.Space (5f);
			GUI.color = Color.white;
			EditorGUILayout.HelpBox ("Gear Hydraulics", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (3f);
			//
			GUILayout.Space (5f);
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.BeginVertical ();
			//
			GUIContent partlabel = new GUIContent("Hydraulics Count");
			SerializedProperty part = this.serializedObject.FindProperty ("hydraulics");
			EditorGUILayout.PropertyField (part.FindPropertyRelative ("Array.size"),partlabel);
			GUILayout.Space (5f);
			for (int i = 0; i < part.arraySize; i++) {
				GUIContent label = new GUIContent ("Hydraulic " + (i + 1).ToString ());
				EditorGUILayout.PropertyField (part.GetArrayElementAtIndex (i), label);
			}
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.EndVertical ();
		}
		//
		GUILayout.Space (3f);
		if (gear.gearOpened) {
			EditorGUILayout.LabelField ("Gear State", "Open");
		}
		if (gear.gearClosed) {
			EditorGUILayout.LabelField ("Gear State", "Closed");
		}
		//
		//
		if (GUI.changed) {
			EditorUtility.SetDirty (gear);
			EditorSceneManager.MarkSceneDirty (gear.gameObject.scene);
		}
		//
		serializedObject.ApplyModifiedProperties();
	}
}
#endif