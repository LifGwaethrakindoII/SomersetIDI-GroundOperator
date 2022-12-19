using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supercargo;

public class AirplaneEngineDijkstra : MonoBehaviour
{
    //Era 5000 Brake Force
    [Header("Steering Attributes:")]
    [SerializeField] private float _steeringSpeed;          /// <summary>Steering Speed.</summary>
    [SerializeField] private float _steeringForce;          /// <summary>Steering Force.</summary>
    [Space(5f)]
    [Header("Dijkstra's Attributes:")]
    [SerializeField] private float _distanceToGetClose;     /// <summary>Distance to be considered close from Waypoint.</summary>
    [SerializeField] private int _startIndex;               /// <summary>Start's Index.</summary>
    [SerializeField] private int _goalIndex;                /// <summary>Goal's Index.</summary>
    [Space(5f)]
    [SerializeField] private bool move;                     /// <summary>Should this Airplane Move?.</summary>
    [SerializeField] private float radius;                  /// <summary>Radius.</summary>
    public DijkstraCalculator calculator;                   /// <summary>Dijkstra Path's Calculator reference.</summary>
    private Vector3? target;                                /// <summary>Airplane's Target.</summary>
    private Airplane _airplane;                             /// <summary>Airplane's Component.</summary>
    [SerializeField] private Command _command;              /// <summary>Current command cahced.</summary>
    private IEnumerator<Vector3> dijkstraPath;              /// <summary>Dijkstra Path's Iterator.</summary>
    private bool stopped;
    private float steeringMultiplier;

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

    /// <summary>Gets and Sets command property.</summary>
    public Command command
    {
        get { return _command; }
        set { _command = value; }
    }
#endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(target.HasValue) Gizmos.DrawWireSphere(target.Value, 5.0f);
    }

    private void OnEnable()
    {
        PatternRecognizer.onPatternRecognized += OnPatternRecognized;
    }

    private void OnDisable()
    {
        PatternRecognizer.onPatternRecognized -= OnPatternRecognized;
    }

    private void Awake()
    {
        target = null;
    }

    private void Start()
    {
        distanceToGetClose *= distanceToGetClose;
        SetDijkstraPath(calculator, startIndex, goalIndex);
    }

    private void Update()
    {
        EvaluateCommands();
    }

    private void FixedUpdate()
    {
        if(move)
        {
            airplane.brakeAirplane.ReleaseBrakes();
            if(target.HasValue)
            {
                ApplySteerTowards(target.Value);
                ApplyAirplaneForces(); 
            }
            //else if(calculator != null) FollowDijkstraPath();

            LimitSpeed();
        }
        else Stop();
    }

    private void EvaluateCommands()
    {
        switch(command)
        {
            //case Command.None:
            case Command.MoveAhead:
            SetTarget(OrientationSemantics.Forward);
            break;

            case Command.TurnRight:
            SetTarget(OrientationSemantics.Right);
            break;

            case Command.TurnLeft:
            SetTarget(OrientationSemantics.Left);
            break;

            case Command.SlowDown:
            break;

            case Command.Stop:
            target = null;
            move = false;
            if(!stopped)
            {
                airplane.OnStopped();
                stopped = true;
            }
            break;

            default:
            break;
        }
    }

    private void SetTarget(OrientationSemantics _orientation)
    {
        Vector3 direction = Extensions.GetOrientation(_orientation);
        target = transform.TransformPoint(direction * radius);
        steeringMultiplier = direction.x;
        move = true;
        stopped = false;
    }

    private void FollowDijkstraPath()
    {
        if(calculator != null)
        {
            airplane.steeringVehicle.maxSpeed = steeringSpeed * airplane.steeringVehicle.ApproximationMultiplier(dijkstraPath.Current, distanceToGetClose);
            ApplySteerTowards(dijkstraPath.Current);
            ApplyAirplaneForces();

            float sqrDistance = (dijkstraPath.Current - transform.position).sqrMagnitude;

            if(sqrDistance < distanceToGetClose)
            {
                if(dijkstraPath.MoveNext());
                else move = false;
            }
        }
        else if(!target.HasValue) move = false;
    }

    public void SetDijkstraPath(DijkstraCalculator _dijkstraPath, int _start = 0, int? _goal = null, bool _move = true)
    {
        if(_dijkstraPath != null)
        {
            if(!_goal.HasValue) _goal = _dijkstraPath.nodesData.Count;
            calculator = _dijkstraPath;
            move = _move;
            dijkstraPath = calculator.IterateDijkstraPath(_start, _goal.Value);
            dijkstraPath.MoveNext();
        }
        else
        {
            calculator = null;
            dijkstraPath = null;
            move = _move;
        }
    }

    private void Stop()
    {
        airplane.brakeAirplane.StopAirplane();
        if(airplane.rigidbody.velocity.magnitude == 0.0f && !stopped) airplane.OnStopped();
    }

    private void ApplySteerTowards(Vector3 _target)
    {
        Vector3 steering = airplane.steeringVehicle.SeekForce(_target);
        Debug.DrawRay(transform.position + airplane.rigidbody.velocity, steering, Color.red);
        steering.y = 0.0f;
        UpdateSteering(GetSteeringForce(steering));
    }

    private float GetSteeringForce(Vector3 _target, Space _space = Space.Self)
    {
        Vector3 relativeVector = _space == Space.Self ? transform.InverseTransformDirection(_target) : transform.TransformDirection(_target);

        //return (relativeVector.x / relativeVector.magnitude) * steeringForce;
        return steeringMultiplier == 0.0f ? steeringMultiplier : ((relativeVector.x / relativeVector.magnitude) * steeringForce);
    }

    private void UpdateSteering(float _steeringForce)
    {
        float clampedRotation = Mathf.Clamp(_steeringForce, airplane.minWheelSystemRotation, airplane.maxWheelSystemRotation);
        float resultingForce = airplane.GetAcceleratedAngularSpeed(clampedRotation, Time.fixedDeltaTime);

        airplane.frontWheelsSystem.transform.localEulerAngles = new Vector3(0, resultingForce, 0);
        airplane.frontLeftWheel.steerAngle = resultingForce;
        airplane.frontRightWheel.steerAngle = resultingForce;    
    }

    private void ApplyAirplaneForces()
    {
        float speedForce = (command == Command.SlowDown ? (airplane.steeringVehicle.minSpeed * Time.fixedDeltaTime) : (airplane.steeringVehicle.maxSpeed * Time.fixedDeltaTime));
        airplane.frontLeftWheel.motorTorque = speedForce;
        airplane.frontRightWheel.motorTorque = speedForce;

        if(airplane.backWheels != null)
        foreach(AirplaneWheel wheel in airplane.backWheels)
        {
            wheel.targetWheel.motorTorque = speedForce;
        }
        //airplane.rigidbody.AddForce(airplane.frontWheelsSystem.transform.forward * speedForce, ForceMode.Force);    
    }

    public void OnPatternRecognized(Command _command)
    {
        Debug.Log("[AirplaneEngineDijkstra] Airplane detected a Command: " + _command.ToString());
        Debug.Log("[AirplaneEngineDijkstra] Target at sight: " + airplane.pilotSight.targetAtSight.ToString());
        if(airplane.pilotSight.targetAtSight)
        {
            command = _command;
            if(stopped && command == Command.Stop) airplane.brakeAirplane.ReleaseBrakes();
        }
    }

    private void LimitSpeed()
    {
        float limitSpeed = airplane.steeringVehicle.maxSpeed;
        if(airplane.rigidbody.velocity.sqrMagnitude > (limitSpeed * limitSpeed) / airplane.rigidbody.mass) airplane.rigidbody.velocity = (airplane.rigidbody.velocity.normalized * limitSpeed); 
    }
}
