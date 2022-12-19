using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAirplane : MonoBehaviour {

    public AirplaneEngineDijkstra Airplane;

    public void Start()
    {
        //Airplane = GetComponent<AirplaneEngineDijkstra>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Airplane")
        {
            Airplane.steeringForce = 0;
            Airplane.airplane.frontLeftWheel.motorTorque = 0;
            Airplane.airplane.frontRightWheel.motorTorque = 0;
        }
    }
}
