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
[RequireComponent(typeof(BoxCollider))]
public class SilantroAerofoil : MonoBehaviour {
	//
	//Aerofoil Type 
	[HideInInspector]private Rigidbody rb;
	[HideInInspector]private Collider col;
	[HideInInspector]public int toolbarTab;
	[HideInInspector]public string currentTab;
	//
	[HideInInspector]private Vector3 dropVelocity;
	public enum AerofoilType
	{
		Wing,
		Stabilizer
	}
	[HideInInspector]public AerofoilType aerofoilType = AerofoilType.Wing;
	//
	[HideInInspector]public SilantroAirfoil airfoil;
	[HideInInspector]public float liftCoefficent;
	[HideInInspector]public float dragCoefficient;
	//
	[HideInInspector]public SweepDirection sweepDirection = SweepDirection.Unswept;
	[HideInInspector]public float aerofoilSweepAngle;float sweepangle;
	//
	//SWEP DIRECTION OF WING
	public enum SweepDirection
	{
		Unswept,
		Forward,
		Backward
	}
	//
	[HideInInspector]public TwistDirection twistDirection = TwistDirection.Untwisted;
	//
	[HideInInspector]public float twistAngle;
	//
	//
	public enum TwistDirection
	{
		Untwisted,
		Upwards,
		Downwards
	}
	//
	[HideInInspector]public float AerofoilTipWidth = 0.0f;
	[HideInInspector]public int AerofoilSubdivisions;
	//
	[HideInInspector]public float rootChord;
	[HideInInspector]public float tipChord;
	[HideInInspector]public float leadingEdgeLength;
	[HideInInspector]public float trailingEdgeLength;
	//
	[HideInInspector]private Vector3 aerofoilRootLeadingEdge = Vector3.zero;
	[HideInInspector]private Vector3 aerofoilRootTrailingEdge = Vector3.zero;
	[HideInInspector]private Vector3 aerofoilTipLeadingEdge = Vector3.zero;
	[HideInInspector]private Vector3 aerofoilTipTrailingEdge = Vector3.zero;
	//
	//EXTRA CONTROL SURFACES
	//1. FLAP
	[HideInInspector]public bool usesFlap;
	[HideInInspector]public bool usesSlats;
	[HideInInspector]public bool[] AffectedFlapSubdivisions;
	[HideInInspector]public float maximumFlapDeflection = 30;
	[HideInInspector]public float CurrentFlapDeflection = 0.0f;
	[HideInInspector]public bool negativeFlapDeflection = false;
	[HideInInspector]public float BottomFlapHingeDistance = 2.5f;
	[HideInInspector]public float FlapRootHeight;
	[HideInInspector]public float TopFlapHingeDistance = 2.5f;
	[HideInInspector]public float FlapTipHeight;
	[HideInInspector]public Vector3 FlapaxisRotation; 
	[HideInInspector]public float totalFlapArea;
	[HideInInspector]public float totalSlatArea;
	float operationalFlapArea;
	float operationalSlatArea;
	//
	//FLAP CONTROL
	[HideInInspector]public float[] flapModes ;
	[HideInInspector]public float currentFlap;
	//
	[HideInInspector]public string increaseFlap;
	[HideInInspector]public string decreaseFlap;
	[HideInInspector]public string actuateSlat;
	[HideInInspector]public string actuateSpoilers;
	//
	[HideInInspector]public bool usesSpoilers;
	[HideInInspector]public bool[] AffectedSpoilerSubdivisions;
	[HideInInspector]public float maximumSpoilerDeflection = 62f;
	[HideInInspector]public float currentSpoilerDeflection = 0f;
	[HideInInspector]public bool negativeSpoilerDeflection;
	[HideInInspector]public float totalSpoilerArea;
	[HideInInspector]public float spoilerDrag;
	[HideInInspector]public GameObject spoilerModel;
	public enum SpoilerRotationAxis
	{
		X,
		Y,
		Z
	}
	[HideInInspector]public SpoilerRotationAxis spoilerrotationAxis = SpoilerRotationAxis.X;
	[HideInInspector]public Vector3 spoilerAxisRotation;
	[HideInInspector]public float targetSpoilerAngle;
	[HideInInspector]public bool spoilerExtended;
	[HideInInspector]public float spoilerAngle;
	[HideInInspector]public float spoilerActuationTime = 4f;
	[HideInInspector]public bool spoilerMoving;
	[HideInInspector]public bool customSpoilerDeflection = false;
	//
	//
	AudioSource mechanicalSounds;int flap = 0;
	[HideInInspector]public AudioClip flapDown;
	[HideInInspector]public AudioClip flapUp;
	//
	//EXTRA DRAG SETTINGS
	[HideInInspector]public float flapDrag;
	[HideInInspector]public float slatDrag;
	//
	[HideInInspector]public float flaprootChord;
	[HideInInspector]public float flaptrailChord;
	//
	float btflaphingeDistance ;
	float tpflaphingeDistance;
	private Vector3 flapRootHingePos = Vector3.zero;
	private Vector3 flapTipHingePos = Vector3.zero;
	//
	[HideInInspector]public GameObject FlapModel;
	//
	[HideInInspector]public bool negativeFlapRotation = false;
	public enum FlapRotationAxis
	{
		X,
		Y,
		Z
	}
	[HideInInspector]public FlapRotationAxis flaprotationAxis = FlapRotationAxis.X;
	[HideInInspector]public Vector3 flapAxisRotation;
	//
	//2. SLAT
	[HideInInspector]public bool[] AffectedSlatSubdivisions;
	[HideInInspector]public float maximumSlatDeflection = 30;
	[HideInInspector]public float CurrentSlatDeflection = 0.0f;
	[HideInInspector]public bool negativeSlatDeflection = false;
	[HideInInspector]public float BottomSlatHingeDistance = 2.5f;
	[HideInInspector]public float SlatRootHeight;
	[HideInInspector]public float TopSlatHingeDistance = 2.5f;
	[HideInInspector]public float SlatTipHeight;
	[HideInInspector]public Vector3 SlataxisRotation; 
	[HideInInspector]public float slatControlInput;
	//
	[HideInInspector]public float slatrootChord;
	[HideInInspector]public float slattrailChord;
	[HideInInspector]public bool slatExtended;
	[HideInInspector]public float slatAngle;
	[HideInInspector]public float slatActuationTime;
	float targetSlatAngle = 0;
	[HideInInspector]public bool slatMoving;
	//

	float btslathingeDistance ;
	float tpslathingeDistance;
	private Vector3 slatRootHingePos = Vector3.zero;
	private Vector3 slatTipHingePos = Vector3.zero;
	//
	[HideInInspector]public GameObject SlatModel;
	//
	[HideInInspector]public bool negativeSlatRotation = false;
	public enum SlatRotationAxis
	{
		X,
		Y,
		Z
	}
	[HideInInspector]public SlatRotationAxis slatrotationAxis = SlatRotationAxis.X;
	[HideInInspector]public Vector3 slatAxisRotation;
	//
	//


	[HideInInspector]Rigidbody AirplaneBody;
	[HideInInspector]Vector3 aerodynamicCenter;
	//
	[HideInInspector]public float aerofoilArea;
	[HideInInspector]public float angleOfAttack;
	[HideInInspector]public float airDensity = 1.225f;
	[HideInInspector]public float trueAirSpeed;
	//
	[HideInInspector]public float TotalDrag;
	[HideInInspector]public float TotalLift;
	//
	[HideInInspector]public SilantroInstrumentation instrumentation;
	//
	[HideInInspector]float localscale;
	[HideInInspector]float rootScale;
	[HideInInspector]float actualTwist;
	[HideInInspector]public Vector3 relativeWind;
	//
	[HideInInspector]private BoxCollider aerofoilCollider;

	[HideInInspector]private Vector3 aerofoilRootLiftPosition = Vector3.zero;
	[HideInInspector]private Vector3 aerofoilTipLiftPosition = Vector3.zero;
	[HideInInspector]private float aerofoilLiftLineChordPosition = 0.75f;
	//
	public enum ControType
	{
		Stationary,
		Controllable
	}
	[HideInInspector]public ControType controlState = ControType.Controllable;
	//
	public enum SurfaceType
	{
		Elevator,
		Rudder,
		Aileron,
		Ruddervator,
		Elevon

	}
	[HideInInspector]public SurfaceType surfaceType = SurfaceType.Aileron;
	//
	[HideInInspector]public GameObject SurfaceModel;
	[HideInInspector]public bool activeControlSurface;
	//
	[HideInInspector]public bool negativeRotation = false;
	public enum RotationAxis
	{
		X,
		Y,
		Z
	}
	[HideInInspector]public RotationAxis rotationAxis = RotationAxis.X;
	[HideInInspector]public float maximumDeflection = 30;
	[HideInInspector]public float CurrentDeflection = 0.0f;
	[HideInInspector]public bool negativeDeflection = false;
	//
	[HideInInspector]public float BottomHingeDistance = 2.5f;
	[HideInInspector]public float rootHeight;
	[HideInInspector]public float TopHingeDistance = 2.5f;
	[HideInInspector]public float tipHeight;
	//
	[HideInInspector]public Vector3 axisRotation;
	[HideInInspector]public bool[] AffectedAerofoilSubdivisions;
	//
	[HideInInspector]public string ControlInput;
	[HideInInspector]public AnimationCurve controlCurve;
	[HideInInspector]public float flapAngle;
	//
	[HideInInspector]public float rootControlChord;
	[HideInInspector]public float trailControlChord;
	//
	[HideInInspector]private Vector3 surfaceRootHingePos = Vector3.zero;
	[HideInInspector]private Vector3 SurfaceTipHingePos = Vector3.zero;
	//
	[HideInInspector]private Quaternion InitialModelRotation = Quaternion.identity;
	[HideInInspector]private Quaternion InitialFlapModelRotation = Quaternion.identity;
	[HideInInspector]private Quaternion InitialSlatModelRotation = Quaternion.identity;
	[HideInInspector]private Quaternion InitialSpoilerModelRotation = Quaternion.identity;
	[HideInInspector]float bthingeDistance ;
	[HideInInspector]float tphingeDistance;
	[HideInInspector]public SilantroControls controlBoard;
	//
	[HideInInspector]public float startingHealth = 100.0f;		// The amount of health to start with
	[HideInInspector]public float currentHealth;
	//
	[HideInInspector]public SilantroAerofoil extraAerofoil;
	[HideInInspector]public GameObject[] attachments;
	//
	[HideInInspector]public GameObject[] weapons;
	[HideInInspector]public GameObject[] engines;
	//
	private bool destroyed;
	[HideInInspector]public bool Explode;
	[HideInInspector]public bool hasExtraAerofoil;[HideInInspector]public int CountAerofoils;
	[HideInInspector]public bool hasAttachedModels;[HideInInspector]public int CountModels;
	[HideInInspector]public bool hasWeapons;[HideInInspector]public int CountWeapons;
	[HideInInspector]public bool hasEngines;[HideInInspector]public int CountEngines;
	//
	[HideInInspector]public GameObject ExplosionPrefab;
	[HideInInspector]public Transform explosionPoint;
	[HideInInspector]public GameObject firePrefab;
	//
	[HideInInspector]public bool isDestructible = true;
	//
	[HideInInspector]public float surfaceInput;
	[HideInInspector]public bool isControllable = true;
	//
	//Combined Controls
	[HideInInspector]public float flaperonControlInput;
	[HideInInspector]public float ruddervatorControlInput;
	[HideInInspector]public float elevonControlInput;
	[HideInInspector]public float Controlmix;
	//
	//RUDDERVATOR
	[HideInInspector]public string rudderControl;
	[HideInInspector]public string elevatorControl;
	//
	[HideInInspector]public float rudderInput;
	[HideInInspector]public float elevatorInput;
	//
	//ELEVON
	[HideInInspector]public float aileronInput;
	[HideInInspector]public string aileronControl;
	//
	//MANUAL CONTROL
	[HideInInspector]public bool ManualControl = false;
	[HideInInspector]public float manualControlInput = 0f;
	//WEATHER SYSTEM
	[HideInInspector]public SilantroSapphire weather;

	// Use this for initialization
	void Start () {
		//
		//
		if (surfaceType == SurfaceType.Aileron) 
		{
			ControlInput = controlBoard.Aileron;
		} 
		else if (surfaceType == SurfaceType.Elevator) 
		{
			ControlInput = controlBoard.Elevator;
		} 
		else if (surfaceType == SurfaceType.Rudder) 
		{
			ControlInput = controlBoard.Rudder;
		}
		else if (surfaceType == SurfaceType.Ruddervator) 
		{
			rudderControl = controlBoard.Rudder;
			elevatorControl = controlBoard.Elevator;
		}
		else if (surfaceType == SurfaceType.Elevon) 
		{
			aileronControl = controlBoard.Aileron;
			elevatorControl = controlBoard.Elevator;
		}
		//
		increaseFlap = controlBoard.FlapIncrease;
		decreaseFlap = controlBoard.FlapDecrease;
		actuateSlat = controlBoard.SlatActuator;
		actuateSpoilers = controlBoard.SpoilerActuator;

		//
		currentHealth = startingHealth;
		//
		//
		if(controlState == ControType.Controllable && aerofoilType == AerofoilType.Wing){
		if(usesFlap || usesSlats){
		GameObject soundPoint = new GameObject();
		soundPoint.transform.parent = this.transform;
		soundPoint.transform.localPosition = new Vector3 (0, 0, 0);
		soundPoint.name = this.name +"Aerofoil Sound Point";
		//
		mechanicalSounds = soundPoint.gameObject.AddComponent<AudioSource>();
		mechanicalSounds.loop = false;
		//
			}
		}
		//
		if (airfoil == null) 
		{
			Debug.Log ("Airfoil for " + transform.name + " has not been assigned");
		}
		//
		aerofoilCollider = (BoxCollider)gameObject.GetComponent<Collider>();
		AirplaneBody = transform.root.gameObject.GetComponent<Rigidbody> ();
		//
		operationalFlapArea = totalFlapArea;
		operationalSlatArea = totalSlatArea;
		//INITIALIZE MAIN CONTROL SURFACE MODEL
		if (negativeRotation) {
			if (rotationAxis == RotationAxis.X) {
				axisRotation = new Vector3 (-1, 0, 0);
			} else if (rotationAxis == RotationAxis.Y) {
				axisRotation = new Vector3 (0, -1, 0);
			} else if (rotationAxis == RotationAxis.Z) {
				axisRotation = new Vector3 (0, 0, -1);
			}
		} else {
			if (rotationAxis == RotationAxis.X) {
				axisRotation = new Vector3 (1, 0, 0);
			} else if (rotationAxis == RotationAxis.Y) {
				axisRotation = new Vector3 (0, 1, 0);
			} else if (rotationAxis == RotationAxis.Z) {
				axisRotation = new Vector3 (0, 0, 1);
			}
		}

		axisRotation.Normalize();

		if ( null != SurfaceModel )
		{
			InitialModelRotation = SurfaceModel.transform.localRotation;
		}
		//
		//INITIALIZE FLAP CONTROL SURFACE MODEL
		if (negativeFlapRotation) {
			if (flaprotationAxis == FlapRotationAxis.X) {
				flapAxisRotation = new Vector3 (-1, 0, 0);
			} else if (flaprotationAxis == FlapRotationAxis.Y) {
				flapAxisRotation = new Vector3 (0, -1, 0);
			} else if (flaprotationAxis == FlapRotationAxis.Z) {
				flapAxisRotation = new Vector3 (0, 0, -1);
			}
		} else {
			if (flaprotationAxis == FlapRotationAxis.X) {
				flapAxisRotation = new Vector3 (1, 0, 0);
			} else if (flaprotationAxis == FlapRotationAxis.Y) {
				flapAxisRotation = new Vector3 (0, 1, 0);
			} else if (flaprotationAxis == FlapRotationAxis.Z) {
				flapAxisRotation = new Vector3 (0, 0, 1);
			}
		}

		flapAxisRotation.Normalize();

		if ( null != FlapModel )
		{
			InitialFlapModelRotation = FlapModel.transform.localRotation;
		}
		//
		//INITIALIZE SLAT CONTROL SURFACE MODEL
		if (negativeSlatRotation) {
			if (slatrotationAxis == SlatRotationAxis.X) {
				slatAxisRotation = new Vector3 (-1, 0, 0);
			} else if (slatrotationAxis == SlatRotationAxis.Y) {
				slatAxisRotation = new Vector3 (0, -1, 0);
			} else if (slatrotationAxis == SlatRotationAxis.Z) {
				slatAxisRotation = new Vector3 (0, 0, -1);
			}
		} else {
			if (slatrotationAxis == SlatRotationAxis.X) {
				slatAxisRotation = new Vector3 (1, 0, 0);
			} else if (slatrotationAxis == SlatRotationAxis.Y) {
				slatAxisRotation = new Vector3 (0, 1, 0);
			} else if (slatrotationAxis == SlatRotationAxis.Z) {
				slatAxisRotation = new Vector3 (0, 0, 1);
			}
		}

		slatAxisRotation.Normalize();

		if ( null != SlatModel )
		{
			InitialSlatModelRotation = SlatModel.transform.localRotation;
		}
		if (controlState == ControType.Controllable && usesFlap && aerofoilType == AerofoilType.Wing) {
			currentFlap = flapModes[flap];
		}
		//
		//INITIALIZE SPOILER CONTROL
		//INITIALIZE FLAP CONTROL SURFACE MODEL
		if (negativeSpoilerDeflection) {
			if (spoilerrotationAxis == SpoilerRotationAxis.X) {
				spoilerAxisRotation = new Vector3 (-1, 0, 0);
			} else if (spoilerrotationAxis == SpoilerRotationAxis.Y) {
				spoilerAxisRotation = new Vector3 (0, -1, 0);
			} else if (spoilerrotationAxis == SpoilerRotationAxis.Z) {
				spoilerAxisRotation = new Vector3 (0, 0, -1);
			}
		} else {
			if (spoilerrotationAxis == SpoilerRotationAxis.X) {
				spoilerAxisRotation = new Vector3 (1, 0, 0);
			} else if (spoilerrotationAxis == SpoilerRotationAxis.Y) {
				spoilerAxisRotation = new Vector3 (0, 1, 0);
			} else if (spoilerrotationAxis == SpoilerRotationAxis.Z) {
				spoilerAxisRotation = new Vector3 (0, 0, 1);
			}
		}

		spoilerAxisRotation.Normalize();

		if ( null != spoilerModel )
		{
			InitialSpoilerModelRotation = spoilerModel.transform.localRotation;
		}
		//
	}
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
		if (currentHealth == 0 && !destroyed)
			Disintegrate();
	}
	//
	//DESTRUCTION SYSTEM
	public void Disintegrate()
	{
		if(isDestructible){
			//
			if (transform.root.gameObject.GetComponent<Rigidbody> ()) {
				dropVelocity = transform.root.gameObject.GetComponent<Rigidbody> ().velocity;
			}
			//
			destroyed = true;
			//
			if (explosionPoint == null) {
				explosionPoint = this.transform;
			}
			//ACTIVATE EXPLOSION AND FIRE
			if (ExplosionPrefab != null) {
				GameObject explosion = Instantiate (ExplosionPrefab, explosionPoint.position, Quaternion.identity);
				explosion.SetActive (true);
				explosion.GetComponentInChildren<AudioSource> ().Play ();
			}	
			if (firePrefab != null) {
				GameObject fire = Instantiate (firePrefab, explosionPoint.position, Quaternion.identity);
				fire.SetActive (true);fire.transform.parent = gameObject.transform;fire.transform.localPosition = new Vector3 (0, 0, 0);
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
			//ADD COLLIDERS TO ATTACHED PARTS
			int a;
			if (attachments.Length > 0) {
				for (a = 0; a < attachments.Length; a++) {
					attachments [a].transform.parent = null;
					//Attach Box collider
					if (!attachments [a].GetComponent<BoxCollider> ()) {
						attachments [a].AddComponent<BoxCollider> ();
					}
					//Attach Rigibbody
					if (!attachments [a].GetComponent<Rigidbody> ()) {
						attachments [a].AddComponent<Rigidbody> ();
					}
					attachments [a].GetComponent<Rigidbody> ().mass = 300.0f;
					attachments [a].GetComponent<Rigidbody> ().velocity = dropVelocity;
				}
			}
			//
			if (extraAerofoil != null) {
				Destroy (extraAerofoil.gameObject);
			}
			//
			//DROP ANY ATTACHED WEAPON
			int d;
			if (weapons.Length > 0) {
				for (d = 0; d < weapons.Length; d++) {
					if (weapons [d] != null && weapons [d].GetComponent<Rigidbody> ()) {
						weapons [d].GetComponent<Rigidbody> ().isKinematic = false;
						weapons [d].GetComponent<Rigidbody> ().velocity = dropVelocity;//weapons [d].transform.root.gameObject.GetComponent<Rigidbody> ().velocity;
						weapons [d].transform.parent = null;
					}
				}
			}
			//

			//DESTROY ATTACHED ENGINES
			int e;
			if (engines.Length > 0) {
				for (e = 0; e < engines.Length; e++) {
					engines [e].SendMessage ("DestroyEngine", SendMessageOptions.DontRequireReceiver);
					engines [e].SendMessage ("Disintegrate", SendMessageOptions.DontRequireReceiver);

				}
			}
			//DESTROY GAMEOBJECT
			Destroy(gameObject);
		}
	}
	//
	void OnCollisionEnter(Collision col)
	{
		if (col.relativeVelocity.magnitude >100f) {
			Disintegrate ();
		}
	}
	//
	public void Update () 
	{
		//
		//
		if (Explode && !destroyed) {
			currentHealth = 0;
			Disintegrate ();
		}
		//
		if (isControllable) {
			//
			if (!ManualControl) { 
				//RUDDERVATOR CONTROLS
				if (surfaceType == SurfaceType.Ruddervator) {
					//
					rudderInput = Input.GetAxis (rudderControl);
					elevatorInput = Input.GetAxis (elevatorControl);
					//
					if (negativeDeflection) {
						elevatorInput *= -1f;
					}
					//
					//FIND AVERAGE OF AVAILABLE CONTROL INPUTS
					surfaceInput = Mathf.Lerp (surfaceInput, (((rudderInput*2f)+(elevatorInput*2f))/2f), 0.3f);
					//
					//
				}
				//
				//ELEVON CONTROL
				else if (surfaceType == SurfaceType.Elevon) {
					//
					aileronInput = Input.GetAxis (aileronControl);
					elevatorInput = Input.GetAxis (elevatorControl);
					//
					if (negativeDeflection) {
						elevatorInput *= -1f;
					}
					//
					//Debug.Log(elevatorInput+" ,"+aileronInput+" ,"+surfaceInput);
					//FIND AVERAGE OF AVAILABLE CONTROL INPUTS
					surfaceInput = Mathf.Lerp (surfaceInput, (((aileronInput*2f)+(elevatorInput*2f))/2f), 0.3f);
					//
				}
				else {
					surfaceInput = Input.GetAxis (ControlInput);
					if (negativeDeflection) {
						surfaceInput *= -1f;
					}
				}
				//
				if (controlState == ControType.Controllable && usesFlap && aerofoilType == AerofoilType.Wing) {
					//FLAP CONTROL SURFACE
					if (Input.GetButtonDown (increaseFlap)) {
						IncreaseFlap ();
						mechanicalSounds.clip = flapUp;
						if (flap < (flapModes.Length - 1)) {
							mechanicalSounds.Play ();
						}
					}
					//
					if (Input.GetButtonDown (decreaseFlap)) {
						DecreaseFlap ();
						mechanicalSounds.clip = flapDown;
						if (flap > 0) {
							mechanicalSounds.Play ();
						}
					}
					//
					flapAngle = Mathf.Lerp (flapAngle, currentFlap, Time.deltaTime * 0.7f);
					//
					if (negativeFlapDeflection) {
						CurrentFlapDeflection = -1f * flapAngle;
					} else {
						CurrentFlapDeflection = flapAngle;
					}

				}
				//
				//

				//SLAT CONTROL SURFACE
				if (controlState == ControType.Controllable && usesSlats && aerofoilType == AerofoilType.Wing) {
					if (Input.GetButtonDown (actuateSlat)) {
						if (!slatExtended) {
							targetSlatAngle = maximumSlatDeflection;
							StartCoroutine (ExtendSlats ());
						} else {
							targetSlatAngle = 0f;
							StartCoroutine (RetractSlats ());
						}
					}
					//
					//ACTUAL CONTROL
					if (negativeSlatDeflection) {
						CurrentSlatDeflection = -1f * slatAngle;
					} else {
						CurrentSlatDeflection = slatAngle;
					}
					//
					if (slatMoving) {
						slatAngle = Mathf.Lerp (slatAngle, targetSlatAngle, Time.deltaTime * 0.5f);
					}
				}
				//
				//SLAT CONTROL SURFACE
				if (controlState == ControType.Controllable && usesSpoilers && aerofoilType == AerofoilType.Wing) {
					if (Input.GetButtonDown (actuateSpoilers)) {
						if (!spoilerExtended) {
							targetSpoilerAngle = maximumSpoilerDeflection;
							StartCoroutine (ExtendSpoilers ());
						} else {
							targetSpoilerAngle = 0f;
							StartCoroutine (RetractSpoilers ());
						}
					}
					//
					currentSpoilerDeflection = spoilerAngle;
					//
					if (spoilerMoving) {
						spoilerAngle = Mathf.Lerp (spoilerAngle, targetSpoilerAngle, Time.deltaTime * 0.5f);
					}
				}
				if (activeControlSurface) {
					//BASE CONTROL SURFACE
					float curveValue = controlBoard.controlCurve.Evaluate (Mathf.Abs (surfaceInput));
					curveValue *= Mathf.Sign (surfaceInput);
					CurrentDeflection = curveValue * maximumDeflection;
				}
				//
				//APPLY ROTATIONS
				//1. MAIN CONTROL SURFACE
				if (null != SurfaceModel) {
					SurfaceModel.transform.localRotation = InitialModelRotation;
					SurfaceModel.transform.Rotate (axisRotation, CurrentDeflection);
				}
				//2. FLAP
				if (null != FlapModel) {
					FlapModel.transform.localRotation = InitialFlapModelRotation;
					FlapModel.transform.Rotate (flapAxisRotation, CurrentFlapDeflection);
				}
				//3. SLAT
				if (null != SlatModel) {
					SlatModel.transform.localRotation = InitialSlatModelRotation;
					SlatModel.transform.Rotate (slatAxisRotation, CurrentSlatDeflection);
				}
				//
				//4. SPOILER
				if (spoilerModel != null) {
					spoilerModel.transform.localRotation = InitialSpoilerModelRotation;
					spoilerModel.transform.Rotate (spoilerAxisRotation, currentSpoilerDeflection);
				}
				//
			}
		}
	}
	//
	//SLATS
	public IEnumerator ExtendSlats()
	{
		slatMoving = true;
		yield return new WaitForSeconds (slatActuationTime);
		slatExtended = true;slatMoving = false;
	}
	public IEnumerator RetractSlats()
	{
		slatMoving = true;
		yield return new WaitForSeconds (slatActuationTime);
		slatExtended = false;slatMoving = false;
	}
	//
	//
	//SPOILERS
	//
	public IEnumerator ExtendSpoilers()
	{
		spoilerMoving = true;
		yield return new WaitForSeconds (spoilerActuationTime);
		spoilerExtended = true;spoilerMoving = false;
	}
	public IEnumerator RetractSpoilers()
	{
		spoilerMoving = true;
		yield return new WaitForSeconds (spoilerActuationTime);
		spoilerExtended = false;spoilerMoving = false;
	}
	//
	public void IncreaseFlap()
	{
		flap += 1;
		//
		if (flap > (flapModes.Length - 1)) {
			flap = flapModes.Length-1;
		}
		currentFlap = flapModes [flap];
	}
	//
	public void DecreaseFlap()
	{
		flap -= 1;
		//
		if (flap < 0) {
			flap = 0;
		}
		currentFlap = flapModes [flap];
	}
	//
	void FixedUpdate () 
	{
		if(isControllable){
			//RUN AEROFOIL SIMULATION
			CalculateAerofoilStructure ();
			//
			for (int i = 0; i < AerofoilSubdivisions; i++) {		
				//
				//Trailing Edge of Aerofoil
				Vector3 trailingForwardChord = aerofoilRootLeadingEdge + ((aerofoilTipLeadingEdge - aerofoilRootLeadingEdge) * (float)i / (float)AerofoilSubdivisions);
				Vector3 trailingBackChord = aerofoilRootLeadingEdge + ((aerofoilTipLeadingEdge - aerofoilRootLeadingEdge) * (float)(i + 1) / (float)AerofoilSubdivisions);
				//Root Edge of Aerofoil
				Vector3 rootForwardChord = aerofoilRootTrailingEdge + ((aerofoilTipTrailingEdge - aerofoilRootTrailingEdge) * (float)(i + 1) / (float)AerofoilSubdivisions);
				Vector3 rootBackChord = aerofoilRootTrailingEdge + ((aerofoilTipTrailingEdge - aerofoilRootTrailingEdge) * (float)i / (float)AerofoilSubdivisions);
				//
				//
				//SEND DATA TO CONTROL SURFACE
				if (activeControlSurface) {
					CalculateControlStructure (i, ref trailingForwardChord, ref trailingBackChord, ref rootForwardChord, ref rootBackChord);
				}
				//SEND DATA TO FLAP
				if (aerofoilType == AerofoilType.Wing && usesFlap) {
					CalculateFlap (i, ref trailingForwardChord, ref trailingBackChord, ref rootForwardChord, ref rootBackChord);	
				}
				//
				//SEND DATA TO SLAT
				if (aerofoilType == AerofoilType.Wing && usesSlats) {
					CalculateSlat (i, ref trailingForwardChord, ref trailingBackChord, ref rootForwardChord, ref rootBackChord);	
				}
				//
				//SEND DATA TO SPOILER
				if (aerofoilType == AerofoilType.Wing && usesSpoilers) {
					CalculateSpoiler (i, ref trailingForwardChord, ref trailingBackChord, ref rootForwardChord, ref rootBackChord);
				}
				//
				Vector3 sectionRootLiftPosition = rootBackChord + ((trailingForwardChord - rootBackChord) * aerofoilLiftLineChordPosition);
				Vector3 sectionTipLiftPosition = rootForwardChord + ((trailingBackChord - rootForwardChord) * aerofoilLiftLineChordPosition);

				//
				aerodynamicCenter = sectionRootLiftPosition + ((sectionTipLiftPosition - sectionRootLiftPosition) * 0.5f);
				//
				//
				Vector3 chordLine = (trailingForwardChord + ((trailingBackChord - trailingForwardChord) * 0.5f)) - (rootBackChord + ((rootForwardChord - rootBackChord) * 0.5f));
				float chordLength = chordLine.magnitude;
				chordLine.Normalize ();
				//
				//
				if (AirplaneBody != null) {
					relativeWind = -AirplaneBody.velocity;
					//
					Vector3 fromCOMToAerodynamicCenter = aerodynamicCenter - AirplaneBody.worldCenterOfMass;
					Vector3 angularVelocity = AirplaneBody.angularVelocity;

					Vector3 localRelativeWind = Vector3.Cross (angularVelocity.normalized, fromCOMToAerodynamicCenter.normalized);
					localRelativeWind *= -((angularVelocity.magnitude) * fromCOMToAerodynamicCenter.magnitude);
					//Apply
					relativeWind += localRelativeWind;
			
					Vector3 correction = gameObject.transform.right;
					float perpChordDotRelativeWind = Vector3.Dot (correction, relativeWind);
					correction *= perpChordDotRelativeWind;
					relativeWind -= correction;
					//
					//Find the angle of attack.	
					Vector3 relativeWindNormalized = relativeWind.normalized;
					angleOfAttack = Vector3.Dot (chordLine, -relativeWindNormalized);
					angleOfAttack = Mathf.Clamp (angleOfAttack, -1.0f, 1.0f);
					angleOfAttack = Mathf.Acos (angleOfAttack);
					angleOfAttack *= Mathf.Rad2Deg;
					//
					Vector3 up = Vector3.Cross (chordLine, (sectionTipLiftPosition - sectionRootLiftPosition).normalized);
					up.Normalize ();

					if (transform.localScale.x < 0.0f) {
						up = -up;
					}

					float yAxisDotRelativeWind = Vector3.Dot (up, relativeWindNormalized);		
					if (yAxisDotRelativeWind < 0.0f) {
						angleOfAttack = -angleOfAttack;
					}
					//
					float totalLift = 0.0f;
					float totalDrag = 0.0f;
					float cM = 0.0f;
					//
					if (instrumentation != null) {
						airDensity = instrumentation.airDensity;
					}
					//
					if (null != airfoil) {
				
						liftCoefficent = airfoil.liftCurve.Evaluate (angleOfAttack);	
						float AerofoilArea = CalculateWingArea (trailingForwardChord, trailingBackChord, rootForwardChord, rootBackChord);
						float v = relativeWind.magnitude;
						totalLift = liftCoefficent * AerofoilArea * 0.5f * airDensity * (v * v);
						//
						dragCoefficient = airfoil.dragCurve.Evaluate (angleOfAttack);
						totalDrag = 0.5f * dragCoefficient * airDensity * (v * v) * AerofoilArea;
						cM = airfoil.momentCurve.Evaluate (angleOfAttack);
						//
						//CALCULATE CONTROL SURFACE DRAG
						if (usesFlap) {
							flapDrag = 1.50f * dragCoefficient * operationalFlapArea * airDensity * (v * v) * Mathf.Sin (CurrentFlapDeflection * Mathf.Deg2Rad);
						}
						if (usesSlats) {
							slatDrag = 1.50f * dragCoefficient * operationalSlatArea * airDensity * (v * v) * Mathf.Sin (CurrentSlatDeflection * Mathf.Deg2Rad);
						}
						//
					}
					trueAirSpeed = relativeWind.magnitude;
					//
					TotalLift = totalLift;
					//
					if (flapDrag < 0) {
						flapDrag *= -1f;
					}
					if (slatDrag < 0) {
						slatDrag *= -1f;
					}
					//SUM UP ALL DARG ON WING
					TotalDrag = totalDrag + flapDrag + slatDrag;
					//
					Vector3 liftForce = Vector3.Cross (gameObject.transform.right, relativeWind);
					liftForce.Normalize ();
					liftForce *= totalLift;
					Vector3 dragForce = relativeWind;
					dragForce.Normalize ();
					dragForce *= totalDrag;
					Vector3 liftDragPoint = aerodynamicCenter;
					//
					//Find wing pitching moment...
					float wingPitchingMoment = cM * chordLength * (0.5f * airDensity * (relativeWind.magnitude * relativeWind.magnitude)) * CalculateWingArea (trailingForwardChord, trailingBackChord, rootForwardChord, rootBackChord);
					Vector3 pitchAxis = Vector3.Cross (chordLine, liftForce.normalized);
					pitchAxis.Normalize ();
					pitchAxis *= wingPitchingMoment;
					//Apply forces.
					AirplaneBody.AddForceAtPosition (liftForce, liftDragPoint, ForceMode.Force);
					AirplaneBody.AddForceAtPosition (dragForce, liftDragPoint, ForceMode.Force);		
					AirplaneBody.AddTorque (pitchAxis, ForceMode.Force);
				} else {
					Debug.Log ("Parent rigidbody is missing!!!!");
				}
			}
		//
	}
}
	//
	private void ClampEditorValues()
	{
		//

		maximumDeflection = Mathf.Clamp( maximumDeflection, 0.0f, 90.0f );
		BottomHingeDistance = Mathf.Clamp( BottomHingeDistance, 0.0f, 100.0f );
		TopHingeDistance = Mathf.Clamp( TopHingeDistance, 0.0f, 100.0f );
		//
		//BASE CONTROL
		tipHeight = trailControlChord * TopHingeDistance / 100f;
		rootHeight = rootControlChord * BottomHingeDistance / 100f;
		if ( (null==AffectedAerofoilSubdivisions) || (AerofoilSubdivisions != AffectedAerofoilSubdivisions.Length) )
		{
			AffectedAerofoilSubdivisions = new bool[AerofoilSubdivisions];
		}
		//FLAP
		maximumFlapDeflection = Mathf.Clamp( maximumFlapDeflection, 0.0f, 90.0f );
		BottomFlapHingeDistance = Mathf.Clamp( BottomFlapHingeDistance, 0.0f, 100.0f );
		TopFlapHingeDistance = Mathf.Clamp( TopFlapHingeDistance, 0.0f, 100.0f );
		//
		FlapTipHeight = flaptrailChord * TopFlapHingeDistance / 100f;
		FlapRootHeight = flaprootChord * BottomFlapHingeDistance / 100f;
		if ( (null==AffectedFlapSubdivisions) || (AerofoilSubdivisions != AffectedFlapSubdivisions.Length) )
		{
			AffectedFlapSubdivisions = new bool[AerofoilSubdivisions];
		}
		//
		//SLAT
		maximumSlatDeflection = Mathf.Clamp( maximumSlatDeflection, 0.0f, 90.0f );
		BottomSlatHingeDistance = Mathf.Clamp( BottomSlatHingeDistance, 0.0f, 100.0f );
		TopSlatHingeDistance = Mathf.Clamp( TopSlatHingeDistance, 0.0f, 100.0f );
		//
		SlatTipHeight = slattrailChord * TopSlatHingeDistance / 100f;
		SlatRootHeight = slatrootChord * BottomSlatHingeDistance / 100f;
		if ( (null==AffectedSlatSubdivisions) || (AerofoilSubdivisions != AffectedSlatSubdivisions.Length) )
		{
			AffectedSlatSubdivisions = new bool[AerofoilSubdivisions];
		}
		//
		//SPOILER
		maximumSpoilerDeflection = Mathf.Clamp(maximumSpoilerDeflection,0.0f,90.0f);
		if ( (null==AffectedSpoilerSubdivisions) || (AerofoilSubdivisions != AffectedSpoilerSubdivisions.Length) )
		{
			AffectedSpoilerSubdivisions = new bool[AerofoilSubdivisions];
		}
	}
	//\
	//
	//PRIMARY CONTROL SURFACE
	public void CalculateControlStructure( int SectionIndex, ref Vector3 PointA, ref Vector3 PointB, ref Vector3 PointC, ref Vector3 PointD)
	{
		bthingeDistance = BottomHingeDistance / 100f;
		tphingeDistance = TopHingeDistance / 100f;


		if ( SectionIndex < AffectedAerofoilSubdivisions.Length )
		{
			if ( AffectedAerofoilSubdivisions[SectionIndex]==true)
			{

				surfaceRootHingePos = PointD + ( ( PointA - PointD ) * bthingeDistance );
				SurfaceTipHingePos = PointC + ( ( PointB - PointC ) * tphingeDistance );
				Vector3 aileronHinge = SurfaceTipHingePos - surfaceRootHingePos;

				Vector3 rootAileronAngle = PointD - surfaceRootHingePos;
				Vector3 tipAileronAngle = PointC - SurfaceTipHingePos;

				//Deflect surface.
				Quaternion hingeRotation = Quaternion.AngleAxis( CurrentDeflection, aileronHinge.normalized);
				rootAileronAngle = hingeRotation * rootAileronAngle;
				tipAileronAngle = hingeRotation * tipAileronAngle;

				//wing chord line.
				PointD = surfaceRootHingePos + rootAileronAngle;
				PointC = SurfaceTipHingePos + tipAileronAngle;
			}
		}
	}
	//
	//CALCULATE FLAP POINTS
	public void CalculateFlap( int SectionIndex, ref Vector3 PointA, ref Vector3 PointB, ref Vector3 PointC, ref Vector3 PointD)
	{
		btflaphingeDistance = BottomFlapHingeDistance / 100f;
		tpflaphingeDistance = TopFlapHingeDistance / 100f;

		if ( SectionIndex < AffectedFlapSubdivisions.Length )
		{
			if ( AffectedFlapSubdivisions[SectionIndex]==true)
			{

				flapRootHingePos = PointD + ( ( PointA - PointD ) * btflaphingeDistance );
				flapTipHingePos = PointC + ( ( PointB - PointC ) * tpflaphingeDistance );
				Vector3 FlapHinge = flapTipHingePos - flapRootHingePos;

				Vector3 rootFlapAngle = PointD - flapRootHingePos;
				Vector3 tipFlapAngle = PointC - flapTipHingePos;

				Quaternion hingeRotation = Quaternion.AngleAxis( CurrentFlapDeflection, FlapHinge.normalized);
				rootFlapAngle = hingeRotation * rootFlapAngle;
				tipFlapAngle = hingeRotation * tipFlapAngle;

				PointD = flapRootHingePos + rootFlapAngle;
				PointC = flapTipHingePos + tipFlapAngle;
			}
		}
	}
	//
	//CALCULATE SLATS
	public void CalculateSlat( int SectionIndex, ref Vector3 PointA, ref Vector3 PointB, ref Vector3 PointC, ref Vector3 PointD)
	{
		btslathingeDistance = BottomSlatHingeDistance / 100f;
		tpslathingeDistance = TopSlatHingeDistance / 100f;

		if ( SectionIndex < AffectedSlatSubdivisions.Length )
		{
			if ( AffectedSlatSubdivisions[SectionIndex]==true)
			{
				slatRootHingePos = PointB + ( ( PointA - PointD ) * btslathingeDistance );
				slatTipHingePos = PointA + ( ( PointB - PointC ) * tpslathingeDistance );
				Vector3 SlatHinge = slatTipHingePos - slatRootHingePos;

				Vector3 rootSlatAngle = PointB - slatRootHingePos;
				Vector3 tipSlatAngle = PointA - slatTipHingePos;

				Quaternion hingeRotation = Quaternion.AngleAxis( CurrentSlatDeflection, SlatHinge.normalized);
				rootSlatAngle = hingeRotation * rootSlatAngle;
				tipSlatAngle = hingeRotation * tipSlatAngle;

				PointB = slatRootHingePos + rootSlatAngle;
				PointA = slatTipHingePos + tipSlatAngle;
			}
		}
	}
	//
	//
	public void CalculateSpoiler( int spoilerIndex,  ref Vector3 PointA, ref Vector3 PointB, ref Vector3 PointC, ref Vector3 PointD)
	{
		if (spoilerIndex < AffectedSpoilerSubdivisions.Length) {
			
			if (AffectedSpoilerSubdivisions [spoilerIndex]) {	
				//
				float totalLiftSpoilt = 0.0f;
				float totalDragGenerated = 0.0f;
				//
				if (null != airfoil) {
					float AerofoilArea = CalculateWingArea (PointA, PointB, PointC, PointD);
					float v = relativeWind.magnitude;	
					float raiseFactor = currentSpoilerDeflection / maximumSpoilerDeflection;
					totalLiftSpoilt = liftCoefficent * AerofoilArea * 0.5f * airDensity * (v * v) * raiseFactor;
					//
					totalDragGenerated = 0.85f * dragCoefficient * airDensity * (v * v) * AerofoilArea * raiseFactor;
					//
				//	Debug.Log ("L: " + TotalLift);
					if (totalLiftSpoilt > 0) {
						TotalLift -= totalLiftSpoilt;
						Vector3 liftForce = Vector3.Cross (gameObject.transform.right, relativeWind);
						liftForce.Normalize ();
						liftForce *= -totalLiftSpoilt;
						//Vector3 liftDragPoint = aerodynamicCenter;
						//AirplaneBody.AddForce (liftForce, ForceMode.Force);
					}
					//
					//Debug.Log ("L: " + TotalLift + " l: " + totalLiftSpoilt);
					TotalDrag += totalDragGenerated;
				}
			}
				

		}
	}
	//
	public void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere (aerodynamicCenter, 0.15f);
		//
		ClampEditorValues();
		totalFlapArea = 0;
		totalSlatArea = 0;
		//
		aerofoilCollider = (BoxCollider)gameObject.GetComponent<Collider> ();
		if (null != aerofoilCollider) {
			
			aerofoilCollider.size = new Vector3 (1.0f, 0.1f, 1.0f);
			//
			//CALCULATE AEROFOIL STRUCTURE
			CalculateAerofoilStructure ();

			Gizmos.color = Color.yellow;
			Gizmos.DrawLine (aerofoilRootLeadingEdge, aerofoilTipLeadingEdge);

			Gizmos.color = Color.red;
			Gizmos.DrawLine (aerofoilTipTrailingEdge, aerofoilRootTrailingEdge);

			Gizmos.color = Color.yellow;
			Gizmos.DrawLine (aerofoilRootTrailingEdge, aerofoilRootLeadingEdge);
			Gizmos.DrawLine (aerofoilTipLeadingEdge, aerofoilTipTrailingEdge);
			//
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(aerofoilRootLeadingEdge,0.07f);
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(aerofoilRootTrailingEdge,0.07f);
			Gizmos.color = Color.grey;
			Gizmos.DrawSphere(aerofoilTipLeadingEdge,0.07f);
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(aerofoilTipTrailingEdge,0.07f);

			rootChord = Vector3.Distance(aerofoilRootLeadingEdge,aerofoilRootTrailingEdge);
			tipChord = Vector3.Distance (aerofoilTipLeadingEdge, aerofoilTipTrailingEdge);
			leadingEdgeLength = Vector3.Distance (aerofoilTipLeadingEdge, aerofoilRootLeadingEdge);
			trailingEdgeLength = Vector3.Distance (aerofoilTipTrailingEdge, aerofoilRootTrailingEdge);
			//
			//Sections.
			Gizmos.color = Color.yellow;
			for (int i = 0; i < AerofoilSubdivisions; i++) {
				Vector3 sectionStart = aerofoilRootTrailingEdge + ((aerofoilTipTrailingEdge - aerofoilRootTrailingEdge) * (float)i / (float)AerofoilSubdivisions);
				Vector3 sectionEnd = aerofoilRootLeadingEdge + ((aerofoilTipLeadingEdge - aerofoilRootLeadingEdge) * (float)i / (float)AerofoilSubdivisions);
				Gizmos.DrawLine (sectionStart, sectionEnd);
			}
			//
			//Lift line.
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine (aerofoilRootLiftPosition, aerofoilTipLiftPosition);

			//
			//Control Hinge'
			if (controlState == ControType.Controllable && activeControlSurface) {
				float rootHingeOffset = BottomHingeDistance / 100f;
				float tipHingeOffset = TopHingeDistance / 100f; 

				Vector3 aerofoilRootAileronHingePos = aerofoilRootTrailingEdge + ((aerofoilRootLeadingEdge - aerofoilRootTrailingEdge) * rootHingeOffset);
				Vector3 aerofoilTipAileronHingePos = aerofoilTipTrailingEdge + ((aerofoilTipLeadingEdge - aerofoilTipTrailingEdge) * tipHingeOffset);

				if (null != AffectedAerofoilSubdivisions) {
					for (int i = 0; i < AffectedAerofoilSubdivisions.Length; i++) {
						if (AffectedAerofoilSubdivisions [i] == true) {
							Vector3 hingeLeft = aerofoilRootAileronHingePos + ((aerofoilTipAileronHingePos - aerofoilRootAileronHingePos) * ((float)i / (float)AffectedAerofoilSubdivisions.Length));
							Vector3 hingeRight = aerofoilRootAileronHingePos + ((aerofoilTipAileronHingePos - aerofoilRootAileronHingePos) * ((float)(i + 1) / (float)AffectedAerofoilSubdivisions.Length));

							Vector3 backLeft = aerofoilRootTrailingEdge + ((aerofoilTipTrailingEdge - aerofoilRootTrailingEdge) * ((float)i / (float)AffectedAerofoilSubdivisions.Length));
							Vector3 backRight = aerofoilRootTrailingEdge + ((aerofoilTipTrailingEdge - aerofoilRootTrailingEdge) * ((float)(i + 1) / (float)AffectedAerofoilSubdivisions.Length));
							//
							rootControlChord = rootChord;
							trailControlChord = tipChord;
							//
							Gizmos.color = Color.gray;
							Gizmos.DrawLine( hingeLeft, backRight );
							Vector3[] vert = new Vector3[4];
							vert [0] = hingeLeft;
							vert [1] = hingeRight;
							vert [2] = backLeft;
							vert [3] = backRight;
							#if UNITY_EDITOR
							DrawControlHandles (vert,Color.red);
							#endif
						}
					}
				}
			}
				//
				//
				//Flap Control
			if (controlState == ControType.Controllable && usesFlap && aerofoilType == AerofoilType.Wing) {
				float rootFlapHingeOffset = BottomFlapHingeDistance/100f;
				float tipFlapHingeOffset = TopFlapHingeDistance/100f; 
				//
			//	float flapArea;
				//
				Vector3 flapRootAileronHingePos = aerofoilRootTrailingEdge + ( ( aerofoilRootLeadingEdge - aerofoilRootTrailingEdge ) * rootFlapHingeOffset );
				Vector3 flapTipAileronHingePos = aerofoilTipTrailingEdge + ( ( aerofoilTipLeadingEdge - aerofoilTipTrailingEdge ) * tipFlapHingeOffset);

				if ( null != AffectedFlapSubdivisions ) 
					{
					for ( int i=0; i<AffectedFlapSubdivisions.Length; i++ )
						{
						if ( AffectedFlapSubdivisions[i] == true )
							{
							Vector3 flaphingeLeft = flapRootAileronHingePos + ( (flapTipAileronHingePos - flapRootAileronHingePos ) * ((float)i / (float)AffectedFlapSubdivisions.Length) );
							Vector3 flaphingeRight = flapRootAileronHingePos + ( (flapTipAileronHingePos - flapRootAileronHingePos ) * ((float)(i+1) / (float)AffectedFlapSubdivisions.Length) );

							Vector3 backLeft = aerofoilRootTrailingEdge + ( (aerofoilTipTrailingEdge - aerofoilRootTrailingEdge ) * ((float)i / (float)AffectedFlapSubdivisions.Length) );
							Vector3 backRight = aerofoilRootTrailingEdge + ( (aerofoilTipTrailingEdge - aerofoilRootTrailingEdge ) * ((float)(i+1) / (float)AffectedFlapSubdivisions.Length) );
							//
							flaprootChord = rootChord;
							flaptrailChord = tipChord;

							//Draw Lines to denote affected area
							Gizmos.color = Color.gray;
							Gizmos.DrawLine( flaphingeLeft, backRight );
							//
							//CALCULATE INDIVIDUAL FLAP AREA
							float y = Vector3.Distance(flaphingeLeft,flaphingeRight);
							float x = Vector3.Distance (flaphingeLeft, backLeft);
							float z = Vector3.Distance (flaphingeRight, backRight);
							float w = Vector3.Distance (backLeft, backRight); 
							//
							float trapezAngle = (90f-sweepangle);
							float multiplier = Mathf.Sin (trapezAngle*Mathf.Deg2Rad);
							//
							float h = x*multiplier;
							float area = 0.5f * ((y + w) * h);
							totalFlapArea += area;
							//
							Vector3[] vert = new Vector3[4];
							vert [0] = flaphingeLeft;
							vert [1] = flaphingeRight;
							vert [2] = backLeft;
							vert [3] = backRight;
							#if UNITY_EDITOR
							DrawControlHandles (vert,Color.blue);
							#endif
							}
						}
					}
			}
			//
			//
			//
			//Slat Control
			if (controlState == ControType.Controllable && usesSlats && aerofoilType == AerofoilType.Wing) {
				float rootSlatHingeOffset = BottomSlatHingeDistance / 100f;
				float tipSlatHingeOffset = TopSlatHingeDistance / 100f; 

				Vector3 slatRootAileronHingePos = aerofoilRootLeadingEdge - ((aerofoilRootLeadingEdge - aerofoilRootTrailingEdge) * rootSlatHingeOffset);
				Vector3 slatTipAileronHingePos = aerofoilTipLeadingEdge - ((aerofoilTipLeadingEdge - aerofoilTipTrailingEdge) * tipSlatHingeOffset

				);

				if (null != AffectedSlatSubdivisions) {
					for (int i = 0; i < AffectedSlatSubdivisions.Length; i++) {
						if (AffectedSlatSubdivisions [i] == true) {
							Vector3 slathingeLeft = slatRootAileronHingePos + ((slatTipAileronHingePos - slatRootAileronHingePos) * ((float)i / (float)AffectedSlatSubdivisions.Length));
							Vector3 slathingeRight = slatRootAileronHingePos + ((slatTipAileronHingePos - slatRootAileronHingePos) * ((float)(i + 1) / (float)AffectedSlatSubdivisions.Length));

							Vector3 backLeft = aerofoilRootLeadingEdge + ((aerofoilTipLeadingEdge - aerofoilRootLeadingEdge) * ((float)i / (float)AffectedSlatSubdivisions.Length));
							Vector3 backRight = aerofoilRootLeadingEdge + ((aerofoilTipLeadingEdge - aerofoilRootLeadingEdge) * ((float)(i + 1) / (float)AffectedSlatSubdivisions.Length));
							//
							slatrootChord = rootChord;
							slattrailChord = tipChord;

							//Draw Lines to denote affected area
							Gizmos.color = Color.gray;
							Gizmos.DrawLine (slathingeLeft, backRight);
							//
							//CALCULATE INDIVIDUAL FLAP AREA
							float y = Vector3.Distance(slathingeLeft,slathingeRight);
							float x = Vector3.Distance (slathingeLeft, backLeft);
							float z = Vector3.Distance (slathingeRight, backRight);
							float w = Vector3.Distance (backLeft, backRight);
							//
							float trapezAngle = (90f-sweepangle);
							float multiplier = Mathf.Sin (trapezAngle*Mathf.Deg2Rad);
							//
							float h = x*multiplier;
							float area = 0.5f * ((y + w) * h);
							totalSlatArea += area;
							//
							Vector3[] vert = new Vector3[4];
							vert [0] = slathingeLeft;
							vert [1] = slathingeRight;
							vert [2] = backLeft;
							vert [3] = backRight;
							#if UNITY_EDITOR
							DrawControlHandles (vert, Color.magenta);
							#endif
						}
					}
				}
			}
		}
	}

	//
	void CalculateAerofoilStructure()
	{
		//Calculate root and tip center points.
		Vector3 wingRootCenter = transform.position - ( transform.right * (transform.root.localScale.x*transform.localScale.x * 0.5f) );
		Vector3 wingTipCenter = transform.position + ( transform.right * (transform.root.localScale.x*transform.localScale.x * 0.5f) );
		//
		if (sweepDirection == SweepDirection.Unswept) {
			sweepangle = aerofoilSweepAngle = 0;
		}
		else if (sweepDirection == SweepDirection.Forward)
		{
			sweepangle = aerofoilSweepAngle;
		} else if (sweepDirection == SweepDirection.Backward)
		{
			sweepangle = -aerofoilSweepAngle;
		}
		//
		localscale = transform.localScale.magnitude;
		rootScale = transform.root.localScale.magnitude;
		//
		wingTipCenter += transform.forward * (sweepangle/90) *rootScale*localscale;

			//Calculate corners.
		aerofoilRootLeadingEdge = wingRootCenter + ( transform.forward * (transform.root.localScale.z*transform.localScale.z * 0.5f) );
		aerofoilRootTrailingEdge = wingRootCenter - ( transform.forward * (transform.root.localScale.z*transform.localScale.z * 0.5f) );
		aerofoilTipLeadingEdge = wingTipCenter + ( transform.forward * ((transform.root.localScale.z*transform.localScale.z * 0.5f) *AerofoilTipWidth/100f) );
		aerofoilTipTrailingEdge = wingTipCenter - ( transform.forward * ((transform.root.localScale.z*transform.localScale.z * 0.5f) * AerofoilTipWidth /100f) );


			//Tweak tip corners based on the angle between them.
		Vector3 tipTrailingEdgeToTipLeadingEdge = aerofoilTipLeadingEdge - aerofoilTipTrailingEdge;
		//
	
		if (twistDirection == TwistDirection.Untwisted) {
			actualTwist = twistAngle = 0;
		} else if (twistDirection == TwistDirection.Downwards) {
			actualTwist = twistAngle;
		} else if (twistDirection == TwistDirection.Upwards) {
			actualTwist = -twistAngle;
		}
		//
		Quaternion rotation = Quaternion.AngleAxis( actualTwist, transform.rotation * new Vector3( 1.0f, 0.0f, 0.0f ));
			tipTrailingEdgeToTipLeadingEdge = rotation * tipTrailingEdgeToTipLeadingEdge;
		aerofoilTipTrailingEdge = wingTipCenter - (tipTrailingEdgeToTipLeadingEdge * 0.5f);
		aerofoilTipLeadingEdge = wingTipCenter + (tipTrailingEdgeToTipLeadingEdge * 0.5f);

		aerofoilRootLiftPosition = aerofoilRootTrailingEdge + ( ( aerofoilRootLeadingEdge - aerofoilRootTrailingEdge ) * aerofoilLiftLineChordPosition );
		aerofoilTipLiftPosition = aerofoilTipTrailingEdge + ( ( aerofoilTipLeadingEdge - aerofoilTipTrailingEdge ) * aerofoilLiftLineChordPosition );

			//Calculate wing area.
		aerofoilArea = CalculateWingArea( aerofoilRootLeadingEdge, aerofoilTipLeadingEdge, aerofoilTipTrailingEdge, aerofoilRootTrailingEdge );

	}
	//
	//CALCULATE RECTANGULAR AREA
	private float CalculateWingArea( Vector3 pointA, Vector3 pointB, Vector3 pointC, Vector3 pointD )
	{
		float ab = (pointB - pointA).magnitude;
		float bc = (pointC - pointB).magnitude;
		float cd = (pointD - pointC).magnitude;
		float da = (pointA - pointD).magnitude;

		float s = ( ab + bc + cd + da ) * 0.5f;
		float squareArea = (s-ab) * (s-bc) * (s-cd) * (s-da);
		float area = Mathf.Sqrt( squareArea );
		//
		aerofoilArea = area;
		return area;

	}
	//
	#if UNITY_EDITOR
	void DrawControlHandles(Vector3[] vectar,Color colar)
	{
		Handles.DrawSolidRectangleWithOutline (vectar,colar,colar);
	}
	#endif
}
//
//
//
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroAerofoil))]
public class AerofoilEditor: Editor
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
		serializedObject.Update();
		//
		SilantroAerofoil aerofoil = (SilantroAerofoil)target;
		//
		GUILayout.Space(3f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Aerofoil Type", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		aerofoil.aerofoilType = (SilantroAerofoil.AerofoilType)EditorGUILayout.EnumPopup("Aerofoil Type",aerofoil.aerofoilType);
		//
		GUILayout.Space(10f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Airfoil Component", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		aerofoil.airfoil = EditorGUILayout.ObjectField ("Airfoil", aerofoil.airfoil, typeof(SilantroAirfoil), true) as SilantroAirfoil;
		GUILayout.Space(2f);
		EditorGUILayout.LabelField ("Lift Coefficient", aerofoil.liftCoefficent.ToString("0.000"));
		GUILayout.Space(1f);
		EditorGUILayout.LabelField ("Drag Coefficient", aerofoil.dragCoefficient.ToString("0.000"));
		//
		GUILayout.Space(20f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Aerofoil Configuration", MessageType.None);
		GUI.color = backgroundColor;
		//
		GUILayout.Space(7f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Sweep", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		aerofoil.sweepDirection = (SilantroAerofoil.SweepDirection)EditorGUILayout.EnumPopup ("Sweep Direction", aerofoil.sweepDirection);
		GUILayout.Space(2f);
		aerofoil.aerofoilSweepAngle = EditorGUILayout.Slider ("Sweep Angle", aerofoil.aerofoilSweepAngle,0f,90f);
		//
		GUILayout.Space(10f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Twist", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		aerofoil.twistDirection = (SilantroAerofoil.TwistDirection)EditorGUILayout.EnumPopup ("Twist Direction", aerofoil.twistDirection);
		GUILayout.Space(2f);
		aerofoil.twistAngle = EditorGUILayout.Slider ("Twist Angle", aerofoil.twistAngle,0f,90f);
		//
		GUILayout.Space(10f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Structure", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		aerofoil.AerofoilTipWidth = EditorGUILayout.Slider ("Tip Width", aerofoil.AerofoilTipWidth, 0f, 100f);
		GUILayout.Space(5f);
		aerofoil.AerofoilSubdivisions = EditorGUILayout.IntSlider ("Subdivisions", aerofoil.AerofoilSubdivisions,0,15);
		//
		//
		GUILayout.Space(25f);

		//
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Aerofoil Dimensions", MessageType.None);
		GUI.color = backgroundColor;
		//
		GUILayout.Space(1f);
		EditorGUILayout.LabelField("Root Chord",aerofoil.rootChord.ToString("0.00") + " m");
		GUILayout.Space(1f);
		EditorGUILayout.LabelField ("Tip Chord", aerofoil.tipChord.ToString ("0.00") + " m");
		GUILayout.Space(1f);
		EditorGUILayout.LabelField ("Leading Edge", aerofoil.leadingEdgeLength.ToString ("0.00") + " m");
		GUILayout.Space(1f);
		EditorGUILayout.LabelField ("Trailing Edge", aerofoil.trailingEdgeLength.ToString ("0.00") + " m");
		//
		GUILayout.Space(5f);
		EditorGUILayout.LabelField ("Aerofoil Area", aerofoil.aerofoilArea.ToString ("0.00") + " m2");
		//
		GUILayout.Space(25f);
		aerofoil.controlState = (SilantroAerofoil.ControType)EditorGUILayout.EnumPopup ("Configuration", aerofoil.controlState);
		GUILayout.Space (10f);
		if (aerofoil.controlState == SilantroAerofoil.ControType.Controllable) {
			GUI.color = Color.red;
			EditorGUILayout.HelpBox ("Primary Control Surface", MessageType.None);
			GUI.color = backgroundColor;
			//
			GUILayout.Space (3f);
			aerofoil.activeControlSurface = EditorGUILayout.Toggle ("Active", aerofoil.activeControlSurface);
			if(aerofoil.activeControlSurface){
			GUILayout.Space (10f);
			GUI.color = Color.white;
			EditorGUILayout.HelpBox ("Control Type", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (3f);
			aerofoil.surfaceType = (SilantroAerofoil.SurfaceType)EditorGUILayout.EnumPopup ("Surface Type", aerofoil.surfaceType);
			//
			GUILayout.Space (20f);
			GUI.color = Color.white;
			EditorGUILayout.HelpBox ("Model", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (5f);
			aerofoil.SurfaceModel = EditorGUILayout.ObjectField ("Surface Model", aerofoil.SurfaceModel, typeof(GameObject), true) as GameObject;
			//
			EditorGUI.indentLevel++;
			//
			GUI.color = Color.white;
			EditorGUILayout.HelpBox ("Rotation Settings", MessageType.None);
			GUI.color = backgroundColor;
			//
			GUILayout.Space (2f);
			aerofoil.rotationAxis = (SilantroAerofoil.RotationAxis)EditorGUILayout.EnumPopup ("Rotation Axis", aerofoil.rotationAxis);
			GUILayout.Space (4f);
			aerofoil.negativeRotation = EditorGUILayout.Toggle ("Negative Rotation", aerofoil.negativeRotation);
			//
			EditorGUI.indentLevel--;
			GUILayout.Space (20f);
			GUI.color = Color.white;
			EditorGUILayout.HelpBox ("Deflection Settings", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (4f);
			aerofoil.maximumDeflection = EditorGUILayout.FloatField ("Maximum Deflection", aerofoil.maximumDeflection);
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Current Deflection", aerofoil.CurrentDeflection.ToString ("0.0") + " °");
			GUILayout.Space (3f);
			aerofoil.negativeDeflection = EditorGUILayout.Toggle ("Negative Deflection", aerofoil.negativeDeflection);
			//
			GUILayout.Space (20f);
			GUI.color = Color.white;
			EditorGUILayout.HelpBox ("Surface Size", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (4f);
			aerofoil.BottomHingeDistance = EditorGUILayout.Slider ("Bottom Hinge", aerofoil.BottomHingeDistance, 0f, 100f);
			GUILayout.Space (2f);
			EditorGUILayout.LabelField ("Root Height", aerofoil.rootHeight.ToString ("0.0") + " m");
			GUILayout.Space (3f);
			aerofoil.TopHingeDistance = EditorGUILayout.Slider ("Top Hinge", aerofoil.TopHingeDistance, 0f, 100f);
			GUILayout.Space (2f);
			EditorGUILayout.LabelField("Tip Height",aerofoil.tipHeight.ToString ("0.0") + " m");
			//
			GUILayout.Space (20f);
			GUI.color = Color.white;
			EditorGUILayout.HelpBox ("Affected Subdivisions", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (4f);
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.BeginVertical ();
			//
			SerializedProperty bools = this.serializedObject.FindProperty("AffectedAerofoilSubdivisions");
			//
			for (int i = 0; i < bools.arraySize; i++) {
				GUIContent label = new GUIContent("Section: " +(i+1).ToString ());
				EditorGUILayout.PropertyField (bools.GetArrayElementAtIndex (i), label);
			}
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.EndVertical ();
			//
			}
			//
			//
			//EXTRA SURFACES
			GUILayout.Space(20f);
			if (aerofoil.aerofoilType == SilantroAerofoil.AerofoilType.Wing) {
				//
				GUI.color = silantroColor;
				EditorGUILayout.HelpBox ("Secondary Control Surfaces", MessageType.None);
				GUI.color = backgroundColor;
				GUILayout.Space (10f);
				aerofoil.toolbarTab = GUILayout.Toolbar (aerofoil.toolbarTab, new string[]{ "Flap", "Slat","Spoilers" });
				//
				switch (aerofoil.toolbarTab) {
				case 0:
					//
					aerofoil.currentTab = "Flap";
					break;
				case 1:
					//
					aerofoil.currentTab = "Slat";
					break;
				
				case 2:
				//
					aerofoil.currentTab = "Spoilers";
				break;
				}
				//
				switch (aerofoil.currentTab) {
				case "Flap":
					GUILayout.Space (5f);
					GUI.color = Color.blue;
					EditorGUILayout.HelpBox ("Flap Settings", MessageType.None);
					GUI.color = backgroundColor;
					GUILayout.Space (5f);
					aerofoil.usesFlap = EditorGUILayout.Toggle ("Active", aerofoil.usesFlap);
					GUILayout.Space (5f);
					if(aerofoil.usesFlap){
					//
					GUILayout.Space (3f);
						GUI.color = Color.white;
						EditorGUILayout.HelpBox ("Flap Angle Settings", MessageType.None);
						GUI.color = backgroundColor;
						GUILayout.Space (3f);
						//
						GUILayout.Space (5f);
						EditorGUILayout.BeginHorizontal ();
						EditorGUILayout.BeginVertical ();
						//
						GUIContent anglelabel = new GUIContent("Available Angles");
						SerializedProperty angle = this.serializedObject.FindProperty ("flapModes");
						EditorGUILayout.PropertyField (angle.FindPropertyRelative ("Array.size"),anglelabel);
						GUILayout.Space (5f);
						for (int i = 0; i < angle.arraySize; i++) {
							GUIContent label = new GUIContent ("Angle " + (i + 1).ToString ());
							EditorGUILayout.PropertyField (angle.GetArrayElementAtIndex (i), label);
						}
						EditorGUILayout.EndHorizontal ();
						EditorGUILayout.EndVertical ();
						//
					GUILayout.Space (5f);
					EditorGUILayout.LabelField ("Current Deflection", aerofoil.CurrentFlapDeflection.ToString ("0.0") + " °");
					GUILayout.Space (3f);
					aerofoil.negativeFlapDeflection = EditorGUILayout.Toggle ("Negative Deflection", aerofoil.negativeFlapDeflection);
					//
						GUILayout.Space (20f);
						GUI.color = Color.white;
						EditorGUILayout.HelpBox ("Model", MessageType.None);
						GUI.color = backgroundColor;
						GUILayout.Space (5f);
						aerofoil.FlapModel = EditorGUILayout.ObjectField ("Flap Model", aerofoil.FlapModel, typeof(GameObject), true) as GameObject;
						//
						EditorGUI.indentLevel++;
						//
						GUI.color = Color.white;
						EditorGUILayout.HelpBox ("Rotation Settings", MessageType.None);
						GUI.color = backgroundColor;
						//
						GUILayout.Space (2f);
						aerofoil.flaprotationAxis = (SilantroAerofoil.FlapRotationAxis)EditorGUILayout.EnumPopup ("Rotation Axis", aerofoil.flaprotationAxis);
						GUILayout.Space (4f);
						aerofoil.negativeFlapRotation = EditorGUILayout.Toggle ("Negative Rotation", aerofoil.negativeFlapRotation);
						//
						EditorGUI.indentLevel--;
						//
					GUILayout.Space (20f);
					GUI.color = Color.white;
					EditorGUILayout.HelpBox ("Flap Size", MessageType.None);
					GUI.color = backgroundColor;
					GUILayout.Space (4f);
					aerofoil.BottomFlapHingeDistance = EditorGUILayout.Slider ("Bottom Flap Hinge", aerofoil.BottomFlapHingeDistance, 0f, 100f);
					GUILayout.Space (2f);
					EditorGUILayout.LabelField ("Root Flap Height", aerofoil.FlapRootHeight.ToString ("0.0") + " m");
					GUILayout.Space (3f);
					aerofoil.TopFlapHingeDistance = EditorGUILayout.Slider ("Top Flap Hinge", aerofoil.TopFlapHingeDistance, 0f, 100f);
					GUILayout.Space (2f);
					EditorGUILayout.LabelField("Tip Height",aerofoil.FlapTipHeight.ToString ("0.0") + " m");
					GUILayout.Space (4f);
					EditorGUILayout.LabelField ("Flap Area", aerofoil.totalFlapArea.ToString("0.0")+ " m2");
					GUILayout.Space (4f);
					EditorGUILayout.LabelField ("Flap Drag", aerofoil.flapDrag.ToString("0.0")+ " N");
					//
					GUILayout.Space (20f);
					GUI.color = Color.white;
					EditorGUILayout.HelpBox ("Flap Subdivisions", MessageType.None);
					GUI.color = backgroundColor;
					GUILayout.Space (4f);
					EditorGUILayout.BeginHorizontal ();
					EditorGUILayout.BeginVertical ();
					//
					SerializedProperty flapbools = this.serializedObject.FindProperty("AffectedFlapSubdivisions");
					//
					for (int i = 0; i < flapbools.arraySize; i++) {
						GUIContent label = new GUIContent("Aerofoil Section: " +(i+1).ToString ());
						EditorGUILayout.PropertyField (flapbools.GetArrayElementAtIndex (i), label);
				}
					EditorGUILayout.EndHorizontal ();
					EditorGUILayout.EndVertical ();
					//
					GUILayout.Space(10f);
					GUI.color = Color.white;
					EditorGUILayout.HelpBox ("Flap Sounds", MessageType.None);
					GUI.color = backgroundColor;
						GUILayout.Space (3f);
					aerofoil.flapDown = EditorGUILayout.ObjectField ("Flap Down", aerofoil.flapDown,typeof(AudioClip),true) as AudioClip;
						GUILayout.Space(2f);
					aerofoil.flapUp = EditorGUILayout.ObjectField ("Flap Up", aerofoil.flapUp,typeof(AudioClip),true) as AudioClip;
				}
					break;
					//
				case "Slat":
					GUILayout.Space (5f);
					GUI.color = Color.magenta;
					EditorGUILayout.HelpBox ("Slat Settings", MessageType.None);
					GUI.color = backgroundColor;
					GUILayout.Space (5f);
					aerofoil.usesSlats = EditorGUILayout.Toggle ("Active", aerofoil.usesSlats);
					if (aerofoil.usesSlats) {
						GUILayout.Space (5f);
						GUI.color = Color.white;
						EditorGUILayout.HelpBox ("Deflection Settings", MessageType.None);
						GUI.color = backgroundColor;
						GUILayout.Space (4f);
						aerofoil.maximumSlatDeflection = EditorGUILayout.FloatField ("Maximum Deflection", aerofoil.maximumSlatDeflection);
						GUILayout.Space (3f);
						EditorGUILayout.LabelField ("Current Deflection", aerofoil.CurrentSlatDeflection.ToString ("0.0") + " °");
						GUILayout.Space (3f);
						aerofoil.negativeSlatDeflection = EditorGUILayout.Toggle ("Negative Deflection", aerofoil.negativeSlatDeflection);
						GUILayout.Space (4f);
						aerofoil.slatActuationTime = EditorGUILayout.FloatField ("Actuation Time", aerofoil.slatActuationTime);
						//
						GUILayout.Space (20f);
						GUI.color = Color.white;
						EditorGUILayout.HelpBox ("Model", MessageType.None);
						GUI.color = backgroundColor;
						GUILayout.Space (5f);
						aerofoil.SlatModel = EditorGUILayout.ObjectField ("Slat Model", aerofoil.SlatModel, typeof(GameObject), true) as GameObject;
						//
						EditorGUI.indentLevel++;
						//
						GUI.color = Color.white;
						EditorGUILayout.HelpBox ("Rotation Settings", MessageType.None);
						GUI.color = backgroundColor;
						//
						GUILayout.Space (2f);
						aerofoil.slatrotationAxis = (SilantroAerofoil.SlatRotationAxis)EditorGUILayout.EnumPopup ("Rotation Axis", aerofoil.slatrotationAxis);
						GUILayout.Space (4f);
						aerofoil.negativeSlatRotation = EditorGUILayout.Toggle ("Negative Rotation", aerofoil.negativeSlatRotation);
						//
						EditorGUI.indentLevel--;
						//
						GUILayout.Space (20f);
						GUI.color = Color.white;
						EditorGUILayout.HelpBox ("Slat Size", MessageType.None);
						GUI.color = backgroundColor;
						GUILayout.Space (4f);
						aerofoil.BottomSlatHingeDistance = EditorGUILayout.Slider ("Bottom Slat Hinge", aerofoil.BottomSlatHingeDistance, 0f, 100f);
						GUILayout.Space (2f);
						EditorGUILayout.LabelField ("Root Slat Height", aerofoil.SlatRootHeight.ToString ("0.0") + " m");
						GUILayout.Space (3f);
						aerofoil.TopSlatHingeDistance = EditorGUILayout.Slider ("Top Slat Hinge", aerofoil.TopSlatHingeDistance, 0f, 100f);
						GUILayout.Space (2f);
						EditorGUILayout.LabelField ("Tip Height", aerofoil.SlatTipHeight.ToString ("0.0") + " m");
						//
						GUILayout.Space (4f);
						EditorGUILayout.LabelField ("Slat Area", aerofoil.totalSlatArea.ToString("0.0")+ " m2");
						GUILayout.Space (4f);
						EditorGUILayout.LabelField ("Slat Drag", aerofoil.slatDrag.ToString("0.0")+ " N");

						//
						GUILayout.Space (20f);
						GUI.color = Color.white;
						EditorGUILayout.HelpBox ("Slat Subdivisions", MessageType.None);
						GUI.color = backgroundColor;
						GUILayout.Space (4f);
						EditorGUILayout.BeginHorizontal ();
						EditorGUILayout.BeginVertical ();
						//
						SerializedProperty slatbools = this.serializedObject.FindProperty ("AffectedSlatSubdivisions");
						//EditorGUILayout.PropertyField (bools.FindPropertyRelative ("Array.size"));
						//
						for (int i = 0; i < slatbools.arraySize; i++) {
							GUIContent label = new GUIContent ("Aerofoil Section: " + (i + 1).ToString ());
							EditorGUILayout.PropertyField (slatbools.GetArrayElementAtIndex (i), label);
						}
						EditorGUILayout.EndHorizontal ();
						EditorGUILayout.EndVertical ();
					}
						//
					break;
					//
					//
				case "Spoilers":
					
					GUILayout.Space (5f);
					GUI.color = Color.yellow;
					EditorGUILayout.HelpBox ("Spoiler Settings", MessageType.None);
					GUI.color = backgroundColor;
					GUILayout.Space (5f);
					aerofoil.usesSpoilers = EditorGUILayout.Toggle ("Active", aerofoil.usesSpoilers);
					if (aerofoil.usesSpoilers) {
						//
						GUILayout.Space (5f);
						GUI.color = Color.white;
						EditorGUILayout.HelpBox ("Deflection Settings", MessageType.None);
						GUI.color = backgroundColor;
						GUILayout.Space (3f);
						aerofoil.customSpoilerDeflection = EditorGUILayout.Toggle ("Advanced Settings", aerofoil.customSpoilerDeflection);
						if (aerofoil.customSpoilerDeflection) {
							GUILayout.Space (4f);
							aerofoil.maximumSpoilerDeflection = EditorGUILayout.FloatField ("Maximum Deflection", aerofoil.maximumSpoilerDeflection);
							GUILayout.Space (4f);
							aerofoil.spoilerActuationTime = EditorGUILayout.FloatField ("Actuation Time", aerofoil.spoilerActuationTime);
						
						}
						//
						GUILayout.Space (10f);
						EditorGUILayout.LabelField ("Current Deflection", aerofoil.currentSpoilerDeflection.ToString ("0.0") + " °");
						//
						GUILayout.Space (20f);
						GUI.color = Color.white;
						EditorGUILayout.HelpBox ("Model", MessageType.None);
						GUI.color = backgroundColor;
						GUILayout.Space (5f);
						aerofoil.spoilerModel = EditorGUILayout.ObjectField ("Spoiler Model", aerofoil.spoilerModel, typeof(GameObject), true) as GameObject;
						//
						EditorGUI.indentLevel++;
						//
						GUI.color = Color.white;
						EditorGUILayout.HelpBox ("Rotation Settings", MessageType.None);
						GUI.color = backgroundColor;
						//
						GUILayout.Space (2f);
						aerofoil.spoilerrotationAxis = (SilantroAerofoil.SpoilerRotationAxis)EditorGUILayout.EnumPopup ("Rotation Axis", aerofoil.spoilerrotationAxis);
						GUILayout.Space (3f);
						aerofoil.negativeSpoilerDeflection = EditorGUILayout.Toggle ("Negative Deflection", aerofoil.negativeSpoilerDeflection);

						//
						EditorGUI.indentLevel--;
						//
						GUILayout.Space (20f);
						GUI.color = Color.white;
						EditorGUILayout.HelpBox ("Spoiler Subdivisions", MessageType.None);
						GUI.color = backgroundColor;
						GUILayout.Space (4f);
						EditorGUILayout.BeginHorizontal ();
						EditorGUILayout.BeginVertical ();
						//
						SerializedProperty spoilerbools = this.serializedObject.FindProperty ("AffectedSpoilerSubdivisions");
						//EditorGUILayout.PropertyField (bools.FindPropertyRelative ("Array.size"));
						//
						for (int i = 0; i < spoilerbools.arraySize; i++) {
							GUIContent label = new GUIContent ("Aerofoil Section: " + (i + 1).ToString ());
							EditorGUILayout.PropertyField (spoilerbools.GetArrayElementAtIndex (i), label);
						}
						EditorGUILayout.EndHorizontal ();
						EditorGUILayout.EndVertical ();
					}

					break;
					//
				}
			}


		}
		//

		GUILayout.Space(30f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Output Data", MessageType.None);
		GUI.color = backgroundColor;
		EditorGUILayout.LabelField ("Angle of Attack", aerofoil.angleOfAttack.ToString ("0.0") + " °");
		GUILayout.Space(1f);
		EditorGUILayout.LabelField ("Drag", aerofoil.TotalDrag.ToString ("0.0") + " N");
		GUILayout.Space(1f);
		EditorGUILayout.LabelField ("Lift", aerofoil.TotalLift.ToString ("0.0") + " N");
		//
		GUILayout.Space(20f);
		aerofoil.isDestructible = EditorGUILayout.Toggle("Is Destructible",aerofoil.isDestructible);
		if (aerofoil.isDestructible) {
			GUILayout.Space(3f);
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("Health", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space(3f);
			aerofoil.startingHealth = EditorGUILayout.FloatField ("Starting Health", aerofoil.startingHealth);
			GUILayout.Space(2f);
			aerofoil.currentHealth = EditorGUILayout.FloatField ("Current Health", aerofoil.currentHealth);
			//
			GUILayout.Space(20f);
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("Effects", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space(3f);
			aerofoil.ExplosionPrefab = EditorGUILayout.ObjectField ("Explosion Prefab", aerofoil.ExplosionPrefab,typeof(GameObject),true) as GameObject;
			GUILayout.Space(1f);
			aerofoil.explosionPoint = EditorGUILayout.ObjectField ("Explosion Point", aerofoil.explosionPoint,typeof(Transform),true) as Transform;
			GUILayout.Space(1f);
			aerofoil.firePrefab = EditorGUILayout.ObjectField ("Fire Prefab", aerofoil.firePrefab,typeof(GameObject),true) as GameObject;
			//
			GUILayout.Space(20f);
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("Extras", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space(3f);
			aerofoil.hasEngines = EditorGUILayout.Toggle ("Attached Engines", aerofoil.hasEngines);
			//
			if(aerofoil.hasEngines){
				GUILayout.Space(3f);
				GUI.color = Color.white;
				EditorGUILayout.HelpBox ("Engines", MessageType.None);
				GUI.color = backgroundColor;
				GUILayout.Space (3f);
				//
				GUIContent engineLabel = new GUIContent ("Engine Count");
				GUILayout.Space(5f);
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.BeginVertical ();
				//
				SerializedProperty engine = this.serializedObject.FindProperty("engines");
				EditorGUILayout.PropertyField (engine.FindPropertyRelative ("Array.size"),engineLabel);
				GUILayout.Space(5f);
				for (int i = 0; i < engine.arraySize; i++) {
					GUIContent label = new GUIContent("Engine " +(i+1).ToString ());
					EditorGUILayout.PropertyField (engine.GetArrayElementAtIndex (i), label);
				}
				EditorGUILayout.EndHorizontal ();
				EditorGUILayout.EndVertical ();
				//
			}
			GUILayout.Space(15f);
			aerofoil.hasExtraAerofoil = EditorGUILayout.Toggle ("Attached Aerofoil", aerofoil.hasExtraAerofoil);
			GUILayout.Space(3f);
			if (aerofoil.hasExtraAerofoil) {
				aerofoil.extraAerofoil = EditorGUILayout.ObjectField ("Aerofoil", aerofoil.extraAerofoil,typeof(SilantroAerofoil),true) as SilantroAerofoil;
			}
			//
			//
			GUILayout.Space(15f);
			aerofoil.hasWeapons = EditorGUILayout.Toggle ("Attached Weapons", aerofoil.hasWeapons);
			if(aerofoil.hasWeapons){
				GUILayout.Space(5f);
				GUIContent weaponLabel = new GUIContent ("Weapon Count");
				GUI.color = Color.white;
				EditorGUILayout.HelpBox ("Weapons", MessageType.None);
				GUI.color = backgroundColor;
				GUILayout.Space (3f);
				//
				//
				GUILayout.Space(5f);
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.BeginVertical ();
				//
				SerializedProperty engine = this.serializedObject.FindProperty("weapons");
				EditorGUILayout.PropertyField (engine.FindPropertyRelative ("Array.size"),weaponLabel);
				GUILayout.Space(5f);
				for (int i = 0; i < engine.arraySize; i++) {
					GUIContent label = new GUIContent("Weapon " +(i+1).ToString ());
					EditorGUILayout.PropertyField (engine.GetArrayElementAtIndex (i), label);
				}
				EditorGUILayout.EndHorizontal ();
				EditorGUILayout.EndVertical ();
				//
			}
			//
			GUILayout.Space (15f);
			aerofoil.hasAttachedModels = EditorGUILayout.Toggle ("Attached Models", aerofoil.hasAttachedModels);
			//
			if (aerofoil.hasAttachedModels) {
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
				GUIContent partlabel = new GUIContent("Part Count");
				SerializedProperty part = this.serializedObject.FindProperty ("attachments");
				EditorGUILayout.PropertyField (part.FindPropertyRelative ("Array.size"),partlabel);
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
		if (GUI.changed) {
			EditorUtility.SetDirty (aerofoil);
			EditorSceneManager.MarkSceneDirty (aerofoil.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
	}

}
#endif