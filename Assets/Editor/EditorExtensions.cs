using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public static class EditorExtensions
{
	public static void Spaces(int spaces = 2)
	{
		for(int i = 0; i < Mathf.Max(spaces, 0); i++)
		{
			EditorGUILayout.Space();
		}
	}

	public static void DrawArraySizeConfiguration<T>(ref T[] _array, string _label = null)
	{
		if(_array == null) _array = new T[0];

		int arraySize = _array.Length;
		
		EditorGUILayout.BeginHorizontal();
		if(!string.IsNullOrEmpty(_label)) GUILayout.Label(_label);
		int newSize = Mathf.Max(0, EditorGUILayout.DelayedIntField("Array Size: ", arraySize));
		int difference = newSize - _array.Length;
		if(difference != 0) Array.Resize(ref _array, newSize);
		if(difference > 0 && arraySize > 0)
		{
			T lastElement = _array[arraySize - 1];
			for(int i = arraySize - 1; i < arraySize + difference; i++)
			{
				_array[i] = lastElement;
			}
		}
		EditorGUILayout.EndHorizontal();
	}

	public static void DrawHorizontalLine()
	{
		EditorGUILayout.LabelField(string.Empty, GUI.skin.horizontalSlider);
	}
}
