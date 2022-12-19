using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Supercargo
{
public delegate void OnAirplaneStopped(Airplane _airplane);

[RequireComponent(typeof(BrakeAirplane))]
[RequireComponent(typeof(AirplaneEngineDijkstra))]
[RequireComponent(typeof(APUAirplane))]
[RequireComponent(typeof(StrobeLightAirplane))]
[RequireComponent(typeof(SteeringVehicle))]
[RequireComponent(typeof(Rigidbody))]
public class Airplane : MonoBehaviour
{
	public static event OnAirplaneStopped onAirplaneStopped;

	[SerializeField] private PilotSight _pilotSight; 			/// <summary>Pilot's Sight.</summary>
	[Space(5f)]
	[Header("Wheels:")]
	[SerializeField] private GameObject _frontWheelsSystem; 	/// <summary>Front Sheels' System.</summary>
	[SerializeField] private WheelCollider _frontLeftWheel; 	/// <summary>Front Left's Wheel.</summary>
	[SerializeField] private WheelCollider _frontRightWheel; 	/// <summary>Front Right's Wheel.</summary>
	[SerializeField] private AirplaneWheel[] _backWheels; 		/// <summary>Airplane's Back Wheels.</summary>
	[Space(5f)]
	[Header("Engines:")]
	[SerializeField] private AudioSource _audioSource; 			/// <summary>AudioSource's Component.</summary>
	[SerializeField] private AudioSource[] _soundEngines;       /// <summary>Sound Engines.</summary>
    [SerializeField] private Light[] _lights;
    [Space(5f)]
    [Header("Attributes:")]
    [SerializeField] private float _minWheelSystemRotation; 	/// <summary>Minimim Wheel System's Local Rotation.</summary>
    [SerializeField] private float _maxWheelSystemRotation; 	/// <summary>Maximim Wheel System's Local Rotation.</summary>
    [SerializeField] private float _maxAngularSpeed; 			/// <summary>Maximum Angular's Speed.</summary>
    [SerializeField] private float _angularAccelerationTime; 	/// <summary>Angular Acceleration's time.</summary>
    [Space(5f)]
    [Header("Chocks:")]
    [SerializeField] private Vector3[] _chocksPoints; 			/// <summary>Chocks's Points.</summary>
    [SerializeField] private float _chockSpawnRadius; 			/// <summary>Chock's Spawn Radius.</summary>
	[SerializeField] private float _additionalDegrees; 			/// <summary>Additional Degrees.</summary>
	private BrakeAirplane _brakeAirplane; 						/// <summary>BrakeAirplane's Component.</summary>
	private AirplaneEngineDijkstra _airplaneDijkstraEngine; 	/// <summary>Dijkstra Calculator's Component.</summary>
	private APUAirplane _APUAirplane; 							/// <summary>APUAirplane's Component.</summary>
	private SteeringVehicle _steeringVehicle; 					/// <summary>SteeringVehicle's Component.</summary>
	private Rigidbody _rigidbody; 								/// <summary>Rigidbody's Component.</summary>
	private Renderer _renderer; 								/// <summary>Renderer's Component.</summary>
	private float wheelSystemAngularVelocity; 					/// <summary>Wheel System's Angular Velocity.</summary>

#region Getters/Setters:
	/// <summary>Gets pilotSight property.</summary>
	public PilotSight pilotSight { get { return _pilotSight; } }

	/// <summary>Gets frontWheelsSystem property.</summary>
	public GameObject frontWheelsSystem { get { return _frontWheelsSystem; } }

	/// <summary>Gets frontLeftWheel property.</summary>
	public WheelCollider frontLeftWheel { get { return _frontLeftWheel; } }

	/// <summary>Gets frontRightWheel property.</summary>
	public WheelCollider frontRightWheel { get { return _frontRightWheel; } }

	/// <summary>Gets backWheels property.</summary>
	public AirplaneWheel[] backWheels { get { return _backWheels; } }

	/// <summary>Gets audioSource property.</summary>
	public AudioSource audioSource { get { return _audioSource; } }

	/// <summary>Gets soundEngines property.</summary>
	public AudioSource[] soundEngines { get { return _soundEngines; } }

    public Light[] lights { get { return _lights; } }

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

	/// <summary>Gets and Sets airplaneDijkstraEngine Component.</summary>
	public AirplaneEngineDijkstra airplaneDijkstraEngine
	{ 
		get
		{
			if(_airplaneDijkstraEngine == null)
			{
				_airplaneDijkstraEngine = GetComponent<AirplaneEngineDijkstra>();
			}
			return _airplaneDijkstraEngine;
		}
	}

	/// <summary>Gets and Sets APUAirplane Component.</summary>
	public APUAirplane APUAirplane
	{ 
		get
		{
			if(_APUAirplane == null)
			{
				_APUAirplane = GetComponent<APUAirplane>();
			}
			return _APUAirplane;
		}
	}

	/// <summary>Gets and Sets steeringVehicle Component.</summary>
	public SteeringVehicle steeringVehicle
	{ 
		get
		{
			if(_steeringVehicle == null)
			{
				_steeringVehicle = GetComponent<SteeringVehicle>();
			}
			return _steeringVehicle;
		}
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

	/// <summary>Gets and Sets renderer Component.</summary>
	public Renderer renderer
	{ 
		get
		{
			if(_renderer == null)
			{
				_renderer = GetComponent<Renderer>();
			}
			return _renderer;
		}
	}

	/// <summary>Gets minWheelSystemRotation property.</summary>
	public float minWheelSystemRotation { get { return _minWheelSystemRotation; } }

	/// <summary>Gets maxWheelSystemRotation property.</summary>
	public float maxWheelSystemRotation { get { return _maxWheelSystemRotation; } }

	/// <summary>Gets maxAngularSpeed property.</summary>
	public float maxAngularSpeed { get { return _maxAngularSpeed; } }

	/// <summary>Gets angularAccelerationTime property.</summary>
	public float angularAccelerationTime { get { return _angularAccelerationTime; } }

	/// <summary>Gets chocksPoints property.</summary>
	public Vector3[] chocksPoints { get { return _chocksPoints; } }

	/// <summary>Gets chockSpawnRadius property.</summary>
	public float chockSpawnRadius { get { return _chockSpawnRadius; } }

	/// <summary>Gets additionalDegrees property.</summary>
	public float additionalDegrees { get { return _additionalDegrees; } }
#endregion

	private void OnDrawGizmos()
	{
		if(chocksPoints != null)
		{
			Vector3[] spawnPoints = GetSpawnPoints(transform.position);
			for(int i = 0; i < chocksPoints.Length; i++)
			{
				Vector3 chockPoint = chocksPoints[i];
				Vector3 spawnPoint = spawnPoints[i];
				Gizmos.DrawWireSphere(transform.TransformPoint(chockPoint), 1.0f);
				Gizmos.DrawLine(transform.transform.position, spawnPoint);	
			}
		}
	}

	public float GetAngularVelocity(float _currentAngularVelocity, float _targetAngularVelocity, float _maxAngularForce, float _deltaTime)
	{
		float result = Mathf.SmoothDamp(_currentAngularVelocity, _targetAngularVelocity, ref wheelSystemAngularVelocity, angularAccelerationTime * _deltaTime, _maxAngularForce, _deltaTime);
		return Mathf.Clamp(result, minWheelSystemRotation, maxWheelSystemRotation);
	}

	public float GetAcceleratedAngularSpeed(float _targetSpeed, float _deltaTime)
	{
		float differenceMultiplier = Mathf.Clamp((_targetSpeed - wheelSystemAngularVelocity),-1.0f, 1.0f);
		return wheelSystemAngularVelocity = Mathf.Clamp(wheelSystemAngularVelocity + (differenceMultiplier * GetAccelerationRate(_deltaTime)), minWheelSystemRotation, maxWheelSystemRotation);
	}

	public float GetDeceleratedAngularSpeed(float _targetSpeed, float _deltaTime)
	{
		float differenceMultiplier = Mathf.Approximately(_targetSpeed - wheelSystemAngularVelocity, 0.0f) ? 0.0f : 1.0f;
		return wheelSystemAngularVelocity = Mathf.Clamp(wheelSystemAngularVelocity - (differenceMultiplier * (Mathf.Sign(_targetSpeed) * GetAccelerationRate(_deltaTime))), minWheelSystemRotation, maxWheelSystemRotation);
	}

	public void OnStopped()
	{
		if(onAirplaneStopped != null) onAirplaneStopped(this);
	}

	private float GetAccelerationRate(float _deltaTime)
	{
		return ((maxAngularSpeed / angularAccelerationTime) * _deltaTime);
	}

	public Vector3[] GetSpawnPoints(Vector3 _center)
	{
		int count = chocksPoints.Length;
		Vector3[] spawnPoints = new Vector3[count];
		float degreeSplit = (360.0f/(float)count) * Mathf.Deg2Rad;
		float aditionalRadians = additionalDegrees * Mathf.Deg2Rad;

		for(int i = 0; i < count; i++)
		{
			float angle = ((float)(i + 1.0f) * degreeSplit) + aditionalRadians;
			float x = Mathf.Cos(angle) * chockSpawnRadius;
			float z = Mathf.Sin(angle) * chockSpawnRadius;
			spawnPoints[i] = _center + new Vector3(x, 0.0f, z);
		}

		return spawnPoints;
	}

	public void ObtainBackWheels()
	{
		_backWheels = GetComponentsInChildren<AirplaneWheel>();
	}

	public void Reset()
	{
		wheelSystemAngularVelocity = minWheelSystemRotation;
	}
}
}