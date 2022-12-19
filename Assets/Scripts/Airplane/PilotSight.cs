using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Supercargo
{
public class PilotSight : MonoBehaviour
{
	[SerializeField] private User _target; 				/// <summary>Pilot's Target.</summary>
	[Space(5f)]
	[Header("Sight's Attributes:")]
	[SerializeField] private LayerMask _sightLayer; 	/// <summary>Sight's Layer Mask.</summary>
	[SerializeField] private float _distance; 			/// <summary>FOV's Distance.</summary>
	[SerializeField] private float _angle; 				/// <summary>FOV's Angle.</summary>
	[SerializeField] private int _tolerance; 			/// <summary>Tolerance.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[SerializeField] private Color color; 				/// <summary>Handles' Color.</summary>
#endif
	private bool _targetAtSight; 						/// <summary>Is Target at Sight?.</summary>

	/// <summary>Gets and Sets target property.</summary>
	public User target
	{
		get { return _target; }
		set { _target = value; }
	}

	/// <summary>Gets and Sets sightLayer property.</summary>
	public LayerMask sightLayer
	{
		get { return _sightLayer; }
		set { _sightLayer = value; }
	}

	/// <summary>Gets and Sets distance property.</summary>
	public float distance
	{
		get { return _distance; }
		set { _distance = value; }
	}

	/// <summary>Gets and Sets angle property.</summary>
	public float angle
	{
		get { return _angle; }
		set { _angle = value; }
	}

	/// <summary>Gets and Sets tolerance property.</summary>
	public int tolerance
	{
		get { return _tolerance; }
		set { _tolerance = value; }
	}

	/// <summary>Gets and Sets targetAtSight property.</summary>
	public bool targetAtSight
	{
		get { return _targetAtSight; }
		private set { _targetAtSight = value; }
	}

	private void OnDrawGizmos()
	{
#if UNITY_EDITOR
		float halfAngle = angle * 0.5f;
		Handles.color = color;
		if(!Application.isPlaying)
		{
			Handles.DrawSolidArc(transform.position, transform.up, transform.forward, halfAngle, distance);
			Handles.DrawSolidArc(transform.position, transform.up, transform.forward, -halfAngle, distance);
			/*Handles.DrawSolidArc(transform.position, transform.right, transform.forward, halfAngle, distance);
			Handles.DrawSolidArc(transform.position, transform.right, transform.forward, -halfAngle, distance);*/
		}
		else
		{
			float distanceRoot = Mathf.Sqrt(distance);

			Handles.DrawSolidArc(transform.position, transform.up, transform.forward, halfAngle, distanceRoot);
			Handles.DrawSolidArc(transform.position, transform.up, transform.forward, -halfAngle, distanceRoot);
			/*Handles.DrawSolidArc(transform.position, transform.right, transform.forward, halfAngle, distanceRoot);
			Handles.DrawSolidArc(transform.position, transform.right, transform.forward, -halfAngle, distanceRoot);*/

			//Handles.color = Color.blue;
			//Handles.DrawLine(transform.position, target.eye.position);
		}	
#endif
	}

	private void Awake()
	{
		distance *= distance;
	}

	private void Update()
	{
		if(target != null) EvaluateTarget();
		else targetAtSight = false;

		//Debug.Log("[PilotSight] TARGET AT SIGHT: " + targetAtSight);
	}

	private void EvaluateTarget()
	{
		Vector3 direction = (target.eye.position - transform.position);

		if(direction.sqrMagnitude <= distance)
		{
			direction.Normalize();
			RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, distance, sightLayer);

			//Debug.Log("[PilotSight] HITS LENGTH: " + hits.Length);

			if(hits != null && hits.Length <= tolerance)
			{
				/*Debug.Log("[PilotSight] Angle Between User: " + Vector3.Angle(transform.forward, direction));
				Debug.Log("[PilotSight] Half angle: " + angle * 0.5f);*/
				targetAtSight = (Vector3.Angle(transform.forward, direction) <= (angle * 0.5f));
			}
			else targetAtSight = false;
		}
		else targetAtSight = false;
	}
}
}