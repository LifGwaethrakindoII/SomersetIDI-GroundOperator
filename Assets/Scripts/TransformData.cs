using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TransformData
{
	[SerializeField] private Vector3 _position;
	[SerializeField] private Vector3 _eulerRotation;
	[SerializeField] private Quaternion _rotation;
	[SerializeField] private Vector3 _scale;

	/// <summary>Gets and Sets position property.</summary>
	public Vector3 position
	{
		get { return _position; }
		set { _position = value; }
	}

	/// <summary>Gets and Sets eulerRotation property.</summary>
	public Vector3 eulerRotation
	{
		get { return _eulerRotation; }
		set
		{
			_eulerRotation = value;
			_rotation = Quaternion.Euler(_eulerRotation);
		}
	}

	/// <summary>Gets and Sets rotation property.</summary>
	public Quaternion rotation
	{
		get { return _rotation; }
		set { _rotation = value; }
	}

	/// <summary>Gets and Sets scale property.</summary>
	public Vector3 scale
	{
		get { return _scale; }
		set { _scale = value; }
	}

	public static implicit operator TransformData(Transform _transform) { return new TransformData(_transform); }

	public TransformData() {  }

	public TransformData(Transform _transform)
	{
		position = _transform.position;
		rotation = _transform.rotation;
		scale = _transform.localScale;
	}

	public void UpdateTransform(Transform _transform)
	{
		_transform.position = position;
		_transform.rotation = rotation;
		_transform.localScale = scale;
	}
}
