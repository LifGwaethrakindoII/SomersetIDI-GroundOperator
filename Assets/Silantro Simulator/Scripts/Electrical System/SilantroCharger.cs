using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
//
public class SilantroCharger : MonoBehaviour {
	public enum PowerSource
	{
		//Generator,
		SolarPanel,
		//WindTurbine
	}
	[HideInInspector]public PowerSource powerSource = PowerSource.SolarPanel;
	//
	[HideInInspector]public SilantroSolarPanel panel;
	[HideInInspector]public float inputVoltage;
	[HideInInspector]public float inputCurrent;
	//
	[HideInInspector]public float outputVoltage;
	[HideInInspector]public float outputCurrent;
	[HideInInspector]public float chargingVoltage;
	//
	[HideInInspector]public SilantroBattery currentBattery;
	//
	[HideInInspector]public bool Activated;
	[HideInInspector]public bool notSuitable;
	[HideInInspector]public bool charging;
	//
	public void Charge()
	{
		//float suitableCurrent = currentBattery.capacity * 0.1f;
		float batteryVoltage = currentBattery.actualVoltage;
		//
		if (outputVoltage < (batteryVoltage*(currentBattery.chargeEfficiency/100f))) {
			notSuitable = true;charging = false;
			currentBattery.state = SilantroBattery.State.Discharging;
		} else {
			charging = true;
			currentBattery.state = SilantroBattery.State.Charging;
			currentBattery.chargingCurrent = outputCurrent;
		}
	}
	//
	void Update()
	{
		//
		if (powerSource == PowerSource.SolarPanel) {
			if (panel) {
				inputVoltage = panel.voltage;
				inputCurrent = panel.current;
				//
				outputCurrent = inputCurrent *0.984f;
				float chargeVoltage = currentBattery.actualVoltage;
				if (inputVoltage > chargeVoltage) {
					outputVoltage = currentBattery.actualVoltage * 1.15f;
				} else {
					outputVoltage = 0.0f;
					if (Activated) {
						//Activated = false;
					}
				}
			}
		}
		if (Activated) {
			Charge ();
		}
	}
}
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroCharger))]
[CanEditMultipleObjects]
public class ChargerEditor: Editor
{
	Color backgroundColor;
	Color silantroColor = Color.cyan;
	//

	public override void OnInspectorGUI()
	{
		backgroundColor = GUI.backgroundColor;
		//
		DrawDefaultInspector ();serializedObject.Update ();
		//
		SilantroCharger charger = (SilantroCharger)target;
		//
		GUILayout.Space(3f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Input", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		charger.powerSource = (SilantroCharger.PowerSource)EditorGUILayout.EnumPopup("Power Source",charger.powerSource);
		//
		if (charger.powerSource == SilantroCharger.PowerSource.SolarPanel) {
			GUILayout.Space(5f);
			charger.panel = EditorGUILayout.ObjectField ("Solar Panel", charger.panel, typeof(SilantroSolarPanel), true) as SilantroSolarPanel;
		}
		//
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Input Voltage", charger.inputVoltage.ToString ("0.0") + " V");
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Input Current", charger.inputCurrent.ToString ("0.0") + " Amps");
		//
		GUILayout.Space(20f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Output", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		charger.currentBattery = EditorGUILayout.ObjectField ("Battery", charger.currentBattery, typeof(SilantroBattery), true) as SilantroBattery;
		GUILayout.Space(3f);
		charger.Activated = EditorGUILayout.Toggle ("Activated", charger.Activated);
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Output Current", charger.outputCurrent.ToString ("0.0") + " Amps");
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Output Voltage", charger.outputVoltage.ToString ("0.0") + " Volts");
	//
		//
		if (GUI.changed) {
			EditorUtility.SetDirty (charger);
			EditorSceneManager.MarkSceneDirty (charger.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
	}
}
#endif