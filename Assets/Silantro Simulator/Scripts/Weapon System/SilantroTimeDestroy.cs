//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class SilantroTimeDestroy : MonoBehaviour {
	[HideInInspector]public float destroyTime = 5f;
	[HideInInspector]public bool contact;
	// Use this for initialization
	void Start () {
		Destroy (gameObject, destroyTime);	
	}
	//DAMAGE
	void OnCollisionEnter(Collision col)
	{
		if (contact) {
			Destroy (gameObject);
		}
	}
	//
}
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroTimeDestroy))]
public class DetroyerEditor: Editor
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
		SilantroTimeDestroy objectD = (SilantroTimeDestroy)target;
		//
		GUILayout.Space(3f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Timer", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		objectD.destroyTime = EditorGUILayout.FloatField ("Destroy Time", objectD.destroyTime);
		GUILayout.Space(5f);
		objectD.contact = EditorGUILayout.Toggle ("Collision Destroy", objectD.contact); 
		//
		//
		if (GUI.changed) {
			EditorUtility.SetDirty (objectD);
			EditorSceneManager.MarkSceneDirty (objectD.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
	}
}
#endif