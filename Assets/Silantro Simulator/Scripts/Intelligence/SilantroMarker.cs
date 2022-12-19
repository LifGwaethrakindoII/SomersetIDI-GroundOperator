using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
//
public class SilantroMarker : MonoBehaviour {
	//
	[HideInInspector]public string silantoTag;
	[HideInInspector]public Texture2D silantroTexture;
	//public Color color;
	//
}
//
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroMarker))]
public class MarkerEditor: Editor
{
	Color backgroundColor;
	Color silantroColor = Color.yellow;
	//
	public override void OnInspectorGUI ()
	{
		backgroundColor = GUI.backgroundColor;
		//
		DrawDefaultInspector ();
		SilantroMarker mark = (SilantroMarker)target;
		//
		serializedObject.Update ();
		//
		GUILayout.Space (2f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Marker Setup", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space (3f);
		mark.silantoTag = EditorGUILayout.TextField ("Definition", mark.silantoTag);
		GUILayout.Space (3f);
		mark.silantroTexture = EditorGUILayout.ObjectField ("Radar Texture", mark.silantroTexture, typeof(Texture2D), true) as Texture2D;
		//
		if (GUI.changed) {
			EditorUtility.SetDirty (mark);
			EditorSceneManager.MarkSceneDirty (mark.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
	}
}
#endif