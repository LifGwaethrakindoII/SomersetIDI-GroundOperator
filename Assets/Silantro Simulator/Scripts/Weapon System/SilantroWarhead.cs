using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
//
public class SilantroWarhead : MonoBehaviour {

	public enum ExplosiveType
	{
		RDX,
		TNT,
		RDX_TNT,
		PETN,
		Nitroglycerine,

	}
	[HideInInspector]public ExplosiveType explosiveType = ExplosiveType.RDX;
	//EXPLOSIVE PROPERTIES
	[HideInInspector]public float density;
	[HideInInspector]public float detonationVelocity;
	[HideInInspector]public float streamingVelocity;
	[HideInInspector]public float detonationPressure;
	[HideInInspector]public float energy;
	//
	//
	[HideInInspector]public GameObject explosionPrefab;
	//
	bool exploded;
	//
	//EXPLODE
	public void Explode(Vector3 position)
	{
		if (explosionPrefab && !exploded) {
			GameObject explosion = Instantiate (explosionPrefab, position, Quaternion.identity);
			explosion.SetActive (true);
			explosion.GetComponentInChildren<AudioSource> ().Play ();
			exploded = true;
		}
	}
}
//
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroWarhead))]
public class WarheadEditor: Editor
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
		SilantroWarhead explosive = (SilantroWarhead)target;
		//
		GUILayout.Space(3f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Explosive Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		explosive.explosiveType = (SilantroWarhead.ExplosiveType)EditorGUILayout.EnumPopup ("Explosive Type", explosive.explosiveType);
		GUILayout.Space(3f);
		if (explosive.explosiveType == SilantroWarhead.ExplosiveType.Nitroglycerine)
		{
			explosive.density = 1.60f;
			explosive.detonationPressure = 254.6f;
			explosive.energy = 6283f;
			explosive.streamingVelocity = 1550f;
			explosive.detonationVelocity = 7327f;

		}
		else if (explosive.explosiveType == SilantroWarhead.ExplosiveType.PETN) 
		{
			explosive.density = 1.60f;
			explosive.detonationPressure = 254.6f;
			explosive.energy = 5881f;
			explosive.streamingVelocity = 1550f;
			explosive.detonationVelocity = 7327f;
		}
		else if (explosive.explosiveType == SilantroWarhead.ExplosiveType.RDX) 
		{
			explosive.density = 1.762f;
			explosive.detonationPressure = 337.9f;
			explosive.energy = 5763f;
			explosive.streamingVelocity = 2213f;
			explosive.detonationVelocity = 8639f;
		} 
		else if (explosive.explosiveType == SilantroWarhead.ExplosiveType.RDX_TNT)
		{
			explosive.density = 1.743f;
			explosive.detonationPressure =312.5f;
			explosive.energy = 4985f;
			explosive.streamingVelocity = 2173f;
			explosive.detonationVelocity = 8252f;
		} 
		else if (explosive.explosiveType == SilantroWarhead.ExplosiveType.TNT)
		{
			explosive.density = 1.637f;
			explosive.detonationPressure = 189.1f;
			explosive.energy = 5810f;
			explosive.streamingVelocity = 1664f;
			explosive.detonationVelocity = 6942f;
		} 
		//
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Density",explosive.density.ToString("0.000")+" gm/l");
		GUILayout.Space(1f);
		EditorGUILayout.LabelField ("Detonation Velocity",explosive.detonationVelocity.ToString("0")+" m/s");
		GUILayout.Space(1f);
		EditorGUILayout.LabelField ("Streaming Velocity",explosive.streamingVelocity.ToString("0")+" m/s");
		GUILayout.Space(1f);
		EditorGUILayout.LabelField ("Detonation Pressure",explosive.detonationPressure.ToString("0")+" e+8 Mpa");
		GUILayout.Space(1f);
		EditorGUILayout.LabelField ("Energy",explosive.energy.ToString("0")+" J/g");
		//
		GUILayout.Space(5f);
		explosive.explosionPrefab = EditorGUILayout.ObjectField ("Explosion Prefab", explosive.explosionPrefab, typeof(GameObject), true) as GameObject;
	//\
		//
		if (GUI.changed) {
			EditorUtility.SetDirty (explosive);
			EditorSceneManager.MarkSceneDirty (explosive.gameObject.scene);
		}
	}
}
#endif