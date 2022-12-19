using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEditor;
using System.IO;
//
public class WingEditor : EditorWindow {
	//
	Color backgroundColor;
	Color silantroColor = Color.cyan;
	//
	[HideInInspector]public int toolbarTab;
	[HideInInspector]public string currentTab;
	//
	public enum FileType
	{
		//csv,
		afl
	}
	public FileType fileType = FileType.afl;
	//
	public string identifier = "NACA 00000";
	//

	[HideInInspector]public TextAsset airfoilPlot;
	[HideInInspector]public TextAsset airfoilPerformancePlot;
	//
	//
	//RAW DATA POINTS
	public string airfoilShapePath;
	public string airfoilPerformancePath;
	//[HideInInspector]
	public SilantroAirfoil airfoil;
	//public GameObject newFoil ;
	bool selected;TextAsset asset;
	//
	[MenuItem("Oyedoyin/Airfoil System/Wingfoil/Edit",false,1)]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow<WingEditor>("Wingfoil Editor");
	}
	//

	//
	void OnGUI()
	{
		backgroundColor = GUI.backgroundColor;
		//
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Wingfoil Editor", MessageType.Info);
		GUI.color = backgroundColor;
		GUILayout.Space (5f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Working Airfoil", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space (5f);
		airfoil = EditorGUILayout.ObjectField ("Airfoil", airfoil,typeof(SilantroAirfoil),true) as SilantroAirfoil;
		//
		GUILayout.Space (5f);
		toolbarTab = GUILayout.Toolbar(toolbarTab, new string[]{"Refined Data","Raw Data"});
		//
		asset =(TextAsset)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Data/Airfoils/Shape/NACA Default Shape.csv",typeof(TextAsset));
		if (asset != null) {
			airfoilPlot = asset;
		}
		//
		switch (toolbarTab) {
		case 0:
			//
			currentTab = "Refined Data";
			break;
		case 1:
			//
			currentTab = "Raw Data";
			break;
		}
		//
		switch (currentTab) {
		case "Refined Data":
			//
			//
			GUILayout.Space (5f);
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("Airfoil Identfier", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (5f);
			identifier = EditorGUILayout.TextField ("Identifier", identifier);
			//
			GUILayout.Space (7f);
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("Data Plots", MessageType.None);
			GUI.color = backgroundColor;
			//
			//
			GUILayout.Space (5f);
			EditorGUILayout.HelpBox ("File containing airfoil data points Plots", MessageType.None);
			airfoilPlot = EditorGUILayout.ObjectField ("Airfoil Plot", airfoilPlot, typeof(TextAsset), true) as TextAsset;
			GUILayout.Space (5f);
			EditorGUILayout.HelpBox ("File containing Cl,Cd,Cm, Plots", MessageType.None);
			airfoilPerformancePlot = EditorGUILayout.ObjectField ("Airfoil Performance Plot", airfoilPerformancePlot, typeof(TextAsset), true) as TextAsset;
			//  
			//  
			GUILayout.Space (20f);
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("Make sure data supply points are complete..", MessageType.Info);
			GUI.color = backgroundColor;
			//
			GUILayout.Space (10f);
			if (GUILayout.Button ("Edit Wingfoil")) {
				//
				GameObject manager = new GameObject ("Airfoil Manager");
				WingfoilEditRefinedData refinedBuilder = manager.AddComponent<WingfoilEditRefinedData> ();
				//
				refinedBuilder.airfoil = airfoil;
				//
				refinedBuilder.identifier = identifier;
				refinedBuilder.airfoilPlot = airfoilPlot;
				refinedBuilder.airfoilPerformancePlot = airfoilPerformancePlot;
				//
				refinedBuilder.Create ();
			}
			//
			break;
		case "Raw Data":
			//

			GUILayout.Space (5f);
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("Airfoil Identfier", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (5f);
			identifier = EditorGUILayout.TextField ("Identifier", identifier);
			//
			GUILayout.Space (7f);
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("Data Plots", MessageType.None);
			GUI.color = backgroundColor;
			//
			GUILayout.Space (5f);
			EditorGUILayout.HelpBox ("File containing airfoil data points Plots", MessageType.None);
			airfoilPlot = EditorGUILayout.ObjectField ("Airfoil Plot", airfoilPlot, typeof(TextAsset), true) as TextAsset;
			//
			GUILayout.Space (5f);
			EditorGUILayout.HelpBox ("File containing airfoil performance Plots", MessageType.None);

			//
			//
			GUILayout.Space (10f);
			fileType = (FileType)EditorGUILayout.EnumPopup ("File Type", fileType);
			//
			GUILayout.Space (5f);
			EditorGUILayout.TextField ("Performance Data", airfoilPerformancePath);

			GUILayout.Space (5f);
			if (fileType == FileType.afl) {
				if (GUILayout.Button ("Select Performance File")) {
					//
					airfoilPerformancePath = EditorUtility.OpenFilePanel ("Foil Performance", "", "afl");
					if (airfoilPerformancePath.Length != 0) {
						selected = true;
					}
				}
			} 

			//
			//  
			if (airfoilPlot == null || airfoilPerformancePath== null|| airfoilPerformancePath.Length <= 0) {
				GUILayout.Space (20f);
				GUI.color = silantroColor;
				EditorGUILayout.HelpBox ("Make sure data supply points are complete..", MessageType.Info);
				GUI.color = backgroundColor;
			}
			//
			if (airfoilPerformancePath != null && airfoilPerformancePath.Length > 0) {
				GUILayout.Space (20f);
				GUI.color = silantroColor;
				EditorGUILayout.HelpBox ("Make sure selected data is valid..", MessageType.Info);
				GUI.color = backgroundColor;
				//
				GUILayout.Space (10f);
				if (GUILayout.Button ("Edit Wingfoil")) {
					//
					GameObject manager = new GameObject ("Airfoil Manager");
					WingfoilEditRawData rawBuilder = manager.AddComponent<WingfoilEditRawData> ();
					//
					rawBuilder.airfoil = airfoil;
					//
					rawBuilder.identifier = identifier;
					rawBuilder.airfoilPlot = airfoilPlot;
					rawBuilder.airfoilPerformancePath = airfoilPerformancePath;
					//
					rawBuilder.CreateFromRaw ();
				}
			}

			//
			break;
		}
	}
}
//
//CREATE AIRFOIL FROM RAW DATA
//
public class WingfoilEditRawData: MonoBehaviour
{
	public string identifier = "NACA 00000";
	//
	[HideInInspector]public TextAsset airfoilPlot;
	[HideInInspector]public string airfoilPerformancePath;
	[HideInInspector]public string airfoilPrefabLocation = "Assets/Silantro Simulator/Prefabs/Default/Airfoils/NACA/";
	//
	//
	private char lineSeperator = '\n';
	private char fieldSeperator = ',';
	//
	//Airfoil Data
	private List<float> length = new List<float>();
	private List<float> height = new List<float> ();
	//[HideInInspector]
	[HideInInspector]public SilantroAirfoil airfoil;
	[HideInInspector]public GameObject newFoil ;
	bool reported;
	string oldname;
	//
	public void CreateFromRaw()
	{
		if (airfoil == null) {
			Debug.Log ("No valid airfoil is selected");
		} else {
			//
			oldname = airfoil.Identifier;
			//
			airfoil.name = identifier;
			airfoil.Identifier = identifier;
			//GET AIRFOIL SHAPE
			if (airfoilPlot != null) {
				string[] foilPlots = airfoilPlot.text.Split (lineSeperator);
				for (int j = 1; (j < foilPlots.Length - 1); j++) {
					string[] plots = foilPlots [j].Split (fieldSeperator);
					length.Add (float.Parse (plots [0]));
					height.Add (float.Parse (plots [1]));
				}
				//PLOT SHAPE FROM DATA
				for (int b = 0; b < length.Count; b++) {
					float x = length [b];
					float y = height [b];
					//
					Keyframe germ = new Keyframe (x, y);
					airfoil.airfoil.AddKey (germ);
				}
			} 
			//PLOT PERFORMANCE DATA
			if (airfoilPerformancePath.Length != 0) {
				//
				airfoil.liftCurve = new AnimationCurve ();
				airfoil.dragCurve = new AnimationCurve ();
				airfoil.momentCurve = new AnimationCurve ();
				//
				StreamReader performance = new StreamReader (airfoilPerformancePath);
				string performanceText = "";
				//
				//IGNORE FILE HEADER
				while (!performanceText.Contains ("alpha")) {
					performanceText = performance.ReadLine ();
				}
				//
				//COLLECT DATA
				while (performanceText != null) {
					//
					performanceText = performance.ReadLine ();
					if (null != performanceText) {
						//
						string[] parts = performanceText.Split (null);
						//
						string[] reducedParts = new string[4];
						int j = 0;
						for (int i = 0; i < parts.Length; i++) {
							if (parts [i].Length > 0) {
								reducedParts [j] = parts [i];
								j++;
							}
						}
						////COLLECT ANGLE OF ATTACK
						float alpha = 0.0f;
						float.TryParse (reducedParts [0], out alpha);
						//STOP POINYT
						if ((int)alpha == 180) {
							return;
						}
						//COLLECT LIFT COEFFICIENT
						float lift = 0.0f;
						float.TryParse (reducedParts [1], out lift);
						//COLLECT DRAG COEFFICIENT
						float drag = 0.0f;
						float.TryParse (reducedParts [2], out drag);
						//COLLECT MOMENT COEFFICIENT
						float moment = 0.0f;
						float.TryParse (reducedParts [3], out moment);
						//ADD VALUES TO CURVES
						airfoil.liftCurve.AddKey (alpha, lift);
						airfoil.dragCurve.AddKey (alpha, drag);
						airfoil.momentCurve.AddKey (alpha, moment);
						//
					}
					////
					if (!reported && this.gameObject != null) {
						reported = true;
						Debug.Log ("Airfoil : " + oldname + " Edited Successfully to " + identifier);
						DestroyImmediate (this.gameObject);
					}
					//
				}


			} 

		}
	}
}
//
//CREATE AIRFOIL FROM REFINED DATA
public class WingfoilEditRefinedData: MonoBehaviour
{
	//
	public string identifier = "NACA 00000";
	//
	[HideInInspector]public TextAsset airfoilPlot;
	[HideInInspector]public TextAsset airfoilPerformancePlot;
	[HideInInspector]public string airfoilPrefabLocation = "Assets/Silantro Simulator/Prefabs/Default/Airfoils/NACA/";

	//
	private char lineSeperator = '\n';
	private char fieldSeperator = ',';
	//
	//Airfoil Data
	private List<float> length = new List<float>();
	private List<float> height = new List<float> ();
	//
	//Airfoil Performance Data
	private List<float> angle = new List<float>();
	private List<float> liftCo = new List<float> ();
	private List<float> dragCo = new List<float> ();
	private List<float> momentCo = new List<float> ();
	//
	//[HideInInspector]
	[HideInInspector]public SilantroAirfoil airfoil;
	[HideInInspector]public GameObject newFoil ;
	//
	//
	public void Create()
	{
		//
		airfoil.name = identifier;
		airfoil.Identifier = identifier;
		//
		//CREATE AIRFOIL DATA
		string[] dataPlots = airfoilPerformancePlot.text.Split (lineSeperator);
		for (int i=1; (i<dataPlots.Length-1); i++){
			string[] staticfields = dataPlots[i].Split (fieldSeperator);
			angle.Add (float.Parse (staticfields [0]));
			liftCo.Add (float.Parse (staticfields [1]));
			dragCo.Add(float.Parse(staticfields[2]));
			momentCo.Add(float.Parse(staticfields[3]));
		}
		//
		//PLOT PERFORMANCE DATA
		for (int a = 0; a < angle.Count; a++) {
			float x = angle [a];
			float y = liftCo [a];
			float z = dragCo [a];
			float w = momentCo [a];
			Keyframe lift = new Keyframe (x, y);
			Keyframe drag = new Keyframe (x, z);
			Keyframe moment = new Keyframe (x, w);
			airfoil.liftCurve.AddKey (lift);
			airfoil.dragCurve.AddKey (drag );
			airfoil.momentCurve.AddKey (moment );
		}
		//
		//
		string [] foilPlots = airfoilPlot.text.Split(lineSeperator);
		for (int j = 1; (j < foilPlots.Length - 1); j++) {
			string[] plots = foilPlots [j].Split (fieldSeperator);
			length.Add (float.Parse (plots [0]));
			height.Add (float.Parse (plots [1]));
		}
		//PLOT
		for (int b = 0; b < length.Count; b++) {
			float x = length [b];
			float y = height [b];
			//
			Keyframe germ = new Keyframe (x,y);
			airfoil.airfoil.AddKey (germ);
		}
		//
		Debug.Log("Airfoil : " +identifier + " Edited Successfully");
		DestroyImmediate(this.gameObject);
	} 

}


