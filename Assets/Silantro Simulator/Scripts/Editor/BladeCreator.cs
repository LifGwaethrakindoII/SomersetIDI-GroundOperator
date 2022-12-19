using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEditor;

//
public class BladefoilCreator : MonoBehaviour {
	//
	public string identifier = "BLD 0000";
	[HideInInspector]public string NACAEquivalent = "NACA 0000";
	//[HideInInspector]public TextAsset airfoilPlot;
	[HideInInspector]public TextAsset propellerPerformancePlot;
	[HideInInspector]public TextAsset propellerStaticPlot;
	[HideInInspector]public string bladePrefabLocation = "Assets/Prefabs/Default/Airfoils/Blades/";
	//
	//
	private char lineSeperator = '\n';
	private char fieldSeperator = ',';
	//
	//Propeller Data
	private List<float> advanceRatio = new List<float>();
	private List<float> thrustCo = new List<float> ();
	private List<float> powerCo = new List<float> ();
	private List<float> etaCo = new List<float> ();
	//
	//Propeller Static Data
	private List<float> rpm = new List<float>();
	private List<float> staticCt = new List<float> ();
	private List<float> staticCp = new List<float> ();
	//
	[HideInInspector]public SilantroBladefoil airfoil;
	[HideInInspector]public GameObject newFoil ;
	//
	public void Data()
	{
		
		//
		newFoil = new GameObject (identifier);
		airfoil = newFoil.AddComponent<SilantroBladefoil> ();
		//
		PrefabUtility.CreatePrefab(bladePrefabLocation+""+identifier+".prefab",newFoil);
		//
		DestroyImmediate(newFoil);
		newFoil = (GameObject)AssetDatabase.LoadAssetAtPath (bladePrefabLocation+""+identifier+".prefab",typeof(GameObject));
		airfoil = newFoil.GetComponent<SilantroBladefoil> ();
		//
		airfoil.identifier =identifier;
		airfoil.NACAEquivalent = NACAEquivalent;
		//
		//CREATE STATIC DATA
		string[] dataPlots = propellerStaticPlot.text.Split (lineSeperator);
		for (int i=1; (i<dataPlots.Length-1); i++){
			string[] staticfields = dataPlots[i].Split (fieldSeperator);
			rpm.Add (float.Parse (staticfields [0]));
			staticCp.Add (float.Parse (staticfields [1]));
			staticCt.Add(float.Parse(staticfields[2]));
		}
		//
		//PLOT STATIC DATA
		for (int a = 0; a < rpm.Count; a++) {
			float x = rpm [a];
			float y = staticCp [a];
			float z = staticCt [a];
			Keyframe power = new Keyframe (x, y);
			Keyframe thrust = new Keyframe (x, z);
			airfoil.StaticThrustCurve.AddKey (thrust );
			airfoil.StaticPowerCurve.AddKey (power );
		}
		//
		//CREATE DYNAMIC DATA
		string[] dynamicPlots = propellerPerformancePlot.text.Split (lineSeperator);
		for (int i=1; (i<dynamicPlots.Length-1); i++){
			string[] dynamicfields = dynamicPlots[i].Split (fieldSeperator);
			advanceRatio.Add (float.Parse (dynamicfields [0]));
			thrustCo.Add (float.Parse (dynamicfields [1]));
			powerCo.Add(float.Parse(dynamicfields[2]));
			etaCo.Add(float.Parse(dynamicfields[3]));
		}
		//
		//PLOT DYNAMIC DATA
		for (int a = 0; a < advanceRatio.Count; a++) {
			float x = advanceRatio [a];
			float y = powerCo [a];
			float z = thrustCo [a];
			float w = etaCo [a];
			Keyframe power = new Keyframe (x, y);
			Keyframe thrust = new Keyframe (x, z);
			Keyframe eff = new Keyframe (x, w);
			airfoil.thrustCurve.AddKey (thrust );
			airfoil.powerCurve.AddKey (power );
			airfoil.etaCurve.AddKey (eff );
		}
		//
		Debug.Log("Blade: " + identifier + " Successfully created in " + bladePrefabLocation);
		DestroyImmediate(this.gameObject);
	}
	//
}
//
public class BladeCreator : EditorWindow {
	Color backgroundColor;
	Color silantroColor = Color.blue;
	//
	public string identifier = "BLD 0000";
	[HideInInspector]public string NACAEquivalent = "NACA 0000";
	[HideInInspector]public TextAsset airfoilPlot;
	[HideInInspector]public TextAsset propellerPerformancePlot;
	[HideInInspector]public TextAsset propellerStaticPlot;
	//
	public SilantroBladefoil airfoil;
	public GameObject newFoil ;
	//
	[MenuItem("Oyedoyin/Airfoil System/Bladefoil/Create")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow<BladeCreator>("Bladefoil");
	}
	//

	//
	void OnGUI()
	{
		backgroundColor = GUI.backgroundColor;
		//
		//
		GUILayout.Space(5f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Bladefoil Identfier", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(5f);
		identifier = EditorGUILayout.TextField ("Identifier", identifier);
		GUILayout.Space (3f);
		NACAEquivalent = EditorGUILayout.TextField ("NACA Equivalent", NACAEquivalent);
		//
		GUILayout.Space(7f);
		GUI.color =silantroColor;
		EditorGUILayout.HelpBox ("Data Plots", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(5f);
		EditorGUILayout.HelpBox ("File containing Propeller performance Plots", MessageType.None);
		propellerPerformancePlot = EditorGUILayout.ObjectField("Dynamic Plot",propellerPerformancePlot,typeof(TextAsset),true) as TextAsset;
		GUILayout.Space(5f);
		EditorGUILayout.HelpBox ("File containing static propeller performance Plots", MessageType.None);
		propellerStaticPlot = EditorGUILayout.ObjectField("Static Plot",propellerStaticPlot,typeof(TextAsset),true) as TextAsset;
		//
		//
		GUILayout.Space(20f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Make sure data supply points are complete..", MessageType.Info);
		GUI.color = backgroundColor;
		//
		GUILayout.Space(10f);
		if (GUILayout.Button ("Create Bladefoil")) {
				//
			GameObject manager = new GameObject("Data Manager");
			BladefoilCreator builder = manager.AddComponent<BladefoilCreator> ();
			//
			//
			builder.identifier = identifier;
			builder.NACAEquivalent = NACAEquivalent;
			builder.propellerPerformancePlot = propellerPerformancePlot;
			builder.propellerStaticPlot = propellerStaticPlot;
			//
			builder.Data ();
		}
	}
}
