using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
//

public class SilantroFreyr : MonoBehaviour {

	[HideInInspector]public Rigidbody Parent;
	//
	//Available aircraft configurations
	[HideInInspector]public bool isControllable = true;
	float drag;
	public enum Configuration
	{
		V22Tiltrotor,
		F35Liftfan
	}
	[HideInInspector]public Configuration configuration = Configuration.V22Tiltrotor;
	//
	//Engine Sources
	[HideInInspector]public SilantroTurboShaft drivePowerplant;
	[HideInInspector]public SilantroBlade[] blades;float meat;
	[HideInInspector]public SilantroLiftFan liftfan;
	[HideInInspector]public SilantroTurboFan mainTurbofanEngine;
	[HideInInspector]public SilantroTurboJet mainTurbojetEngine;
	//
	//Total Force available for lift
	[HideInInspector]public float LiftForce;
	//
	//Takeoff mode
	public enum Mode
	{
		Normal,
		VTOL,
		STOL
	}

	[HideInInspector]public Mode mode = Mode.Normal;
	//
	//Hydraulic systems
	[HideInInspector]public SilantroHydraulicSystem VTOLHydraulics;
	[HideInInspector]public SilantroHydraulicSystem STOLHydraulics;
	[HideInInspector]public SilantroHydraulicSystem liftSystemHydraulics;
	//
	//Control switches
	[HideInInspector]public bool transitionToVTOL;
	[HideInInspector]public bool transitionToSTOL;
	[HideInInspector]public bool transitionToNormal;
	bool goNormal;
	bool goSTOL;
	//
	[HideInInspector]public float transitionTime = 10f;
	bool transitioning = false;
	//
	float STOLmultiplier;
	float VTOLmultiplier;
	//
	public LayerMask surfaceMask;
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
	[HideInInspector]public float powerFactor = 1.2f;
	//
	string VTOL;
	string STOL;
	string Normal;
	SilantroControls controlBoard;
	[HideInInspector]public SilantroController controller;
	//
	//
	void Awake () {
		controlBoard = GameObject.FindGameObjectWithTag ("GameController").GetComponent<SilantroControls> ();
		if (controlBoard == null) {
			Debug.LogError ("Control Board is missing, Place Control Board in scene and restart!");
		}
		//
		VTOL= controlBoard.VTOLTransition;
		STOL = controlBoard.STOLTransition;
		Normal = controlBoard.NormalTransition;
	}
	//
	//
	void Start()
	{
		//DETERMINE FORCE MODE
		if (liftForceMode == LiftForceMode.Linear)
		{
			exponent = 1f;
		} 
		else if (liftForceMode == LiftForceMode.Quadratic) 
		{
			exponent = 2f;
		} 
		else if (liftForceMode == LiftForceMode.Quatic) 
		{
			exponent = 3f;
		}
		//
		//
		if (configuration == Configuration.V22Tiltrotor) {
			if (mode == Mode.VTOL) {
				foreach (SilantroBlade blade in blades) {
					blade.VTOLmode = true;
				}
			} else if (mode == Mode.Normal) {
				foreach (SilantroBlade blade in blades) {
					blade.VTOLmode = false;//blade.STOLmode = false;
				}
			}
		}
	}
	//
	//
	void FixedUpdate () {
		//
		ApplyLiftForce ();
		//
		LiftForce = 0;
		if (configuration == Configuration.V22Tiltrotor) {
			foreach (SilantroBlade blade in blades) {
				LiftForce += blade.thrust;
			}
		}
		else if (configuration == Configuration.F35Liftfan) {
			if (mode == Mode.VTOL) {
				LiftForce = mainTurbofanEngine.EngineThrust + liftfan.FanThrust;
			}
			else//+;
			{
				LiftForce = liftfan.FanThrust;
			}
		}
		//Debug.Log (LiftForce + "  FAN" + mainTurbofanEngine.EngineThrust);
	}
	//
	//
	void Update()
	{
		//
		if (isControllable) {
			//Controls
			if (configuration == Configuration.F35Liftfan) {
				if (Input.GetButtonDown (VTOL) && mainTurbofanEngine.EngineOn == true) {
					if (mainTurbofanEngine.AfterburnerOperative) {
						mainTurbofanEngine.AfterburnerOperative = false;
					}
					transitionToVTOL = true;
				}
				if (Input.GetButtonDown (STOL) && mainTurbofanEngine.EngineOn == true) {
					transitionToSTOL = true;
					if (mainTurbofanEngine.AfterburnerOperative) {
						mainTurbofanEngine.AfterburnerOperative = false;
					}

				}
				if (Input.GetButtonDown (Normal) && mainTurbofanEngine.EngineOn == true) {
					transitionToNormal = true;
					if (!mainTurbofanEngine.AfterburnerOperative) {
						mainTurbofanEngine.AfterburnerOperative = true;
					}
				}
			} else if (configuration == Configuration.V22Tiltrotor) {
				if (Input.GetButtonDown (VTOL) && drivePowerplant.EngineOn == true) {
					transitionToVTOL = true;
				}
				if (Input.GetButtonDown (STOL) && drivePowerplant.EngineOn == true) {
					transitionToSTOL = true;
				}
				if (Input.GetButtonDown (Normal) && drivePowerplant.EngineOn == true) {
					transitionToNormal = true;
				}
			}
		}
		//
		if (Parent) {
			switch (mode) {
			case Mode.Normal:
				NormalState ();
				break;
			case Mode.VTOL:
				VTOLState ();
				break;
			case Mode.STOL:
				STOLState ();
				break;
			}
		}
		//
		VTOLmultiplier = VTOLHydraulics.currentDragPercentage / 100;
		STOLmultiplier = STOLHydraulics.currentDragPercentage / 100;
		//
		if (configuration == Configuration.V22Tiltrotor) {
			foreach (SilantroBlade blade in blades) {
				if (goNormal) {
					blade.VTOLmodeMultiplier = 1-VTOLmultiplier;
				} else if (goSTOL) {
					blade.VTOLmodeMultiplier = STOLmultiplier;
				}
			}
		}
		else if (configuration == Configuration.F35Liftfan) {
				if (goNormal) {
				mainTurbofanEngine.VTOLmodeMultiplier = (1-VTOLmultiplier);
				} else if (goSTOL) {
				mainTurbofanEngine.VTOLmodeMultiplier = (1-STOLmultiplier);
				}
		}
	}
	//
	//
	private void NormalState ()
	{
		//FOR V-22 OSPREY
		if (configuration == Configuration.V22Tiltrotor) {
			if (transitionToVTOL && !transitioning) {
				VTOLHydraulics.open = true;
				transitioning = true;
				StartCoroutine (TransitionToVTOL ());
			}
			if (transitionToSTOL && !transitioning) {
				STOLHydraulics.open = true;
				transitioning = true;
				StartCoroutine (TransitionToSTOL ());
			}
		}
		//FOR F-35 LIGHTNING
		if (configuration == Configuration.F35Liftfan) {
			if (transitionToVTOL && !transitioning) {
				liftSystemHydraulics.open = true;
				transitioning = true;
				StartCoroutine (OpenEngineDoors(1));
			}
			if (transitionToSTOL && !transitioning) {
				liftSystemHydraulics.open = true;
				transitioning = true;
				StartCoroutine (OpenEngineDoors(2));
			}
		}
	}

	//
	IEnumerator OpenEngineDoors(int level)
	{
		yield return new WaitForSeconds (liftSystemHydraulics.openTime);
		if (level == 1) {
			OpenF35VTOLDoors();
		} else if (level == 2) {
			STOLHydraulics.open = true;
			OpenF35STOLDoors();
		}
	}
	//
	//F35 Settings
	//
	void OpenF35VTOLDoors()
	{
		VTOLHydraulics.open = true;
		StartCoroutine(TransitionToVTOL ());
	}
	void OpenF35STOLDoors()
	{
		
		StartCoroutine(TransitionToSTOL ());
	}
	//
	private void VTOLState ()
	{
		if (configuration == Configuration.V22Tiltrotor) {
			if (transitionToNormal && !transitioning) {
				goNormal = true;
				VTOLHydraulics.close = true;
				transitioning = true;
				StartCoroutine (TransitionToNormal ());
			}
			if (transitionToSTOL && !transitioning) {
				goSTOL = true;
				STOLHydraulics.open = true;
				transitioning = true;
				StartCoroutine (TransitionToSTOL ());
			}
		} else if (configuration == Configuration.F35Liftfan) {
			if (transitionToNormal && !transitioning) {
				goNormal = true;
				VTOLHydraulics.close = true;liftfan.stop = true;
				transitioning = true;
				StartCoroutine (CloseF35Doors());
			}
			if (transitionToSTOL && !transitioning) {
				goSTOL = true;
				STOLHydraulics.open = true;
				transitioning = true;
				StartCoroutine (TransitionToSTOL());
			}
		}
	}
	//

	IEnumerator CloseF35Doors()
	{
		yield return new WaitForSeconds (liftSystemHydraulics.closeTime);
		liftSystemHydraulics.close = true;
		StartCoroutine (TransitionToNormal ());
	}

	//
	private void STOLState ()
	{
		if (configuration == Configuration.V22Tiltrotor) {
			if (transitionToNormal && !transitioning) {
				VTOLHydraulics.close = true;
				transitioning = true;
				StartCoroutine (TransitionToNormal ());
			}
			if (transitionToVTOL && !transitioning) {
				STOLHydraulics.close = true;
				transitioning = true;
				StartCoroutine (TransitionToVTOL ());
			}
		} else if (configuration == Configuration.F35Liftfan) {
			if (transitionToNormal && !transitioning) {
				STOLHydraulics.close = true;liftfan.stop = true;
				transitioning = true;
				StartCoroutine (CloseF35Doors());
			}
			if (transitionToVTOL && !transitioning) {
				VTOLHydraulics.open = true;
				transitioning = true;
				StartCoroutine (TransitionToVTOL ());
			}
		}
	}
	//
	private IEnumerator TransitionToVTOL ()
	{
		if (configuration == Configuration.F35Liftfan) {
			liftfan.start = true;
		}
		yield return new WaitForSeconds (transitionTime);
		mode = Mode.VTOL;transitioning = false;ReturnIgnition ();
		controller.currentMode = SilantroController.CurrentMode.VTOL;
	}
	private IEnumerator TransitionToNormal ()
	{
		
		yield return new WaitForSeconds (transitionTime);
		mode = Mode.Normal;transitioning = false;ReturnIgnition ();
		controller.currentMode = SilantroController.CurrentMode.Normal;
	}
	private IEnumerator TransitionToSTOL ()
	{
		if (configuration == Configuration.F35Liftfan) {
			liftfan.start = true;
		}
		yield return new WaitForSeconds (transitionTime);
		mode = Mode.STOL;transitioning = false;ReturnIgnition ();
		controller.currentMode = SilantroController.CurrentMode.STOL;
		if (configuration == Configuration.V22Tiltrotor) {
			foreach (SilantroBlade blade in blades) {
				blade.VTOLmode = false;
			}
		}
	}
	//
	void ReturnIgnition()
	{
		transitionToSTOL = false;
		transitionToVTOL = false;goSTOL = false;
		transitionToNormal = false;goNormal = false;
	}
	//APPLY LIFT FORCE
	private	void ApplyLiftForce()
	{
		RaycastHit groundHit;
		//CALCULATE DIRECTION OF FORCE
		var up = Parent.transform.up;
		var gravity = Physics.gravity.normalized;
		//
		up = Vector3.RotateTowards(up, - gravity,hoverAngleDrift*Mathf.Deg2Rad,1);
		powerFactor = 0;
		if(!Physics.Raycast(transform.position,-up, out groundHit,maximumHoverHeight,surfaceMask))
		{
			return;
		}
		//CALCULATE POWER FALLOFF
		powerFactor = Mathf.Pow((maximumHoverHeight - (groundHit.distance*3.28f))/maximumHoverHeight,exponent);

		if (powerFactor < 0) {
			powerFactor = 0;
		}
		//
		float liftForce = powerFactor * LiftForce;// * meat;
		//
		//CALCULATE DAMPING
		float velocity = Vector3.Dot(Parent.GetPointVelocity(transform.position),up);
		drag = -velocity * Mathf.Abs (velocity) * hoverDamper;
		//
		//APPLY FORCE
		//	
		if ((transform.position.y * 3.28f)< maximumHoverHeight) {
			if (mode == Mode.VTOL) {
				Parent.AddForce (up * (liftForce + drag), ForceMode.Force);
			}
			if (mode == Mode.STOL) {
				Parent.AddForce (up * (liftForce));
			}
		}

	}
}
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroFreyr))]
public class FreyrEditor: Editor
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
		SilantroFreyr controller = (SilantroFreyr)target;
		//
		GUILayout.Space(10f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Aircraft Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		controller.configuration = (SilantroFreyr.Configuration)EditorGUILayout.EnumPopup (" ", controller.configuration);
		//
		GUILayout.Space (5f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Engine System", MessageType.None);
		GUI.color = backgroundColor;
		//
		if (controller.configuration == SilantroFreyr.Configuration.F35Liftfan) {
			//
			GUILayout.Space(3f);
			controller.mainTurbofanEngine = EditorGUILayout.ObjectField ("Main Engine", controller.mainTurbofanEngine, typeof(SilantroTurboFan), true) as SilantroTurboFan;
			GUILayout.Space (3f);
			controller.liftfan = EditorGUILayout.ObjectField ("LiftFan Engine", controller.liftfan, typeof(SilantroLiftFan), true) as SilantroLiftFan;
			//
		}
		else if (controller.configuration == SilantroFreyr.Configuration.V22Tiltrotor) {
			//
			GUILayout.Space(3f);
			controller.drivePowerplant = EditorGUILayout.ObjectField ("Drive Powerplant", controller.drivePowerplant, typeof(SilantroTurboShaft), true) as SilantroTurboShaft;
			GUILayout.Space (3f);
			GUI.color = Color.white;
			EditorGUILayout.HelpBox ("Attached Blades", MessageType.None);
			GUI.color = backgroundColor;
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.BeginVertical ();
			//
			SerializedProperty slatbools = this.serializedObject.FindProperty ("blades");
			//EditorGUILayout.PropertyField (bools.FindPropertyRelative ("Array.size"));
			//
			for (int i = 0; i < slatbools.arraySize; i++) {
				GUIContent label = new GUIContent ("Blade: " + (i + 1).ToString ());
				EditorGUILayout.PropertyField (slatbools.GetArrayElementAtIndex (i), label);
			}
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.EndVertical ();
			//
		}
		//
		GUILayout.Space(20f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Hydraulics Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		controller.VTOLHydraulics = EditorGUILayout.ObjectField ("VTOL Hydraulics Actuator", controller.VTOLHydraulics, typeof(SilantroHydraulicSystem), true) as SilantroHydraulicSystem;
		GUILayout.Space (3f);
		controller.STOLHydraulics = EditorGUILayout.ObjectField ("STOL Hydraulics Actuator", controller.STOLHydraulics, typeof(SilantroHydraulicSystem), true) as SilantroHydraulicSystem;
		//
		if (controller.configuration == SilantroFreyr.Configuration.F35Liftfan) {
			//
			GUILayout.Space (3f);
			controller.liftSystemHydraulics = EditorGUILayout.ObjectField ("LiftFan Hydraulics Actuator", controller.liftSystemHydraulics, typeof(SilantroHydraulicSystem), true) as SilantroHydraulicSystem;
		}
		//
		GUILayout.Space(10f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Transition Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space (3f);
		controller.transitionTime = EditorGUILayout.FloatField ("Transition Time", controller.transitionTime);
		GUILayout.Space (2f);
		EditorGUILayout.LabelField ("Current Mode", controller.mode.ToString ());
		//
		GUILayout.Space(20f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Hover Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		controller.maximumHoverHeight = EditorGUILayout.FloatField ("Maximum Hover Height", controller.maximumHoverHeight);
		GUILayout.Space (2f);
		controller.hoverDamper = EditorGUILayout.FloatField ("Hover Damper", controller.hoverDamper);
		GUILayout.Space (2f);
		controller.hoverAngleDrift = EditorGUILayout.FloatField ("Hover Angle Drift", controller.hoverAngleDrift);
		GUILayout.Space (2f);
		controller.liftForceMode = (SilantroFreyr.LiftForceMode)EditorGUILayout.EnumPopup ("Liftforce Mode", controller.liftForceMode);
		GUILayout.Space (2f);
		EditorGUILayout.LabelField ("Power Factor", (controller.powerFactor*100f).ToString("0.00")+" %");
	//
		//
		if (GUI.changed) {
			EditorUtility.SetDirty (controller);
			EditorSceneManager.MarkSceneDirty (controller.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
	}
}
#endif