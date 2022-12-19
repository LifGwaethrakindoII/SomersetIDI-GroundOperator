using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoidlessUtilities.VR
{
[System.Flags]
public enum BodyDetection 										/// <summary>Body Velocities' detenction types.</summary>
{
	Position = 1, 												/// <summary>Position Detection.</summary>
	Rotation = 2, 												/// <summary>Rotation Detection.</summary>

	All = Position | Rotation 									/// <summary>Position and Rotation Detection.</summary>
}

public class Body : MonoBehaviour
{
	[SerializeField] private BodyDetection _detectionType; 		/// <summary>Properties that this bodyu will update.</summary>
	[SerializeField] private float _minimumMagnitudeToChange; 	/// <summary>Minimum magnitude between the velocity to register a change.</summary>
	private Vector3 _velocity; 									/// <summary>Body's Velocity.</summary>
	private Vector3 _angularVelocity; 							/// <summary>Body's Angular Velocity.</summary>
	private Vector3 _accumulatedVelocity; 						/// <summary>Accumulated's Velocity.</summary>
	private Vector3	_accumulatedAngularVelocity; 				/// <summary>Accumulated's Angular Velocity.</summary>
	private Vector3 _lastPosition; 								/// <summary>Body's Last Position.</summary>
	private Vector3 _lastEulerRotation; 						/// <summary>Body's Last Euler Rotation.</summary>

#if UNITY_EDITOR
	[SerializeField] private bool debug; 						/// <summary>Debug Velocities?.</summary>
	[SerializeField] private float rayDuration; 				/// <summary>Ray's Duration.</summary>
#endif

#region Getters/Setters:
	/// <summary>Gets and Sets detectionType property.</summary>
	public BodyDetection detectionType
	{
		get { return _detectionType; }
		set { _detectionType = value; }
	}

	/// <summary>Gets and Sets minimumMagnitudeToChange property.</summary>
	public float minimumMagnitudeToChange
	{
		get { return _minimumMagnitudeToChange; }
		set { _minimumMagnitudeToChange = value * value; }
	}

	/// <summary>Gets and Sets velocity property.</summary>
	public Vector3 velocity
	{
		get { return _velocity; }
		private set { _velocity = value; }
	}

	/// <summary>Gets and Sets angularVelocity property.</summary>
	public Vector3 angularVelocity
	{
		get { return _angularVelocity; }
		set { _angularVelocity = value; }
	}

	/// <summary>Gets and Sets accumulatedVelocity property.</summary>
	public Vector3 accumulatedVelocity
	{
		get { return _accumulatedVelocity; }
		private set { _accumulatedVelocity = value; }
	}

	/// <summary>Gets and Sets accumulatedAngularVelocity property.</summary>
	public Vector3 accumulatedAngularVelocity
	{
		get { return _accumulatedAngularVelocity; }
		set { _accumulatedAngularVelocity = value; }
	}

	/// <summary>Gets and Sets lastPosition property.</summary>
	public Vector3 lastPosition
	{
		get { return _lastPosition; }
		private set { _lastPosition = value; }
	}

	/// <summary>Gets and Sets lastEulerRotation property.</summary>
	public Vector3 lastEulerRotation
	{
		get { return _lastEulerRotation; }
		private set { _lastEulerRotation = value; }
	}
#endregion

	private void OnDisable()
	{
		ResetBody();
	}

	private void Awake()
	{
		_minimumMagnitudeToChange *= _minimumMagnitudeToChange;
		ResetBody();
	}

	private void Update()
	{
		if(detectionType.HasFlag(BodyDetection.Position)) UpdateVelocity();
		if(detectionType.HasFlag(BodyDetection.Rotation)) UpdateAngularVelocity();
#if UNITY_EDITOR
		if(debug) DebugBody();
#endif
	}

	/// <summary>Updates Velocity's Data.</summary>
	private void UpdateVelocity()
	{
		velocity = (transform.localPosition - lastPosition);
		if(velocity.sqrMagnitude >= minimumMagnitudeToChange * Time.deltaTime)
		accumulatedVelocity += velocity;
		else accumulatedVelocity = Vector3.zero;
		lastPosition = transform.localPosition;
	}

	/// <summary>Updates Angular velocity's Data.</summary>
	private void UpdateAngularVelocity()
	{
		angularVelocity = (transform.localEulerAngles - lastEulerRotation);
		if(velocity.sqrMagnitude >= minimumMagnitudeToChange * Time.deltaTime)
		accumulatedAngularVelocity = angularVelocity;
		else accumulatedAngularVelocity = Vector3.zero;
		lastEulerRotation = transform.localEulerAngles;
	}

	/// <summary>Resets Body's Data.</summary>
	private void ResetBody()
	{
		lastPosition = transform.localPosition;
		lastEulerRotation = transform.localEulerAngles;
		accumulatedVelocity = Vector3.zero;
	}

	/// <summary>Debug Body's Velocities [Only in Editor Mode].</summary>
	private void DebugBody()
	{
#if UNITY_EDITOR
		if(detectionType.HasFlag(BodyDetection.Position)) Debug.DrawRay(transform.position + lastPosition, velocity, Color.blue, rayDuration);
		if(detectionType.HasFlag(BodyDetection.Rotation)) Debug.DrawRay(transform.position + lastEulerRotation, angularVelocity, Color.red, rayDuration);
#endif
	}
}
}