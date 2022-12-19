using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
//
public class SilantroBladefoil : MonoBehaviour {

	[HideInInspector]public string identifier = "BLD 0256";
	[HideInInspector]public string NACAEquivalent = "NACA 0000";
	//
	//[HideInInspector]public AnimationCurve bladefoil = null;
	//Propeller
	[HideInInspector]public AnimationCurve thrustCurve = null;
	[HideInInspector]public AnimationCurve powerCurve = null;
	[HideInInspector]public AnimationCurve etaCurve = null;
	//
	//Static Plot
	[HideInInspector]public AnimationCurve StaticThrustCurve = null;
	[HideInInspector]public AnimationCurve StaticPowerCurve = null;
	//
	[HideInInspector]public float stallAngle;
	[HideInInspector]public float maxClCd;
	[HideInInspector]public float NCrit = 9;
	[HideInInspector]public float reynoldsNumber = 50000f;

}
//
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroBladefoil))]
public class BladefoilEditor: Editor
{
	Color backgroundColor;
	Color silantroColor = Color.yellow;
	//
	public override void OnInspectorGUI()
	{
		backgroundColor = GUI.backgroundColor;
		//
		DrawDefaultInspector ();serializedObject.Update();
		//
		serializedObject.Update();
		//
		SilantroBladefoil foil = (SilantroBladefoil)target;
		//
		GUILayout.Space(3f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Properties", MessageType.None);
		GUI.color = backgroundColor;
		//Write Airfoil Name
		EditorGUILayout.LabelField ("Identifier",foil.identifier);
		GUILayout.Space(1f);
		EditorGUILayout.LabelField ("NACA Equivalent",foil.NACAEquivalent);
		GUILayout.Space(3f);
		//Draw airfoil
		//foil.bladefoil = EditorGUILayout.CurveField ("Airfoil Shape", foil.bladefoil);
		//
		GUILayout.Space(5f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Data Plots", MessageType.None);
		GUI.color = backgroundColor;
		//
		GUILayout.Space(5f);//
		foil.thrustCurve = EditorGUILayout.CurveField ("Thrust Curve", foil.thrustCurve);
		foil.powerCurve = EditorGUILayout.CurveField ("Power Curve", foil.powerCurve);
		foil.etaCurve = EditorGUILayout.CurveField ("Efficiency Curve", foil.etaCurve);
		//
		EditorGUILayout.HelpBox ("Static Data Plots", MessageType.None);
		//
		foil.StaticThrustCurve = EditorGUILayout.CurveField ("Static Thrust Curve", foil.StaticThrustCurve);
		foil.StaticPowerCurve = EditorGUILayout.CurveField ("Static Power Curve", foil.StaticPowerCurve);
		//
		//
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Performance Data", MessageType.None);
		GUI.color = backgroundColor;
		//
		GUILayout.Space(5f);
		//
		EditorGUILayout.LabelField ("Reynolds Number",foil.reynoldsNumber.ToString());
		EditorGUILayout.LabelField ("Ncrit",foil.NCrit.ToString());
		EditorGUILayout.LabelField ("Stall Angle",foil.stallAngle.ToString("0.00"));
		EditorGUILayout.LabelField ("Max Cl/Cd",foil.maxClCd.ToString("0.00"));
		//
		//
		if (GUI.changed) {
			EditorUtility.SetDirty (foil);
			EditorSceneManager.MarkSceneDirty (foil.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
	}

}
#endif