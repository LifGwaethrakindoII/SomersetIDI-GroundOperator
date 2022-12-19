using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Supercargo
{
public class AirplanePathfindingEngine : MonoBehaviour
{
	[Header("Steering Attributes:")]
	[SerializeField] private float _steeringSpeed; 			/// <summary>Steering Speed.</summary>
	[SerializeField] private float _steeringForce; 			/// <summary>Steering Force.</summary>
	[Space(5f)]
	[Header("Dijkstra's Attributes:")]
	[SerializeField] private float _distanceToGetClose; 	/// <summary>Distance to be considered close from Waypoint.</summary>
	[SerializeField] private int _startIndex; 				/// <summary>Start's Index.</summary>
	[SerializeField] private int _goalIndex; 				/// <summary>Goal's Index.</summary>
	[SerializeField] private bool _startFollowingPath; 		/// <summary>Follow Path on Start?.</summary>
	private Airplane _airplane; 							/// <summary>Airplane's Component.</summary>
	private BrakeAirplane _brakeAirplane; 					/// <summary>BrakeAirplane's Component.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets steeringSpeed property.</summary>
	public float steeringSpeed
	{
		get { return _steeringSpeed; }
		set { _steeringSpeed = value; }
	}

	/// <summary>Gets and Sets steeringForce property.</summary>
	public float steeringForce
	{
		get { return _steeringForce; }
		set { _steeringForce = value; }
	}

	/// <summary>Gets and Sets distanceToGetClose property.</summary>
	public float distanceToGetClose
	{
		get { return _distanceToGetClose; }
		set { _distanceToGetClose = value; }
	}

	/// <summary>Gets and Sets startIndex property.</summary>
	public int startIndex
	{
		get { return _startIndex; }
		set { _startIndex = value; }
	}

	/// <summary>Gets and Sets goalIndex property.</summary>
	public int goalIndex
	{
		get { return _goalIndex; }
		set { _goalIndex = value; }
	}

	/// <summary>Gets and Sets airplane Component.</summary>
	public Airplane airplane
	{ 
		get
		{
			if(_airplane == null)
			{
				_airplane = GetComponent<Airplane>();
			}
			return _airplane;
		}
	}

	/// <summary>Gets and Sets brakeAirplane Component.</summary>
	public BrakeAirplane brakeAirplane
	{ 
		get
		{
			if(_brakeAirplane == null)
			{
				_brakeAirplane = GetComponent<BrakeAirplane>();
			}
			return _brakeAirplane;
		}
	}
#endregion

	private void Awake()
	{
		distanceToGetClose *= distanceToGetClose;
	}
}
}