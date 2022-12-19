using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class SilantroAirfoil : MonoBehaviour 
{
	[HideInInspector]public string Identifier = "NACA Default";
	//
	[HideInInspector]public AnimationCurve airfoil = null;
	//
	[HideInInspector]public AnimationCurve liftCurve = AnimationCurve.Linear(-180.0f,0.0f,180,0);
	[HideInInspector]public AnimationCurve dragCurve =  AnimationCurve.Linear(-180.0f,0.0f,180,0);
	[HideInInspector]public AnimationCurve momentCurve = AnimationCurve.Linear(-180.0f,0.0f,180,0);
	//
	[HideInInspector]public float stallAngle;
	[HideInInspector]public float maxClCd;
	[HideInInspector]public float NCrit = 9;
	[HideInInspector]public float reynoldsNumber = 50000f;
}
//
//
//
//
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroAirfoil))]
public class AirfoilEditor: Editor
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
		SilantroAirfoil foil = (SilantroAirfoil)target;
		//
		GUILayout.Space(3f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Properties", MessageType.None);
		GUI.color = backgroundColor;
		//Write Airfoil Name
		EditorGUILayout.LabelField ("Identifier",foil.Identifier);
		GUILayout.Space(3f);
		//Draw airfoil
		foil.airfoil = EditorGUILayout.CurveField ("Airfoil Shape", foil.airfoil);
		GUILayout.Space(3f);
		GUILayout.Space(5f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Data Plots", MessageType.None);
		GUI.color = backgroundColor;
		//
		GUILayout.Space(5f);
		foil.liftCurve = EditorGUILayout.CurveField ("Lift Curve", foil.liftCurve);
		foil.dragCurve = EditorGUILayout.CurveField ("Drag Curve", foil.dragCurve);
		foil.momentCurve = EditorGUILayout.CurveField ("Moment Curve", foil.momentCurve);
		GUILayout.Space(5f);
		//
		GUI.color = Color.yellow;
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
		if (GUI.changed) {
			EditorUtility.SetDirty (foil);
			EditorSceneManager.MarkSceneDirty (foil.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
	}
}
#endif