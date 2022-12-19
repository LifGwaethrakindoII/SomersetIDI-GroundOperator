using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoidlessUtilities
{
[System.Serializable]
public struct Vector3Range
{
	public Vector3 min;
	public Vector3 max;

	public Vector3Range(Vector3 _min, Vector3 _max)
	{
		min = _min;
		max = _max;
	}
}
}