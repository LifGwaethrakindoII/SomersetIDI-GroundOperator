using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilantroJATOController : MonoBehaviour {
	public SilantroRocketMotor[] boosters;
	public float TotalThrust;
	SilantroControls controlBoard;
	string boosterControl;
	public bool isControllable = true;
	// Use this for initialization
	void Start () {
		controlBoard = GameObject.FindGameObjectWithTag ("GameController").GetComponent<SilantroControls> ();
		if (controlBoard == null) {
			Debug.LogError ("Control Board is missing, Place Control Board in scene and restart!");
		}
		boosters = GetComponentsInChildren<SilantroRocketMotor> ();
		boosterControl = controlBoard.AfterburnerControl;
	}
	
	// Update is called once per frame
	void Update () {
		if (isControllable) {
			if (Input.GetButtonDown (boosterControl)) {
				foreach (SilantroRocketMotor motor in boosters) {
					if (!motor.active) {
						motor.active = true;
					}
				}
			}
			//
			TotalThrust = 0;
			foreach (SilantroRocketMotor motor in boosters) {
				if (motor.active) {
					TotalThrust += motor.Thrust;
				}
			}
		}
	}
}
