using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Supercargo
{
[RequireComponent(typeof(Rigidbody))]
public class SteeringVehicle : MonoBehaviour
{
	[SerializeField] private float _minSpeed; 			/// <summary>Vehicle's Minimum Speed.</summary>
	[SerializeField] private float _maxSpeed; 			/// <summary>Vehicle's Maximum Speed.</summary>
	[SerializeField] private float _minSteeringForce; 	/// <summary>Vehicle's Minimum Steering Force.</summary>
	[SerializeField] private float _maxSteeringForce; 	/// <summary>Vehicle's Maximum Steering Force.</summary>
	private Rigidbody _rigidbody; 						/// <summary>Rigidbody's Component.</summary>

	/// <summary>Gets and Sets minSpeed property.</summary>
	public float minSpeed
	{
		get { return _minSpeed; }
		set { _minSpeed = value;  Debug.LogError("[SteeringVehicle] NOOOOO!!!"); }
	}

	/// <summary>Gets and Sets maxSpeed property.</summary>
	public float maxSpeed
	{
		get { return _maxSpeed; }
		set { _maxSpeed = value; Debug.LogError("[SteeringVehicle] NOOOOO!!!"); }
	}

	/// <summary>Gets and Sets minSteeringForce property.</summary>
	public float minSteeringForce
	{
		get { return _minSteeringForce; }
		set { _minSteeringForce = value; }
	}

	/// <summary>Gets and Sets maxSteeringForce property.</summary>
	public float maxSteeringForce
	{
		get { return _maxSteeringForce; }
		set { _maxSteeringForce = value; }
	}

	/// <summary>Gets and Sets rigidbody Component.</summary>
	public Rigidbody rigidbody
	{ 
		get
		{
			if(_rigidbody == null)
			{
				_rigidbody = GetComponent<Rigidbody>();
			}
			return _rigidbody;
		}
	}

	/// <summary>Calculates Steering Force.</summary>
	/// <param name="_target">Target's point.</param>
	/// <returns>Steering Force.</returns>
	public Vector3 SeekForce(Vector3 _target, Vector3? _velocity = null)
	{
		if(!_velocity.HasValue) _velocity = rigidbody.velocity;
		Vector3 desiredDirection = (_target - transform.position).normalized * maxSpeed;
		Vector3 steeringForce = (desiredDirection - _velocity.Value);

		if(steeringForce.sqrMagnitude > maxSteeringForce * maxSteeringForce)
	    {
	        steeringForce.Normalize();
	        steeringForce *= maxSteeringForce;
	    }

		return steeringForce;
	}

	public float ApproximationMultiplier(Vector3 _target, float _radius)
	{
		float distance = (_target - (transform.position + rigidbody.velocity)).sqrMagnitude;
		float distanceRatio = (distance / (_radius * _radius));
		
		return Mathf.Clamp(distanceRatio, 0.0f, 1.0f);
	}
}
}