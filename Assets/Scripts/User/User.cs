using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoidlessUtilities.VR;

namespace Supercargo
{
/// <summary>Event Invoked when the User picks both paddles.</summary>
/// <param name="_picked">Were the paddles picked?.</param>
public delegate void OnPaddlesPicked(bool _picked);

public class User : MonoBehaviour
{
	public const SteamVR_TrackedObject.EIndex INDEX_LEFT_HAND = SteamVR_TrackedObject.EIndex.Device4;
	public const SteamVR_TrackedObject.EIndex INDEX_RIGHT_HAND = SteamVR_TrackedObject.EIndex.Device3;
	public const SteamVR_TrackedObject.EIndex INDEX_LEFT_HAND_ALT = SteamVR_TrackedObject.EIndex.Device2;
	public const SteamVR_TrackedObject.EIndex INDEX_RIGHT_HAND_ALT = SteamVR_TrackedObject.EIndex.Device1;

	public event OnPaddlesPicked onPaddlesPicked; 								/// <summary>OnPaddlesPicked event delegate.</summary>

	[SerializeField] private CharacterController _character; 					/// <summary>CharacterController's Component.</summary>
	[SerializeField] private Transform _eye; 									/// <summary>User's Eye.</summary>
	[SerializeField] private Transform _torax; 									/// <summary>User's Torax.</summary>
	[Space(5f)]
	[Header("User's Inputs:")]
	[SerializeField] private Hand _leftHand; 									/// <summary>Left Hand.</summary>
	[SerializeField] private Hand _rightHand; 									/// <summary>Right Hand.</summary>
	[Space(5f)]
	[Header("Movement Stats:")]
	/// \TODO MOVE TO A STATS' SCRIPTABLE OBJECT
	[SerializeField] private float speed; 										/// <summary>Movement Speed.</summary>
	[SerializeField]
	[Range(1.0f, 10.0f)] private float speedMultiplier; 						/// <summary>Additional Speed's Multiplier.</summary>
	[SerializeField] [Range(0.0f, 1.0f)] private float backMovementMultiplier; 	/// <summary>Back movement multiplier.</summary>
	[SerializeField] private float toraxOffset; 								/// <summary>Torax's Offset relative to the Camera.</summary>
#if UNITY_EDITOR
	[SerializeField] private Mesh headMesh; 									/// <summary>Head's Mesh.</summary>
	[SerializeField] private Mesh humanMesh; 									/// <summary>Human's Mesh.</summary>
	[SerializeField] private Color color; 										/// <summary>Gizmos' Color.</summary>
	[SerializeField] private float jointRadius; 								/// <summary>Joint Radius on Gizmos' mode.</summary>
#endif
	private int ID;

	/// <summary>Gets eye property.</summary>
	public Transform eye { get { return _eye; } }

	/// <summary>Gets torax property.</summary>
	public Transform torax { get { return _torax; } }

	/// <summary>Gets leftHand property.</summary>
	public Hand leftHand { get { return _leftHand; } }

	/// <summary>Gets rightHand property.</summary>
	public Hand rightHand { get { return _rightHand; } }

	/// <summary>Gets hasPaddles property.</summary>
	public bool hasPaddles { get { return (leftHand.paddle != null && rightHand.paddle != null); } }

	/// <summary>Gets and Sets character Component.</summary>
	public CharacterController character
	{ 
		get
		{
			if(_character == null)
			{
				_character = GetComponent<CharacterController>();
			}
			return _character;
		}
	}

	private void OnDrawGizmos()
	{
#if UNITY_EDITOR
		Gizmos.color = color;
		if(torax != null)
		{
			if(eye != null)
			{
				Gizmos.DrawWireSphere(eye.position, jointRadius);
				Gizmos.DrawLine(eye.position, torax.position);
			}
			if(leftHand != null)
			{
				Gizmos.DrawWireSphere(leftHand.transform.position, jointRadius);
				Gizmos.DrawLine(torax.position, leftHand.transform.position);
				if(leftHand.paddle != null)
				Gizmos.DrawLine(leftHand.transform.position, leftHand.paddle.transform.TransformPoint(leftHand.paddle.tipOffset));
			}
			if(rightHand != null)
			{
				Gizmos.DrawWireSphere(rightHand.transform.position, jointRadius);
				Gizmos.DrawLine(torax.position, rightHand.transform.position);
				if(rightHand.paddle != null)
				Gizmos.DrawLine(rightHand.transform.position, rightHand.paddle.transform.TransformPoint(rightHand.paddle.tipOffset));
			}
		}

		if(headMesh != null && humanMesh != null)
		{
			Vector3 offset = new Vector3(0.0f, -humanMesh.bounds.size.y, 0.0f);
			Gizmos.matrix = eye.localToWorldMatrix;
			Gizmos.DrawWireMesh(headMesh, offset);
			Gizmos.DrawWireMesh(humanMesh, offset);
		}
#endif
	}

	private void OnEnable()
	{
		rightHand.onPicked += OnHandPicked;
		leftHand.onPicked += OnHandPicked;
	}

	private void OnDisable()
	{
		rightHand.onPicked -= OnHandPicked;
		leftHand.onPicked -= OnHandPicked;
	}

	private void Awake()
	{
		//RecalibrateControllers();
		torax.localPosition = (Vector3.up * toraxOffset);
	}

	private void Update()
	{
		TrackInputs();
		if(!character.isGrounded) character.SimpleMove(Vector3.down);
	}

	private void TrackInputs()
	{
		Vector2 axis = (leftHand.device.GetAxis() + rightHand.device.GetAxis()).normalized;
		float velocityMultiplier = (leftHand.device.GetPress(SteamVR_Controller.ButtonMask.Grip) || rightHand.device.GetPress(SteamVR_Controller.ButtonMask.Grip)) ? speedMultiplier : 1.0f;

		if(leftHand.device.GetPress(SteamVR_Controller.ButtonMask.Touchpad) || rightHand.device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
		character.SimpleMove(eye.forward * (axis.y > 0.0f ? axis.y * velocityMultiplier : axis.y * backMovementMultiplier) * speed);
	}

	/// <summary>Recalibrates Controllers.</summary>
	public void RecalibrateControllers()
	{
		switch(leftHand.steamDevice.trackedObject.index)
		{
			case INDEX_RIGHT_HAND:
			case INDEX_RIGHT_HAND_ALT:
			Hand temp = rightHand;
			_rightHand = leftHand;
			_leftHand = temp;
			break;
		}

		switch(rightHand.steamDevice.trackedObject.index)
		{
			case INDEX_LEFT_HAND:
			case INDEX_LEFT_HAND_ALT:
			Hand temp = rightHand;
			_rightHand = rightHand;
			_rightHand = temp;
			break;
		}
	}

	public Vector3 GetAverageJointPoint()
	{
		return (rightHand.transform.position + leftHand.transform.position + rightHand.paddle.transform.position + leftHand.paddle.transform.position) / 4.0f;
	}

	private void OnHandPicked(Hand _hand)
	{
		if(onPaddlesPicked != null) onPaddlesPicked(hasPaddles);
	}
}
}