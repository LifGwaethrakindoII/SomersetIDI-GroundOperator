using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEngine.UI;

public class SilantroData : MonoBehaviour {

	//
	public SilantroInstrumentation cog;
	public SilantroController controller;
	//
	public SilantroAerofoil rightWing;
	public SilantroSapphire weatherController;
	public SilantroWeaponsManager storesManager;
	//
	public GameObject panel;
	//
	public Text gearState;
	public Text speed;
	public Text altitude;
	public Text fuel;
	public Text weight;
	public Text brake;
	public Text density;
	public Text temperature;
	public Text pressure;
	public Text enginePower;
	public Text thrust;
	public Text incrementalThrust;
	public Text flapLevel;
	public Text slatLevel;
	public Text Time;
	public Text weaponCount;
	public Text ActiveWeapon;
	//
	void Start()
	{
		weaponCount.enabled = false;
		ActiveWeapon.enabled = false;
		//
		if (storesManager != null) {
			weaponCount.enabled = true;
			ActiveWeapon.enabled = true;
			//
			weaponCount.text = "Available Weapons: "+storesManager.availableWeapons;
		}
	}
	// Update is called once per frame
	void FixedUpdate () {
		//
		if (rightWing) {
			flapLevel.text = "Flaps = " + (rightWing.CurrentFlapDeflection * -1f).ToString ("0.0") + " °";
			slatLevel.text = "Slats = " + rightWing.CurrentSlatDeflection.ToString ("0.0") + " °";
		
		}//
		if (controller) {
			incrementalThrust.text = "Incremental Brake = " + (controller.gearHelper.brakeControl * 100f).ToString ("0.0") + " %";
			weight.text = "Weight = " + controller.currentWeight.ToString ("0.0") + " kg";
			if (controller.engineType != SilantroController.AircraftType.Electric) {
				fuel.text = "Fuel = " + controller.fuelsystem.currentTankFuel.ToString ("0.0") + " kg";
			}
		//
			if (controller.engineType == SilantroController.AircraftType.Piston && controller.pistons != null) {
				enginePower.text = "Engine Throttle = "+(controller.pistons [0].FuelInput * 100f).ToString("0.0")+ " %";
			}
			if (controller.engineType == SilantroController.AircraftType.TurboProp && controller.turboprop != null) {
				enginePower.text = "Engine Throttle = "+(controller.turboprop [0].FuelInput * 100f).ToString("0.0")+ " %";
			}
			if (controller.engineType == SilantroController.AircraftType.TurboFan && controller.turbofans != null) {
				enginePower.text = "Engine Throttle = "+(controller.turbofans [0].FuelInput * 100f).ToString("0.0")+ " %";
			}
			if (controller.engineType == SilantroController.AircraftType.Turboshaft && controller.shaftEngines != null) {
				enginePower.text = "Engine Throttle = "+(controller.shaftEngines [0].FuelInput * 100f).ToString("0.0")+ " %";
			}
			if (controller.engineType == SilantroController.AircraftType.TurboJet && controller.turboJet != null) {
				enginePower.text = "Engine Throttle = "+(controller.turboJet [0].FuelInput * 100f).ToString("0.0")+ " %";
			}
			if (controller.engineType == SilantroController.AircraftType.Electric && controller.electricMotors != null) {
				enginePower.text = "Engine Throttle = "+(controller.electricMotors [0].powerInput * 100f).ToString("0.0")+ " %";
			}
			//
			if (weatherController != null) {
				Time.text = weatherController.CurrentTime;
			}
			//
			if (controller.gearHelper.brakeActivated == true) {
				brake.text = "Brake State = On";
			} else {
				brake.text = "Brake State = Off";
			}
			//
			//
			if (controller.gearHelper.gearOpened) {
				gearState.text = "Gear State = Open";
			} else if (controller.gearHelper.gearClosed) {
				gearState.text = "Gear State = Closed";
			}
			//
			thrust.text = "Total Thrust = " + controller.totalThrustGenerated.ToString ("0.0") + " N";
		}
		//
		if (cog) {
			speed.text = "Airspeed = " + cog.currentSpeed.ToString ("0.0") + " knots";
			pressure.text = "Pressure = " + cog.ambientPressure.ToString ("0.0") + " kpa";
			temperature.text = "Temperature = " + cog.ambientTemperature.ToString ("0.0") + " °C";
			density.text = "Air Density = " + cog.airDensity.ToString ("0.000") + " kg/m3";

			altitude.text = "Altitude = " + cog.currentAltitude.ToString ("0.0") + " ft";
		}
		//
		if (storesManager) {
			ActiveWeapon.text = "Current Weapon: " + storesManager.currentWeapon;
		}
		//
	}
}
