//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
//
public class SilantroDataLogger : MonoBehaviour {
	[HideInInspector]public string savefileName;
	public enum FileExtension
	{
		txt,
		csv
	}
	[HideInInspector]public FileExtension dataExtension = FileExtension.txt;
	[HideInInspector]public string saveLocation = "C:\\Users\\Public\\";
	//
	[HideInInspector]public float logRate = 5f;
	public enum DataType
	{
		//EngineData, //COMING SOON
		FlightData
	}
	[HideInInspector]public DataType dataType = DataType.FlightData;
	[HideInInspector]public SilantroController Aircraft;
	//
	bool isTurbojet;
	bool isTurboFan;
	bool isTurboProp;
	bool isPiston;
	//
	SilantroInstrumentation datalog;
	//Engine Data
	SilantroTurboJet[] turboengines;
	SilantroTurboFan[] jetengines;
	SilantroTurboProp[] propengines;
	SilantroPistonEngine[] pistonEngines;
	//STATIC DATA
	string engineName;
	string fuelType;
	float massFactor;
	float fuelTankCapacity;
	//DYNAMIC DATA
	float enginePower;
	float engineRPM;
	float propEfficiency;
	float EGT;
	float currentFuel;
	float cumbustionRate;
	float engineThrust;
	//
	bool csv;
	bool txt;
	//
	//FLIGHT DATA
	//STATIC
	string aircraftName;
	float takeoffweight;
	int noOfEngines;
	float startFuel;
	//DYNAMIC DATA
	float currentSpeed;
	float climbRate;
	int headingDirection;
	float currentAltitude;
	float fuelRemaining;
	float totalThrutGenerated;
	float currentHealth;
	//
	float flightTIme;
	float timer;
	float actualLogRate;
	//
	private List<string[]> dataRow = new List<string[]>();
	//StreamWriter streamWriter;
	//INDIVIDUAL PART HEALTHS.....COMING SOON
	// Use this for initialization
	void Start () {
		//
		if (Aircraft == null) {
			Debug.Log ("No Aircraft is connected to the Black Box");
		}
		datalog = Aircraft.GetComponentInChildren<SilantroInstrumentation>();//
		//Property of Oyedoyin Dada
		//cc dadaoyedoyin@gmail.com
		//
		//
		//
		if (datalog == null) {
			Debug.Log ("No instrumentation is connected to the Black Box");
		}

		if (Aircraft == null) {
			Debug.Log ("Controller has not been added to the Airplane");
		}
		if (Aircraft.engineType == SilantroController.AircraftType.TurboFan) {
			isTurboFan = true;
			jetengines = Aircraft.turbofans;
		}
		//
		if (Aircraft.engineType == SilantroController.AircraftType.TurboJet) {
			isTurbojet = true;
			turboengines = Aircraft.turboJet;
		}
		//
		if (Aircraft.engineType == SilantroController.AircraftType.TurboProp) {
			isTurboProp = true;
			propengines = Aircraft.turboprop;
		}
		if (Aircraft.engineType == SilantroController.AircraftType.Piston) {
			isPiston = true;
			pistonEngines = Aircraft.pistons;
		}
		//
		if (dataExtension == FileExtension.csv)
		{
			csv = true;
			txt = false;
		}

		else if (dataExtension == FileExtension.txt)
		{
			txt = true;
			csv = false;
		}
		//

		timer = 0.0f;
		//
		if (logRate != 0)
		{
			actualLogRate = 1.0f / logRate;
		} else 
		{
			actualLogRate = 0.10f;
		}
		//

		aircraftName = Aircraft.gameObject.name;
		takeoffweight = Aircraft.currentWeight;
		noOfEngines = Aircraft.AvailableEngines;
		//
		if (Aircraft.engineType != SilantroController.AircraftType.Electric) {
			startFuel = Aircraft.fuelsystem.TotalFuelRemaining;
		}


	//	engines = aircraft.Engines;
		//
		if (dataType == DataType.FlightData) {

			//WRITE INITIAL TXT FILE
			if (txt) {
				File.WriteAllText (saveLocation + "" + savefileName + ".txt", "Flight Data");
				//
				using (System.IO.StreamWriter writeText = System.IO.File.AppendText (saveLocation + "" + savefileName + ".txt")) {
					writeText.WriteLine ("<<>>");
					writeText.WriteLine ("<<>>");
					writeText.WriteLine ("Aircraft Name: " + aircraftName);
					writeText.WriteLine ("Gross Weight: " + takeoffweight.ToString () + " kg");
					writeText.WriteLine ("No of Engines: " + noOfEngines.ToString ());
					writeText.WriteLine ("Available Fuel: " + startFuel.ToString () + " kg");
					writeText.WriteLine ("Date: " + DateTime.Now.ToString ("f"));
					//writeText.WriteLine ("Current Time: " + DateTime.Now.ToString ("));
					writeText.WriteLine ("<<>>");
					writeText.WriteLine ("Flight Time  " + "Current Speed  " + " Climb Rate  " + "    Heading  " + " Current Altitude  " + " Fuel Remaining  " + " Thrust Generated  " + " Current Health");
				}
			} else if (csv) {
				//WRITE INITIAL CSV FILE
				string[] nameTemp = new string[1];nameTemp[0] = "Aircraft Name: " +aircraftName;
				dataRow.Add(nameTemp);
				string[] typeTemp = new string[1];typeTemp[0] = "Gross Weight: " + (Aircraft.emptyWeight+startFuel).ToString () + " kg";
				dataRow.Add(typeTemp);
				string[] cylinderTemp = new string[1];cylinderTemp[0] = "Number Of Engines: " +noOfEngines.ToString ();
				dataRow.Add(cylinderTemp);
				string[] cabTemp = new string[1];cabTemp[0] = "Available Fuel: " +  startFuel.ToString () + " kg";
				dataRow.Add(cabTemp);
				string[] dateTemp = new string[1];dateTemp[0] = "Date: " +  DateTime.Now.ToString ("f");
				dataRow.Add(dateTemp);
				string[] space = new string[2];space [0] = " ";space[1] = " ";
				dataRow.Add(space);
				//
				string[] dataRowTemp = new string[8];
				dataRowTemp [0] = "Flight Time";
				dataRowTemp [1] = "Current Speed";
				dataRowTemp [2] = "Climb Rate";
				dataRowTemp [3] = "Heading";
				dataRowTemp [4] = "Current Altitude";
				dataRowTemp [5] = "Fuel Remaining";
				dataRowTemp [6] = "Total Thrust Generated";
				dataRowTemp [7] = "Current Health";
				dataRow.Add (dataRowTemp);
				//
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
	//	flightTIme += Time.deltaTime;
		//
		if (isTurboFan) 
		{
			if(jetengines[0].EngineOn)
			{
				flightTIme += Time.deltaTime;
			}
			//
			if (timer > actualLogRate && jetengines[0].EngineOn) {
				if (txt) 
				{
					WriteLogTxt ();
				}
				else if (csv)
				{
					GetCSVData ();
				}

			}
		} 
		//
		if (isTurbojet) 
		{
			if(turboengines[0].EngineOn)
			{
				flightTIme += Time.deltaTime;
			}
			//
			if (timer > actualLogRate && turboengines[0].EngineOn) {
				if (txt) 
				{
					WriteLogTxt ();
				}
				else if (csv)
				{
					GetCSVData ();
				}

			}
		} 

		//
		if (isTurboProp) 
		{
			if(propengines[0].EngineOn)
			{
				flightTIme += Time.deltaTime;
			}
			//
			if (timer > actualLogRate && propengines[0].EngineOn) {
				if (txt) 
				{
					WriteLogTxt ();
				}
				else if (csv)
				{
					GetCSVData ();
				}

			}
		} 
		if (isPiston) 
		{
			if(pistonEngines[0].EngineOn)
			{
				flightTIme += Time.deltaTime;
			}
			//
			if (timer > actualLogRate && pistonEngines[0].EngineOn) {
				if (txt) 
				{
					WriteLogTxt ();
				}
				else if (csv)
				{
					GetCSVData ();
				}

			}
		} 

	}
	//WRITES TXT LOG FILE
	void WriteLogTxt()
	{
		timer = 0.0f;
		//
		//string minSec = string.Format ("{0}:{1:00}", (int)controller.timeLeft / 60, (int)controller.timeLeft % 60);
		//
		if(datalog != null)
		{
			currentSpeed = datalog.currentSpeed;
			climbRate = datalog.verticalSpeed;
			currentAltitude = datalog.currentAltitude;
			totalThrutGenerated = Aircraft.totalThrustGenerated;
			headingDirection = (int)datalog.headingDirection;
			currentFuel = Aircraft.fuelsystem.TotalFuelRemaining;
		}

		string minSec = string.Format ("{0}:{1:00}", (int)flightTIme / 60, (int)flightTIme % 60);

		//WRITE TXT DATA

		using (System.IO.StreamWriter writeText = System.IO.File.AppendText (saveLocation + "" + savefileName + ".txt")) {
			writeText.Write (minSec + " mins     " + currentSpeed.ToString ("0000.0") + " knots    " + climbRate.ToString ("  0000.0") + " ft/min    " + headingDirection.ToString ("000.0 ") + "°      " + currentAltitude.ToString ("000000.0") + " ft          " + currentFuel.ToString ("00000.0") + " kg            " + totalThrutGenerated.ToString ("000000.0") + " N             " + 100.ToString ("0000.00") + Environment.NewLine);
		}
	}
	//
	void GetCSVData()
	{

		if(datalog != null)
		{
			currentSpeed = datalog.currentSpeed;
			climbRate = datalog.verticalSpeed;
			currentAltitude = datalog.currentAltitude;
			totalThrutGenerated = Aircraft.totalThrustGenerated;
			headingDirection = (int)datalog.headingDirection;
			currentFuel = Aircraft.fuelsystem.TotalFuelRemaining;
		}

	//	if (Aircraft.gameObject.GetComponent<SilantroHealth> ()) {
		//	HealthValue = Aircraft.gameObject.GetComponent<SilantroHealth> ().currentHealth;
	//	}
		string minSec = string.Format ("{0}:{1:00}", (int)flightTIme / 60, (int)flightTIme % 60);

		WriteLogCsv(minSec + " mins",currentSpeed.ToString ("0.0") + " knots",climbRate.ToString ("0.0") + " ft/min",headingDirection.ToString ("0.0 ") + " °",currentAltitude.ToString ("0.0") + " ft",currentFuel.ToString ("0.0") + " kg",totalThrutGenerated.ToString ("0.0") + " N",100.ToString ("0.00"));

	}
	//WRITES CSV LOG FILE
	void WriteLogCsv(string flightTIme, string currentSpeed, string climbRate, string Heading, string currentAltitude, string fuelRemaining, string totalThrust, string currentHealth)
	{
		timer = 0.0f;
		//
		string[] dataAdd = new string[8];
		//
		dataAdd[0] = flightTIme;
		dataAdd [1] = currentSpeed;
		dataAdd [2] = climbRate;
		dataAdd [3] = Heading;
		dataAdd [4] = currentAltitude;
		dataAdd [5] = fuelRemaining;
		dataAdd [6] = totalThrust;
		dataAdd [7] = currentHealth;
		dataRow.Add (dataAdd);

	}

	void OnApplicationQuit()
	{
		//
		if (csv) {
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

			StreamWriter streamWriter = System.IO.File.CreateText (saveLocation + "" + savefileName + ".csv");
			streamWriter.WriteLine (builder);
			streamWriter.Close ();
		}
	}

}
//
//
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroDataLogger))]
public class BlackBoxEditor: Editor
{
	Color backgroundColor;
	Color silantroColor = Color.cyan;
	//
	//
	public override void OnInspectorGUI()
	{
		backgroundColor = GUI.backgroundColor;
		//
		DrawDefaultInspector ();
		//
		serializedObject.Update ();
		//
		SilantroDataLogger box = (SilantroDataLogger)target;
		//
		//
		GUILayout.Space (3f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Connection", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space (3f);
		box.Aircraft = EditorGUILayout.ObjectField ("Aircraft", box.Aircraft, typeof(SilantroController), true) as SilantroController;
		//
		GUILayout.Space (10f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("File Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space (3f);
		box.savefileName = EditorGUILayout.TextField ("Identifier", box.savefileName);
		GUILayout.Space (4f);
		box.dataExtension = (SilantroDataLogger.FileExtension)EditorGUILayout.EnumPopup ("Extension", box.dataExtension);
		GUILayout.Space (5f);
		box.saveLocation = EditorGUILayout.TextField ("Save Location", box.saveLocation);
		//
		GUILayout.Space (10f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Data Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space (4f);
		box.dataType = (SilantroDataLogger.DataType)EditorGUILayout.EnumPopup ("Type", box.dataType);
		GUILayout.Space (3f);
		box.logRate = EditorGUILayout.FloatField ("Log Rate", box.logRate);
		//
		//
		if (GUI.changed) {
			EditorUtility.SetDirty (box);
			EditorSceneManager.MarkSceneDirty (box.gameObject.scene);
		}
	}
}
#endif