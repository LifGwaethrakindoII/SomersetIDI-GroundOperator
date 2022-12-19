using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Supercargo;

[CustomEditor(typeof(AirplaneEngineDijkstra))]
public class AirplaneEngineDijkstraInspector : Editor
{
	private AirplaneEngineDijkstra o;
	private Command command;

	private void OnEnable()
	{
		o = target as AirplaneEngineDijkstra;
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		EditorGUILayout.BeginHorizontal();
		command = (Command)EditorGUILayout.EnumPopup("Forced Command: ", command);
		if(GUILayout.Button("Force Command Evaluation")) o.OnPatternRecognized(command);
		EditorGUILayout.EndHorizontal();
		EditorUtility.SetDirty(o);
	}
}
