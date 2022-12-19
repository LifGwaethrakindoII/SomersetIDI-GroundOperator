using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilantroSequenceHydraulics : MonoBehaviour {

	[Header("Sequence")]
	public SilantroHydraulicSystem wheelAxle;
	public SilantroHydraulicSystem mainGearRotate;
	public SilantroHydraulicSystem mainGearClose;
	public SilantroHydraulicSystem doors;
	//
	[Header("Switches")]
	public bool open;
	[HideInInspector]
	public bool opened = true;
	public bool close ;
	[HideInInspector]
	public bool closed= false;
	//
	bool activated;
	//
	[Header("Control Values")]
	public float openTime = 5f;
	public float closeTime = 5f;
	//

	//
	void Update () {
		if (open && !opened) {
			//
			if (!activated) {
				OpenDoors ();
			//	StartCoroutine (Open ());
				activated = true;
			}
		}
		if (close && !closed ) {
			if (!activated) {
				RotateWheel ();
				//StartCoroutine (Close ());
				activated = true;

			}
		}
	}
		
	void CloseSwitches()
	{
		open =  false;
		close = false;
	}
	//
	//
	void RotateWheel()
	{
		wheelAxle.open = true;
		StartCoroutine (NextActionGear ());
	}
	IEnumerator NextActionGear()
	{
		yield return new WaitForSeconds (openTime);
		RotateGearOne ();
	}
	//
	void RotateGearOne()
	{
		mainGearRotate.open = true;
		StartCoroutine (NextActionGearTwo ());
	}
	IEnumerator NextActionGearTwo()
	{
		yield return new WaitForSeconds (openTime);
		RotateGearTwo ();
	}
	void RotateGearTwo()
	{
		mainGearClose.open = true;
		StartCoroutine (NextActionCloseDoor ());
	}
	//
	IEnumerator NextActionCloseDoor()
	{
		yield return new WaitForSeconds (openTime);
		CloseDoors ();
	}
	void CloseDoors()
	{
		doors.open = true;
		opened = false;closed =true;CloseSwitches ();
		activated = false;
	}
	//
	//
	//
	void OpenDoors()
	{
		doors.close = true;
		StartCoroutine (NextActionOpenGear ());
	}
	IEnumerator NextActionOpenGear()
	{
		yield return new WaitForSeconds (openTime);
		OpenGearOne ();
	}
	//
	void OpenGearOne()
	{
		mainGearClose.close = true;
		StartCoroutine (NextActionOpenGearTwo ());
	}
	IEnumerator NextActionOpenGearTwo()
	{
		yield return new WaitForSeconds (openTime);
		OpenGearTwo ();
	}
	void OpenGearTwo()
	{
		mainGearRotate.close = true;
		StartCoroutine (NextActionRotateWheel ());
	}
	//
	IEnumerator NextActionRotateWheel()
	{
		yield return new WaitForSeconds (openTime);
		ReturnWheel ();
	}
	void ReturnWheel()
	{
		wheelAxle.close = true;
		closed = false;opened = true;CloseSwitches ();
		activated = false;

	}
	//

}
