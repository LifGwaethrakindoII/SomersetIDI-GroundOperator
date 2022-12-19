//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class SilantroControls : MonoBehaviour {
	//
	[Header("Buttons")]
	[HideInInspector]public string engineStart;
	[HideInInspector]public string engineShutdown;
	[HideInInspector]public string engineThrottleControl;
	[HideInInspector]public string LandingGear;
	[HideInInspector]public string BrakeHoldRelease;
	[HideInInspector]public string AfterburnerControl;
	[HideInInspector]public string reverseThrustControl;
	[HideInInspector]public string dumpFuelControl;
	[HideInInspector]public string refuelControl;
	[HideInInspector]public string VTOLTransition;
	[HideInInspector]public string STOLTransition;
	[HideInInspector]public string NormalTransition;
	[HideInInspector]public string IncrementalBrake;
	[HideInInspector]public string FlapIncrease;
	[HideInInspector]public string FlapDecrease;
	[HideInInspector]public string SlatActuator;
	[HideInInspector]public string SpoilerActuator;
	[HideInInspector]public string LightSwitch;
	[HideInInspector]public string BombDrop;
	//
	[HideInInspector]public string Aileron;
	[HideInInspector]public string Elevator;
	[HideInInspector]public string Rudder;
	[HideInInspector]public AnimationCurve controlCurve;
	//


}
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroControls))]
public class ControlsEditor: Editor
{
	Color backgroundColor;
	public override void OnInspectorGUI()
	{
		backgroundColor = GUI.backgroundColor;
		//
		DrawDefaultInspector ();
		SilantroControls control = (SilantroControls)target;
		//
		serializedObject.Update();
		//
		GUILayout.Space(10f);
		//
		GUI.color = Color.yellow;
		EditorGUILayout.HelpBox ("Control Buttons", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		//
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Engine", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		control.engineStart = EditorGUILayout.TextField ("Engine Start", control.engineStart);
		GUILayout.Space(3f);
		control.engineShutdown = EditorGUILayout.TextField ("Engine Shutdown", control.engineShutdown);
		GUILayout.Space(3f);
		control.reverseThrustControl = EditorGUILayout.TextField ("Reverse Thrust", control.reverseThrustControl);
		GUILayout.Space(3f);
		control.AfterburnerControl = EditorGUILayout.TextField ("Afterburner Switch", control.AfterburnerControl);
		//
		//
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Fuel", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		control.dumpFuelControl = EditorGUILayout.TextField (" Dump Fuel", control.dumpFuelControl);
		GUILayout.Space(3f);
		control.refuelControl = EditorGUILayout.TextField ("Refill Tank", control.refuelControl);
		GUILayout.Space(3f);
		//
		//
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Actuators", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		control.LandingGear = EditorGUILayout.TextField ("Landing Gear", control.LandingGear);
		GUILayout.Space(3f);
		control.BrakeHoldRelease = EditorGUILayout.TextField ("Parking Brake", control.BrakeHoldRelease);
		GUILayout.Space(3f);
		control.IncrementalBrake = EditorGUILayout.TextField ("Incremental Brake", control.IncrementalBrake);
		GUILayout.Space(3f);
		control.SpoilerActuator = EditorGUILayout.TextField ("Spoiler Actuator", control.SpoilerActuator);
		GUILayout.Space(3f);
		control.SlatActuator = EditorGUILayout.TextField ("Slat Actuator", control.SlatActuator);
		//
		GUILayout.Space(3f);
		control.LightSwitch = EditorGUILayout.TextField ("Light Switch", control.LightSwitch);
		GUILayout.Space(3f);
		control.FlapIncrease = EditorGUILayout.TextField ("Flap Increase", control.FlapIncrease);
		GUILayout.Space(3f);
		control.FlapDecrease = EditorGUILayout.TextField ("Flap Decrease", control.FlapDecrease);
		//
		//
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("STOVL Control", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		control.VTOLTransition = EditorGUILayout.TextField ("VTOL Transition", control.VTOLTransition);
		GUILayout.Space(3f);
		control.STOLTransition = EditorGUILayout.TextField ("STOL Transition", control.STOLTransition);
		GUILayout.Space(3f);
		control.NormalTransition = EditorGUILayout.TextField ("Normal Transition", control.NormalTransition);
		//
		//
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Weapons", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		control.BombDrop = EditorGUILayout.TextField ("Bomb Drop", control.BombDrop);
		//
		//
		GUILayout.Space(15f);
		//
		GUI.color = Color.yellow;
		EditorGUILayout.HelpBox ("Control Levers", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		control.engineThrottleControl = EditorGUILayout.TextField ("Engine Throttle", control.engineThrottleControl);
		GUILayout.Space(3f);
		control.Aileron = EditorGUILayout.TextField ("Engine Throttle", control.Aileron);
		GUILayout.Space(3f);
		control.Elevator = EditorGUILayout.TextField ("Engine Throttle", control.Elevator);
		GUILayout.Space(3f);
		control.Rudder = EditorGUILayout.TextField ("Engine Throttle", control.Rudder);
		//
		//
		GUILayout.Space(15f);
		//
		GUI.color = Color.yellow;
		EditorGUILayout.HelpBox ("Control Determinant", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		control.controlCurve = EditorGUILayout.CurveField ("Control Curve", control.controlCurve);
		//
		GUILayout.Space(15f);
		//

		//
		if (GUI.changed) {
			EditorUtility.SetDirty (control);
			EditorSceneManager.MarkSceneDirty (control.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
	}
}
#endif