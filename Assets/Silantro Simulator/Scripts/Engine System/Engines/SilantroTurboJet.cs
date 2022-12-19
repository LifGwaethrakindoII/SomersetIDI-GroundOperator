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

//[AddComponentMenu("Oyedoyin/Engine System/Turbo Jet")]
public class SilantroTurboJet : MonoBehaviour {
	//
	public enum EngineState
	{
		Off,
		Starting,
		Running
	}
	//
	//ENGINE DIMENSIONS
	[HideInInspector]public float EngineDiameter = 1f;
	[HideInInspector]public float IntakeDiameterPercentage = 90f;
	[HideInInspector]public float ExhaustDiameterPercentage = 90f;
	[HideInInspector]public float IntakeDiameter;
	float intakeDiameter;
	[HideInInspector]public float ExhaustDiameter;
	[HideInInspector]public float weight = 500f;
	[HideInInspector]public float overallLength = 4f;
	//
	[HideInInspector]public bool diplaySettings;
	float intakeFactor;
	float num5;
	//
	[HideInInspector]public float OverallPressureRatio =10f;
	public enum ReheatSystem
	{
		Afterburning,
		noReheat
	}
	[HideInInspector]public ReheatSystem reheatSystem = ReheatSystem.noReheat;
	[HideInInspector]public bool AfterburnerOperative;
	[HideInInspector]public float AfterburnerTSFC =2f;
	[HideInInspector]public bool canUseAfterburner;
	//
	//
	[HideInInspector]public bool EngineOn;
	[HideInInspector]public float engineAcceleration = 0.2f;
	//
	[HideInInspector]public bool adjustPitchSettings;
	[HideInInspector]public bool isAccelerating;
	[HideInInspector]public float EnginePower;
	[HideInInspector]public float EGT;
	[HideInInspector]public float enginePower;

	//
	[HideInInspector]public float LowPressureFanRPM = 100f;
	[HideInInspector]public float HighPressureFanRPM = 1000f;
	[HideInInspector]public float RPMAcceleration = 0.5f;
	[HideInInspector]public float LPRPM;
	[HideInInspector]public float HPRPM;
	//
	//
	float LPIdleRPM;
	float HPIdleRPM;
	float currentHPRPM;
	float targetHPRPM;
	//
	[HideInInspector]public float intakeAirVelocity ;
	[HideInInspector]public float intakeAirMassFlow ;
	[HideInInspector]public float exhaustAirVelocity ;
	[HideInInspector]public float coreAirMassFlow;
	//
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
	float sfc;
	//
	[HideInInspector]public SilantroFuelTank attachedFuelTank;
	//public float StartAmount;
	[HideInInspector]public float TSFC = 0.1f;
	[HideInInspector]public float currentTankFuel;
	[HideInInspector]public float criticalFuelLevel = 10f;
	[HideInInspector]public float actualConsumptionrate;
	[HideInInspector]bool InUse;
	//
	[HideInInspector]public bool LowFuel;
	float fanAirVelocity;
	//
	//

	[HideInInspector]
	public float TargetRPM;
	//
	//Property of Oyedoyin Dada
	//cc dadaoyedoyin@gmail.com
	//
	//
	//
	[HideInInspector]
	public float CurrentRPM;
	//
	[HideInInspector]public AudioClip EngineStartSound;
	[HideInInspector]public AudioClip EngineIdleSound;
	[HideInInspector]public AudioClip EngineShutdownSound;
	[HideInInspector]public float EngineAfterburnerPitch = 1.75f;
	[HideInInspector]public float EngineIdlePitch = 0.5f;
	[HideInInspector]public float EngineMaximumRPMPitch = 1f;
	//

	[HideInInspector]public float maximumPitch = 2f;
	[HideInInspector]public float engineSoundVolume = 2f;
	//
	[HideInInspector]public Rigidbody Parent;
	[HideInInspector]public Transform intakeFanPoint;
	public enum RotationAxis
	{
		X,
		Y,
		Z
	}
	[HideInInspector]public RotationAxis rotationAxis = RotationAxis.X;
	//
	public enum RotationDirection
	{
		CW,
		CCW
	}
	[HideInInspector]public RotationDirection rotationDirection = RotationDirection.CCW;
	//

	[HideInInspector]public Transform Thruster;
	//
	private AudioSource EngineStart;
	private AudioSource EngineRun;
	private AudioSource EngineShutdown;
	//
	[HideInInspector]
	public bool start;
	[HideInInspector]
	public bool stop;
	private bool starting;
	private float velocityMe;
	private bool lowFuel;
	//
	//
	[HideInInspector]public bool saveEngineData = false;
	[HideInInspector]public string saveLocation = "C:/Users/";
	[HideInInspector]public float dataLogRate = 5f;
	[HideInInspector]public bool InculdeUnits = true;
	//
	//
	[HideInInspector]public float FuelInput = 0.2f;
	[HideInInspector]public float throttleSpeed = 0.15f;
	//Controls
	string startEngine ;
	string stopEngine ;
	string throttleControl;
	string afterburnerControl;
	//
	[HideInInspector]public EngineState CurrentEngineState;
	//
	[HideInInspector]public float airDensity = 1.225f;

	[HideInInspector]public float EngineThrust;
	[HideInInspector]float EngineLinearSpeed;
	//
	bool fuelAlertActivated;
	float fuelFactor = 1f;
	float combusionFactor;
	//CALCULATION VALUES
	[HideInInspector]public float fuelMassFlow;
	[HideInInspector]public float intakeArea;
	[HideInInspector]public float exhaustArea ;
	float fanThrust;
	float fanAirMassFlow;
	[HideInInspector]public bool captured;
	//
	//
	[HideInInspector]public SilantroInstrumentation instrumentation;
	//
	float ambientPressure;
	//Thrust Values
	[HideInInspector]public float coreThrust;
	//Health Values
	[HideInInspector]public float startingHealth = 100.0f;		// The amount of health to start with
	[HideInInspector]public float currentHealth;	
	//
	[HideInInspector]public GameObject[] attachments;
	//
	[HideInInspector]public bool hasAttachedModels;
	[HideInInspector]public bool isDestructible;
	[HideInInspector]public GameObject engineFire;
	[HideInInspector]GameObject actualFire;
	[HideInInspector]public GameObject ExplosionPrefab;
	//
	float coreFactor;
	[HideInInspector]public ParticleSystem exhaustSmoke;
	[HideInInspector]ParticleSystem.EmissionModule smokeModule;
	[HideInInspector]public float maximumEmissionValue = 50f;
	[HideInInspector]public float controlValue;
	//
	[HideInInspector]public Material engineMaterial;
	[HideInInspector]Color baseColor;
	[HideInInspector]Color finalColor;
	[HideInInspector]public float maximumNormalEmission;
	[HideInInspector]public float maximumAfterburnerEmission;
	//
	[HideInInspector]private bool destroyed;
	[HideInInspector]private bool engineOnFire;
	[HideInInspector]private Vector3 dropVelocity;
	//
	[HideInInspector]public bool Explode;
	[HideInInspector]public bool isControllable = true;

	SilantroControls controlBoard;
	//
	//
	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		//
		ExhaustDiameter = EngineDiameter * ExhaustDiameterPercentage/100f;
		Handles.color = Color.red;
		if(Thruster != null){
			Handles.DrawWireDisc (Thruster.position, Thruster.transform.forward, (ExhaustDiameter/2f));
		}
		IntakeDiameter = EngineDiameter * IntakeDiameterPercentage / 100f;
		Handles.color = Color.blue;
		if(intakeFanPoint != null && Parent!=null){
			Handles.DrawWireDisc (intakeFanPoint.transform.position, Parent.transform.forward, (IntakeDiameter / 2f));
		}
		//
		Handles.color = Color.cyan;
		if(Thruster != null && intakeFanPoint != null ){
			Handles.DrawLine (intakeFanPoint.transform.position, Thruster.position);
		}
	}
	//
	#endif
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
			EngineThrust = 0;
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
	void Start () {
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
		//
		if (Parent == null) {
			Debug.Log ("Engine cannot be Operated without a Rigidbody parent!!, add Rigidbody to an empty gameobject if engine is for Test");
		}
		if (attachedFuelTank == null) {
			Debug.Log ("No fuel tank is attached to this Engine!!, Note: Engine will not function correctly");
		}
		//RECIEVE DIAMETERS
		intakeDiameter = IntakeDiameter;
		//
		//SET UP MASS FACTOR FOR EGT CALCULATION
		massFactor = UnityEngine.Random.Range(1.6f,2.2f);
		fuelMassFlow = TSFC / 1000f;
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
		intakeFactor = UnityEngine.Random.Range(0.38f,0.45f);//FACTOR OF TEMPERATURE IN FUTURE UPDATES
		combusionFactor = combustionEnergy/42f;
		//
		//


		EngineOn = false;
		start = false;
		starting = false;
		stop = false;
	}
	//
	//
	//
	void Awake()
	{
		//
		//ADD AUDIOSOURCES
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
			GameObject soundPoint = new GameObject();
			soundPoint.transform.parent = this.transform;
			soundPoint.transform.localPosition = new Vector3 (0, 0, 0);
			soundPoint.name = this.name +" Sound Point";
			//

			EngineRun = soundPoint.gameObject.AddComponent<AudioSource>();
			EngineRun.clip = EngineIdleSound;
			EngineRun.loop = true;
			EngineRun.Play();
			EngineRun.volume = 0f;
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
		//
		GameObject brain =GameObject.FindGameObjectWithTag ("Brain");
		if (brain != null && brain.transform.root == gameObject.transform.root) {
			instrumentation = brain.GetComponent<SilantroInstrumentation> ();
		}

		if (instrumentation == null) {
			//instrumentation.Store ();
			Debug.LogError ("Instrumentation is missing!! If Engine is just for Test, add Instrumentation Prefab to the Scene");
		} else {
			instrumentation.boom = EngineRun;
		}
		//
		controlBoard = GameObject.FindGameObjectWithTag ("GameController").GetComponent<SilantroControls> ();
		if (controlBoard == null) {
			Debug.LogError ("Control Board is missing, Place Control Board in scene and restart!");
		}
		startEngine= controlBoard.engineStart;
		stopEngine = controlBoard.engineShutdown;
		throttleControl = controlBoard.engineThrottleControl;
		afterburnerControl = controlBoard.AfterburnerControl;
		//
		LPIdleRPM = LowPressureFanRPM * 0.1f;
		HPIdleRPM = HighPressureFanRPM * 0.09f;
		//
		if (reheatSystem == ReheatSystem.Afterburning) {
			canUseAfterburner = true;
		} else if (reheatSystem == ReheatSystem.noReheat) {
			canUseAfterburner = false;
		}
		//
		AfterburnerOperative = false;
	}
	//
	public void FixedUpdate()
	{
		if(isControllable)
		{
		if (Parent) {
			LPRPM = CurrentRPM;
			HPRPM = currentHPRPM;
			if (EngineThrust > 0f) {
				Vector3 force = Thruster.forward * EngineThrust;
				Parent.AddForce (force, ForceMode.Force);
			}
		}
		if (CurrentRPM <= 0f) {
			CurrentRPM = 0f;
		}
		if(attachedFuelTank != null)
		{currentTankFuel = attachedFuelTank.CurrentAmount;}
		//
			if (intakeFanPoint) {
				if (rotationDirection == RotationDirection.CCW) {
					if (rotationAxis == RotationAxis.X) {
						intakeFanPoint.Rotate (new Vector3 (CurrentRPM * Time.deltaTime, 0, 0));
					}
					if (rotationAxis == RotationAxis.Y) {
						intakeFanPoint.Rotate (new Vector3 (0, CurrentRPM * Time.deltaTime, 0));
					}
					if (rotationAxis == RotationAxis.Z) {
						intakeFanPoint.Rotate (new Vector3 (0, 0, CurrentRPM * Time.deltaTime));
					}
				}
				//
				if (rotationDirection == RotationDirection.CW) {
					if (rotationAxis == RotationAxis.X) {
						intakeFanPoint.Rotate (new Vector3 (-1f * CurrentRPM * Time.deltaTime, 0, 0));
					}
					if (rotationAxis == RotationAxis.Y) {
						intakeFanPoint.Rotate (new Vector3 (0, -1f * CurrentRPM * Time.deltaTime, 0));
					}
					if (rotationAxis == RotationAxis.Z) {
						intakeFanPoint.Rotate (new Vector3 (0, 0, -1f * CurrentRPM * Time.deltaTime));
					}
				}
			}
		}
	}
	//
	//CALCULATE FUEL FLOW
	//
	void CalculateFuelFlow(float currentThrust)
	{
		float poundThrust = currentThrust / 4.448f;
		if (AfterburnerOperative) {
			sfc = (poundThrust * AfterburnerTSFC) / 3600f;
		} else {
			sfc = (poundThrust * TSFC) / 3600f;
		}

		//
		fuelMassFlow = sfc*0.4536f;
	}
	//
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
			EngineThrust = 0f;
		}
	}
	//
	//
	///
	///STOP ENGINE IF DESTROYED
	public void DestroyEngine()
	{

		EngineOn = false;
		EngineThrust = 0f;
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
	//
	private void RunEngine()
	{
		if (EngineStart.isPlaying)
		{
			EngineStart.Stop();
		}
		FuelInput = Mathf.Clamp(FuelInput, 0f, 1f);
		TargetRPM = LPIdleRPM + (LowPressureFanRPM - LPIdleRPM) * FuelInput;
		targetHPRPM = HPIdleRPM + (HighPressureFanRPM - HPIdleRPM) * FuelInput;
		InUse = true;

		if (stop)
		{
			CurrentEngineState = EngineState.Off;
			EngineOn = false;
			EngineShutdown.Play();EngineThrust = 0;
			FuelInput = 0f;EngineThrust = 0f;AfterburnerOperative = false;
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
		TargetRPM = LPIdleRPM;
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
		TargetRPM = 0f;
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
			if (Input.GetButtonDown (startEngine) && attachedFuelTank != null && attachedFuelTank.CurrentAmount > 0) {
				start = true;
			}
			if (Input.GetButtonDown (stopEngine)) {
				stop = true;
			}
			//JOYSTICK FUEL CONTROL
			float input = (1 + Input.GetAxis (throttleControl));
			FuelInput = input / 2f;

			//KEYBOARD FUEL CONTROL

			//FuelInput = Input.GetAxis (throttleControl);
			//CLAMP FUEL LEVEL
			FuelInput = Mathf.Clamp (FuelInput, 0f, 100f);
			//
			//Afterburner Control
			if (Input.GetButtonDown (afterburnerControl) && canUseAfterburner && enginePower > 0.5f && FuelInput > 0.5f) {
				AfterburnerOperative = !AfterburnerOperative;
			}
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
			//performanceConfiguration.EngineLinearSpeed = num * 1.94384444f;
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
			//INTERPOLATE ENGINE RPM
			if (EngineOn) {
				CurrentRPM = Mathf.Lerp (CurrentRPM, TargetRPM, RPMAcceleration * Time.deltaTime * (enginePower * fuelFactor * fuelFactor));
				currentHPRPM = Mathf.Lerp (currentHPRPM, targetHPRPM, RPMAcceleration * Time.deltaTime * (enginePower * fuelFactor * fuelFactor));
				//exhaustModule.rateOverTime = (enginePower * maximumEmmisionValue);
			} else {
				CurrentRPM = Mathf.Lerp (CurrentRPM, 0.0f, RPMAcceleration * Time.deltaTime);
				currentHPRPM = Mathf.Lerp (currentHPRPM, 0.0f, RPMAcceleration * Time.deltaTime);
			}
			//

			if (null != EngineRun) {
				if (Parent != null) {
					//PERFORM MINOR ENGINE CALCULATIONS TO CONTROL SOUND PITCH
					float magnitude = Parent.velocity.magnitude;
					float num2 = magnitude * 1.94384444f;
					float num3 = CurrentRPM + num2 * 10f;
					float num4 = (num3 - LPIdleRPM) / (LowPressureFanRPM - LPIdleRPM);
			
					if (AfterburnerOperative) {
						num5 = EngineIdlePitch + (EngineAfterburnerPitch - EngineIdlePitch) * num4;
					} else if (!AfterburnerOperative) {
						num5 = EngineIdlePitch + (EngineMaximumRPMPitch - EngineIdlePitch) * num4;
					}
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
				if (CurrentRPM < LPIdleRPM) {
					EngineRun.volume = engineSoundVolume * num5;
					if (CurrentRPM < LPIdleRPM * 0.1f) {
						EngineRun.volume = 0f;
					}
				} else {
					EngineRun.volume = engineSoundVolume * num5;
				}

			}
		}

	}
	//


	//CALCULATE ENGINE THRUST
	public void EngineCalculation()
	{
		if (instrumentation != null) {
			airDensity = instrumentation.airDensity;
			ambientPressure = instrumentation.ambientPressure;
		}
		coreFactor = CurrentRPM / LowPressureFanRPM;
		//
		EnginePower = enginePower * 100f;
		//
		if (Parent != null) {
			float velocity = Parent.velocity.magnitude;
			EngineLinearSpeed = velocity;
		}
		//
		intakeArea = (3.142f * intakeDiameter * intakeDiameter) / 4f;
		exhaustArea = (3.142f * ExhaustDiameter * ExhaustDiameter) / 4f;
		//
		intakeAirVelocity = (3.142f * intakeDiameter * LPRPM) / 60f;
		exhaustAirVelocity = (3.142f * ExhaustDiameter * HPRPM) / 60f;
		fanAirVelocity = intakeAirVelocity * intakeFactor;
		//
		if (AfterburnerOperative && FuelInput < 0.5f) {
			AfterburnerOperative = false;
		}
		//
		fanAirMassFlow = 0.1f* intakeAirMassFlow;
		fanThrust = fanAirMassFlow *(intakeAirVelocity - EngineLinearSpeed);
		//
		intakeAirMassFlow = airDensity * intakeArea * fanAirVelocity;
		coreAirMassFlow = intakeAirMassFlow;
		//Afterburner Calculations
		if (AfterburnerOperative) {
			coreThrust = (((coreAirMassFlow + fuelMassFlow) * (exhaustAirVelocity * 1.5f)) - (coreAirMassFlow * EngineLinearSpeed) + (exhaustArea * ((OverallPressureRatio * ambientPressure) - ambientPressure)));
		} else {
			coreThrust = (((coreAirMassFlow + fuelMassFlow) * (exhaustAirVelocity)) - (coreAirMassFlow * EngineLinearSpeed) + (exhaustArea * ((OverallPressureRatio * ambientPressure) - ambientPressure)));
		}
		//
		EngineThrust = (coreThrust+fanThrust);//TOTAL THRUST GENERATED
		//
		//MAKE SURE THRUST IS NEVER NEGATIVE
		if (EngineThrust < 0) {
			EngineThrust = 0;
		}
		//
		//
		if (coreThrust > 0) {
			CalculateFuelFlow (EngineThrust);
		}
	}
}
//
//
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroTurboJet))]
public class TurboJetEditor: Editor
{
	Color backgroundColor;
	Color silantroColor = Color.cyan;
	//
	[HideInInspector]public int toolbarTab;
	[HideInInspector]public string currentTab;
	//

	public override void OnInspectorGUI()
	{
		backgroundColor = GUI.backgroundColor;
		//
		DrawDefaultInspector ();
		//
		SilantroTurboJet fan = (SilantroTurboJet)target;
		//
		GUILayout.Space(10f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Engine Dimensions", MessageType.None);
		GUI.color = backgroundColor;
		//
		//DISPLAY ENGINE DIMENSIONS
		fan.EngineDiameter = EditorGUILayout.FloatField("Engine Diameter",fan.EngineDiameter);
		fan.IntakeDiameterPercentage = EditorGUILayout.Slider ("Intake Diameter Percentage",fan.IntakeDiameterPercentage,0,100);
		//shaft.intakeDiameter = shaft.engineDiameter * shaft.intakeDiameterPercentage / 100f;
		EditorGUILayout.LabelField ("Intake Diameter", fan.IntakeDiameter.ToString ("0.00") + " m");
		fan.ExhaustDiameterPercentage = EditorGUILayout.Slider ("Exhaust Diameter Percentage",fan.ExhaustDiameterPercentage,0,100);
		//shaft.exhaustDiameter = shaft.engineDiameter * shaft.exhaustDiameterPercentage / 100f;
		EditorGUILayout.LabelField ("Exhaust Diameter", fan.ExhaustDiameter.ToString ("0.00") + " m");
		//
		GUILayout.Space(3f);
		fan.weight = EditorGUILayout.FloatField("Engine Weight",fan.weight);
		GUILayout.Space(2f);
		fan.overallLength = EditorGUILayout.FloatField("Overall Length",fan.overallLength);
		//
		GUILayout.Space(25f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Turbine Configuration", MessageType.None);
		GUI.color = backgroundColor;
		fan.LowPressureFanRPM = EditorGUILayout.FloatField ("Low Pressure Fan RPM", fan.LowPressureFanRPM);
		GUILayout.Space(2f);
		fan.HighPressureFanRPM = EditorGUILayout.FloatField ("High Pressure Fan RPM", fan.HighPressureFanRPM);

		EditorGUILayout.LabelField ("N1",fan.LPRPM.ToString("0.00")+ " RPM");
		EditorGUILayout.LabelField ("N2",fan.HPRPM.ToString("0.00")+ " RPM");
		GUILayout.Space(9f);
		fan.OverallPressureRatio = EditorGUILayout.FloatField ("Overall Pressure Ratio", fan.OverallPressureRatio);
		//
		GUILayout.Space(6f);
		fan.reheatSystem = (SilantroTurboJet.ReheatSystem)EditorGUILayout.EnumPopup ("Reheat System", fan.reheatSystem);
		//
		if (fan.reheatSystem == SilantroTurboJet.ReheatSystem.Afterburning) {
			GUILayout.Space(3f);
			GUI.color = Color.white;
			EditorGUILayout.HelpBox ("Afterburner Control", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space(2f);
			fan.AfterburnerOperative = EditorGUILayout.Toggle ("Afterburner Switch", fan.AfterburnerOperative);
		}

		//
		//
		GUILayout.Space(25f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Fuel Configuration", MessageType.None);
		GUI.color = backgroundColor;
		//
		//DISPLAY FUEL CONFIGURATION
		EditorGUILayout.LabelField ("Mass Factor",fan.massFactor.ToString("0.000"));
		fan.fuelType = (SilantroTurboJet.FuelType)EditorGUILayout.EnumPopup ("Fuel Type", fan.fuelType);
		//SET UP ENGINE FUEL COMBUSTION VALUES
		if (fan.fuelType == SilantroTurboJet.FuelType.JetB)
		{
			fan.combustionEnergy = 42.8f;
		}
		else if (fan.fuelType == SilantroTurboJet.FuelType.JetA1) 
		{
			fan.combustionEnergy = 45.5f;
		}
		else if (fan.fuelType == SilantroTurboJet.FuelType.JP6) 
		{
			fan.combustionEnergy = 47.6f;
		} 
		else if (fan.fuelType == SilantroTurboJet.FuelType.JP8)
		{
			fan.combustionEnergy = 43.28f;
		} 
		//
		EditorGUILayout.LabelField ("Combustion Energy",fan.combustionEnergy.ToString("0.0")+" MJoules");
		//EditorGUILayout.LabelField ("Combustion Factor",fan.combusionFactor.ToString("0.00"));
		//
		GUILayout.Space(3f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Fuel Usage Settings", MessageType.None);
		GUI.color = backgroundColor;
		//
		GUILayout.Space(3f);
		fan.attachedFuelTank = EditorGUILayout.ObjectField ("Fuel Tank", fan.attachedFuelTank, typeof(SilantroFuelTank), true) as SilantroFuelTank;
		EditorGUILayout.LabelField ("Fuel Remaining", fan.currentTankFuel.ToString ("0.00") + " kg");
		GUILayout.Space(5f);
		EditorGUILayout.HelpBox ("Power Specific fuel consumption in lb/hp.hr", MessageType.None);
		GUILayout.Space(3f);
		fan.TSFC = EditorGUILayout.FloatField ("Normal TSFC", fan.TSFC);
		if (fan.reheatSystem == SilantroTurboJet.ReheatSystem.Afterburning) {
			GUILayout.Space(2f);
			fan.AfterburnerTSFC = EditorGUILayout.FloatField ("Afterburner TSFC", fan.AfterburnerTSFC);
		}
		GUILayout.Space(5f);
		EditorGUILayout.LabelField ("Actual Consumption Rate",fan.actualConsumptionrate.ToString("0.00")+" kg/s");
		fan.criticalFuelLevel = EditorGUILayout.FloatField ("Critical Fuel Level", fan.criticalFuelLevel);
		//
		GUILayout.Space(25f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Connections", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		fan.Parent = EditorGUILayout.ObjectField ("Engine Parent", fan.Parent, typeof(Rigidbody), true) as Rigidbody;
		GUILayout.Space(2f);
		fan.intakeFanPoint = EditorGUILayout.ObjectField ("Intake Fan", fan.intakeFanPoint, typeof(Transform), true) as Transform;
		GUILayout.Space(3f);
		fan.rotationAxis = (SilantroTurboJet.RotationAxis)EditorGUILayout.EnumPopup("Rotation Axis",fan.rotationAxis);
		GUILayout.Space(3f);
		fan.rotationDirection = (SilantroTurboJet.RotationDirection)EditorGUILayout.EnumPopup("Rotation Direction",fan.rotationDirection);
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
		fan.EngineStartSound = EditorGUILayout.ObjectField ("Ignition Sound", fan.EngineStartSound, typeof(AudioClip), true) as AudioClip;
		fan.EngineIdleSound = EditorGUILayout.ObjectField ("Engine Idle Sound", fan.EngineIdleSound, typeof(AudioClip), true) as AudioClip;
		fan.EngineShutdownSound = EditorGUILayout.ObjectField ("Shutdown Sound", fan.EngineShutdownSound, typeof(AudioClip), true) as AudioClip;
		//
		GUILayout.Space(3f);
		fan.adjustPitchSettings = EditorGUILayout.Toggle("Show Pitch Settings",fan.adjustPitchSettings);
		GUILayout.Space(1f);
		if (fan.adjustPitchSettings) {
			fan.EngineIdlePitch = EditorGUILayout.FloatField ("Engine Idle Pitch", fan.EngineIdlePitch);
			fan.EngineMaximumRPMPitch = EditorGUILayout.FloatField ("Engine Maximum Pitch", fan.EngineMaximumRPMPitch);
			//
			if (fan.reheatSystem == SilantroTurboJet.ReheatSystem.Afterburning) {
				GUILayout.Space(3f);
				fan.EngineAfterburnerPitch = EditorGUILayout.FloatField ("Afterburner Pitch", fan.EngineAfterburnerPitch);
			}
		}
		//
		GUILayout.Space(25f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Throttle Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(2f);
		EditorGUILayout.LabelField ("Throttle Level",(fan.FuelInput*100f).ToString("0.00") + " %");
		fan.throttleSpeed = EditorGUILayout.FloatField ("Throttle Speed",fan.throttleSpeed);
		//
		GUILayout.Space(20f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Data Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(2f);
		fan.saveEngineData = EditorGUILayout.Toggle ("Log Engine Data", fan.saveEngineData);
		if (fan.saveEngineData) {
			GUILayout.Space (3f);
			fan.InculdeUnits = EditorGUILayout.Toggle ("Include Data Units", fan.InculdeUnits);
			fan.saveLocation = EditorGUILayout.TextField ("Data Location", fan.saveLocation);
			fan.dataLogRate = EditorGUILayout.FloatField ("Data Log Rate",fan.dataLogRate);
		}
		//
		//
		GUILayout.Space(25f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Engine Display", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		//
		EditorGUILayout.LabelField ("Engine State",fan.CurrentEngineState.ToString());
		EditorGUILayout.LabelField ("Engine Power",fan.EnginePower.ToString("0.00") + " %");
		EditorGUILayout.LabelField ("EGT",fan.EGT.ToString("0.0")+ " °C");
		//
		EditorGUILayout.LabelField ("Core Speed",fan.LPRPM.ToString("0.0")+ " RPM");
		GUILayout.Space(3f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Engine Output", MessageType.None);
		GUI.color = backgroundColor;
		EditorGUILayout.LabelField ("Engine Thrust",fan.EngineThrust.ToString("0.0")+ " N");
		//EditorGUILayout.LabelField(shaft.
		//
		//
		GUILayout.Space(10f);
		fan.diplaySettings = EditorGUILayout.Toggle ("Show Extras", fan.diplaySettings);
		if (fan.diplaySettings) {
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
				fan.exhaustSmoke = EditorGUILayout.ObjectField ("Exhaust Smoke", fan.exhaustSmoke, typeof(ParticleSystem), true) as ParticleSystem;
				GUILayout.Space (2f);
				fan.maximumEmissionValue = EditorGUILayout.FloatField ("Maximum Emission", fan.maximumEmissionValue);
				//
				//
				GUILayout.Space (10f);
				GUI.color = Color.white;
				EditorGUILayout.HelpBox ("Exhaust Emission Configuration", MessageType.None);
				GUI.color = backgroundColor;
				GUILayout.Space (3f);
				fan.engineMaterial = EditorGUILayout.ObjectField ("Exhaust Material", fan.engineMaterial, typeof (Material), true) as Material;
				GUILayout.Space (3f);
				fan.maximumNormalEmission = EditorGUILayout.FloatField ("Maximum Emission", fan.maximumNormalEmission);
				GUILayout.Space (2f);
				if (fan.reheatSystem == SilantroTurboJet.ReheatSystem.Afterburning) {
					fan.maximumAfterburnerEmission = EditorGUILayout.FloatField ("Maximum Afterburner Emission", fan.maximumAfterburnerEmission);
				}
				break;
			case "Destruction":
				//
				GUILayout.Space (5f);
				fan.isDestructible = EditorGUILayout.Toggle ("Is Destructible", fan.isDestructible);
				if (fan.isDestructible) {
					GUILayout.Space (5f);
					GUI.color = Color.white;
					EditorGUILayout.HelpBox ("Health Settings", MessageType.None);
					GUI.color = backgroundColor;
					GUILayout.Space (5f);
					fan.startingHealth = EditorGUILayout.FloatField ("Starting Health", fan.startingHealth);
					GUILayout.Space (3f);
					EditorGUILayout.LabelField ("Current Health", fan.currentHealth.ToString ("0.0"));
					//
					GUILayout.Space (5f);
					GUI.color = Color.white;
					EditorGUILayout.HelpBox ("Effect Settings", MessageType.None);
					GUI.color = backgroundColor;
					GUILayout.Space (5f);
					fan.engineFire = EditorGUILayout.ObjectField ("Fire Prefab", fan.engineFire, typeof(GameObject), true) as GameObject;
					GUILayout.Space (3f);
					fan.ExplosionPrefab = EditorGUILayout.ObjectField ("Explosion Prefab", fan.ExplosionPrefab, typeof(GameObject), true) as GameObject;
					//
					GUILayout.Space (15f);
					fan.hasAttachedModels = EditorGUILayout.Toggle ("Attached Models", fan.hasAttachedModels);
					//

					if (fan.hasAttachedModels) {
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
			EditorUtility.SetDirty (fan);
			EditorSceneManager.MarkSceneDirty (fan.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
		//
	}
}
#endif