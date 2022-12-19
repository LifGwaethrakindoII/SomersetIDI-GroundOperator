using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Supercargo
{
public delegate void OnAirplaneLanded(Airplane _airplane);

public class LandingZone : MonoBehaviour
{
	public static event OnAirplaneLanded onAirplaneLanded;

	[SerializeField] private LayerMask _airplaneMask; 	/// <summary>Airplane's LayerMask.</summary>
	[SerializeField] private float _zoneRadius; 		/// <summary>Zone's Radius.</summary>
	[SerializeField] private float _toleranceRadius; 	/// <summary>Tolerance's Radius.</summary>
	[SerializeField] private float _toleranceAngle; 	/// <summary>Tolerance's Angle between landing airplane and the landing zone's line.</summary>
#if UNITY_EDITOR
	[SerializeField] private Color color; 				/// <summary>Gizmos and Handles' color.</summary>
#endif

	/// <summary>Gets airplaneMask property.</summary>
	public LayerMask airplaneMask { get { return _airplaneMask; } }

	/// <summary>Gets zoneRadius property.</summary>
	public float zoneRadius { get { return _zoneRadius; } }

	/// <summary>Gets toleranceRadius property.</summary>
	public float toleranceRadius { get { return _toleranceRadius; } }

	/// <summary>Gets toleranceAngle property.</summary>
	public float toleranceAngle { get { return _toleranceAngle; } }

	private void OnDrawGizmos()
	{
#if UNITY_EDITOR
		float halfAngle = toleranceAngle * 0.5f;
		Gizmos.color = color;
		Handles.color = color;

		Gizmos.DrawWireSphere(transform.position, zoneRadius);
		Gizmos.DrawWireSphere(transform.position, toleranceRadius);
		Handles.DrawSolidArc(transform.position, transform.up, transform.forward, halfAngle, zoneRadius);
		Handles.DrawSolidArc(transform.position, transform.up, transform.forward, -halfAngle, zoneRadius);
		Handles.DrawSolidArc(transform.position, transform.up, -transform.forward, halfAngle, zoneRadius);
		Handles.DrawSolidArc(transform.position, transform.up, -transform.forward, -halfAngle, zoneRadius);
#endif
	}

	private void OnEnable()
    {
        //PatternRecognizer.onPatternRecognized += OnPatternRecognized;
        Airplane.onAirplaneStopped += OnAirplaneStopped;
    }

    private void OnDisable()
    {
        //PatternRecognizer.onPatternRecognized -= OnPatternRecognized;
        Airplane.onAirplaneStopped -= OnAirplaneStopped;
    }

    private void Start()
    {
    	OnPatternRecognized(Command.Stop);
    }

    private void OnPatternRecognized(Command _command)
    {
    	switch(_command)
    	{
    		case Command.Stop:
    		Collider[] colliders = Physics.OverlapSphere(transform.position, zoneRadius, airplaneMask);
			if(colliders.Length > 0)
	    	{
	    		foreach(Collider collider in colliders)
	    		{
	    			Airplane airplane = collider.transform.GetComponent<Airplane>();
		    		if(airplane != null)
		    		{
		    			EvaluateAirplane(airplane);
		    			break;
		    		}
	    		}
	    	} else Debug.Log("[LandingZone] No Contact");
    		break;

    		default:
    		break;
    	}
    }

    private void OnAirplaneStopped(Airplane _airplane)
    {
    	EvaluateAirplane(_airplane);
    }

    private void EvaluateAirplane(Airplane _airplane)
    {
    	Vector3 direction = _airplane.transform.position - transform.position;
    	direction.y = 0.0f;

    	if(direction.magnitude <= toleranceRadius)
    	{
    		float halfAngle = toleranceAngle * 0.5f;
    		float angle = Vector3.Angle(transform.forward, _airplane.transform.forward);
    		if(angle <= halfAngle)
    		{
    			Debug.Log("[LandingZone] Airplane " + _airplane + " landed correctly.");
    			if(onAirplaneLanded != null) onAirplaneLanded(_airplane);
    		} else Debug.Log("[LandingZone] Airplane " + _airplane + " landed incorrectly.");
    	}
    	else Debug.Log("[LandingZone] Airplane " + _airplane + " not close enough.");
    }
}
}