using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
//
public class SilantroWeaponsManager : MonoBehaviour {

	[HideInInspector]public SilantroMinigun[] guns;
	[HideInInspector]public SilantroRocketPod[] rockets;
	[HideInInspector]public SilantroBombPod bombs;
	//
	[HideInInspector]public int availableWeapons = 0;
	[HideInInspector]public List<string> weapons = new List<string>();
	//
	[HideInInspector]public string currentWeapon;
	[HideInInspector]public int selectedWeapon;
	// Use this for initialization
	void Start () {
		//INITIALIZE MINIGUNS
		guns = GetComponentsInChildren<SilantroMinigun> ();
		if (guns.Length > 0) {
			weapons.Add ("Minigun");
		}
		//INITIALIZE ROCKETS
		rockets = GetComponentsInChildren<SilantroRocketPod> ();
		if (rockets.Length > 0) {
			weapons.Add ("Rockets");
		}
		//INITIALIZE BOMBS
		bombs = GetComponentInChildren<SilantroBombPod> ();
		if (bombs != null) {
			weapons.Add ("Bombs");
		}
		availableWeapons = weapons.Count;
		//
		//SELECT INITIAL WEAPON
		selectedWeapon = 0;
		currentWeapon = weapons [selectedWeapon];
		//
		SetupWeapon(currentWeapon);
	}
	//
	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Z)) {
			ChangeWeapon ();
		}
	}
	//
	void ChangeWeapon()
	{
		selectedWeapon += 1;
		if (selectedWeapon > (availableWeapons-1)) {
			selectedWeapon = 0;
		}
		currentWeapon = weapons [selectedWeapon];
		//
		SetupWeapon(currentWeapon);
	}
	//
	public void SetupWeapon(string currentWeapon)
	{
		//ACTIVATE CURRENT
		if (currentWeapon == "Minigun") {
			foreach (SilantroMinigun gun in guns) {
				gun.isOnline = true;
			}
			foreach (SilantroRocketPod pod in rockets) {
				pod.isOnline = false;
			}
			bombs.isOnline = false;
		}
		if (currentWeapon == "Rockets") {
			foreach (SilantroMinigun gun in guns) {
				gun.isOnline = false;
			}
			foreach (SilantroRocketPod pod in rockets) {
				pod.isOnline = true;
			}
			bombs.isOnline = false;
		}
		if (currentWeapon == "Bombs") {
			foreach (SilantroMinigun gun in guns) {
				gun.isOnline = false;
			}
			foreach (SilantroRocketPod pod in rockets) {
				pod.isOnline = false;
			}
			bombs.isOnline = true;
		}
		//
	}
}
//
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroWeaponsManager))]
public class StoresEditor: Editor
{
	Color backgroundColor;
	Color silantroColor = Color.cyan;
	//

	public override void OnInspectorGUI()
	{
		backgroundColor = GUI.backgroundColor;
		//
		DrawDefaultInspector ();serializedObject.Update();
		//
		SilantroWeaponsManager stores = (SilantroWeaponsManager)target;
		//
		GUILayout.Space(10f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("System Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Available Weapons", stores.availableWeapons.ToString ());
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Current Weapon", stores.currentWeapon);
	}
}
#endif