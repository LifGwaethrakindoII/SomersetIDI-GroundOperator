using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneRim : MonoBehaviour
{

    public WheelCollider targetRim;
    public float forceWheels;

    private Vector3 rimPosition = new Vector3();
    private Quaternion rimRotation = new Quaternion();

	// Update is called once per frame
	void Update ()
    {
        ForceWheels();
        targetRim.GetWorldPose(out rimPosition , out rimRotation);
        transform.rotation = rimRotation;
        //Debug.Log(transform.rotation);
	}

    private void ForceWheels()
    {
        targetRim.motorTorque = forceWheels;
    }


}
