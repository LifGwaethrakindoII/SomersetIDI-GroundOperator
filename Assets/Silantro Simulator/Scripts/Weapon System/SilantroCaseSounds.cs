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

public class SilantroCaseSounds : MonoBehaviour {

	[HideInInspector]public AudioClip[] sounds;
	[HideInInspector]public float soundRange = 300f;
	[HideInInspector]private AudioSource audio;
	[HideInInspector]public float soundVolume =0.4f;
	[HideInInspector]public int soundCount = 1;

	// Use this for initialization
	void OnCollisionEnter (Collision col) {
		if (col.collider.tag == "Ground") {
			AudioSource audio = gameObject.AddComponent<AudioSource> ();
			audio.dopplerLevel = 0f;
			audio.spatialBlend = 1f;
			audio.rolloffMode = AudioRolloffMode.Custom;
			audio.maxDistance = soundRange;
			audio.volume = soundVolume;
			audio.PlayOneShot (sounds [Random.Range (0, sounds.Length)]);
		}
	}

}
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroCaseSounds))]
public class SoundEditor: Editor
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
		SilantroCaseSounds sounds = (SilantroCaseSounds)target;
		//
		GUILayout.Space(3f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Sounds", MessageType.None);
		GUI.color = backgroundColor;
		//sounds.soundCount = EditorGUILayout.IntField("Sound Clips",sounds.soundCount);
		GUILayout.Space(5f);
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.BeginVertical ();
		//
		GUIContent soundLabel = new GUIContent ("Sound Clips");

		SerializedProperty muzs = this.serializedObject.FindProperty("sounds");
		EditorGUILayout.PropertyField (muzs.FindPropertyRelative ("Array.size"),soundLabel);
		GUILayout.Space(3f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Clips", MessageType.None);
		GUI.color = backgroundColor;
		for (int i = 0; i < muzs.arraySize; i++) {
			GUIContent label = new GUIContent("Clip " +(i+1).ToString ());
			EditorGUILayout.PropertyField (muzs.GetArrayElementAtIndex (i), label);
		}
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.EndVertical ();
		//
		GUILayout.Space(3f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Settings", MessageType.None);
		GUI.color = backgroundColor;
		//
		GUILayout.Space (3f);
		sounds.soundRange = EditorGUILayout.FloatField("Range",sounds.soundRange);
		GUILayout.Space (2f);
		sounds.soundVolume = EditorGUILayout.Slider ("Volume", sounds.soundVolume,0f,1f);
		//
		//
		if (GUI.changed) {
			EditorUtility.SetDirty (sounds);
			EditorSceneManager.MarkSceneDirty (sounds.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
		serializedObject.ApplyModifiedProperties();
	}
}
#endif