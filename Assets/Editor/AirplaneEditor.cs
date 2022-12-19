using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Supercargo
{
[CustomEditor(typeof(Airplane))]
public class AirplaneEditor : Editor
{
	private Airplane airplane;

	private void OnEnable()
	{
		airplane = target as Airplane;
	}

	public override void OnInspectorGUI()
    {
    	DrawDefaultInspector();
    	if(GUILayout.Button("Obtain Back Wheels")) airplane.ObtainBackWheels();
    }
}
}