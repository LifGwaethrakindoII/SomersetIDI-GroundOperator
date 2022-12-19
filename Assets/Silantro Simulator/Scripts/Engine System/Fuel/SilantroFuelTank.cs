//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

//
public class SilantroFuelTank : MonoBehaviour {
	//
	public enum TankType
	{
		Internal,
		External
	}
	[HideInInspector]public TankType tankType = TankType.Internal;
	//
	[HideInInspector]public float Capacity;
	[HideInInspector]public float CurrentAmount;
	[HideInInspector]public bool attached = true;
	//
	//Health Values
	[HideInInspector]public float startingHealth = 100.0f;		// The amount of health to start with
	[HideInInspector]public float currentHealth;
	//
	[HideInInspector]public bool isDestructible = false;
	private bool destroyed;

	[HideInInspector]public GameObject tankGameobject;
	[HideInInspector]public GameObject ExplosionPrefab;
	SilantroFuelDistributor attachedDitributor;
	//
	void Start () {
		currentHealth = startingHealth;CurrentAmount = Capacity;
		//
		if (ExplosionPrefab != null)
			ExplosionPrefab.SetActive (false);
	}
	//
	public void Detach()
	{
		CurrentAmount = 0;
		if (attachedDitributor != null) {
			if (this.GetComponent<SilantroFuelTank>().tankType == TankType.External && attachedDitributor.externalTanks.Contains (this.GetComponent<SilantroFuelTank> ())) {
				attachedDitributor.externalTanks.Remove (this.GetComponent<SilantroFuelTank> ());
			}
		}
		attached = false;if(tankGameobject){tankGameobject.AddComponent<CapsuleCollider>();tankGameobject.AddComponent<Rigidbody>().mass = Capacity;}tankGameobject.transform.parent = null;
	//Remove fuel from total amount
	}
	//HIT SYSTEM
	public void SilantroDamage(float amount)
	{
		currentHealth += amount;

		// If the health runs out, then Die.
		if (currentHealth < 0)
		{
			currentHealth = 0;
		}
		//Die Procedure
		if (currentHealth == 0 && !destroyed)
			CurrentAmount = 0;Disintegrate();
	}
	//
	//DESTRUCTION SYSTEM
	public void Disintegrate()
	{
		if (isDestructible) {
			destroyed = true;
			//ACTIVATE EXPLOSION AND FIRE
			if (ExplosionPrefab != null)
				Instantiate (ExplosionPrefab, transform.position, Quaternion.identity);
			ExplosionPrefab.GetComponentInChildren<AudioSource> ().Play ();
			//
			//DESTROY GAMEOBJECT
			Destroy (gameObject);
		}
	}
	//
	//DAMAGE
	void OnCollisionEnter(Collision col)
	{
		if (col.relativeVelocity.magnitude > 50f) {
			Disintegrate ();
		}
	}
	void Update()
	{
		if (CurrentAmount < 0f)
		{
			CurrentAmount = 0f;
		}
	}
}
//
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroFuelTank))]
public class FuelTankEditor: Editor
{
	Color backgroundColor;
	public override void OnInspectorGUI()
	{
		backgroundColor = GUI.backgroundColor;
		//
		DrawDefaultInspector ();
		SilantroFuelTank tank = (SilantroFuelTank)target;
		//
		serializedObject.Update();
		//
		GUILayout.Space(10f);
		GUI.color = Color.yellow;
		EditorGUILayout.HelpBox ("Tank Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		tank.tankType = (SilantroFuelTank.TankType)EditorGUILayout.EnumPopup ("Tank Type", tank.tankType);
		GUILayout.Space(10f);
		tank.Capacity = EditorGUILayout.FloatField ("Capacity", tank.Capacity);
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Current Amount", tank.CurrentAmount.ToString ("0.00") + " kg");
		//
		GUILayout.Space(20f);
		tank.isDestructible = EditorGUILayout.Toggle ("Is Destructible", tank.isDestructible);
		//
		GUILayout.Space(3f);
		if (tank.isDestructible) {
			GUILayout.Space(1f);
			GUI.color = Color.white;
			EditorGUILayout.HelpBox ("Health Settings", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space(3f);
			tank.startingHealth = EditorGUILayout.FloatField ("Starting Health", tank.startingHealth);
			GUILayout.Space(2f);
			EditorGUILayout.LabelField ("Current Health", tank.currentHealth.ToString("0.00"));
			GUI.color = Color.white;
			EditorGUILayout.HelpBox ("Destruction Settings", MessageType.None);
			GUI.color = backgroundColor;
			tank.tankGameobject = EditorGUILayout.ObjectField ("Tank BodyObject", tank.tankGameobject, typeof(GameObject), true) as GameObject;
			tank.ExplosionPrefab = EditorGUILayout.ObjectField ("Explosion Prefab", tank.ExplosionPrefab, typeof(GameObject), true) as GameObject;
		}
		if (GUI.changed) {
			EditorUtility.SetDirty (tank);
			EditorSceneManager.MarkSceneDirty (tank.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
	}
}
#endif
