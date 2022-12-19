//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;using System;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
//
using System.IO;
using System.Text;
//
public class SilantroTurboShaft : MonoBehaviour {
	//
	//ENGINE PROPERTIES
	[HideInInspector]public float engineDiameter = 1f;
	[HideInInspector]public float intakeDiameterPercentage = 50f;
	[HideInInspector]public float exhaustDiameterPercentage = 50f;
	[HideInInspector]public float intakeDiameter;float IntakeDiameter;float ExhaustDiameter;
	[HideInInspector]public float exhaustDiameter;
	[HideInInspector]public float intakeArea;
	[HideInInspector]public float exhaustArea;
	[HideInInspector]public float weight = 500f;
	[HideInInspector]public float overallLength= 4f;
	[HideInInspector]public float OverallPressureRatio = 1f;
	[HideInInspector]public bool diplaySettings;
	//
	float num5;float intakeFactor;float ambientPressure;
	//
	[HideInInspector]public float LowPressureFanRPM = 100f;
	[HideInInspector]public float HighPressureFanRPM = 1000f;
	[HideInInspector]public float RPMAcceleration = 0.2f;
	[HideInInspector]public float LPRPM;
	[HideInInspector]public float HPRPM;
	//
	[HideInInspector]public float LPIdleRPM;
	[HideInInspector]public float HPIdleRPM;
	[HideInInspector]public float currentHPRPM;
	[HideInInspector]public float targetHPRPM;
	[HideInInspector]public float targetLPRPM;
	[HideInInspector]public float currentLPRPM;
	[HideInInspector]public float fanThrust;
	float fanAirMassFlow;
	//
	//
	//ENGINE CONFIGURATION
	[HideInInspector]public bool EngineOn;
	[HideInInspector]public float engineAcceleration = 0.2f;
	[HideInInspector]public float EnginePower;
	[HideInInspector]public float EGT;
	[HideInInspector]public bool hasAttachedModels;
	//
	float enginePower;
	[HideInInspector]
	public bool isAccelerating;
	//
	//FUEL SETTINGS
	[HideInInspector]public SilantroFuelTank attachedFuelTank;
	[HideInInspector]public float PSFC = 0.1f;
	[HideInInspector]public float currentTankFuel;
	[HideInInspector]public float criticalFuelLevel = 10f;
	[HideInInspector]public float actualConsumptionrate;
	[HideInInspector]bool InUse;
	[HideInInspector]public bool LowFuel;
	//
	//FUEL TYPE AND CONFIGURATION
	[Header("Fuel Type and Combustion System")]
	[HideInInspector]public float massFactor;
	public enum FuelType
	{
		JetB,
		JetA1,
		JP6,
		JP8
	}
	[HideInInspector]public FuelType fuelType = FuelType.JetB;
	[HideInInspector]public float combustionEnergy;
	//
	//ENGINE STATE
	public enum EngineState
	{
		Off,
		Starting,
		Running
	}
	//
	float sfc;float coreFactor;
	[HideInInspector]public bool isDestructible;
	//ENGINE SOUND SETTINGS
	[HideInInspector]public AudioClip EngineStartSound;
	[HideInInspector]public AudioClip EngineIdleSound;
	[HideInInspector]public AudioClip EngineShutdownSound;
	//
	[HideInInspector]public float EngineIdlePitch = 0.5f;
	[HideInInspector]public float EngineMaximumRPMPitch = 1f;
	[HideInInspector]public float maximumPitch = 2f;
	[HideInInspector]public float engineSoundVolume = 2f;
	//
	[HideInInspector]public bool adjustPitchSettings;
	//
	[HideInInspector]public Rigidbody Parent;
	[HideInInspector]public Transform Thruster;
	[HideInInspector]public Transform intakeFan;
	[HideInInspector]public Transform turbineBlade;
	//
	//CONTROLS
	[HideInInspector]public bool start;
	[HideInInspector]public bool stop;
	[HideInInspector]public bool starting;
	[HideInInspector]public float velocityMe;
	[HideInInspector]public bool lowFuel;
	//
	//
	[HideInInspector]public bool saveEngineData = false;
	[HideInInspector]public string saveLocation = "C:/Users/";
	[HideInInspector]public float dataLogRate = 5f;
	[HideInInspector]public bool InculdeUnits = true;
	//
	////AVAILABLE ENGNE DATA
	[HideInInspector]public string engineName;
	[HideInInspector]public string EngineType;
	[HideInInspector]public float compressionRate;
	[HideInInspector]public float airDensity = 1.225f;
	//
	[HideInInspector]public float enginerpm;
	//
	[HideInInspector]public float shaftHorsePower;
	//
	float propellerefficiency;
	float currentenginePower;
	float consumptionRate;
	float turbochargerRpm;
	float Egt;
	float horsePower;
	float thrust;
	float runTime;
	float airspeed;
	float altitude;
	[HideInInspector]public float fanAirVelocity;
	//
	//
	//Property of Oyedoyin Dada
	//cc dadaoyedoyin@gmail.com
	//
	//
	[HideInInspector]
	public float intakeAirVelocity ;
	[HideInInspector]
	public float intakeAirMassFlow ;
	[HideInInspector]
	public float exhaustAirVelocity ;
	[HideInInspector]	
	public float coreAirMassFlow;
	//
	private List<string[]> dataRow = new List<string[]>();
	float actualLogRate;
	//
	//THROTTLE CONTROLS
	[HideInInspector]public float FuelInput = 0.2f;
	[HideInInspector]public float throttleSpeed = 0.15f;
	//
	//
	float fuelMassFlow;
	bool fuelAlertActivated;
	[HideInInspector]public float fuelFactor = 1f;
	[HideInInspector]public float combusionFactor;
	//
	//
	[HideInInspector]public ParticleSystem exhaustSmoke;
	[HideInInspector]ParticleSystem.EmissionModule smokeModule;
	[HideInInspector]public float maximumEmissionValue = 50f;
	[HideInInspector]public float controlValue;
	//
	//Health Values
	[HideInInspector]public float startingHealth = 100.0f;		// The amount of health to start with
	[HideInInspector]public float currentHealth;	
	//
	[HideInInspector]public GameObject[] attachments;
	//
	[HideInInspector]public GameObject engineFire;
	[HideInInspector]GameObject actualFire;
	[HideInInspector]public GameObject ExplosionPrefab;
	//
	[HideInInspector]private bool destroyed;
	[HideInInspector]private bool engineOnFire;
	[HideInInspector]private Vector3 dropVelocity;
	//
	[HideInInspector]public bool Explode;
	//
	private AudioSource EngineStart;
	private AudioSource EngineRun;
	private AudioSource EngineShutdown;
	//
	[HideInInspector]public SilantroInstrumentation instrumentation;
	SilantroControls controlBoard;
	float EngineLinearSpeed;
	//
	//CONTROL KEYCODES
	string startEngine;
	string stopEngine;
	string throttleControl;
	//
	[HideInInspector]public EngineState CurrentEngineState;
	[HideInInspector]public bool isControllable = true;
	//
	//MAIN CODE AREA
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
		LPIdleRPM = LowPressureFanRPM * 0.1f;
		HPIdleRPM = HighPressureFanRPM * 0.09f;
		//

		//EngineMaximumRPM = norminalRPM;
		//EngineIdleRPM = norminalRPM * 0.1f;
	}
	//
	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		//
		exhaustDiameter = engineDiameter * exhaustDiameterPercentage/100f;
		Handles.color = Color.red;
		if(Thruster != null){
			Handles.DrawWireDisc (Thruster.position, Thruster.transform.forward, (exhaustDiameter/2f));
		}
		intakeDiameter = engineDiameter * intakeDiameterPercentage / 100f;
		Handles.color = Color.blue;
		if(intakeFan != null && Parent!=null){
			Handles.DrawWireDisc (intakeFan.transform.position, intakeFan.transform.forward, (intakeDiameter / 2f));
		}
		//
		Handles.color = Color.cyan;
		if(Thruster != null && intakeFan != null ){
			Handles.DrawLine (intakeFan.transform.position, Thruster.position);
		}
	}
	#endif
	//
	//
	void Start()
	{
		//
		currentHealth = startingHealth;engineOnFire = false;
		if (engineFire != null) {
			actualFire = Instantiate (engineFire, this.transform.position, Quaternion.identity);
			actualFire.transform.parent = this.transform;
		}
		if(actualFire != null)
			actualFire.SetActive (false);
		//
		if (exhaustSmoke != null) {
			smokeModule = exhaustSmoke.emission;
			smokeModule.rateOverTime = 0f;
		}
		//
		if (Parent == null) {
			Debug.Log ("Engine cannot be Operated without a Rigidbody parent!!, add Rigidbody to an empty gameobject if engine is for Test");
		}
		if (attachedFuelTank == null) {
			Debug.Log ("No fuel tank is attached to this Engine!!, Note: Engine will not function correctly");
		}
		if (instrumentation == null) {
			//instrumentation.Store ();
			Debug.LogError ("Instrumentation is missing!! If Engine is just for Test, add Instrumentation Prefab to the Scene");
		} 
		massFactor = UnityEngine.Random.Range(1.6f,2.2f);
		//
		if (dataLogRate != 0)
		{
			actualLogRate = 1.0f / dataLogRate;
		} else 
		{
			actualLogRate = 0.10f;
		}
		IntakeDiameter = intakeDiameter;ExhaustDiameter = exhaustDiameter;
		//WRITE ENGINE DATA...SilantroPistonEngine l283-316
		//
		//SET UP ENGINE FUEL COMBUSTION VALUES
		if (fuelType == FuelType.JetB)
		{
			combustionEnergy = 42.8f;
		}
		else if (fuelType == FuelType.JetA1) 
		{
			combustionEnergy = 43.5f;
		}
		else if (fuelType == FuelType.JP6) 
		{
			combustionEnergy = 49.6f;
		} 
		else if (fuelType == FuelType.JP8) 
		{
			combustionEnergy = 43.28f;
		}
		//
		intakeFactor = UnityEngine.Random.Range(0.90f,0.93f);//FACTOR OF TEMPERATURE IN FUTURE UPDATES
		combusionFactor = combustionEnergy/42f;
		//
		if (null != EngineStartSound)
		{
			EngineStart = Thruster.gameObject.AddComponent<AudioSource>();
			EngineStart.clip = EngineStartSound;
			EngineStart.loop = false;
			EngineStart.dopplerLevel = 0f;
			EngineStart.spatialBlend = 1f;
			EngineStart.rolloffMode = AudioRolloffMode.Custom;
			EngineStart.maxDistance = 650f;
		}
		if (null != EngineIdleSound)
		{
			EngineRun = Thruster.gameObject.AddComponent<AudioSource>();
			EngineRun.clip = EngineIdleSound;
			EngineRun.loop = true;
			EngineRun.Play();
			engineSoundVolume = 0f;
			EngineRun.spatialBlend = 1f;
			EngineRun.dopplerLevel = 0f;
			EngineRun.rolloffMode = AudioRolloffMode.Custom;
			EngineRun.maxDistance = 600f;
		}
		if (null != EngineShutdownSound)
		{
			EngineShutdown = Thruster.gameObject.AddComponent<AudioSource>();
			EngineShutdown.clip = EngineShutdownSound;
			EngineShutdown.loop = false;
			EngineShutdown.dopplerLevel = 0f;
			EngineShutdown.spatialBlend = 1f;
			EngineShutdown.rolloffMode = AudioRolloffMode.Custom;
			EngineShutdown.maxDistance = 650f;
		}
		//
		EngineOn = false;
		start = false;
		starting = false;
		stop = false;
		//
	}
	public void FixedUpdate()
	{
		if (Parent) {
			LPRPM = currentLPRPM;
			HPRPM = currentHPRPM;
			//Add Force
		}
		if (currentLPRPM <= 0f) {
			currentLPRPM = 0f;
		}
		if(attachedFuelTank != null)
		{currentTankFuel = attachedFuelTank.CurrentAmount;}

	}
	void CalculateFuelFlow(float currentPower)
	{
		sfc = (currentPower * PSFC) / 3600f;
		//
		fuelMassFlow = sfc*0.4536f;
	}
	//
	//
	//DEPLETE FUEL LEVEL WITH USAGE
	public void UseFuel()
	{
		{
			actualConsumptionrate = combusionFactor*fuelMassFlow * (FuelInput +0.000001f)* EngineRun.pitch ;
			//
			if (attachedFuelTank != null) {
				attachedFuelTank.CurrentAmount -= actualConsumptionrate * Time.deltaTime;
			}
		}

		if (attachedFuelTank != null && attachedFuelTank.CurrentAmount == 0f)
		{
			EngineRun.volume = 0f;
			EngineRun.pitch = 0f;
			stop = true;
			shaftHorsePower = 0f;
		}
	}
	///
	///STOP ENGINE IF DESTROYED
	public void DestroyEngine()
	{

		EngineOn = false;
		shaftHorsePower = 0f;
	}
	//
	//
	//ACCELERATE AND DECELERATE ENGINE
	private void EngineActive()
	{
		if (EngineOn)
		{
			if (enginePower < 1f && !isAccelerating)
			{
				enginePower += Time.deltaTime * engineAcceleration;
				//Calculate EGT
			}
		}
		else if (enginePower > 0f)
		{
			enginePower -= Time.deltaTime * engineAcceleration;
		}
		else
		{
			enginePower = 0f;
			EGT = 0f;
		}
	}
	///
	//
	private void RunEngine()
	{
		if (EngineStart.isPlaying)
		{
			EngineStart.Stop();
		}
		FuelInput = Mathf.Clamp(FuelInput, 0f, 1f);
		targetLPRPM = LPIdleRPM + (LowPressureFanRPM - LPIdleRPM) * FuelInput;
		targetHPRPM = HPIdleRPM + (HighPressureFanRPM - HPIdleRPM) * FuelInput;
		InUse = true;

		if (stop)
		{
			CurrentEngineState = EngineState.Off;
			EngineOn = false;
			EngineShutdown.Play();shaftHorsePower = 0;
			FuelInput = 0f;
			StartCoroutine(ReturnIgnition());
		}
	}
	//
	//
	//
	private void StartEngine()
	{
		if (starting)
		{
			if (!EngineStart.isPlaying)
			{
				CurrentEngineState = EngineState.Running;
				starting = false;
				//exhaustModule.enabled = true;
				RunEngine();
			}
		}
		else
		{
			EngineStart.Stop();
			CurrentEngineState = EngineState.Off;
		}
		targetLPRPM = LPIdleRPM;
		targetHPRPM = HPIdleRPM;
	}
	//
	//
	//
	private void ShutdownEngine()
	{
		if (EngineStart.isPlaying)
		{
			EngineStart.Stop();
			start = false;
		}
		if (start)
		{
			EngineOn = true;
			EngineStart.Play();
			CurrentEngineState = EngineState.Starting;
			starting = true;
			StartCoroutine(ReturnIgnition());
		}
		targetLPRPM = 0f;
		targetHPRPM = 0f;
	}
	//
	//
	public IEnumerator ReturnIgnition()
	{
		yield return new WaitForSeconds (0.5f);
		start = false;
		stop = false;
	}
	//
	//DESTRUCTION SYSTEM
	public void Disintegrate()
	{
		if (isDestructible) {

			if (transform.root.GetComponent<Rigidbody> ()) {
				dropVelocity = transform.root.GetComponent<Rigidbody> ().velocity;
			}
			destroyed = true;
			stop = true;
			EngineOn = false;
			shaftHorsePower = 0;
			//
			gameObject.SendMessage ("DestroyEngine", SendMessageOptions.DontRequireReceiver);
			//
			//ADD COLLIDERS TO ATTACHED PARTS
			int j;
			if (attachments.Length > 0) {
				for (j = 0; j < attachments.Length; j++) {
					attachments [j].transform.parent = null;
					//Attach Box collider
					if (!attachments [j].GetComponent<BoxCollider> ()) {
						attachments [j].AddComponent<BoxCollider> ();
					}
					//Attach Rigibbody
					if (!attachments [j].GetComponent<Rigidbody> ()) {
						attachments [j].AddComponent<Rigidbody> ();
					}
					attachments [j].GetComponent<Rigidbody> ().mass = 300.0f;
					attachments [j].GetComponent<Rigidbody> ().velocity = dropVelocity;
				}
			}
			gameObject.transform.parent = null;
			//
			if (GetComponent<Collider> () == null) {
				gameObject.AddComponent<CapsuleCollider> ();
			}
			if (!gameObject.GetComponent<Rigidbody> ()) {
				gameObject.AddComponent<Rigidbody> ();
			}
			gameObject.GetComponent<Rigidbody> ().isKinematic = false;
			gameObject.GetComponent<Rigidbody> ().mass = 200f;
			gameObject.GetComponent<Rigidbody> ().velocity = dropVelocity;
			//
			if (actualFire) {
				actualFire.SetActive (true);

			}
			if (ExplosionPrefab != null) {
				GameObject explosion = Instantiate (ExplosionPrefab, this.transform.position, Quaternion.identity);
				explosion.GetComponentInChildren<AudioSource> ().Play ();
			}	
			if (gameObject.GetComponent<SilantroTurboProp> ()) {
				//Destroy (gameObject.GetComponent<SilantroTurboProp> ());
			}
			if (gameObject.GetComponent<SilantroTurboFan> ()) {
				//Destroy (gameObject.GetComponent<SilantroTurbofan> ());
			}
		}
	}
	/// 
	//HIT SYSTEM
	public void SilantroDamage(float amount)
	{
		currentHealth += amount;

		// If the health runs out, then Die.
		if (currentHealth < 0)
		{
			currentHealth = 0;
		}
		if (currentHealth < 50f && !engineOnFire && engineFire != null) {
			engineFire.SetActive (true);
		}
		//Die Procedure
		if (currentHealth == 0 && !destroyed)
			Disintegrate();
	}
	//
	void Update () {
		//
		//
		if (Explode && !destroyed) {
			currentHealth = 0;
			Disintegrate ();
		}
		//
		if (isControllable) {
			//ENGINE EFFECTS
			if (exhaustSmoke) {
				smokeModule.rateOverTime = maximumEmissionValue * enginePower * coreFactor;
			}
			//
			//
			///CONTROLS
			if (Input.GetButtonDown (startEngine) && attachedFuelTank != null && attachedFuelTank.CurrentAmount > 0) {
				start = true;
			}
			if (Input.GetButtonDown (stopEngine)) {
				stop = true;
			}
			//JOYSTICK FUEL CONTROL
			float input = (1 + Input.GetAxis (throttleControl));
			FuelInput = input / 2f;
		
			//CLAMP FUEL LEVEL
			FuelInput = Mathf.Clamp (FuelInput, 0f, 100f);
			//
			EngineActive ();
			if (enginePower > 0f) {
				EngineCalculation ();
			}
			//
			if (InUse) {
				UseFuel ();
			}
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
			//INTERPOLATE ENGINE RPM
			if (EngineOn) {
				currentLPRPM = Mathf.Lerp (currentLPRPM, targetLPRPM, RPMAcceleration * Time.deltaTime * (enginePower * fuelFactor * fuelFactor));
				currentHPRPM = Mathf.Lerp (currentHPRPM, targetHPRPM, RPMAcceleration * Time.deltaTime * (enginePower * fuelFactor * fuelFactor));
				//exhaustModule.rateOverTime = (enginePower * maximumEmmisionValue);
			} else {
				currentLPRPM = Mathf.Lerp (currentLPRPM, 0.0f, RPMAcceleration * Time.deltaTime);
				currentHPRPM = Mathf.Lerp (currentHPRPM, 0.0f, RPMAcceleration * Time.deltaTime);
			}
			//

			if (null != EngineRun) {
				if (Parent != null) {
					//PERFORM MINOR ENGINE CALCULATIONS TO CONTROL SOUND PITCH
					float magnitude = Parent.velocity.magnitude;
					float num2 = magnitude * 1.94384444f;
					float num3 = currentLPRPM + num2 * 10f;
					float num4 = (num3 - LPIdleRPM) / (LowPressureFanRPM - LPIdleRPM);
					num5 = EngineIdlePitch + (EngineMaximumRPMPitch - EngineIdlePitch) * num4;
					num5 = Mathf.Clamp (num5, 0f, maximumPitch);
					//
				}
				if (attachedFuelTank.CurrentAmount <= 0) {
					stop = true;
				}
				//
				if (attachedFuelTank.CurrentAmount <= criticalFuelLevel) {
					if (EngineOn) {
						float startRange = 0.6f;
						float endRange = 1.0f;
						//
						float cycleRange = (endRange - startRange) / 2f;
						float offset = cycleRange + startRange;
						//
						fuelFactor = offset + Mathf.Sin (Time.time * 3f) * cycleRange;
						//
						EngineRun.pitch = fuelFactor;
					}
				}
			//
			else {
					EngineRun.pitch = num5 * enginePower;
				}
				//
				//
				EngineRun.volume = engineSoundVolume;
				if (currentLPRPM < LPIdleRPM) {
					EngineRun.volume = engineSoundVolume * num5;
					if (currentLPRPM < LPIdleRPM * 0.1f) {
						EngineRun.volume = 0f;
					}
				} else {
					EngineRun.volume = engineSoundVolume * num5;
				}

			}
		}

	}
	//
	public void EngineCalculation()
	{
		if (instrumentation != null) {
			airDensity = instrumentation.airDensity;
			ambientPressure = instrumentation.ambientPressure;
		}
		//
		coreFactor = currentLPRPM/LowPressureFanRPM;
		//
		EnginePower = enginePower * 100f;
		//
		if (Parent != null) {
			float velocity = Parent.velocity.magnitude;
			EngineLinearSpeed = velocity;
		}
		//
		intakeArea = (3.142f * IntakeDiameter * IntakeDiameter) / 4f;
		exhaustArea = (3.142f *ExhaustDiameter * ExhaustDiameter) / 4f;
		//
		intakeAirVelocity = (3.142f * IntakeDiameter * LPRPM) / 60f;
		exhaustAirVelocity = (3.142f * ExhaustDiameter * HPRPM) / 60f;
		fanAirVelocity = intakeAirVelocity * intakeFactor;
		//
		//Debug.Log(intakeFactor);
		//
		fanAirMassFlow = 0.1f * intakeAirMassFlow;
		fanThrust = fanAirMassFlow * (intakeAirVelocity - EngineLinearSpeed);
		//
		intakeAirMassFlow = airDensity * intakeArea * fanAirVelocity;
		coreAirMassFlow = intakeAirMassFlow;
		//
		float coreThrust;float totalThrust;
		coreThrust = (((coreAirMassFlow + fuelMassFlow) * (exhaustAirVelocity)) - (coreAirMassFlow * EngineLinearSpeed) + (exhaustArea * ((OverallPressureRatio * ambientPressure) - ambientPressure)));
		//
		totalThrust=coreThrust+fanThrust;
		//
		shaftHorsePower = (totalThrust/(4.45f*2.5f));
		if (coreThrust > 0) {
			CalculateFuelFlow (shaftHorsePower);
		}
	}
}
//
//
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroTurboShaft))]
public class TurboShaftEditor: Editor
{
	Color backgroundColor;
	Color silantroColor = Color.cyan;
	[HideInInspector]public int toolbarTab;
	[HideInInspector]public string currentTab;
	//

	public override void OnInspectorGUI()
	{
		backgroundColor = GUI.backgroundColor;
		//
		DrawDefaultInspector ();
		//
		//SilantroTurboJet kk;
		SilantroTurboShaft shaft = (SilantroTurboShaft)target;
		//
		GUILayout.Space(10f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Engine Dimensions", MessageType.None);
		GUI.color = backgroundColor;
		//
		//DISPLAY ENGINE DIMENSIONS
		shaft.engineDiameter = EditorGUILayout.FloatField("Engine Diameter",shaft.engineDiameter);
		shaft.intakeDiameterPercentage = EditorGUILayout.Slider ("Intake Diameter Percentage",shaft.intakeDiameterPercentage,0,100);
		//shaft.intakeDiameter = shaft.engineDiameter * shaft.intakeDiameterPercentage / 100f;
		EditorGUILayout.LabelField ("Intake Diameter", shaft.intakeDiameter.ToString ("0.00") + " m");
		shaft.exhaustDiameterPercentage = EditorGUILayout.Slider ("Exhaust Diameter Percentage",shaft.exhaustDiameterPercentage,0,100);
		//shaft.exhaustDiameter = shaft.engineDiameter * shaft.exhaustDiameterPercentage / 100f;
		EditorGUILayout.LabelField ("Exhaust Diameter", shaft.exhaustDiameter.ToString ("0.00") + " m");
		//
		GUILayout.Space(3f);
		shaft.weight = EditorGUILayout.FloatField("Engine Weight",shaft.weight);
		shaft.overallLength = EditorGUILayout.FloatField("Overall Length",shaft.overallLength);
		//
		//
		GUILayout.Space(25f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Turbine Configuration", MessageType.None);
		GUI.color = backgroundColor;
		shaft.LowPressureFanRPM = EditorGUILayout.FloatField ("Low Pressure Fan RPM", shaft.LowPressureFanRPM);
		shaft.HighPressureFanRPM = EditorGUILayout.FloatField ("HighPressure Fan RPM", shaft.HighPressureFanRPM);

		EditorGUILayout.LabelField ("N1",shaft.LPRPM.ToString("0.00")+ " RPM");
		EditorGUILayout.LabelField ("N2",shaft.HPRPM.ToString("0.00")+ " RPM");
		GUILayout.Space(5f);
		shaft.OverallPressureRatio = EditorGUILayout.FloatField ("Overall Pressure Ratio", shaft.OverallPressureRatio);
		//
	
		//DISPLAY ENGINE CONFIGURATION
		//
		GUILayout.Space(25f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Fuel Configuration", MessageType.None);
		GUI.color = backgroundColor;
		//
		//DISPLAY FUEL CONFIGURATION
		EditorGUILayout.LabelField ("Mass Factor",shaft.massFactor.ToString("0.000"));
		shaft.fuelType = (SilantroTurboShaft.FuelType)EditorGUILayout.EnumPopup ("Fuel Type", shaft.fuelType);
		//SET UP ENGINE FUEL COMBUSTION VALUES
		if (shaft.fuelType == SilantroTurboShaft.FuelType.JetB)
		{
			shaft.combustionEnergy = 42.8f;
		}
		else if (shaft.fuelType == SilantroTurboShaft.FuelType.JetA1) 
		{
			shaft.combustionEnergy = 45.5f;
		}
		else if (shaft.fuelType == SilantroTurboShaft.FuelType.JP6) 
		{
			shaft.combustionEnergy = 47.6f;
		} 
		else if (shaft.fuelType == SilantroTurboShaft.FuelType.JP8)
		{
			shaft.combustionEnergy = 43.28f;
		} 
		//
		shaft.combusionFactor = shaft.combustionEnergy/42f;	
		EditorGUILayout.LabelField ("Combustion Energy",shaft.combustionEnergy.ToString("0.0")+" MJoules");
		EditorGUILayout.LabelField ("Combustion Factor",shaft.combusionFactor.ToString("0.00"));
		//
		GUILayout.Space(3f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Fuel Usage Settings", MessageType.None);
		GUI.color = backgroundColor;
		//
		GUILayout.Space(3f);
		shaft.attachedFuelTank = EditorGUILayout.ObjectField ("Fuel Tank", shaft.attachedFuelTank, typeof(SilantroFuelTank), true) as SilantroFuelTank;
		EditorGUILayout.LabelField ("Fuel Remaining", shaft.currentTankFuel.ToString ("0.00") + " kg");
		GUILayout.Space(5f);
		EditorGUILayout.HelpBox ("Power Specific fuel consumption in lb/hp.hr", MessageType.None);
		GUILayout.Space(3f);
		shaft.PSFC = EditorGUILayout.FloatField ("Fuel Consumption", shaft.PSFC);
		EditorGUILayout.LabelField ("Actual Consumption Rate",shaft.actualConsumptionrate.ToString("0.00")+" kg/s");
		shaft.criticalFuelLevel = EditorGUILayout.FloatField ("Critical Fuel Level", shaft.criticalFuelLevel);
		//
		//
		GUILayout.Space(25f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Connections", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		shaft.Parent = EditorGUILayout.ObjectField ("Engine Parent", shaft.Parent, typeof(Rigidbody), true) as Rigidbody;
		GUILayout.Space(2f);
		shaft.intakeFan = EditorGUILayout.ObjectField ("Intake Fan", shaft.intakeFan, typeof(Transform), true) as Transform;
		GUILayout.Space(2f);
		shaft.Thruster = EditorGUILayout.ObjectField ("Thruster", shaft.Thruster, typeof(Transform), true) as Transform;
		//
		GUILayout.Space(25f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Sound Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		shaft.EngineStartSound = EditorGUILayout.ObjectField ("Ignition Sound", shaft.EngineStartSound, typeof(AudioClip), true) as AudioClip;
		shaft.EngineIdleSound = EditorGUILayout.ObjectField ("Engine Idle Sound", shaft.EngineIdleSound, typeof(AudioClip), true) as AudioClip;
		shaft.EngineShutdownSound = EditorGUILayout.ObjectField ("Shutdown Sound", shaft.EngineShutdownSound, typeof(AudioClip), true) as AudioClip;
		//
		GUILayout.Space(3f);
		shaft.adjustPitchSettings = EditorGUILayout.Toggle("Show Pitch Settings",shaft.adjustPitchSettings);
		GUILayout.Space(1f);
		if (shaft.adjustPitchSettings) {
			shaft.EngineIdlePitch = EditorGUILayout.FloatField ("Engine Idle Pitch", shaft.EngineIdlePitch);
			shaft.EngineMaximumRPMPitch = EditorGUILayout.FloatField ("Engine Maximum Pitch", shaft.EngineMaximumRPMPitch);
		}
		GUILayout.Space(25f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Throttle Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(2f);
		EditorGUILayout.LabelField ("Throttle Level",(shaft.FuelInput*100f).ToString("0.00") + " %");
		shaft.throttleSpeed = EditorGUILayout.FloatField ("Throttle Speed",shaft.throttleSpeed);
		//
		GUILayout.Space(20f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Data Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(2f);
		shaft.saveEngineData = EditorGUILayout.Toggle ("Log Engine Data", shaft.saveEngineData);
		if (shaft.saveEngineData) {
			GUILayout.Space (3f);
			shaft.InculdeUnits = EditorGUILayout.Toggle ("Include Data Units", shaft.InculdeUnits);
			shaft.saveLocation = EditorGUILayout.TextField ("Data Location", shaft.saveLocation);
			shaft.dataLogRate = EditorGUILayout.FloatField ("Data Log Rate",shaft.dataLogRate);
		}
		//
		GUILayout.Space(25f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Engine Display", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		//
		EditorGUILayout.LabelField ("Engine State",shaft.CurrentEngineState.ToString());
		EditorGUILayout.LabelField ("Engine Power",shaft.EnginePower.ToString("0.00") + " %");
		EditorGUILayout.LabelField ("EGT",shaft.EGT.ToString("0.0")+ " °C");
		//
		EditorGUILayout.LabelField ("Core Speed",shaft.currentLPRPM.ToString("0.0")+ " RPM");
		GUILayout.Space(3f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Engine Output", MessageType.None);
		GUI.color = backgroundColor;
		EditorGUILayout.LabelField ("Shaft Power",shaft.shaftHorsePower.ToString("0.0")+ " Hp");
	//
		//
		//
		GUILayout.Space(10f);
		shaft.diplaySettings = EditorGUILayout.Toggle ("Show Extras", shaft.diplaySettings);
		if (shaft.diplaySettings) {
			//
			GUILayout.Space (10f);
			toolbarTab = GUILayout.Toolbar (toolbarTab, new string[]{ "Effects", "Destruction" });
			//
			switch (toolbarTab) {
			case 0:
				//
				currentTab = "Effects";
				break;
			case 1:
				//
				currentTab = "Destruction";
				break;
			}
			//
			switch (currentTab) {
			case "Effects":
				//
				GUILayout.Space (15f);
				GUI.color = Color.yellow;
				EditorGUILayout.HelpBox ("Engine Effects Configuration", MessageType.None);
				GUI.color = backgroundColor;
				GUILayout.Space (3f);
				shaft.exhaustSmoke = EditorGUILayout.ObjectField ("Exhaust Smoke", shaft.exhaustSmoke, typeof(ParticleSystem), true) as ParticleSystem;
				GUILayout.Space (2f);
				shaft.maximumEmissionValue = EditorGUILayout.FloatField ("Maximum Emission", shaft.maximumEmissionValue);
				break;
			case "Destruction":
				//
				GUILayout.Space (5f);
				shaft.isDestructible = EditorGUILayout.Toggle ("Is Destructible", shaft.isDestructible);
				if (shaft.isDestructible) {
					GUILayout.Space (5f);
					GUI.color = Color.white;
					EditorGUILayout.HelpBox ("Health Settings", MessageType.None);
					GUI.color = backgroundColor;
					GUILayout.Space (5f);
					shaft.startingHealth = EditorGUILayout.FloatField ("Starting Health", shaft.startingHealth);
					GUILayout.Space (3f);
					EditorGUILayout.LabelField ("Current Health", shaft.currentHealth.ToString ("0.0"));
					//
					GUILayout.Space (5f);
					GUI.color = Color.white;
					EditorGUILayout.HelpBox ("Effect Settings", MessageType.None);
					GUI.color = backgroundColor;
					GUILayout.Space (5f);
					shaft.engineFire = EditorGUILayout.ObjectField ("Fire Prefab", shaft.engineFire, typeof(GameObject), true) as GameObject;
					GUILayout.Space (3f);
					shaft.ExplosionPrefab = EditorGUILayout.ObjectField ("Explosion Prefab", shaft.ExplosionPrefab, typeof(GameObject), true) as GameObject;
					//
					GUILayout.Space (15f);
					shaft.hasAttachedModels = EditorGUILayout.Toggle ("Attached Models", shaft.hasAttachedModels);
					//

					if (shaft.hasAttachedModels) {
						GUILayout.Space (5f);
						GUI.color = Color.white;
						EditorGUILayout.HelpBox ("Models", MessageType.None);
						GUI.color = backgroundColor;
						GUILayout.Space (3f);
						//
						GUILayout.Space (5f);
						EditorGUILayout.BeginHorizontal ();
						EditorGUILayout.BeginVertical ();
						//
						GUIContent partlabel = new GUIContent ("Part Count");
						SerializedProperty part = this.serializedObject.FindProperty ("attachments");
						EditorGUILayout.PropertyField (part.FindPropertyRelative ("Array.size"), partlabel);
						GUILayout.Space (5f);
						for (int i = 0; i < part.arraySize; i++) {
							GUIContent label = new GUIContent ("Part " + (i + 1).ToString ());
							EditorGUILayout.PropertyField (part.GetArrayElementAtIndex (i), label);
						}
						EditorGUILayout.EndHorizontal ();
						EditorGUILayout.EndVertical ();
					}
					//
				}
				//
				break;
			}


		}
		//
		//
		if (GUI.changed) {
			EditorUtility.SetDirty (shaft);
			EditorSceneManager.MarkSceneDirty (shaft.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
		//
	}
}

#endif
