using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneWheel : MonoBehaviour
{

    public WheelCollider targetWheel;
    public float forceWheels;
    

    private Vector3 wheelPosition = new Vector3();
    private Quaternion wheelRotation = new Quaternion();

 

    public void Update()
    {
        ForceAirplane();
        targetWheel.GetWorldPose(out wheelPosition, out wheelRotation);
        transform.rotation = wheelRotation;
       
    }

    private void ForceAirplane()
    {
        targetWheel.motorTorque = forceWheels;
    }
}
