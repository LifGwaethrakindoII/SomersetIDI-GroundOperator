//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
//
public class SilantroFuelDistributor : MonoBehaviour {
	//
	[HideInInspector]public SilantroFuelTank internalFuelTank;//[Header("")]
	[HideInInspector]public List<SilantroFuelTank> externalTanks = new List<SilantroFuelTank> ();
	[HideInInspector]public float TotalFuelRemaining;
	//
	[HideInInspector]public string currentTank;
	[HideInInspector]public SilantroFuelTank CurrentTank;
	[HideInInspector]public string tankType;
	[HideInInspector]public float currentTankFuel;
	[HideInInspector]public string timeLeft;
	[HideInInspector]public float totalConsumptionRate =1f;
	//
	[HideInInspector]public bool dumpFuel = false;
	[HideInInspector]public float fuelDumpRate = 1f;
	[HideInInspector]public float actualFlowRate;
	//
	[HideInInspector]public bool refillTank = false;
	[HideInInspector]public float refuelRate = 1f;
	[HideInInspector]public float actualrefuelRate;

	[HideInInspector]public bool lowFuel;
	bool fuelAlertActivated;
	[HideInInspector]public float minimumFuelAmount = 50f;
	[HideInInspector]public AudioClip fuelAlert;
	AudioSource FuelAlert;
	SilantroControls controlBoard;
	//
	string stopEngine ;
	string dumpFuelKey;
	string refuelTankKey;
	// Use this for initialization
	//
	[HideInInspector]public bool isControllable = true;
	//
	void Awake () {
		//
		controlBoard = GameObject.FindGameObjectWithTag ("GameController").GetComponent<SilantroControls> ();
		if (controlBoard == null) {
			Debug.LogError ("Control Board is missing, Place Control Board in scene and restart!");
		}
		stopEngine = controlBoard.engineShutdown;dumpFuelKey = controlBoard.dumpFuelControl;refuelTankKey = controlBoard.refuelControl;
		//FUEL SETUP
		if (internalFuelTank != null) {
			TotalFuelRemaining = 0;
			if (externalTanks.Count > 0) {
				foreach (SilantroFuelTank external in externalTanks) {
					TotalFuelRemaining += external.Capacity;
				}
			}
			TotalFuelRemaining += internalFuelTank.Capacity;
			CurrentTank = internalFuelTank;
			//
		} else {
			Debug.Log ("No internal fuel tank is assigned to distributor!!");
		}
		//
	}
	//
	void Start () {
		
		GameObject soundPoint = new GameObject("Warning Horn");
		soundPoint.transform.parent = this.transform;
		//
		if (null != fuelAlert) {
			FuelAlert = soundPoint.gameObject.AddComponent<AudioSource> ();
			FuelAlert.clip = fuelAlert;
			FuelAlert.loop = true;
			FuelAlert.volume = 1f;
			FuelAlert.dopplerLevel = 0f;
			FuelAlert.spatialBlend = 1f;
			FuelAlert.rolloffMode = AudioRolloffMode.Custom;
			FuelAlert.maxDistance = 650f;
		}

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//
		if (internalFuelTank.CurrentAmount < minimumFuelAmount) {
			if (externalTanks.Count > 0) {
				CurrentTank = externalTanks [0];
				if (CurrentTank.tankType == SilantroFuelTank.TankType.External && CurrentTank.CurrentAmount<0.1f) {
					if (externalTanks.Contains (CurrentTank)) {
						externalTanks.Remove (CurrentTank);
						CurrentTank = externalTanks [0];
					}
				}
			} 
			else {
				CurrentTank = internalFuelTank;
				//Activate fuel Alert
				if (CurrentTank.CurrentAmount <= minimumFuelAmount)
				{
					lowFuel = true;
					if (!fuelAlertActivated) {
						//
						StartCoroutine(LowFuelAction());
						fuelAlertActivated = true;
					}
				}
				Debug.Log("Fuel Low");
			}
		}
		//
		if (CurrentTank != null) {
			currentTank = CurrentTank.gameObject.name;//
			currentTankFuel = CurrentTank.CurrentAmount;
			tankType = CurrentTank.tankType.ToString ();
			if (totalConsumptionRate > 0) {
				float flightTime = (CurrentTank.CurrentAmount) / totalConsumptionRate;
				timeLeft = string.Format ("{0}:{1:00}", (int)flightTime / 60, (int)flightTime % 60);
			}
		}
		TotalFuelRemaining = 0;
		if (externalTanks.Count > 0) {
			foreach (SilantroFuelTank external in externalTanks) {
				TotalFuelRemaining += external.Capacity;
			}
		}
		TotalFuelRemaining += internalFuelTank.CurrentAmount;
	}
	//
	void Update()
	{
		if (isControllable) {
			if (Input.GetButtonDown (stopEngine) && fuelAlertActivated) {
				FuelAlert.Stop ();
				lowFuel = false;
				//fuelAlertActivated = false;
			}
			if (CurrentTank.CurrentAmount <= 0) {
				CurrentTank.CurrentAmount = 0;
			}
			if (Input.GetButtonDown (dumpFuelKey)) {
				if (!refillTank) {
					dumpFuel = !dumpFuel;
				}
			}
			if (Input.GetButtonDown (refuelTankKey)) {
				if (!dumpFuel) {
					refillTank = !refillTank;
				}
			}
			if (dumpFuel) {
				DumpFuel ();
			}
			if (refillTank) {
				RefuelTank ();
			}
		}
	}
	////REFUEL TANKS
	 void RefuelTank()
	{
		
		actualrefuelRate = refuelRate * Time.deltaTime;
		if (internalFuelTank != null) {
			internalFuelTank.CurrentAmount += actualrefuelRate;
		}
		//CONTROL AMOUNT
		if (internalFuelTank.CurrentAmount > CurrentTank.Capacity) {
			internalFuelTank.CurrentAmount = CurrentTank.Capacity;
			refillTank = false;
		}
	}
	///
	////REFUEL TANKS
	 void DumpFuel()
	{
		actualFlowRate = fuelDumpRate * Time.deltaTime;
		if (CurrentTank != null) {
			CurrentTank.CurrentAmount -= actualFlowRate;
		}
		if (CurrentTank.CurrentAmount <= 0) {
			CurrentTank.CurrentAmount = 0;
			dumpFuel = false;
		}
	}
	///
	//ACTIVATE FUEL ALERT SOUND
	IEnumerator LowFuelAction()
	{
		yield return new WaitForSeconds (0.5f);
		FuelAlert.Play ();
	}
}
//
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroFuelDistributor))]
public class DistributorEditor: Editor
{
	Color backgroundColor;
	Color silantroColor = Color.cyan;
	//

	public override void OnInspectorGUI()
	{
		backgroundColor = GUI.backgroundColor;
		//
		DrawDefaultInspector ();serializedObject.Update();
		//
		SilantroFuelDistributor system = (SilantroFuelDistributor)target;
		//
		GUILayout.Space(10f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Tank Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		//
		EditorGUI.indentLevel++;
		//
		system.internalFuelTank = EditorGUILayout.ObjectField("Internal Fuel Tank",system.internalFuelTank,typeof(SilantroFuelTank),true) as SilantroFuelTank;
		//
		GUILayout.Space(5f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("External Tanks", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		//
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.BeginVertical ();
		//
		SerializedProperty tanks = this.serializedObject.FindProperty("externalTanks");
		GUIContent barrelLabel = new GUIContent ("Tank Count");
		//
		EditorGUILayout.PropertyField (tanks.FindPropertyRelative ("Array.size"),barrelLabel);
		GUILayout.Space(5f);
		for (int i = 0; i < tanks.arraySize; i++) {
			GUIContent label = new GUIContent("Tank " +(i+1).ToString ());
			EditorGUILayout.PropertyField (tanks.GetArrayElementAtIndex (i), label);
		}
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.EndVertical ();
		//
		EditorGUI.indentLevel--;
		GUILayout.Space(5f);

		EditorGUILayout.LabelField ("Total Fuel: ", system.TotalFuelRemaining.ToString ("0.0") + " kg");
		//
		GUILayout.Space(5f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Current Tank Properties", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		if (system.CurrentTank == null) {
			system.CurrentTank = system.internalFuelTank;
		}
		EditorGUILayout.LabelField ("Type: ", system.CurrentTank.tankType.ToString ());
		EditorGUILayout.LabelField ("Fuel Amount: ", system.CurrentTank.CurrentAmount.ToString ("0.0")+ " kg");
		EditorGUILayout.LabelField ("Time Left: ", system.timeLeft+ " mins");
		EditorGUILayout.LabelField ("Burn Rate: ", system.totalConsumptionRate.ToString ("0.00") + " kg/s");
		//
		GUILayout.Space(15f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Tank Operations", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		system.dumpFuel = EditorGUILayout.Toggle ("Dump Fuel", system.dumpFuel);
		if (system.dumpFuel) {
			system.refillTank = false;
			GUILayout.Space(5f);
			GUI.color = Color.white;
			EditorGUILayout.HelpBox ("Dump Settings", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space(3f);
			system.fuelDumpRate = EditorGUILayout.FloatField ("Dump Rate", system.fuelDumpRate);
			GUILayout.Space(2f);
			EditorGUILayout.LabelField ("Fuel Flow: ", system.actualFlowRate.ToString ("0.00") + " kg/s");
		}
		GUILayout.Space(5f);
		system.refillTank = EditorGUILayout.Toggle ("Refuel Tank", system.refillTank);
		if (system.refillTank) {
			system.dumpFuel = false;
			GUILayout.Space(5f);
			GUI.color = Color.white;
			EditorGUILayout.HelpBox ("Refil Settings", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space(3f);
			system.refuelRate = EditorGUILayout.FloatField ("Transfer Rate", system.refuelRate);
			GUILayout.Space(2f);
			EditorGUILayout.LabelField ("Fuel Flow: ", system.actualrefuelRate.ToString ("0.00") + " kg/s");
		}
		//
		GUILayout.Space(15f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Alert Settings", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		system.minimumFuelAmount = EditorGUILayout.FloatField ("Minimum Fuel Amount", system.minimumFuelAmount);
		GUILayout.Space(4f);
		system.fuelAlert = EditorGUILayout.ObjectField ("Fuel Alert", system.fuelAlert, typeof(AudioClip), true) as AudioClip;
		//
		if (GUI.changed) {
			EditorUtility.SetDirty (system);
			EditorSceneManager.MarkSceneDirty (system.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
		serializedObject.ApplyModifiedProperties();
	}
}
#endif