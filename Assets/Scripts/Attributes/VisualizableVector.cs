using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Field, AllowMultiple = true)]
public class VisualizableVector : PropertyAttribute
{
	private Vector3 _vector;

	/// <summary>Gets and Sets vector property.</summary>
	public Vector3 vector
	{
		get { return _vector; }
		set { _vector = value; }
	}

	public VisualizableVector(Vector3 _vector)
	{
		vector = _vector;
	}

	public VisualizableVector(){}
}
