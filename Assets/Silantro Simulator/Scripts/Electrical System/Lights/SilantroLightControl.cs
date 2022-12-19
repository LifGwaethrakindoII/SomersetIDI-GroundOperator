using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilantroLightControl : MonoBehaviour {
	//
	[HideInInspector]public List<SilantroLight> navigationLight;
	[HideInInspector]public List<SilantroLight> strobeLight;
	[HideInInspector]public List<SilantroLight> beaconLight;
	[HideInInspector]public List<SilantroLight> landingLight;
	//
	[HideInInspector]public SilantroControls Controlboard;
	[HideInInspector]public string LightSwitch;
	[HideInInspector]public bool isControllable = true;
	//
	[HideInInspector]public SilantroLight[] lights;
	//
	void Start()
	{
		foreach (SilantroLight light in lights) {
			if (light.lightType == SilantroLight.LightType.Navigation) {
				navigationLight.Add (light);
			}
			if (light.lightType == SilantroLight.LightType.Strobe) {
				strobeLight.Add (light);
			}
			if (light.lightType == SilantroLight.LightType.Beacon) {
				beaconLight.Add (light);
			}
			if (light.lightType == SilantroLight.LightType.Landing) {
				landingLight.Add (light);
			}
		}
		//
		LightSwitch = Controlboard.LightSwitch;
	}
	//
	void Update()
	{
		if(isControllable){
			if (Input.GetButtonDown (LightSwitch)) {
				//
				foreach (SilantroLight light in lights) {
					//
					if (light.state == SilantroLight.CurrentState.On) {
						light.TurnOff ();	
					} else {
						light.TurnOn ();
					}
				}
			}

		}
	}

}
