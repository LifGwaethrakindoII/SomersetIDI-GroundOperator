using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Supercargo
{
public enum PickableState 									/// <summary>Pickable's States.</summary>
{
	Unpicked, 												/// <summary>Unpicked State.</summary>
	Picked, 												/// <summary>Picked State.</summary>
	Dropped 												/// <summary>Dropped State.</summary>
}

[RequireComponent(typeof(Rigidbody))]
public class Pickable : MonoBehaviour, IFiniteStateMachine<PickableState>
{
	[SerializeField] private Quaternion _pickedRotation; 	/// <summary>Rotation when the Pickable is held.</summary>
	[SerializeField] private Vector3 _eulerPickedRotation; 	/// <summary>Rotation in Eulers when the pickable is held.</summary>
	[SerializeField] private Vector3 _anchorOffset; 		/// <summary>Pickable's Anchor Offset.</summary>
	private Hand _hand; 									/// <summary>Hand that is picking thi object.</summary>
	private PickableState _state; 							/// <summary>Pickable's Current State.</summary>
	private PickableState _previousState; 					/// <summary>Pickable's Current State.</summary>
	private Rigidbody _rigidbody; 							/// <summary>Rigidbody's Component.</summary>
#if UNITY_EDITOR
	[SerializeField] private float projection; 				/// <summary>Normals' projection for the gizmos' normals.</summary>
#endif

#region Getters/Setters:
	/// <summary>Gets pickedRotation property.</summary>
	public Quaternion pickedRotation { get { return _pickedRotation = Quaternion.Euler(transform.TransformDirection(eulerPickedRotation)); } }

	/// <summary>Gets eulerPickedRotation property.</summary>
	public Vector3 eulerPickedRotation { get { return _eulerPickedRotation; } }

	/// <summary>Gets anchorOffset property.</summary>
	public Vector3 anchorOffset { get { return _anchorOffset; } }

	/// <summary>Gets and Sets hand property.</summary>
	public Hand hand
	{
		get { return _hand; }
		set { _hand = value; }
	}

	/// <summary>Gets and Sets state property.</summary>
	public PickableState state
	{
		get { return _state; }
		set { _state = value; }
	}

	/// <summary>Gets and Sets previousState property.</summary>
	public PickableState previousState
	{
		get { return _previousState; }
		set { _previousState = value; }
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
#endregion

	/// <summary>Draws desired rotation and anchor when this object is picked.</summary>
	private void OnDrawGizmos()
	{
		DrawOrientationNormals();
	}

#region FiniteStateMachine:
	/// <summary>Enters PickableState State.</summary>
	/// <param name="_state">PickableState State that will be entered.</param>
	public void OnEnter(PickableState _state)
	{
		switch(_state)
		{
			case PickableState.Unpicked:
			rigidbody.isKinematic = false;
			//rigidbody.useGravity = true;
			break;

			case PickableState.Picked:
			rigidbody.isKinematic = true;
			//rigidbody.useGravity = false;
			break;

			case PickableState.Dropped:
			rigidbody.isKinematic = false;
			this.ChangeState(PickableState.Unpicked);
			//rigidbody.useGravity = true;	
			break;
				
			default:
			break;
		}
	}
	
	/// <summary>Leaves PickableState State.</summary>
	/// <param name="_state">PickableState State that will be left.</param>
	public void OnExit(PickableState _state)
	{
		switch(_state)
		{
			case PickableState.Unpicked:
			break;

			case PickableState.Picked:
			break;

			case PickableState.Dropped:
			break;
	
			default:
			break;
		}
	}
#endregion

	protected virtual void DrawOrientationNormals()
	{
#if UNITY_EDITOR
		Vector3 offsetedPosition = transform.TransformPoint(anchorOffset);

		Gizmos.color = Color.red;
		Gizmos.DrawRay(offsetedPosition, pickedRotation * Vector3.right * projection);
		Gizmos.color = Color.green;
		Gizmos.DrawRay(offsetedPosition, pickedRotation * Vector3.up * projection);
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(offsetedPosition, pickedRotation * Vector3.forward * projection);
#endif
	}

	/// <summary>Event triggered when this Collider/Rigidbody begun having contact with another Collider/Rigidbody.</summary>
	/// <param name="col">The Collision data associated with this collision Event.</param>
	/*void OnCollisionEnter(Collision col)
	{
		if(state == PickableState.Dropped) this.ChangeState(PickableState.Unpicked);
	}*/
}
}