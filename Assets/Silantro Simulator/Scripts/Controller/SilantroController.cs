//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
//
public class SilantroController : MonoBehaviour {

	[HideInInspector]public string aircraftName = "Default";
	//[Header("Aircraft Engine Type")]
	[HideInInspector]public AircraftType engineType = AircraftType.TurboFan;
	public enum AircraftType
	{
		TurboProp,
		TurboFan,
		TurboJet,
		Piston,
		Turboshaft,
		Electric
	}
	//
	[HideInInspector]public AircraftConfiguration aircraftType = AircraftConfiguration.CTOL;
	public enum AircraftConfiguration
	{
		CTOL,
		VTOL
	}
	public enum CurrentMode
	{
		Normal,
		STOL,
		VTOL,
	}
	//[HideInInspector]
	[HideInInspector]public CurrentMode currentMode = CurrentMode.Normal;
	//
	public enum ControlType
	{
		External,
		Internal
	}
	//
	//ENTER_EXIT SYSTEM
	[HideInInspector]public ControlType controlType = ControlType.External;
	[HideInInspector]public bool pilotOnboard = false;
	[HideInInspector]public SilantroCockpit cockpitControl;
	//
	[Header("Lift System")]
	[HideInInspector]public LayerMask surfaceMask;
	[HideInInspector]public float maximumHoverHeight = 30f;
	[HideInInspector]public float hoverDamper = 9000f;
	[HideInInspector]public float hoverAngleDrift = 25f;
	public enum LiftForceMode
	{
		Linear,
		Quadratic,
		Quatic
	}
	[HideInInspector]public LiftForceMode liftForceMode = LiftForceMode.Linear;
	private float exponent;
	//
	[HideInInspector]public SilantroBlade[] blades;
	[HideInInspector]public float BrakingTorque = 1000f;
	[HideInInspector]public float RollCompensationTorque=1000;
	[HideInInspector]public float PitchCompensationTorque = 1000;
	[HideInInspector]public SilantroFreyr Controller;
	///[Header("Basic Drag System")]
	//
	[HideInInspector]public bool captured;	
	[HideInInspector]public float maximumPossibleEngineThrust;
	//[Header("Aircraft Data")]
	[HideInInspector]public float wingArea = 10f;
	//[SerializeField][WeightOnly]public float loadedWeight = 1000f;
	[HideInInspector]public int AvailableEngines = 1;
	[HideInInspector]public float totalThrustGenerated = 1f;
	[HideInInspector]public float totalConsumptionRate;
	//
	[HideInInspector]public float wingLoading;
	[HideInInspector]public float thrustToWeightRatio;
	//
	[HideInInspector]public SilantroFuelTank currentTank;
	//
	//
	//[Header("Aircraft Weight System")]
	[HideInInspector]public float emptyWeight = 1000f;
	[HideInInspector]public float currentWeight;
	[HideInInspector]public float maximumWeight = 5000f;
	//
	[HideInInspector]public bool isDestructible;
	//
	[HideInInspector]public bool InstrumentBoardState = false;
	[HideInInspector]public bool WheelSystemState = false;
	[HideInInspector]public bool WeaponSystemState =false;
	[HideInInspector]public bool FuelSystemState = false;

	//
	[HideInInspector]public SilantroFuelDistributor fuelsystem;
	[HideInInspector]public SilantroInstrumentation datalog;
	[HideInInspector]public SilantroBombPod bombPod;
	[HideInInspector]public SilantroRocketPod rocketPod;
	[HideInInspector]public SilantroLightControl lightControl;
	[HideInInspector]public SilantroGearSystem gearHelper;
	[HideInInspector]public Rigidbody aircraft;
	//
	//Engine
	[HideInInspector]public SilantroTurboFan[] turbofans;
	[HideInInspector]public SilantroTurboJet[] turboJet;
	[HideInInspector]public SilantroTurboProp[] turboprop;
	[HideInInspector]public SilantroPistonEngine[] pistons;
	[HideInInspector]public SilantroTurboShaft[] shaftEngines;
	[HideInInspector]public SilantroElectricMotor[] electricMotors;
	SilantroAerofoil[] wings;
	//
	[HideInInspector]public int toolbarTab;[HideInInspector]public int lowerTab;
	[HideInInspector]public string currentTab;[HideInInspector]public string currentLowerTab;
	//
	//
	[HideInInspector]public SilantroHydraulicSystem[] hydraulics;
	[HideInInspector]public float additionalDrag;
	[HideInInspector]public float totalDrag;
	float initialDrag;
	//
	[HideInInspector]public bool isGrounded;
	//
	[HideInInspector]public float startingHealth = 500.0f;
	[HideInInspector]public float currentHealth;	
	[HideInInspector]public GameObject[] engines;
	[HideInInspector]public GameObject[] attachments;
//	[HideInInspector]public SilantroAerofoilHealth[] aerofoilHealth;
	//
	[HideInInspector]public GameObject ExplosionPrefab;
	[HideInInspector]Transform explosionPoint;
	[HideInInspector]public GameObject firePrefab;
	//
	[HideInInspector]private bool destroyed;
	[HideInInspector]public bool Explode;
	//
	[HideInInspector]private Rigidbody rb;
	//[HideInInspector]private Collider col;
	[HideInInspector]private Vector3 dropVelocity;
	[HideInInspector]public SilantroCamera camisole;
	//
	[HideInInspector]public bool hasAttachedModels;[HideInInspector]public int CountModels;
	[HideInInspector]public bool hasEngines;[HideInInspector]public int CountEngines;
	//
	//GameObject cog;
	SilantroSapphire weather;//
	SilantroLight[] lights;
	[HideInInspector]public SilantroControls controlBoard;
	SilantroRadar radar;
	//
	//
	void Start () {
		currentHealth = startingHealth;
		//
		explosionPoint = this.transform;
		//
		camisole = GetComponentInChildren<SilantroCamera> ();
		if (controlType == ControlType.Internal) {
			cockpitControl.controller = this.GetComponent<SilantroController> ();
			camisole.enabled = false;
			//Disable controls for characters
			DisableControls ();
		} else if(controlType == ControlType.External) {
			EnableControls ();
		}

	}
	//
	void Awake () {
		//
		controlBoard = GameObject.FindGameObjectWithTag ("GameController").GetComponent<SilantroControls> ();
		if (controlBoard == null) {
			Debug.LogError ("Control Board is missing, Place Control Board in scene and restart!");
		}
		//
		cockpitControl = GetComponentInChildren<SilantroCockpit>();
		//
		initialDrag = GetComponent<Rigidbody>().drag;
		if (initialDrag <= 0) {
			initialDrag = 0.01f;
		}
		//
		aircraft = GetComponent<Rigidbody> ();
		if (aircraft == null) {
			Debug.Log ("Add Rigidbody component to airplane body");
		}
		if (aircraftType == AircraftConfiguration.VTOL) {
			Controller.controller = this.GetComponent<SilantroController> ();
		}
		//
		lights = GetComponentsInChildren<SilantroLight> ();
		//SETUP NON ELECTRIC PLANES
		if (engineType != AircraftType.Electric) {
			fuelsystem = aircraft.gameObject.GetComponentInChildren<SilantroFuelDistributor>();
			currentTank = fuelsystem.internalFuelTank;
		}
		radar = GetComponentInChildren<SilantroRadar> ();
		datalog = aircraft.gameObject.GetComponentInChildren<SilantroInstrumentation>();
		//datalog.controller = this.gameObject.GetComponent<SilantroController> ();
		bombPod = aircraft.gameObject.GetComponentInChildren<SilantroBombPod> ();
		rocketPod = aircraft.gameObject.GetComponentInChildren<SilantroRocketPod> ();
		//
		if (bombPod != null) {
			bombPod.Aircraft = this.transform;
			bombPod.controlBoard = controlBoard;
		}
		//
		hydraulics = GetComponentsInChildren<SilantroHydraulicSystem> ();
		gearHelper = GetComponentInChildren<SilantroGearSystem> ();
		gearHelper.control = this.gameObject.GetComponent<SilantroController> ();

		//AIRCRAFT SETUP
		if (engineType == AircraftType.TurboFan) {
			turbofans = GetComponentsInChildren<SilantroTurboFan> ();
			foreach (SilantroTurboFan turbofan in turbofans) {
			turbofan.instrumentation = datalog;
			turbofan.attachedFuelTank = currentTank;
			}
			AvailableEngines = turbofans.Length;
			//Input.GetKeyDown(
			//
		} else if (engineType == AircraftType.TurboJet) {
			turboJet = GetComponentsInChildren<SilantroTurboJet> ();
			AvailableEngines = turboJet.Length;
			foreach (SilantroTurboJet jet in turboJet) {
			jet.instrumentation = datalog;
			jet.attachedFuelTank = currentTank;
			}
		} else if (engineType == AircraftType.TurboProp) {
			turboprop = GetComponentsInChildren<SilantroTurboProp> ();
			blades = GetComponentsInChildren<SilantroBlade> ();
			AvailableEngines = turboprop.Length;
			foreach (SilantroTurboProp turboProp in turboprop) {
				turboProp.instrumentation = datalog;
				turboProp.attachedFuelTank = currentTank;

			}
		} else if (engineType == AircraftType.Piston) {
			pistons = GetComponentsInChildren<SilantroPistonEngine> ();
			blades = GetComponentsInChildren<SilantroBlade> ();
			AvailableEngines = pistons.Length;
			foreach (SilantroPistonEngine piston in pistons) {
				piston.instrumentation = datalog;
				piston.attachedFuelTank = currentTank;
			}
		}
		else if (engineType == AircraftType.Electric) {
			electricMotors = GetComponentsInChildren<SilantroElectricMotor> ();
			blades = GetComponentsInChildren<SilantroBlade> ();
			AvailableEngines = electricMotors.Length;
			foreach (SilantroElectricMotor motor in electricMotors) {
				motor.instrumentation = datalog;
			}
		}
		else if (engineType == AircraftType.Turboshaft) {
			shaftEngines = GetComponentsInChildren<SilantroTurboShaft> ();
			AvailableEngines = pistons.Length;
			blades = GetComponentsInChildren<SilantroBlade> ();
			foreach (SilantroTurboShaft shaft in shaftEngines) {
				shaft.instrumentation = datalog;
				shaft.attachedFuelTank = currentTank;
			}
		}
		//
		GameObject lighter = GameObject.FindGameObjectWithTag ("Weather");
		if (lighter != null) {
			weather = lighter.GetComponent<SilantroSapphire> ();
		}
		//
		wings = GetComponentsInChildren<SilantroAerofoil>();
		wingArea = 0;
		if (wings.Length > 0) {
			foreach (SilantroAerofoil wing in wings) {
				wingArea += wing.aerofoilArea;
				wing.instrumentation = datalog;
				wing.weather = weather;
				wing.controlBoard = controlBoard;
			}
		}
		//
		lightControl = GetComponentInChildren<SilantroLightControl>();
		if (lightControl != null) {
			lightControl.Controlboard = controlBoard;
			lightControl.lights = lights;
			gearHelper.lights = lights;
		}
		//
		//cog = datalog.gameObject;
		//
		Collider col = GetComponent<CapsuleCollider>();
		if (col == null) {
			Debug.Log ("Attach a capsule collider to the airplane and restart!!");
		} 
		//
		//
		if (datalog != null) {
			InstrumentBoardState = true;
		} 
		if(fuelsystem != null ) {
			FuelSystemState = true;
		}
		if (gearHelper != null) {
			WheelSystemState = true;
		}
		if (bombPod != null) {
			WeaponSystemState = true;
		}

	}

	//
	void Update()
	{
		if (Explode && !destroyed) {
			currentHealth = 0;
			Disintegrate ();
		}
		if(Input.GetKeyDown(KeyCode.F5))
		{
			Explode = true;
		}
	}//
	//
	//
	//ENTER_EXIT SYSTEM
	//
	public void EnableControls()
	{
		//ENABLE WINGS
		foreach (SilantroAerofoil wing in wings) {
			wing.isControllable = true;
		}
		gearHelper.isControllable = true;
		if (Controller != null) {
			Controller.isControllable = true;
		}
		fuelsystem.isControllable = true;
		if (lightControl) {
			lightControl.isControllable = true;
		}
		if (bombPod != null) {
			bombPod.isControllable = true;
		}
		//
		if (radar != null) {
			radar.enabled = true;
		}
		//
		if (rocketPod != null) {
			rocketPod.isControllable = true;
		}
		//
		SilantroMinigun[] guns = GetComponentsInChildren<SilantroMinigun> ();
		foreach (SilantroMinigun gun in guns) {
			if (gun != null) {
				gun.isControllable = true;
			}
		}
		foreach (SilantroBlade blade in blades) {
			blade.isControllable = true;
		}
		//ENABLE ENGINES
		if (engineType == AircraftType.TurboFan) {
			totalThrustGenerated = 0;totalConsumptionRate = 0;
			foreach (SilantroTurboFan turbofan in turbofans) {
				turbofan.isControllable = true;
			}
		}
		if (engineType == AircraftType.TurboJet) {
			totalThrustGenerated = 0;totalConsumptionRate = 0;
			foreach (SilantroTurboJet turbojet in turboJet) {
				turbojet.isControllable = true;
			}
		}
		if (engineType == AircraftType.TurboProp) {
			totalThrustGenerated = 0;totalConsumptionRate = 0;
			foreach (SilantroTurboProp turboProp in turboprop) {
				turboProp.isControllable = true;
			}
		}
		if (engineType == AircraftType.Piston) {
			totalThrustGenerated = 0;totalConsumptionRate = 0;
			foreach (SilantroPistonEngine piston in pistons) {
				piston.isControllable = true;
			}
		}
		if (engineType == AircraftType.Turboshaft) {
			totalThrustGenerated = 0;totalConsumptionRate = 0;
			foreach (SilantroTurboShaft shaft in shaftEngines) {
				shaft.isControllable = true;
			}
		}
		if (engineType == AircraftType.Electric) {
			
		}
		//
		//ENABLE CAMERA
		camisole.enabled = true;
	}
	//
	public void DisableControls()
	{
		//DISABLE WINGS
		//ENABLE WINGS
		foreach (SilantroAerofoil wing in wings) {
			wing.isControllable = false;
		}
		gearHelper.isControllable = false;
		if (Controller != null) {
			Controller.isControllable = false;
		}
		fuelsystem.isControllable = false;
		if (lightControl) {
			lightControl.isControllable = false;
		}
		if (bombPod != null) {
			bombPod.isControllable = false;
		}
		//
		if (rocketPod != null) {
			rocketPod.isControllable = false;
		}
		//
		//
		if (radar != null) {
			radar.enabled = false;
		}
		//
		SilantroMinigun[] guns = GetComponentsInChildren<SilantroMinigun> ();
		foreach (SilantroMinigun gun in guns) {
			if (gun != null) {
				gun.isControllable = false;
			}
		}
		//
		foreach (SilantroBlade blade in blades) {
			blade.isControllable = false;
		}
		//DISABLE ENGINES
		if (engineType == AircraftType.TurboFan) {
			totalThrustGenerated = 0;totalConsumptionRate = 0;
			foreach (SilantroTurboFan turbofan in turbofans) {
				turbofan.isControllable = false;
			}
		}
		if (engineType == AircraftType.TurboJet) {
			totalThrustGenerated = 0;totalConsumptionRate = 0;
			foreach (SilantroTurboJet turbojet in turboJet) {
				turbojet.isControllable = false;
			}
		}
		if (engineType == AircraftType.TurboProp) {
			totalThrustGenerated = 0;totalConsumptionRate = 0;
			foreach (SilantroTurboProp turboProp in turboprop) {
				turboProp.isControllable = false;
			}
		}
		if (engineType == AircraftType.Piston) {
			totalThrustGenerated = 0;totalConsumptionRate = 0;
			foreach (SilantroPistonEngine piston in pistons) {
				piston.isControllable = false;
			}
		}
		if (engineType == AircraftType.Turboshaft) {
			totalThrustGenerated = 0;totalConsumptionRate = 0;
			foreach (SilantroTurboShaft shaft in shaftEngines) {
				shaft.isControllable = false;
			}
		}
		if (engineType == AircraftType.Electric) {

		}
		//ENABLE CAMERA
		camisole.enabled = false;
	}
	//
	//
	//HIT SYSTEM
	public void SilantroDamage(float amount)
	{
		currentHealth += amount;

		// If the health runs out, then Die.
		if (currentHealth < 0)
		{
			currentHealth = 0;
		}

		//Die Procedure
		if (currentHealth == 0 && !destroyed && isDestructible)
			Disintegrate();
	}
	//
	//DESTRUCTION SYSTEM
	public void Disintegrate()
	{
		if (isDestructible && !destroyed) {
			
			if (transform.root.GetComponent<Rigidbody> ()) {
				dropVelocity = transform.root.GetComponent<Rigidbody> ().velocity;
			}
			transform.root.GetComponent<Rigidbody> ().isKinematic = false;
			destroyed = true;
			//
			//KILL PILOT
			if (cockpitControl != null && controlType == ControlType.Internal && cockpitControl.pilotOnboard) {
				cockpitControl.pilot.SetActive (false);
				cockpitControl.player.transform.SetParent (null);
				cockpitControl.player.transform.position = cockpitControl.getOutPosition.position;
				cockpitControl.player.transform.rotation = cockpitControl.getOutPosition.rotation;
				cockpitControl.player.transform.rotation = Quaternion.Euler (0f, cockpitControl.player.transform.eulerAngles.y, 0f);
				cockpitControl.player.SetActive (true);
			}
			//
			if (controlType == ControlType.Internal && cockpitControl != null) {
				//
				//KILL CHARACTER
				if (cockpitControl.pilotOnboard) {
					GameObject player = cockpitControl.player;
					//
					//player.SendMessage ("Die");
				}
				//
			}
			//ACTIVATE EXPLOSION AND FIRE
			if (explosionPoint == null) {
				explosionPoint = transform;
			}
			//
			if (ExplosionPrefab != null) {
				GameObject explosion = Instantiate (ExplosionPrefab, explosionPoint.position, Quaternion.identity);
				explosion.SetActive (true);
				explosion.GetComponentInChildren<AudioSource> ().Play ();
			}
			if (firePrefab != null) {
				GameObject fire = Instantiate (firePrefab, explosionPoint.position, Quaternion.identity);
				fire.SetActive (true);fire.transform.parent = gameObject.transform.root;fire.transform.localPosition = new Vector3 (0, 0, 0);
				fire.GetComponentInChildren<AudioSource> ().Play ();
			}
			//
			//DESTROY LIGHTS
			SilantroLight[] lights = GetComponentsInChildren<SilantroLight>();
			foreach (SilantroLight light in lights) {
				if (light != null) {
					light.active = false;
					Destroy (light.gameObject);
				}
			}	
			//DESTROY GUNS
			SilantroMinigun[] guns = GetComponentsInChildren<SilantroMinigun>();
			foreach (SilantroMinigun gun in guns) {
				if (gun != null) {
					gun.Disintegrate ();
				}
			}	
			//
			int a;
			if (wings.Length > 0) {
				for (a = 0; a < wings.Length; a++) {
					if (wings [a] != null) {
						wings [a].Disintegrate ();
					}
				}
			}
			//
			SilantroBomb[] bombs = GetComponentsInChildren<SilantroBomb>();
			if (bombs.Length > 0) {
				foreach (SilantroBomb bomb in bombs) {
					bomb.Explode (bomb.transform.position);
				}
			}
			//
			//
			WheelCollider[] wheels = GetComponentsInChildren<WheelCollider> ();
			if (wheels.Length > 0) {
				int y;
				for (y = 0; y < wheels.Length; y++) {
					Destroy (wheels [y].gameObject);
				}
			}
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
			//

			//
			//SHUTDOWN ENGINE
			int l;
			if (engines.Length > 0) {
				for (l = 0; l < engines.Length; l++) {

					engines [l].SendMessage ("DestroyEngine", SendMessageOptions.DontRequireReceiver);
					engines [l].SendMessage ("Disintegrate", SendMessageOptions.DontRequireReceiver);

				}
			}
			if (bombPod != null) {
				foreach (SilantroBomb bomb in bombPod.availableBombs) {
					if (bomb != null) {
						bomb.Explode (bomb.transform.position);
					}
				}
				Destroy (bombPod.gameObject);
			}
		}
	}
	//
	//DAMAGE
	void OnCollisionEnter(Collision col)
	{
		//if (col.collider.transform.root.tag != "Ground") {
		if (col.relativeVelocity.magnitude >50f && isDestructible) {
			Disintegrate ();
		}
		///	}
	}
	//
	void FixedUpdate () {
		
		totalConsumptionRate = 0;

		if (engineType == AircraftType.TurboFan) {
			totalThrustGenerated = 0;totalConsumptionRate = 0;
			foreach (SilantroTurboFan turbofan in turbofans) {
				//CAPTURE ENGINE THRUST
				totalThrustGenerated += turbofan.EngineThrust;
				totalConsumptionRate += turbofan.actualConsumptionrate;
				//
			}
		}
		if (engineType == AircraftType.TurboJet) {
			totalThrustGenerated = 0;totalConsumptionRate = 0;
			foreach (SilantroTurboJet turbojet in turboJet) {
				//CAPTURE ENGINE THRUST
				//	
				totalThrustGenerated += turbojet.EngineThrust;
				totalConsumptionRate += turbojet.actualConsumptionrate;
			}
		}
		if (engineType == AircraftType.TurboProp) {
			totalThrustGenerated = 0;totalConsumptionRate = 0;
			foreach (SilantroTurboProp turboProp in turboprop) {
				totalConsumptionRate += turboProp.actualConsumptionrate;
			}
			foreach (SilantroBlade blade in blades) {
				totalThrustGenerated += blade.thrust;
			}
		}
		if (engineType == AircraftType.Piston) {
			totalThrustGenerated = 0;totalConsumptionRate = 0;
			foreach (SilantroPistonEngine piston in pistons) {
				//CAPTURE ENGINE THRUST
				totalConsumptionRate += piston.actualConsumptionrate;
			}
			foreach (SilantroBlade blade in blades) {
				totalThrustGenerated += blade.thrust;
			}
		}
		if (engineType == AircraftType.Turboshaft) {
			totalThrustGenerated = 0;totalConsumptionRate = 0;
			foreach (SilantroTurboShaft shaft in shaftEngines) {
				//CAPTURE ENGINE THRUST
				totalConsumptionRate += shaft.actualConsumptionrate;
			}
			foreach (SilantroBlade blade in blades) {
				totalThrustGenerated += blade.thrust;
			}
		}
		if (engineType == AircraftType.Electric) {
			totalThrustGenerated = 0;
			foreach (SilantroBlade blade in blades) {
				totalThrustGenerated += blade.thrust;
			}
		}
		//
		SilantroJATOController jato = GetComponentInChildren<SilantroJATOController>();
		if (jato != null) {
			totalThrustGenerated += jato.TotalThrust;
		}
		//
		if (engineType != AircraftType.Electric) {
			currentWeight = emptyWeight + fuelsystem.TotalFuelRemaining;
		} else {
			currentWeight = emptyWeight;
		}
		//
		//
		if (bombPod) {	
			if (engineType != AircraftType.Electric) {
				currentWeight = emptyWeight + fuelsystem.TotalFuelRemaining+bombPod.totalWeight;
			} else {
				currentWeight = emptyWeight + bombPod.totalWeight;;
			}
		}
		//
		if (rocketPod) {
			currentWeight = currentWeight + rocketPod.totalWeight;
		}
		//
		if(aircraft != null)aircraft.mass = currentWeight;
		if (currentWeight > maximumWeight) {
			Debug.Log ("Aircraft is too Heavy for takeoff, Dump some Fuel...");
		}
		//
		additionalDrag = 0;
		foreach (SilantroHydraulicSystem hydraulic in hydraulics) {
			additionalDrag += hydraulic.dragAmount;
		}
		totalDrag = initialDrag + additionalDrag;
		GetComponent<Rigidbody> ().drag = totalDrag;
		if (additionalDrag <= 0) {
			additionalDrag = 0;
		}
		//
		wingLoading = currentWeight/wingArea;
		thrustToWeightRatio = totalThrustGenerated / currentWeight;
		gearHelper.availablePushForce = totalThrustGenerated;
		//
		//
		if (aircraftType == AircraftConfiguration.VTOL) {
			if (currentMode == CurrentMode.VTOL) {
				VTOLCalculations ();
			}
		}
		//
		if (engineType != AircraftType.Electric) {
			fuelsystem.totalConsumptionRate = totalConsumptionRate;
		}
	}
	//
	void VTOLCalculations()
	{
		var localR = Vector3.Dot(aircraft.angularVelocity, transform.up);
		aircraft.AddRelativeTorque(0, -localR*BrakingTorque, 0);
		//
		if (transform.position.y > 5f) {
			// find current pitch and roll. We need them in local space, not world space!
			var grav = -Physics.gravity.normalized;
			var pitch = Mathf.Asin (Vector3.Dot (transform.forward, grav)) * Mathf.Rad2Deg;
			var roll = Mathf.Asin (Vector3.Dot (transform.right, grav)) * Mathf.Rad2Deg;
			pitch = Mathf.DeltaAngle (pitch, 0); 
			roll = Mathf.DeltaAngle (roll, 0);

			// apply compensation torque
			var pt = -pitch * PitchCompensationTorque;
			var rt = roll * RollCompensationTorque;
			aircraft.AddRelativeTorque (pt, 0, rt);
		}
	}
	//
	//
	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		//
		//
		if (aircraft == null && GetComponent<Rigidbody> () ) {
			 aircraft = GetComponent<Rigidbody> ();
		//
		}

	}
	//
	#endif
}
//
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroController))]
public class ControllerEditor: Editor
{
	Color backgroundColor;

	public override void OnInspectorGUI()
	{
		backgroundColor = GUI.backgroundColor;
		//
		DrawDefaultInspector ();serializedObject.Update ();
		//
		SilantroController controller = (SilantroController)target;
		//
		GUILayout.Space(3f);
		GUI.color = Color.cyan;
		EditorGUILayout.HelpBox ("Control Type", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		controller.controlType = (SilantroController.ControlType)EditorGUILayout.EnumPopup("Control Type",controller.controlType);
		//
		GUILayout.Space(10f);
		GUI.color = Color.cyan;
		EditorGUILayout.HelpBox ("Aircraft Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		//
		controller.toolbarTab = GUILayout.Toolbar (controller.toolbarTab, new string[]{ "Setup", "Weight Settings"});
		controller.lowerTab = GUILayout.Toolbar (controller.lowerTab, new string[]{ "Destruction", "Display" });
		switch (controller.toolbarTab) {
		case 0:
			//
			controller.currentTab = "Setup";
			break;
		case 1:
			//
			controller.currentTab = "Weight Settings";
			break;
		}
		switch (controller.lowerTab) {
		case 0:
			//
			controller.currentLowerTab = "Destruction";
			break;
		case 1:
			//
			controller.currentLowerTab = "Display";
			break;
		}

		//

		switch (controller.currentTab) {
		case "Setup":
			//
			GUILayout.Space (15f);
			GUI.color = Color.yellow;
			EditorGUILayout.HelpBox ("Aircraft Configuration", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (3f);
			controller.aircraftName = EditorGUILayout.TextField ("Aircraft Name", controller.aircraftName);
			GUILayout.Space (7f);
			controller.aircraftType = (SilantroController.AircraftConfiguration)EditorGUILayout.EnumPopup ("System Type", controller.aircraftType);
			//

				GUILayout.Space (5f);
				GUI.color = Color.white;
				EditorGUILayout.HelpBox ("Engine Configuration", MessageType.None);
				GUI.color = backgroundColor;
				GUILayout.Space (3f);
				controller.engineType = (SilantroController.AircraftType)EditorGUILayout.EnumPopup ("Engine Type", controller.engineType);
				//
		if (controller.aircraftType == SilantroController.AircraftConfiguration.VTOL) {
				GUILayout.Space (5f);
				EditorGUILayout.LabelField ("Current Mode", controller.currentMode.ToString ());
				GUILayout.Space (5f);
				GUI.color = Color.white;
				EditorGUILayout.HelpBox ("Freyr Controller", MessageType.None);
				GUI.color = backgroundColor;
				GUILayout.Space (3f);
				controller.Controller = EditorGUILayout.ObjectField (" ", controller.Controller, typeof(SilantroFreyr), true) as SilantroFreyr;
				GUILayout.Space (3f);
				GUI.color = Color.white;
				EditorGUILayout.HelpBox ("Balance Configuration", MessageType.None);
				GUI.color = backgroundColor;
				GUILayout.Space (3f);
				controller.BrakingTorque = EditorGUILayout.FloatField ("Braking Torque", controller.BrakingTorque);
				GUILayout.Space (2f);
				controller.RollCompensationTorque = EditorGUILayout.FloatField ("Longitudinal Balance Torque", controller.RollCompensationTorque);
				GUILayout.Space (2f);
				controller.PitchCompensationTorque = EditorGUILayout.FloatField ("Lateral Balance Torque", controller.PitchCompensationTorque);
				//GUILayout.Space (2f);
			}
			//
			break;
		
		case "Weight Settings":
			//
			//
			GUILayout.Space (15f);
			GUI.color = Color.white;
			EditorGUILayout.HelpBox ("Aircraft Weight Configuration", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (3f);
			//
			controller.emptyWeight = EditorGUILayout.FloatField ("Empty Weight", controller.emptyWeight);
			GUILayout.Space (1f);
			controller.maximumWeight = EditorGUILayout.FloatField ("Maximum Weight", controller.maximumWeight);
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Current Weight", controller.currentWeight.ToString ("0.00") + " kg");
			//
			//
			break;
		}
		//
		//
		//
		switch (controller.currentLowerTab) {
		case "Destruction":
			//
			GUILayout.Space (15f);
			GUI.color = Color.cyan;
			EditorGUILayout.HelpBox ("Aircraft Destruction Switch", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (3f);
			controller.isDestructible = EditorGUILayout.Toggle ("Is Destructible", controller.isDestructible);
			//
			if (controller.isDestructible) {
				GUILayout.Space (5f);
				GUI.color = Color.white;
				EditorGUILayout.HelpBox ("Health Settings", MessageType.None);
				GUI.color = backgroundColor;
				GUILayout.Space (5f);
				controller.startingHealth = EditorGUILayout.FloatField ("Starting Health", controller.startingHealth);
				GUILayout.Space (3f);
				EditorGUILayout.LabelField ("Current Health", controller.currentHealth.ToString ("0.0"));
				//
				GUILayout.Space (5f);
				GUI.color = Color.white;
				EditorGUILayout.HelpBox ("Effect Settings", MessageType.None);
				GUI.color = backgroundColor;
				GUILayout.Space (5f);
				controller.firePrefab = EditorGUILayout.ObjectField ("Fire Prefab", controller.firePrefab, typeof(GameObject), true) as GameObject;
				GUILayout.Space (3f);
				controller.ExplosionPrefab = EditorGUILayout.ObjectField ("Explosion Prefab", controller.ExplosionPrefab, typeof(GameObject), true) as GameObject;
				//
				GUILayout.Space (15f);
				controller.hasAttachedModels = EditorGUILayout.Toggle ("Attached Models", controller.hasAttachedModels);
				//

				if (controller.hasAttachedModels) {
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
				GUILayout.Space (15f);
				controller.hasEngines = EditorGUILayout.Toggle ("Attached Engines", controller.hasEngines);
				//
				if (controller.hasEngines) {
					GUILayout.Space (3f);
					GUI.color = Color.white;
					EditorGUILayout.HelpBox ("Engines", MessageType.None);
					GUI.color = backgroundColor;
					GUILayout.Space (3f);
					//
					GUIContent engineLabel = new GUIContent ("Engine Count");
					GUILayout.Space (5f);
					EditorGUILayout.BeginHorizontal ();
					EditorGUILayout.BeginVertical ();
					//
					SerializedProperty engine = this.serializedObject.FindProperty ("engines");
					EditorGUILayout.PropertyField (engine.FindPropertyRelative ("Array.size"), engineLabel);
					GUILayout.Space (5f);
					for (int i = 0; i < engine.arraySize; i++) {
						GUIContent label = new GUIContent ("Engine " + (i + 1).ToString ());
						EditorGUILayout.PropertyField (engine.GetArrayElementAtIndex (i), label);
					}
					EditorGUILayout.EndHorizontal ();
					EditorGUILayout.EndVertical ();
					//
				}
				//
			}
			//
			break;
		//
		case "Display":
			//
			GUILayout.Space (15f);
			GUI.color = Color.yellow;
			EditorGUILayout.HelpBox ("Aircraft Data Display", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Wing Area", controller.wingArea.ToString ("0.00") + " m2");
			EditorGUILayout.LabelField ("Available Engines", controller.AvailableEngines.ToString ());
			EditorGUILayout.LabelField ("Total Thrust", controller.totalThrustGenerated.ToString ("0.00") + " N");
			EditorGUILayout.LabelField ("Wing Loading", controller.wingLoading.ToString ("0.00") + " kg/m2");
			EditorGUILayout.LabelField ("Thrust/Weight Ratio", controller.thrustToWeightRatio.ToString ("0.00"));
			//
			GUILayout.Space (10f);
			GUI.color = Color.white;
			EditorGUILayout.HelpBox ("Aircraft components observation board. Updates when aircraft is active", MessageType.Info);
			GUI.color = backgroundColor;
			//
			GUILayout.Space (2f);
			if (controller.InstrumentBoardState) {
				GUI.color = Color.green;
				EditorGUILayout.HelpBox ("Instrument Board is Active", MessageType.None);
				GUI.color = backgroundColor;
			} else {
				GUI.color = Color.red;
				EditorGUILayout.HelpBox ("Instrument Board is Missing", MessageType.None);
				GUI.color = backgroundColor;
			}
			//
			if (controller.WheelSystemState) {
				GUI.color = Color.green;
				EditorGUILayout.HelpBox ("Wheel System is Active", MessageType.None);
				GUI.color = backgroundColor;
			} else {
				GUI.color = Color.red;
				EditorGUILayout.HelpBox ("Wheel System is Missing", MessageType.None);
				GUI.color = backgroundColor;
			}
			//
			//
			if (controller.FuelSystemState) {
				GUI.color = Color.green;
				EditorGUILayout.HelpBox ("Fuel System is Active", MessageType.None);
				GUI.color = backgroundColor;
			} else {
				GUI.color = Color.red;
				EditorGUILayout.HelpBox ("Fuel System is Missing", MessageType.None);
				GUI.color = backgroundColor;
			}
			//
			//
			if (controller.WeaponSystemState) {
				GUI.color = Color.green;
				EditorGUILayout.HelpBox ("Weapon System is Active", MessageType.None);
				GUI.color = backgroundColor;
			} else {
				GUI.color = Color.red;
				EditorGUILayout.HelpBox ("Weapon System is Missing", MessageType.None);
				GUI.color = backgroundColor;
			}
			//
			//
			//
			break;
		
		}
		if (GUI.changed) {
			EditorUtility.SetDirty (controller);
			EditorSceneManager.MarkSceneDirty (controller.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();

	}
}
#endif