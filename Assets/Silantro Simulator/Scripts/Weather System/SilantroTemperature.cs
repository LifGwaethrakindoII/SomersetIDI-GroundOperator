using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
public class SilantroTemperature : MonoBehaviour {
	//
	[HideInInspector]public float maximumTemperature;
	[HideInInspector]public float minimumTemperature;
	[HideInInspector]public AnimationCurve temperature;
}
//
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroTemperature))]
public class TemperatureEditor: Editor
{
	Color backgroundColor;
	Color silantroColor = Color.yellow;
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
		SilantroTemperature temp = (SilantroTemperature)target;
		//
		GUILayout.Space(3f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Properties", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		temp.temperature = EditorGUILayout.CurveField ("Temperature Plot", temp.temperature);
		GUILayout.Space(10f);
		EditorGUILayout.LabelField ("Maximum Temperature", temp.maximumTemperature.ToString ("0.0"));
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Minimum Temperature", temp.minimumTemperature.ToString ("0.0"));
	//
		//
		if (GUI.changed) {
			EditorUtility.SetDirty (temp);
			EditorSceneManager.MarkSceneDirty (temp.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
	}
}
#endif