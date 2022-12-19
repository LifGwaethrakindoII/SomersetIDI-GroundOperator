//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;using System;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
//
using System.IO;
using System.Text;
//
public class SilantroBlade : MonoBehaviour {

	public enum EngineType
	{
		TurbopropEngine,
		PistonEngine,
		TurboshaftEngine,
		ElectricMotor
	}
	[HideInInspector]public EngineType engineType = EngineType.PistonEngine;
	[HideInInspector]public SilantroTurboShaft shaftEngine;
	[HideInInspector]public SilantroTurboProp propEngine;
	[HideInInspector]public SilantroPistonEngine pistonEngine;
	[HideInInspector]public SilantroElectricMotor electricMotor;
	//
	//
	public enum BladeType
	{
		Rotor,
		Propeller
	}
	[HideInInspector]public BladeType bladeType = BladeType.Rotor;
	//
	[HideInInspector]public float availablePower;
	[HideInInspector]public float powerPercentage;
	[HideInInspector]public float usefulPower;
	//
	public enum GearSelection
	{
		Gear1,Gear2,Gear3,Gear4,Gear5,Gear6,Gear0
	}
	[HideInInspector]public GearSelection gearSelection = GearSelection.Gear1;
	[HideInInspector]public float gearRatio;
	//
	[HideInInspector]public SilantroBladefoil bladefoil;
	[HideInInspector]public float bladeDiameter;
	[HideInInspector]public float advanceRatio;
	[HideInInspector]public float thrustCoefficient;
	[HideInInspector]public float powerCoefficient;
	[HideInInspector]public float bladeEfficency;
	//
	[HideInInspector]public float currentRPM;
	[HideInInspector]public float engineRPM;
	[HideInInspector]public float divisionRatio;
	//
	[HideInInspector]public float thrust;
	[HideInInspector]public float combustionFactor;
	[HideInInspector]public float fuelInput;
	[HideInInspector]public float fuelFactor;
	//
	[HideInInspector]public float linearSpeed;
	[HideInInspector]public float airDensity;
	//
	//
	[HideInInspector]public Transform Propeller;
	[HideInInspector]public Transform Rotor;
	[HideInInspector]public bool useFastPropeller;
	[HideInInspector]public Rigidbody airplane;
	//
	public enum RotationAxis
	{
		X,
		Y,
		Z
	}
	[HideInInspector]public RotationAxis rotationAxis = RotationAxis.X;
	//
	//
	public enum RotationDirection
	{
		CW,
		CCW
	}
	[HideInInspector]public RotationDirection rotationDirection = RotationDirection.CCW;
	//
	[HideInInspector]public Transform thruster;
	//
	[HideInInspector]public bool VTOLmode;
	//[HideInInspector]public bool STOLmode;
	[HideInInspector]public float VTOLmodeMultiplier;
	//[HideInInspector]public float STOLmodeMultiplier;
	[HideInInspector]public bool isControllable = true;
	//
	void Start()
	{
		divisionRatio = gearRatio;
		//
		if (engineType == EngineType.PistonEngine)
		{
			combustionFactor = pistonEngine.combusionFactor;
			fuelFactor = pistonEngine.fuelFactor;
			thruster = pistonEngine.Thruster;
			airplane = pistonEngine.Parent;
		}
		else if (engineType == EngineType.TurbopropEngine) 
		{
			combustionFactor = propEngine.combusionFactor;
			fuelFactor = propEngine.fuelFactor;
			thruster = propEngine.Thruster;airplane = propEngine.Parent;
		}
		else if (engineType == EngineType.TurboshaftEngine)
		{
			combustionFactor = shaftEngine.combusionFactor;
			fuelFactor = shaftEngine.fuelFactor;
			thruster = shaftEngine.Thruster;airplane = shaftEngine.Parent;
		}
		else if (engineType == EngineType.ElectricMotor)
		{
			thruster = electricMotor.Thruster;
			airplane = electricMotor.Parent;
		}
	}
	//
	void FixedUpdate()
	{
		if (isControllable) {
			//
			if (engineType == EngineType.PistonEngine) {
				availablePower = pistonEngine.brakeHorsePower;
				linearSpeed = pistonEngine.Parent.velocity.magnitude;
				engineRPM = pistonEngine.CurrentRPM;
				airDensity = pistonEngine.airDensity;
			} else if (engineType == EngineType.TurbopropEngine) {
				availablePower = propEngine.shaftHorsePower;
				linearSpeed = propEngine.Parent.velocity.magnitude;
				engineRPM = propEngine.currentLPRPM;
				airDensity = propEngine.airDensity;
			} else if (engineType == EngineType.TurboshaftEngine) {
				availablePower = shaftEngine.shaftHorsePower;
				linearSpeed = shaftEngine.Parent.velocity.magnitude;
				engineRPM = shaftEngine.currentLPRPM;
				airDensity = shaftEngine.airDensity;
			} else if (engineType == EngineType.ElectricMotor) {
				availablePower = electricMotor.horsePower;
				linearSpeed = electricMotor.Parent.velocity.magnitude;
				engineRPM = electricMotor.coreRPM;
				airDensity = electricMotor.airDensity;
			}
			//
			//
			CalculateThrust ();
			//
			if (Propeller) {
				if (rotationDirection == RotationDirection.CCW) {
					if (rotationAxis == RotationAxis.X) {
						Propeller.Rotate (new Vector3 (engineRPM / 2f * Time.deltaTime, 0, 0));
					}
					if (rotationAxis == RotationAxis.Y) {
						Propeller.Rotate (new Vector3 (0, engineRPM / 2f * Time.deltaTime, 0));
					}
					if (rotationAxis == RotationAxis.Z) {
						Propeller.Rotate (new Vector3 (0, 0, engineRPM / 2f * Time.deltaTime));
					}
				}
				//
				if (rotationDirection == RotationDirection.CW) {
					if (rotationAxis == RotationAxis.X) {
						Propeller.Rotate (new Vector3 (-1f * engineRPM / 2f * Time.deltaTime, 0, 0));
					}
					if (rotationAxis == RotationAxis.Y) {
						Propeller.Rotate (new Vector3 (0, -1f * engineRPM / 2f * Time.deltaTime, 0));
					}
					if (rotationAxis == RotationAxis.Z) {
						Propeller.Rotate (new Vector3 (0, 0, -1f * engineRPM / 2f * Time.deltaTime));
					}
				}
			}
			//
			if (Rotor) {
				if (rotationDirection == RotationDirection.CCW) {
					if (rotationAxis == RotationAxis.X) {
						Rotor.Rotate (new Vector3 (engineRPM / 3f * Time.deltaTime, 0, 0));
					}
					if (rotationAxis == RotationAxis.Y) {
						Rotor.Rotate (new Vector3 (0, engineRPM / 3f * Time.deltaTime, 0));
					}
					if (rotationAxis == RotationAxis.Z) {
						Rotor.Rotate (new Vector3 (0, 0, engineRPM / 3f * Time.deltaTime));
					}
				}
				//
				if (rotationDirection == RotationDirection.CW) {
					if (rotationAxis == RotationAxis.X) {
						Rotor.Rotate (new Vector3 (-1f * engineRPM / 3f * Time.deltaTime, 0, 0));
					}
					if (rotationAxis == RotationAxis.Y) {
						Rotor.Rotate (new Vector3 (0, -1f * engineRPM / 3f * Time.deltaTime, 0));
					}
					if (rotationAxis == RotationAxis.Z) {
						Rotor.Rotate (new Vector3 (0, 0, -1f * engineRPM / 3f * Time.deltaTime));
					}
				}
			}
		}
	}
	//
	void CalculateThrust()
	{
		//
		currentRPM = engineRPM/divisionRatio;
		float superRPM = currentRPM / 60f;
		//
		advanceRatio = linearSpeed/(currentRPM*bladeDiameter);
		if (linearSpeed > 40f) {
			thrustCoefficient = bladefoil.thrustCurve.Evaluate (advanceRatio);
			powerCoefficient = bladefoil.powerCurve.Evaluate (advanceRatio);
			bladeEfficency = bladefoil.etaCurve.Evaluate (advanceRatio) * 100f;
		} else {
			thrustCoefficient = bladefoil.StaticThrustCurve.Evaluate (engineRPM);
			powerCoefficient = bladefoil.StaticPowerCurve.Evaluate (engineRPM);
		}
		//
		//
		//THRUST CALCULATION
		float dynamicShaftPower = Mathf.Pow((availablePower * 550f),2/3f);
		//Calculate Propeller Area
		float PropArea = (3.142f * Mathf.Pow((3.28084f *bladeDiameter),2f))/4f;
		float dynamicArea = Mathf.Pow((2f * airDensity * 0.0624f * PropArea),1/3f);
		//
		if (engineType != EngineType.ElectricMotor) {
			thrust = combustionFactor * fuelFactor * dynamicShaftPower * dynamicArea;
		} else {
			thrust = dynamicShaftPower * dynamicArea;
		}
		//
		if (thrust > 0f)
		{
			float thrustSetup;
			//
			if (VTOLmode) {
				thrustSetup = thrust * VTOLmodeMultiplier;
				Vector3 force = thruster.forward * thrustSetup;
				airplane.AddForce (force, ForceMode.Force);
			} 
			else {
				Vector3 force = thruster.forward * thrust;
				airplane.AddForce (force, ForceMode.Force);
			}
		}
		///
		//
	}
}
//
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroBlade))]
public class BladeEditor: Editor
{
	Color backgroundColor;
	Color silantroColor = Color.yellow;
	//

	public override void OnInspectorGUI()
	{
		backgroundColor = GUI.backgroundColor;
		//
		DrawDefaultInspector ();serializedObject.Update ();
		//
		//SilantroTurboJet kk;
		SilantroBlade blade = (SilantroBlade)target;
		//
		GUILayout.Space(10f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Power Configuration", MessageType.None);
		GUI.color = backgroundColor;
		//
		GUILayout.Space(5f);
		blade.engineType = (SilantroBlade.EngineType)EditorGUILayout.EnumPopup("Powerplant",blade.engineType);
		GUILayout.Space(3f);if (blade.engineType == SilantroBlade.EngineType.PistonEngine)
		{
			blade.pistonEngine = EditorGUILayout.ObjectField (" ", blade.pistonEngine, typeof(SilantroPistonEngine), true) as SilantroPistonEngine;
			GUILayout.Space(5f);
			EditorGUILayout.LabelField("Shaft Power",blade.availablePower.ToString("0.00")+ " Hp");	
		}
		else if (blade.engineType == SilantroBlade.EngineType.TurbopropEngine) 
		{
			blade.propEngine = EditorGUILayout.ObjectField (" ", blade.propEngine, typeof(SilantroTurboProp), true) as SilantroTurboProp;
			GUILayout.Space(5f);
			EditorGUILayout.LabelField("Shaft Power",blade.availablePower.ToString("0.00")+ " Hp");
		} 
		else if (blade.engineType == SilantroBlade.EngineType.TurboshaftEngine)
		{
			blade.shaftEngine = EditorGUILayout.ObjectField (" ", blade.shaftEngine, typeof(SilantroTurboShaft), true) as SilantroTurboShaft;
			GUILayout.Space(5f);
			EditorGUILayout.LabelField("Shaft Power",blade.availablePower.ToString("0.00")+ " Hp");
		}
		else if (blade.engineType == SilantroBlade.EngineType.ElectricMotor)
		{
			blade.electricMotor = EditorGUILayout.ObjectField (" ", blade.electricMotor, typeof(SilantroElectricMotor), true) as SilantroElectricMotor;
			GUILayout.Space(5f);
			EditorGUILayout.LabelField("Shaft Power",blade.availablePower.ToString("0.00")+ " Hp");
		}
		//
		//blade.powerPercentage = EditorGUILayout.Slider("Extraction Percentage",blade.powerPercentage,0,100);
		//
		GUILayout.Space(15f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Blade Configuration", MessageType.None);
		GUI.color = backgroundColor;
		//
		GUILayout.Space(5f);
		blade.bladeType = (SilantroBlade.BladeType)EditorGUILayout.EnumPopup("Blade Type",blade.bladeType);
		GUILayout.Space(3f);
		blade.bladefoil = EditorGUILayout.ObjectField ("Blade Airfoil", blade.bladefoil, typeof(SilantroBladefoil), true)as SilantroBladefoil;
		GUILayout.Space(3f);blade.bladeDiameter = EditorGUILayout.FloatField ("Blade Diameter", blade.bladeDiameter);
		GUILayout.Space(10f);
		EditorGUILayout.LabelField ("Advance Ratio", blade.advanceRatio.ToString ("0.00"));
		EditorGUILayout.LabelField ("Thrust Coefficient", blade.thrustCoefficient.ToString ("0.00"));
		EditorGUILayout.LabelField ("Power Coefficient", blade.powerCoefficient.ToString ("0.00"));
		EditorGUILayout.LabelField ("Blade Efficiency", blade.bladeEfficency.ToString ("0.0")+ " %");
		//
		GUILayout.Space(15f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Model Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		if (blade.bladeType == SilantroBlade.BladeType.Propeller) {
			blade.Propeller = EditorGUILayout.ObjectField ("Propeller Transform", blade.Propeller, typeof(Transform), true)as Transform;
		} else if (blade.bladeType == SilantroBlade.BladeType.Rotor) {
			blade.Rotor = EditorGUILayout.ObjectField ("Rotor Transform", blade.Rotor, typeof(Transform), true)as Transform;
		}
		//
		GUILayout.Space(3f);
		blade.rotationAxis = (SilantroBlade.RotationAxis)EditorGUILayout.EnumPopup("Rotation Axis",blade.rotationAxis);
		GUILayout.Space(3f);
		blade.rotationDirection = (SilantroBlade.RotationDirection)EditorGUILayout.EnumPopup("Rotation Direction",blade.rotationDirection);
		//
		//
		GUILayout.Space(15f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Gear Configuration", MessageType.None);
		GUI.color = backgroundColor;
		//
		GUILayout.Space(3f);
		blade.gearSelection = (SilantroBlade.GearSelection)EditorGUILayout.EnumPopup("Gear Selection",blade.gearSelection);
		//
		if (blade.gearSelection == SilantroBlade.GearSelection.Gear1) {
			blade.gearRatio = 17.4f;
		}
		else if (blade.gearSelection == SilantroBlade.GearSelection.Gear2) {
			blade.gearRatio = 14.6f;
		}
		else if (blade.gearSelection == SilantroBlade.GearSelection.Gear3) {
			blade.gearRatio = 11.8f;
		}
		else if (blade.gearSelection == SilantroBlade.GearSelection.Gear4) {
			blade.gearRatio = 9.0f;
		}
		else if (blade.gearSelection == SilantroBlade.GearSelection.Gear5) {
			blade.gearRatio = 6.2f;
		}
		else if (blade.gearSelection == SilantroBlade.GearSelection.Gear6) {
			blade.gearRatio = 2.4f;
		}
		else if (blade.gearSelection == SilantroBlade.GearSelection.Gear0) {
			blade.gearRatio = 1.0f;
		}
		//
		EditorGUILayout.LabelField ("Gear Ratio", blade.gearRatio.ToString("0.0"));
		//
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Current RPM", blade.currentRPM.ToString("0.0")+ " RPM");
		//
		GUILayout.Space(15f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Output", MessageType.None);
		GUI.color = backgroundColor;
		//
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Thrust Generated", blade.thrust.ToString("0.00")+ " N");
		//
		if (GUI.changed) {
			EditorUtility.SetDirty (blade);
			EditorSceneManager.MarkSceneDirty (blade.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();

	}
}
#endif