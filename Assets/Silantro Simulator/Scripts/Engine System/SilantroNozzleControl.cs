using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilantroNozzleControl : MonoBehaviour {
	//
	[HideInInspector]public SilantroControls controlBoard;
	//AILERON
	[HideInInspector]public string aileronControl;
	[HideInInspector]public float aileronInput;
	//ELEVATOR
	[HideInInspector]public string elevatorControl;
	[HideInInspector]public float elevatorInput;
	//RUDDER
	[HideInInspector]public string rudderControl;
	[HideInInspector]public float rudderInput;
	//
	public Transform Thruster;
	[HideInInspector]public Quaternion initialThrusterRotation;
	public enum RotationAxis
	{
		X,
		Y,
		Z
	}
	[Header("Axis Rotation")]
	public RotationAxis rotationAxis = RotationAxis.X;
	[HideInInspector]public Vector3 axisRotation;
	//
	public float maximumDeflection;
	public float currentDeflection;
	//
	public bool negativeDeflection;
	//
	public bool isControllable = true;
	[HideInInspector]public float nozzleInput;
	//
	public List<NozzleSystem> nozzleSystem = new List<NozzleSystem>();
	[System.Serializable]
	public class NozzleSystem
	{
		[Header("Connections")]
		public string Identifier;
		public Transform nozzleModel;
		//
		public enum RotationAxis
		{
			X,
			Y,
			Z
		}
		[Header("Axis Rotation")]
		public RotationAxis rotationAxis = RotationAxis.X;
		//
		[HideInInspector]public Vector3 axisRotation;
		public bool negativeRotation = false;
		[HideInInspector]public Quaternion initalRotation;
	}
	//
	[HideInInspector]public enum ControlBinding
	{
		Aileron,
		Elevator,
		Rudder
	}
	public ControlBinding controlBinding = ControlBinding.Elevator;
	// Use this for initialization
	void Start () {
		//RECIEVE CONTROLS
		aileronControl = controlBoard.Aileron;
		elevatorControl = controlBoard.Elevator;
		rudderControl = controlBoard.Rudder;
		//
			if (rotationAxis == RotationAxis.X) {
				axisRotation = new Vector3 (1, 0, 0);
			} else if (rotationAxis == RotationAxis.Y) {
				axisRotation = new Vector3 (0, 1, 0);
			} else if (rotationAxis == RotationAxis.Z) {
				axisRotation = new Vector3 (0, 0, 1);
			}
		//
		if ( null != Thruster )
		{
			initialThrusterRotation = Thruster.localRotation;
		}
		axisRotation.Normalize();
		//
		foreach (NozzleSystem nozzle in nozzleSystem) {
			//
			if (nozzle.negativeRotation) {
				if (nozzle.rotationAxis == NozzleSystem.RotationAxis.X) {
					nozzle.axisRotation = new Vector3 (-1, 0, 0);
				} else if (nozzle.rotationAxis == NozzleSystem.RotationAxis.Y) {
					nozzle.axisRotation = new Vector3 (0, -1, 0);
				} else if (nozzle.rotationAxis == NozzleSystem.RotationAxis.Z) {
					nozzle.axisRotation = new Vector3 (0, 0, -1);
				}
			} else {
				if (nozzle.rotationAxis == NozzleSystem.RotationAxis.X) {
					nozzle.axisRotation = new Vector3 (1, 0, 0);
				} else if (nozzle.rotationAxis == NozzleSystem.RotationAxis.Y) {
					nozzle.axisRotation = new Vector3 (0, 1, 0);
				} else if (nozzle.rotationAxis == NozzleSystem.RotationAxis.Z) {
					nozzle.axisRotation = new Vector3 (0, 0, 1);
				}
			}
			//
			nozzle.axisRotation.Normalize();
			//
			if (nozzle.nozzleModel != null) {
				nozzle.initalRotation = nozzle.nozzleModel.localRotation;
			}
			//
		}
	}
	// Update is called once per frame
	void Update () {
		//
		if (isControllable) {
			//COLLECT CONTROL INPUTS
			aileronInput = Input.GetAxis (aileronControl);
			elevatorInput = Input.GetAxis (elevatorControl);
			rudderInput = Input.GetAxis (rudderControl);
			//SELECT CONTROL TYPE
			if (controlBinding == ControlBinding.Aileron)
			{
				nozzleInput = aileronInput;
			} 
			else if (controlBinding == ControlBinding.Elevator)
			{
				nozzleInput = elevatorInput;
			} 
			else if (controlBinding == ControlBinding.Rudder) {
				nozzleInput = rudderInput;
			}
			//
			if (negativeDeflection) {
				nozzleInput *= -1f;
			}
			//
			//DECODE CONTROL INPUT
			float curveValue = controlBoard.controlCurve.Evaluate (Mathf.Abs (nozzleInput));
			curveValue *= Mathf.Sign (nozzleInput);
			currentDeflection = curveValue * maximumDeflection;
			//
			foreach (NozzleSystem nozzle in nozzleSystem) {
				nozzle.nozzleModel.transform.localRotation = nozzle.initalRotation;
				nozzle.nozzleModel.transform.Rotate (nozzle.axisRotation, currentDeflection);
			}
			//
			if (Thruster != null) {
				Thruster.localRotation = initialThrusterRotation;
				Thruster.Rotate (axisRotation, currentDeflection);
			}
		}
	}
}
