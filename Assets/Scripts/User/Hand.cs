using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoidlessUtilities.VR;

namespace Supercargo
{
public enum HandState
{
	Empty,
	Picking
}

/// <summary>Event invoked when the Hand picks something.</summary>
/// <param name="_hand">Hand that picked something.</param>
public delegate void OnPicked(Hand _hand);

[RequireComponent(typeof(SteamVRDevice))]
[RequireComponent(typeof(Body))]
public class Hand : MonoBehaviour, IFiniteStateMachine<HandState>
{
	public event OnPicked onPicked; 						/// <summary>OnPicked event delegate.</summary>

	[SerializeField] private LayerMask _pickableLayer; 		/// <summary>Pickable Layer Mask.</summary>
	[SerializeField] private Vector3 _pickedPointOffset; 	/// <summary>Picked Point's Offset relative to this transform's position.</summary>
	[SerializeField] private float _pickRadius; 			/// <summary>Pick's Radius.</summary>
	[SerializeField] private float _pickDistance; 			/// <summary>Pick's Distance.</summary>
	[SerializeField] private float _force; 					/// <summary>Hand's Force.</summary>
	[SerializeField] private float _pickDuration; 			/// <summary>Smooth pick's duration.</summary>
	private SteamVRDevice _steamDevice; 					/// <summary>SteamVRDevice's Component.</summary>
	private Body _body; 									/// <summary>Body's Component.</summary>
	private SteamVR_Controller.Device _device; 				/// <summary>Hand's Device.</summary>
	private Pickable _pickable;
	private HandState _state;
	private HandState _previousState;
	private Coroutine pickRoutine;
	private LandingPaddle _paddle; 							/// <summary>Hand's Paddle.</summary>

#if UNITY_EDITOR
	[SerializeField] private float gizmoRadius; 			/// <summary>Gizmo's Radius.</summary>
#endif

#region Getters/Setters:
	/// <summary>Gets pickableLayer property.</summary>
	public LayerMask pickableLayer { get { return _pickableLayer; } }

	/// <summary>Gets pickedPointOffset property.</summary>
	public Vector3 pickedPointOffset { get { return _pickedPointOffset; } }

	/// <summary>Gets pickRadius property.</summary>
	public float pickRadius { get { return _pickRadius; } }

	/// <summary>Gets pickDistance property.</summary>
	public float pickDistance { get { return _pickDistance; } }

	/// <summary>Gets force property.</summary>
	public float force { get { return _force; } }

	/// <summary>Gets pickDuration property.</summary>
	public float pickDuration { get { return _pickDuration; } }

	/// <summary>Gets and Sets steamDevice Component.</summary>
	public SteamVRDevice steamDevice
	{ 
		get
		{
			if(_steamDevice == null)
			{
				_steamDevice = GetComponent<SteamVRDevice>();
			}
			return _steamDevice;
		}
	}

	/// <summary>Gets and Sets body Component.</summary>
	public Body body
	{ 
		get
		{
			if(_body == null)
			{
				_body = GetComponent<Body>();
			}
			return _body;
		}
	}

	/// <summary>Gets and Sets paddle property.</summary>
	public LandingPaddle paddle
	{
		get { return _paddle; }
		set { _paddle = value; }
	}

	/// <summary>Gets and Sets device Component.</summary>
	public SteamVR_Controller.Device device
	{ 
		get
		{
			//if(_device == null)
			{
				_device = steamDevice.device;
			}
			return _device;
		}
	}

	/// <summary>Gets and Sets pickable property.</summary>
	public Pickable pickable
	{
		get { return _pickable; }
		set { _pickable = value; }
	}

	/// <summary>Gets and Sets state property.</summary>
	public HandState state
	{
		get { return _state; }
		set { _state = value; }
	}

	/// <summary>Gets and Sets previousState property.</summary>
	public HandState previousState
	{
		get { return _previousState; }
		set { _previousState = value; }
	}
#endregion

#region UnityMethods:
	private void OnDrawGizmos()
	{
#if UNITY_EDITOR
		Vector3 relativePoint = transform.TransformPoint(pickedPointOffset);

		Gizmos.DrawWireSphere(relativePoint, gizmoRadius);
		Gizmos.DrawWireSphere(relativePoint, pickRadius);
		Gizmos.DrawRay(relativePoint, transform.forward * pickDistance);
#endif
	}

	private void Awake()
	{
		this.ChangeState(HandState.Empty);
	}

	private void Update()
	{
		if(device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && pickable == null) OnPick();
		//if(device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger) && pickable != null) OnDrop();
	}
#endregion

#region FinitestateMachine:
	/// <summary>Enters HandState State.</summary>
	/// <param name="_state">HandState State that will be entered.</param>
	public void OnEnter(HandState _state)
	{
		switch(_state)
		{
			case HandState.Empty:
			if(pickable != null)
			{
				//pickable.rigidbody.isKinematic = false;
				pickable.rigidbody.AddForce(body.accumulatedVelocity * force, ForceMode.Impulse);
				pickable.rigidbody.AddTorque(body.accumulatedAngularVelocity * force, ForceMode.Impulse);
			}
			body.enabled = false;
			break;

			case HandState.Picking:
			body.enabled = true;
			break;
	
			default:
			break;
		}
	}
	
	/// <summary>Leaves HandState State.</summary>
	/// <param name="_state">HandState State that will be left.</param>
	public void OnExit(HandState _state)
	{
		switch(_state)
		{
			case HandState.Empty:
			break;

			case HandState.Picking:
			break;
	
			default:
			break;
		}
	}
#endregion

	public Vector3 GetRelativePaddlePoint()
	{
		return paddle != null ? transform.TransformPoint(paddle.GetOffsetTipPosition() - transform.position) : Vector3.zero;
	}

#region PickMethods:
	private void OnPick()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.TransformPoint(pickedPointOffset), pickRadius, pickableLayer);
		if(colliders != null && colliders.Length > 0)
		{
			Pickable currentPickable = colliders[0].gameObject.GetComponent<Pickable>();
			if(currentPickable != null)
			{
				switch(currentPickable.state)
				{
					case PickableState.Unpicked:
					Pick(currentPickable);
					break;

					case PickableState.Picked:
					if(currentPickable.hand != null && currentPickable.hand != this)
					{
						currentPickable.hand.OnDrop();
						Pick(currentPickable);
					}
					break;
				}
			}
		}

		/*Ray ray = new Ray(transform.TransformPoint(pickedPointOffset), transform.forward * pickDistance);
		RaycastHit hit;

		if(Physics.SphereCast(ray, pickRadius, out hit, 0, pickableLayer))
		{
			Pickable currentPickable = hit.transform.gameObject.GetComponent<Pickable>();
			if(currentPickable != null)
			{
				switch(currentPickable.state)
				{
					case PickableState.Unpicked:
					Pick(currentPickable);
					break;

					case PickableState.Picked:
					if(currentPickable.hand != null && currentPickable.hand != this)
					{
						currentPickable.hand.pickable = null;
						Pick(currentPickable);
					}
					break;
				}
			}
		}*/
	}

	private void Pick(Pickable _pickable)
	{
		_pickable.ChangeState(PickableState.Picked);
		pickable = _pickable;
		if(_pickable is LandingPaddle)
		paddle = _pickable as LandingPaddle;
		pickable.hand = this;

		this.DispatchCoroutine(ref pickRoutine);
		//pickRoutine = StartCoroutine(PickSmoothly());
		pickable.transform.parent = transform;
		this.ChangeState(HandState.Picking);

		if(onPicked != null) onPicked(this);
	}

	public void OnDrop()
	{
		pickable.transform.parent = null;
		pickable.ChangeState(PickableState.Unpicked);
		this.ChangeState(HandState.Empty);
		pickable = null;
		if(paddle != null) paddle = null;
		this.DispatchCoroutine(ref pickRoutine);

		if(onPicked != null) onPicked(this);
	}
#endregion

#region Coroutines:
	private IEnumerator PickSmoothly()
	{
		float t = 0.0f;
		Vector3 originalPosition = _pickable.transform.position;
		Vector3 destinyPosition = transform.TransformPoint(pickedPointOffset - Vector3.Scale(_pickable.anchorOffset, _pickable.transform.localScale));
		Quaternion originalRotation = _pickable.transform.rotation;
		Quaternion destinyRotation = Quaternion.Euler(transform.eulerAngles + _pickable.eulerPickedRotation);

		while(t < (1.0f + Mathf.Epsilon))
		{
			_pickable.transform.position = Vector3.Lerp(originalPosition, destinyPosition, t);
			_pickable.transform.rotation = Quaternion.Lerp(originalRotation, destinyRotation, t);
			t += (Time.deltaTime / pickDuration);
			yield return null;
			destinyPosition = transform.TransformPoint(pickedPointOffset - Vector3.Scale(_pickable.anchorOffset, _pickable.transform.localScale));
			destinyRotation = Quaternion.Euler(transform.eulerAngles + _pickable.eulerPickedRotation);
		}

		_pickable.transform.position = destinyPosition;
		_pickable.transform.rotation = destinyRotation;
		_pickable.transform.parent = transform;
	}
#endregion
}
}