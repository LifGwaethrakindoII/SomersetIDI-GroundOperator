
//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
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
public class SilantroPistonEngine : MonoBehaviour {
	//
	public enum EngineState
	{
		Off,
		Starting,
		Running
	}
	//
	[HideInInspector]public float stroke = 5;
	[HideInInspector]public float bore = 6;
	[HideInInspector]float pistonArea;float boreArea;
	[HideInInspector]public float displacement = 1000;
	[HideInInspector]public float compressionRatio = 10;
	[HideInInspector]public int numberOfCylinders = 1;
	[HideInInspector]public bool corrected;
	public enum CarburatorType
	{
		SUCarburettor,
		RAECorrected
	}
	[HideInInspector]public CarburatorType carburettorType = CarburatorType.SUCarburettor;
	//
	[HideInInspector]public float brakeHorsePower;
	//
	[HideInInspector]public bool Supercharger = false;
	public enum SuperChargerType
	{
		Solangex210,
		Solangex220,
		Solangex150,
	}
	[HideInInspector]public SuperChargerType superchargerType = SuperChargerType.Solangex150;
	//
	[HideInInspector]public float boost;
	[HideInInspector]public float rpmMultiplier;
	[HideInInspector]public float powerRequired;
	[HideInInspector]public float airflow;
	[HideInInspector]public float superchargerRPM;
	[HideInInspector]public float volumetricEfficiency;
	[HideInInspector]public float pressureRatio ;
	//
	[HideInInspector]public float norminalRPM = 100f;
	[HideInInspector]public float EngineRPM;
	[HideInInspector]public float RPMAcceleration = 0.5f;
	//
	[HideInInspector]public bool EngineOn;
	[HideInInspector]public float engineAcceleration = 0.2f;
	[HideInInspector]public float EnginePower;
	[HideInInspector]public float enginePower;
	//
	[HideInInspector]public float EGT;
	[HideInInspector]float engineInverseEfficiency;
	[HideInInspector]public bool isAccelerating;
	//
	[HideInInspector]public bool hasAttachedModels;
	[HideInInspector]public	float massFactor;
	public enum FuelType
	{
		AVGas100,
		AVGas100LL,
		AVGas82UL
	}
	[HideInInspector]public FuelType fuelType = FuelType.AVGas100;
	[HideInInspector]public float combustionEnergy;
	//
	[HideInInspector]public SilantroFuelTank attachedFuelTank;
	//public float StartAmount;
	[HideInInspector]public float FuelConsumption = 1.5f;
	[HideInInspector]public float currentTankFuel;
	[HideInInspector]public float criticalFuelLevel = 10f;
	[HideInInspector]public float actualConsumptionrate;
	[HideInInspector]bool InUse;[HideInInspector]public bool isDestructible;
	//
	[HideInInspector]public float DesiredRPM;
	[HideInInspector]float EngineIdleRPM;
	[HideInInspector]float EngineMaximumRPM;
	[HideInInspector]public float CurrentRPM;
	//
	[HideInInspector]public bool LowFuel;
	//
	[HideInInspector]public AudioClip EngineStartSound;
	[HideInInspector]public AudioClip EngineIdleSound;
	[HideInInspector]public AudioClip EngineShutdownSound;
	//
	[HideInInspector]public float EngineIdlePitch = 0.5f;
	[HideInInspector]public float EngineMaximumRPMPitch = 1f;
	[HideInInspector]public float maximumPitch = 2f;
	[HideInInspector]public float engineSoundVolume = 2f;
	[HideInInspector]public bool adjustPitchSettings;
	//
	[HideInInspector]public Rigidbody Parent;
	[HideInInspector]public Transform Thruster;
	//
	private AudioSource EngineStart;
	private AudioSource EngineRun;
	private AudioSource EngineShutdown;
	//
	//
	[HideInInspector]public bool start;
	[HideInInspector]public bool stop;
	private bool starting;
	private float velocityMe;
	private bool lowFuel;
	//+  
	float gControl; 
	//
	[HideInInspector]public bool saveEngineData = false;
	[HideInInspector]public string saveLocation = "C:/Users/";
	[HideInInspector]public float dataLogRate = 5f;
	[HideInInspector]public bool InculdeUnits = true;
	//AVAILABLE ENGNE DATA
	string engineName;
	string EngineType;
	int cylinders;
	string carbruettorType;
	float compressionRate;
	//
	float enginerpm;
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
	//
	private List<string[]> dataRow = new List<string[]>();
	float timer;
	float actualLogRate;
	//
	[HideInInspector]public float FuelInput = 0.2f;
	[HideInInspector]public float throttleSpeed = 0.15f;
	//
	string startEngine;
	string stopEngine;
	string throttleControl;
	//
	[HideInInspector]public EngineState CurrentEngineState;
	//
	[HideInInspector]public float airDensity = 1.225f;
	//
	[HideInInspector]public float PropellerThrust;
	[HideInInspector]float EngineLinearSpeed;
	//
	[HideInInspector]public SilantroInstrumentation instrumentation;
	//
	float coreFactor;
	float fuelMassFlow;
	bool fuelAlertActivated;
	[HideInInspector] public float fuelFactor = 1f;
	[HideInInspector]public float combusionFactor;
	[HideInInspector]public bool diplaySettings;
	//
	[HideInInspector]public ParticleSystem exhaustSmoke;
	[HideInInspector]ParticleSystem.EmissionModule smokeModule;
	[HideInInspector]public float maximumEmissionValue = 50f;
	[HideInInspector]public float controlValue;
	//
	SilantroControls controlBoard;
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
	[HideInInspector]public bool isControllable = true;
	// Use this for initialization
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

		EngineMaximumRPM = norminalRPM;
		EngineIdleRPM = norminalRPM * 0.1f;
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
		}
		//
		gControl=1f;
		//
		if (Parent == null) {
			Debug.Log ("Engine cannot be Operated without a Rigidbody parent!!, add Rigidbody to an empty gameobject if engine is for Test");
		}
		engineInverseEfficiency = UnityEngine.Random.Range(38f,44f);
		massFactor = UnityEngine.Random.Range(1.6f,2.2f);
		//

		timer = 0.0f;
		//
		if (dataLogRate != 0)
		{
			actualLogRate = 1.0f / dataLogRate;
		} else 
		{
			actualLogRate = 0.10f;
		}
		//
		engineName = gameObject.name;
		EngineType = "Piston Engine";
		cylinders = (int)numberOfCylinders;
		carbruettorType = carburettorType.ToString ();
		compressionRate = compressionRatio;
		//
		string[] nameTemp = new string[1];nameTemp[0] = "Engine Name: " +engineName;
		dataRow.Add(nameTemp);
		string[] typeTemp = new string[1];typeTemp[0] = "Engine Type: " +EngineType;
		dataRow.Add(typeTemp);
		string[] cylinderTemp = new string[1];cylinderTemp[0] = "Number Of Cylinders: " + cylinders.ToString();
		dataRow.Add(cylinderTemp);
		string[] cabTemp = new string[1];cabTemp[0] = "Carburettor Type: " + carbruettorType;
		dataRow.Add(cabTemp);
		string[] space = new string[2];space [0] = " ";space[1] = " ";
		dataRow.Add(space);


		//
		string[] dataRowTemp = new string[11];
		dataRowTemp [0] = "Run Time";
		dataRowTemp [1] = "True Air Speed";
		dataRowTemp [2] = "Current Altitude";
		dataRowTemp [3] = "Engine Power";
		dataRowTemp [4] = "Engine RPM";
		dataRowTemp [5] = "EGT";
		dataRowTemp [6] = "Fuel Consumption Rate";
		dataRowTemp [7] = "Supercharger RPM";
		dataRowTemp [8] = "Airflow";
		dataRowTemp [9] = "Engine HorsePower";
		dataRowTemp [10] = "Thrust Generated";
		dataRow.Add (dataRowTemp);
		//

		boreArea = ((3.142f * bore * bore) / 4f);
		pistonArea = boreArea*0.000645f;
		//
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
			engineSoundVolume = EngineRun.volume * 2f;
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

		EngineOn = false;
		start = false;
		starting = false;
		stop = false;
		//
		if (carburettorType == CarburatorType.RAECorrected) {
			corrected = true;
		} else if(carburettorType == CarburatorType.SUCarburettor){
			corrected = false;
		}
		//
		if (instrumentation == null) {
			//instrumentation.Store ();
			Debug.LogError ("Instrumentation is missing!! If Engine is just for Test, add Instrumentation Prefab to the Scene");
		} 
	}
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
			brakeHorsePower = 0;
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
	//ce per frame
	void Update () {
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
			timer += Time.deltaTime;
			if (EngineOn) {
				runTime += Time.deltaTime;	
			}
			//
			if (timer > actualLogRate && saveEngineData) {
				ComputeData ();
			}
			//
			if (attachedFuelTank != null) {
				currentTankFuel = attachedFuelTank.CurrentAmount;
			}
			//+
			if (Supercharger) {
				superchargerRPM = CurrentRPM * rpmMultiplier;
				//
				float a = CurrentRPM * displacement;
				float b = (a / (1728 * 2));
				//
				float temp = ((9 / 5) * (273.15f + instrumentation.ambientTemperature));
				airflow = ((instrumentation.ambientPressure * 0.14678f * b * 29) / (10.73f * temp));
				float eff = UnityEngine.Random.Range (0.85f, 0.9f);
				airflow = airflow * eff;
			}
			//
			//CONTROLS
			if (Input.GetButtonDown (startEngine) && attachedFuelTank != null && attachedFuelTank.CurrentAmount > 0) {
				start = true;
			}
			if (Input.GetButtonDown (stopEngine)) {
				stop = true;
			}
			//
			//JOYSTICK FUEL CONTROL
			float input = (1 + Input.GetAxis (throttleControl));
			FuelInput = input / 2f;

			//KEYBOARD FUEL CONTROL
			//FuelInput = Input.GetAxis (throttleControl);
			//CLAMP FUEL LEVEL
			FuelInput = Mathf.Clamp (FuelInput, 0f, 100f);
			//
			EngineActive ();
			if (enginePower > 0f) {
				EngineCalculation ();
			}
			//
			if (InUse && EngineOn) {
				UseFuel ();
			}
			//
			//performanceConfiguration.EngineLinearSpeed = num * 1.94384444f;
			if (Parent) {
				switch (CurrentEngineState) {
				case EngineState.Off:
					UpdateOff ();
					break;
				case EngineState.Starting:
					UpdateStarting ();
					break;
				case EngineState.Running:
					UpdateRunning ();
					break;
				}

			}
			//INTERPOLATE ENGINE RPM
			if (EngineOn) {
				CurrentRPM = Mathf.Lerp (CurrentRPM, (DesiredRPM), RPMAcceleration * Time.deltaTime * (enginePower * fuelFactor * fuelFactor));
			} else {
				CurrentRPM = Mathf.Lerp (CurrentRPM, 0.0f, RPMAcceleration * Time.deltaTime);
			}
			//

			if (null != EngineRun) {
				float magnitude = Parent.velocity.magnitude;
				float num2 = magnitude * 1.94384444f;
				float num3 = CurrentRPM + num2 * 10f;
				float num4 = (num3 - EngineIdleRPM) / (EngineMaximumRPM - EngineIdleRPM);

				float num5 = EngineIdlePitch + (EngineMaximumRPMPitch - EngineIdlePitch) * num4;
				if (gControl > 0 && !corrected) {
					num5 = num5 * gControl;
				}
				num5 = Mathf.Clamp (num5, 0f, maximumPitch);
				//
				if (attachedFuelTank != null && attachedFuelTank.CurrentAmount <= 0) {
					stop = true;
				}
				//
				if (attachedFuelTank != null && attachedFuelTank.CurrentAmount <= criticalFuelLevel) {
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
					if (CurrentEngineState == EngineState.Starting) {
						EngineRun.pitch = 1f;
					}
					EngineRun.pitch = num5 * enginePower;
				}
				//
				//
				EngineRun.volume = engineSoundVolume;
				if (CurrentRPM < EngineIdleRPM) {
					EngineRun.volume = engineSoundVolume * num5;
					if (CurrentRPM < EngineIdleRPM * 0.1f) {
						EngineRun.volume = 0f;
					}
				} else {
					EngineRun.volume = engineSoundVolume * num5;
				}

			}
		}
		//

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
	private void UpdateOff()
	{
		if (EngineStart.isPlaying)
		{
			EngineStart.Stop();
			start = false;
		}
		if (start && attachedFuelTank!= null && attachedFuelTank.CurrentAmount >0)
		{
			EngineOn = true;
			EngineStart.Play();
			CurrentEngineState = EngineState.Starting;
			starting = true;
			StartCoroutine(ReturnIgnition());
		}
		DesiredRPM = 0f;
	}
	//
	private void UpdateStarting()
	{
		if (starting)
		{
			if (!EngineStart.isPlaying)
			{
				CurrentEngineState = EngineState.Running;
				starting = false;
				UpdateRunning();
			}
		}
		else
		{
			EngineStart.Stop();
			CurrentEngineState = EngineState.Off;
		}
		DesiredRPM = EngineIdleRPM;
	}
	//
	void ComputeData()
	{
		airspeed = instrumentation.currentSpeed;
		altitude = instrumentation.currentAltitude;
		enginerpm = CurrentRPM;
		currentenginePower = EnginePower;
		//
		string minSec = string.Format ("{0}:{1:00}", (int)runTime / 60, (int)runTime % 60);
		if (InculdeUnits) {
			LogData (
				minSec + " mins",
				airspeed.ToString ("0.0") + " knots",
				altitude.ToString ("0.0") + " ft",
				currentenginePower.ToString ("0.0") + " %",
				enginerpm.ToString ("0.0") + " RPM",
				EGT.ToString ("0.0") + " °C",
				actualConsumptionrate.ToString ("0.00") + " kg/s",

				(enginerpm * rpmMultiplier).ToString ("0.0") + " RPM",
				airflow.ToString ("0.00") + " cfm",
				brakeHorsePower.ToString ("0.0") + " Hp",
				PropellerThrust.ToString ("0.00") + " N"
			);
		}
		else {
			LogData (
				minSec,
				airspeed.ToString ("0.0") ,
				altitude.ToString ("0.0"),
				currentenginePower.ToString ("0.0") ,
				enginerpm.ToString ("0.0"),
				EGT.ToString ("0.0") ,
				actualConsumptionrate.ToString ("0.00"),

				(enginerpm * rpmMultiplier).ToString ("0.0") ,
				airflow.ToString ("0.00") ,
				brakeHorsePower.ToString ("0.0"),
				PropellerThrust.ToString ("0.00") 
			);
		}
			
	}
	//
	void LogData(
		string time,
		string speed,
		string height,
		string power,
		string rpm,
		string temperature,
		string consumption,
		string superRPM,
		string airflow,
		string horsePower,
		string thrust
	)
	{
		timer = 0.0f;
		//
		string[] dataAdd = new string[11];
		//
		dataAdd[0] = time;
		dataAdd [1] = speed;
		dataAdd [2] = height;
		dataAdd [3] = power;
		dataAdd [4] = rpm;
		dataAdd [5] = temperature;
		dataAdd [6] = consumption;
		if (Supercharger) {
			dataAdd [7] = superRPM;
		} else {
			dataAdd [7] = "0 RPM";
		}
		dataAdd [8] = airflow;
		dataAdd [9] = horsePower;
		dataAdd [10] = thrust;
		dataRow.Add (dataAdd);
	}
	//
	private void UpdateRunning()
	{
		if (EngineStart.isPlaying)
		{
			EngineStart.Stop();
		}
		FuelInput = Mathf.Clamp(FuelInput, 0f, 1f);
		DesiredRPM = EngineIdleRPM + (EngineMaximumRPM - EngineIdleRPM) * FuelInput;
		if (gControl > 0 && !corrected) {
			DesiredRPM = DesiredRPM * gControl;
		}
		InUse = true;

		if (stop)
		{
			CurrentEngineState = EngineState.Off;
			EngineOn = false;
			//Stop Fuel Alert
			EngineShutdown.Play();PropellerThrust=0;
			FuelInput = 0f;
			StartCoroutine(ReturnIgnition());
		}
	}
	//
	//
	private void EngineActive()
	{
		if (instrumentation.xforce < 0) {
			float altcontrol = (2f + instrumentation.xforce);
			gControl = Mathf.Lerp (gControl, altcontrol, actualConsumptionrate*0.8f );
			if (gControl <= 0.5f && !corrected) {
				stop = true;EngineOn = false;
			}
		}

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
			EGT = 0f;
		}
		//
		EnginePower = enginePower * 100f ;
		if (gControl > 0 && !corrected) {
			EnginePower = EnginePower * gControl;
		}
	}
	///
	public void DestroyEngine()
	{

		EngineOn = false;
		brakeHorsePower = 0f;
	}
	//
	public void UseFuel()
	{
		
		{
			actualConsumptionrate = pressureRatio*combusionFactor*FuelConsumption * (FuelInput +0.1f)* EngineRun.pitch ;
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
			PropellerThrust = 0f;
		}

	}
	//
	//

	//
	public void EngineCalculation()
	{
		if (instrumentation != null) {
			airDensity=instrumentation.airDensity;
		}
		//
		coreFactor = CurrentRPM/norminalRPM;
		//CALCULATE ENGINE POWER

		float effectivePressure = compressionRatio * (instrumentation.ambientPressure / 6.895f)*0.16567f;
		if (gControl > 0 && !corrected) {
			effectivePressure = effectivePressure * gControl;
		}
		float marker = CurrentRPM / norminalRPM;
		//
		if (Supercharger) {
			float poundPressure = (instrumentation.ambientPressure * 0.145038f);
			pressureRatio = (poundPressure + boost) / poundPressure;
			//Debug.Log (boost + " " + pressureRatio);
			float newDensity = instrumentation.airDensity * pressureRatio;
			//
			brakeHorsePower = ((FuelInput*marker * boreArea * (norminalRPM / 2) * numberOfCylinders *stroke * (effectivePressure * newDensity)) / 33000) - powerRequired;

		} else {
			pressureRatio = instrumentation.airDensity;
			brakeHorsePower = (FuelInput *marker* boreArea * (norminalRPM / 2) * numberOfCylinders *stroke * (effectivePressure * pressureRatio)) / 33000;

		}
		//
		if (brakeHorsePower < 0) {
			brakeHorsePower = 0;
		}
		//
	}
	//
	public void FixedUpdate()
	{
		

		if (CurrentRPM <= 0f)
		{
			CurrentRPM = 0f;
		}
	}
	//
	void OnApplicationQuit()
	{
		//
		if(saveEngineData){
			string[][] output = new string[dataRow.Count][];
			//
			for (int i = 0; i < output.Length; i++) {
				output [i] = dataRow [i];
			}
			//
			int length = output.GetLength (0);
			string delimiter = ",";
			//
			StringBuilder builder = new StringBuilder ();
			for (int index = 0; index < length; index++)
				builder.AppendLine (string.Join (delimiter, output [index]));

			StreamWriter streamWriter = System.IO.File.CreateText (saveLocation + "" + engineName + ".csv");
			streamWriter.WriteLine (builder);
			streamWriter.Close ();

	}
	//
	}
	//
}
//

#if UNITY_EDITOR
[CustomEditor(typeof(SilantroPistonEngine))]
public class PistonEngineEditor: Editor
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
		DrawDefaultInspector ();serializedObject.Update ();
		//
		SilantroPistonEngine engine = (SilantroPistonEngine)target;//
		GUILayout.Space(10f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Engine Properties", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space (3f);
		engine.stroke = EditorGUILayout.FloatField ("Stroke", engine.stroke);
		GUILayout.Space (2f);
		engine.bore = EditorGUILayout.FloatField ("Bore", engine.bore);
		GUILayout.Space (2f);
		engine.displacement = EditorGUILayout.FloatField ("Displacement", engine.displacement);
		GUILayout.Space (2f);
		engine.compressionRatio = EditorGUILayout.FloatField ("Compression Ratio", engine.compressionRatio);
		GUILayout.Space (2f);
		engine.numberOfCylinders = EditorGUILayout.IntField ("No of Cylinders", engine.numberOfCylinders);
		GUILayout.Space (5f);
		engine.carburettorType = (SilantroPistonEngine.CarburatorType)EditorGUILayout.EnumPopup ("Carburettor Type", engine.carburettorType);
		GUILayout.Space (5f);

		engine.Supercharger = EditorGUILayout.Toggle ("Supercharger", engine.Supercharger);
		GUILayout.Space (3f);
		//EditorGUI.indentLevel++;
		//
		if (engine.Supercharger) {
			engine.superchargerType = (SilantroPistonEngine.SuperChargerType)EditorGUILayout.EnumPopup ("Type", engine.superchargerType);
			//
			if (engine.superchargerType == SilantroPistonEngine.SuperChargerType.Solangex150) {
				engine.boost = 4.8f;
				engine.powerRequired = 210.13f;
				engine.rpmMultiplier = 4.5f;
			} else if (engine.superchargerType == SilantroPistonEngine.SuperChargerType.Solangex210) {
				engine.boost = 5.6f;
				engine.powerRequired = 254.92f;
				engine.rpmMultiplier = 5.2f;
			} else if (engine.superchargerType == SilantroPistonEngine.SuperChargerType.Solangex220) {
				engine.boost = 9.3f;
				engine.powerRequired = 345.375f;
				engine.rpmMultiplier = 6.8f;
			}

			//
			GUILayout.Space (5f);

			GUI.color = Color.white;
			EditorGUILayout.HelpBox ("Supercharger Properties", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Label",engine.superchargerType.ToString ());
			EditorGUILayout.LabelField ("Pressure Boost",engine.boost + " lb");
			EditorGUILayout.LabelField  ("Power Required", engine.powerRequired + " Hp");
			EditorGUILayout.LabelField ("Total Airflow",engine.airflow.ToString("0.00") + " cfm");
			EditorGUILayout.LabelField ("Supercharger RPM", engine.superchargerRPM.ToString("0.0") + " RPM");
			//

			//
		} else {

			engine.boost = 0;
			engine.powerRequired = 0;
			engine.rpmMultiplier = 1;
		}
		//
		//EditorGUI.indentLevel--;
		//
		GUILayout.Space(5f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Engine Configuration", MessageType.None);
		GUI.color = backgroundColor;
		engine.norminalRPM = EditorGUILayout.FloatField ("Norminal RPM", engine.norminalRPM);
		GUILayout.Space (2f);
		engine.RPMAcceleration = EditorGUILayout.FloatField ("RPM Acceleration", engine.RPMAcceleration);
		//EditorGUILayout.LabelField ("N2",shaft.HPRPM.ToString("0.00")+ " RPM");
		GUILayout.Space(5f);
		//
		GUILayout.Space(15f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Fuel Configuration", MessageType.None);
		GUI.color = backgroundColor;
		//
		//DISPLAY FUEL CONFIGURATION
		EditorGUILayout.LabelField ("Mass Factor",engine.massFactor.ToString("0.000"));
		engine.fuelType = (SilantroPistonEngine.FuelType)EditorGUILayout.EnumPopup ("Fuel Type", engine.fuelType);
		//SET UP ENGINE FUEL COMBUSTION VALUES
		if (engine.fuelType == SilantroPistonEngine.FuelType.AVGas100)
		{
			engine.combustionEnergy = 42.8f;
		}
		else if (engine.fuelType == SilantroPistonEngine.FuelType.AVGas100LL) 
		{
			engine.combustionEnergy = 43.5f;
		}
		else if (engine.fuelType == SilantroPistonEngine.FuelType.AVGas82UL) 
		{
			engine.combustionEnergy = 49.6f;
		} 

		//
		engine.combusionFactor = engine.combustionEnergy/42f;	
		EditorGUILayout.LabelField ("Combustion Energy",engine.combustionEnergy.ToString("0.0")+" MJoules");
		EditorGUILayout.LabelField ("Combustion Factor",engine.combusionFactor.ToString("0.00"));
		//
		GUILayout.Space(3f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Fuel Usage Settings", MessageType.None);
		GUI.color = backgroundColor;
		//
		GUILayout.Space(3f);
		engine.attachedFuelTank = EditorGUILayout.ObjectField ("Fuel Tank", engine.attachedFuelTank, typeof(SilantroFuelTank), true) as SilantroFuelTank;
		EditorGUILayout.LabelField ("Fuel Remaining", engine.currentTankFuel.ToString ("0.00") + " kg");
		GUILayout.Space(5f);
		EditorGUILayout.HelpBox ("Power Specific fuel consumption in lb/hp.hr", MessageType.None);
		GUILayout.Space(3f);
		engine.FuelConsumption = EditorGUILayout.FloatField ("Fuel Consumption", engine.FuelConsumption);
		EditorGUILayout.LabelField ("Actual Consumption Rate",engine.actualConsumptionrate.ToString("0.00")+" kg/s");
		engine.criticalFuelLevel = EditorGUILayout.FloatField ("Critical Fuel Level", engine.criticalFuelLevel);
		//
		//
		GUILayout.Space(25f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Connections", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		engine.Parent = EditorGUILayout.ObjectField ("Engine Parent", engine.Parent, typeof(Rigidbody), true) as Rigidbody;
		GUILayout.Space(2f);
		engine.Thruster = EditorGUILayout.ObjectField ("Thruster", engine.Thruster, typeof(Transform), true) as Transform;
		//
		//
		GUILayout.Space(25f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Sound Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		engine.EngineStartSound = EditorGUILayout.ObjectField ("Ignition Sound", engine.EngineStartSound, typeof(AudioClip), true) as AudioClip;
		engine.EngineIdleSound = EditorGUILayout.ObjectField ("Engine Idle Sound", engine.EngineIdleSound, typeof(AudioClip), true) as AudioClip;
		engine.EngineShutdownSound = EditorGUILayout.ObjectField ("Shutdown Sound", engine.EngineShutdownSound, typeof(AudioClip), true) as AudioClip;
		//
		GUILayout.Space(3f);
		engine.adjustPitchSettings = EditorGUILayout.Toggle("Show Pitch Settings",engine.adjustPitchSettings);
		GUILayout.Space(1f);
		if (engine.adjustPitchSettings) {
			engine.EngineIdlePitch = EditorGUILayout.FloatField ("Engine Idle Pitch", engine.EngineIdlePitch);
			engine.EngineMaximumRPMPitch = EditorGUILayout.FloatField ("Engine Maximum Pitch", engine.EngineMaximumRPMPitch);
		}
		GUILayout.Space(25f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Throttle Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(2f);
		EditorGUILayout.LabelField ("Throttle Level",(engine.FuelInput*100f).ToString("0.00") + " %");
		engine.throttleSpeed = EditorGUILayout.FloatField ("Throttle Speed",engine.throttleSpeed);
		//
		GUILayout.Space(20f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Data Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(2f);
		engine.saveEngineData = EditorGUILayout.Toggle ("Log Engine Data", engine.saveEngineData);
		if (engine.saveEngineData) {
			GUILayout.Space (3f);
			engine.InculdeUnits = EditorGUILayout.Toggle ("Include Data Units", engine.InculdeUnits);
			engine.saveLocation = EditorGUILayout.TextField ("Data Location", engine.saveLocation);
			engine.dataLogRate = EditorGUILayout.FloatField ("Data Log Rate",engine.dataLogRate);
		}
		//
		GUILayout.Space(25f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Engine Display", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		//
		EditorGUILayout.LabelField ("Engine State",engine.CurrentEngineState.ToString());
		EditorGUILayout.LabelField ("Engine Power",engine.EnginePower.ToString("0.00") + " %");
		EditorGUILayout.LabelField ("EGT",engine.EGT.ToString("0.0")+ " °C");
		//
		EditorGUILayout.LabelField ("Core Speed",engine.CurrentRPM.ToString("0.0")+ " RPM");
		GUILayout.Space(3f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Engine Output", MessageType.None);
		GUI.color = backgroundColor;
		EditorGUILayout.LabelField ("Brake Power",engine.brakeHorsePower.ToString("0.0")+ " Hp");
		//
		GUILayout.Space(10f);
		engine.diplaySettings = EditorGUILayout.Toggle ("Show Extras", engine.diplaySettings);
		if (engine.diplaySettings) {
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
				engine.exhaustSmoke = EditorGUILayout.ObjectField ("Exhaust Smoke", engine.exhaustSmoke, typeof(ParticleSystem), true) as ParticleSystem;
				GUILayout.Space (2f);
				engine.maximumEmissionValue = EditorGUILayout.FloatField ("Maximum Emission", engine.maximumEmissionValue);
				break;
			case "Destruction":
				//
				GUILayout.Space (5f);
				engine.isDestructible = EditorGUILayout.Toggle ("Is Destructible", engine.isDestructible);
				if (engine.isDestructible) {
					GUILayout.Space (5f);
					GUI.color = Color.white;
					EditorGUILayout.HelpBox ("Health Settings", MessageType.None);
					GUI.color = backgroundColor;
					GUILayout.Space (5f);
					engine.startingHealth = EditorGUILayout.FloatField ("Starting Health", engine.startingHealth);
					GUILayout.Space (3f);
					EditorGUILayout.LabelField ("Current Health", engine.currentHealth.ToString ("0.0"));
					//
					GUILayout.Space (5f);
					GUI.color = Color.white;
					EditorGUILayout.HelpBox ("Effect Settings", MessageType.None);
					GUI.color = backgroundColor;
					GUILayout.Space (5f);
					engine.engineFire = EditorGUILayout.ObjectField ("Fire Prefab", engine.engineFire, typeof(GameObject), true) as GameObject;
					GUILayout.Space (3f);
					engine.ExplosionPrefab = EditorGUILayout.ObjectField ("Explosion Prefab", engine.ExplosionPrefab, typeof(GameObject), true) as GameObject;
					//
					GUILayout.Space (15f);
					engine.hasAttachedModels = EditorGUILayout.Toggle ("Attached Models", engine.hasAttachedModels);
					//

					if (engine.hasAttachedModels) {
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
		if (GUI.changed) {
			EditorUtility.SetDirty (engine);
			EditorSceneManager.MarkSceneDirty (engine.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
	}
}
#endif