using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEditor;
using System.Linq;

public class WeatherPlotter : MonoBehaviour {
	//
	public string Identifier;
	//
	private char lineSeperator = '\n';
	private char fieldSeperator = ',';
	//
	public TextAsset temperaturePlot;
	//
	[HideInInspector]public string PrefabLocation = "Assets/Prefabs/Default/Weather/";

	//Temperature Data
	private List<float> time = new List<float>();
	private List<float> temperature = new List<float> ();
	//
	//[HideInInspector]
	[HideInInspector]public SilantroTemperature temperatureSettings;
	public GameObject newTemp ;
	//
	void Start()
	{
		PlotData ();
	}
	//
	public void PlotData()
	{
		temperatureSettings = newTemp.GetComponent<SilantroTemperature> ();
		//
		//temperatureSettings.Identifier = identifier;
		//
		string [] foilPlots = temperaturePlot.text.Split(lineSeperator);
		for (int j = 1; (j < foilPlots.Length - 1); j++) {
			string[] plots = foilPlots [j].Split (fieldSeperator);
			time.Add (float.Parse (plots [0]));
			temperature.Add (float.Parse (plots [1]));
		}
		float minimum = temperature.Min ();
		float maximum = temperature.Max ();
		//PLOT
		for (int b = 0; b < time.Count; b++) {
			float x = time [b];
			float y = temperature [b];
			//
			Keyframe germ = new Keyframe (x,y);
			temperatureSettings.temperature.AddKey (germ);
		}
		temperatureSettings.maximumTemperature = maximum;
		temperatureSettings.minimumTemperature = minimum;
		//
		DestroyImmediate(this.gameObject);
	}
}

public class SilantroWeatherPlotter :EditorWindow  {
	//
	Color backgroundColor;
	Color silantroColor = Color.cyan;
	//
	public string identifier = "Local Weather";
	///
	[HideInInspector]public TextAsset temperaturePlot;
	//
	public SilantroTemperature tempy;
	public GameObject newTemp ;
	// Use this for initialization

	//
	void OnGUI()
	{
		backgroundColor = GUI.backgroundColor;
		//
		//
		GUILayout.Space (5f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Temperature Settings", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(5f);
		identifier = EditorGUILayout.TextField ("Identifier", identifier);
		GUILayout.Space(7f);
		temperaturePlot = EditorGUILayout.ObjectField("Temperature Plot",temperaturePlot,typeof(TextAsset),true) as TextAsset;
		//
		GUILayout.Space(10f);
		if (GUILayout.Button ("Plot Temperature")) {
			//
			GameObject manager = new GameObject("Weather Manager");
			WeatherPlotter builder = manager.AddComponent<WeatherPlotter> ();
			//
			//
			builder.Identifier = identifier;
			builder.temperaturePlot = temperaturePlot;
			//
			builder.PlotData ();
		}
	}
}