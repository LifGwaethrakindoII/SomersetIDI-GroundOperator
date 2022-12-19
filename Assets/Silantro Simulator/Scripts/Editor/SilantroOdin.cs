using UnityEngine;
using System;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class Odin : MonoBehaviour {

	public class SilantroMenu
	{
		//
		//
		[MenuItem("Oyedoyin/Electrical System/Engines/Electric Motor")]
		private static void AddMotorEngine()
		{
			if (Selection.activeGameObject != null) {
				GameObject thruster = new GameObject ();
				thruster.name = "Thruster";
				thruster.transform.parent = Selection.activeGameObject.transform;
				thruster.transform.localPosition = new Vector3 (0, 0, -2);
				//
				EditorSceneManager.MarkSceneDirty (thruster.gameObject.scene);
				//
				Selection.activeGameObject.name = "Default Motor Engine";
				//
				Rigidbody parent = Selection.activeGameObject.transform.root.gameObject.GetComponent<Rigidbody> ();
				if (parent == null) {
					Debug.Log ("Engine is not parented to an Aircraft!! Create a Kinematic Rigidbody if you're just testing the Engine");
				}
				SilantroElectricMotor motor = Selection.activeGameObject.AddComponent<SilantroElectricMotor> ();
				if (parent != null) {
					motor.Parent = parent;
				}//
				//
				GameObject Props = new GameObject ("Default Propeller");
				//
				Props.transform.parent = Selection.activeGameObject.transform;
				SilantroBlade blade = Props.AddComponent<SilantroBlade> ();
				blade.bladeType = SilantroBlade.BladeType.Propeller;
				blade.engineType = SilantroBlade.EngineType.ElectricMotor;
				blade.electricMotor = motor;
				GameObject bladefoil = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Airfoils/Blades/BLD 08F5.prefab",typeof(GameObject));
				blade.bladefoil = bladefoil.GetComponent<SilantroBladefoil> ();
				AudioClip run = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Propeller/Propeller Running.wav",typeof(AudioClip));
				motor.runningSound = run;
				//
			}
			else {
				Debug.Log ("Please Select GameObject to add Engine to..");
			}
		}
		//
		//
		//
		//
		//
		[MenuItem("Oyedoyin/Electrical System/Solar Energy/Solar Module",false,911)]
		private static void AddSolarModule()
		{
			GameObject moduleObject = new GameObject ("Solar Module");
			//
			SilantroModule module = moduleObject.AddComponent <SilantroModule>();
			GameObject cell = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Solar/Cells/SFD046.prefab",typeof(GameObject));
			module.solarCell = cell.GetComponent<SilantroCell>();
			EditorSceneManager.MarkSceneDirty (moduleObject.scene);
				//
			moduleObject.transform.position = new Vector3(0,2,0);
		}
		//
		//
		[MenuItem("Oyedoyin/Electrical System/Solar Energy/Solar Panel")]
		private static void AddSolarPanel()
		{
			GameObject panelObject = new GameObject ("Solar Panel");
			//
			SilantroSolarPanel panel = panelObject.AddComponent <SilantroSolarPanel>();
			GameObject moduleObject = new GameObject ("Solar Module");
			//
			SilantroModule module = moduleObject.AddComponent <SilantroModule>();
			GameObject cell = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Solar/Cells/SFD046.prefab",typeof(GameObject));
			module.solarCell = cell.GetComponent<SilantroCell>();
			//
			moduleObject.transform.parent = panel.transform;
			panel.connectionType = SilantroSolarPanel.ConnectionType.Series;panel.seriesModules= new SilantroModule[]{module};
		//
			//
			panelObject.transform.position = new Vector3(0,2,0);
		}
		//
		//
		[MenuItem("Oyedoyin/Electrical System/Power/Battery",false,100)]
		private static void AddBattery()
		{
			GameObject batteryObject = new GameObject ("Lead Acid Battery");
			//
			SilantroBattery battery = batteryObject.AddComponent <SilantroBattery>();
			battery.batteryType = SilantroBattery.BatteryType.LeadAcid;
			//
			//module.solarCell = cell.GetComponent<SilantroCell>();
			EditorSceneManager.MarkSceneDirty (batteryObject.scene);
			//
			batteryObject.transform.position = new Vector3(0,2,0);
		}
		//
		//
		[MenuItem("Oyedoyin/Electrical System/Power/Battery Charger")]
		private static void AddBatteryCharger()
		{
			GameObject batteryObject = new GameObject ("Solar Battery Charger");
			//
			SilantroCharger charger = batteryObject.AddComponent <SilantroCharger>();
			EditorSceneManager.MarkSceneDirty (batteryObject.scene);
			//
			batteryObject.transform.position = new Vector3(0,2,0);
		}
		[MenuItem("Oyedoyin/Electrical System/Power/Battery Pack")]
		private static void AddBatteryPack()
		{
			GameObject batteryObject = new GameObject ("Lead Acid Battery");
			//
			SilantroBattery battery = batteryObject.AddComponent <SilantroBattery>();
			battery.batteryType = SilantroBattery.BatteryType.LeadAcid;
			//
			GameObject batteryObject1 = new GameObject ("Lead Acid Battery");
			//
			SilantroBattery battery1 = batteryObject1.AddComponent <SilantroBattery>();
			battery1.batteryType = SilantroBattery.BatteryType.LeadAcid;
			//
			//module.solarCell = cell.GetComponent<SilantroCell>();
			EditorSceneManager.MarkSceneDirty (batteryObject.scene);
			//
			GameObject packObject = new GameObject("Battery Pack");
			SilantroBatteryPack pack = packObject.AddComponent <SilantroBatteryPack> ();
			pack.seriesBatteries = new SilantroBattery[]{ battery ,battery1};
			battery.transform.parent = pack.transform;battery1.transform.parent = pack.transform;
			//
			packObject.transform.position = new Vector3(0,2,0);
		}
		//
		//
		//
		//
		[MenuItem("Oyedoyin/Propulsion System/Engines/Reaction Engines/TurboJet Engine")]
		private static void AddTurboJetEngine()
		{
			if (Selection.activeGameObject != null) {
				GameObject thruster = new GameObject ();
				thruster.name = "Thruster";
				thruster.transform.parent = Selection.activeGameObject.transform;
				thruster.transform.localPosition = new Vector3 (0, 0, -2);
				//
				EditorSceneManager.MarkSceneDirty (thruster.gameObject.scene);
				//
				GameObject fan = new GameObject ();
				fan.name = "Fan";
				fan.transform.parent = Selection.activeGameObject.transform;
				fan.transform.localPosition = new Vector3 (0, 0, 2);
				Selection.activeGameObject.name = "Default TurboJet Engine";
				//
				//
				GameObject effects = new GameObject("Engine Effects");
				effects.transform.parent = Selection.activeGameObject.transform;
				effects.transform.localPosition = new Vector3 (0, 0, -2);
				//
				GameObject smoke = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Componets/Exhaust Smoke.prefab",typeof(GameObject));
				GameObject smokeEffect = GameObject.Instantiate (smoke, effects.transform.position, Quaternion.Euler(0,-180,0),effects.transform);
				//
				Rigidbody parent = Selection.activeGameObject.transform.root.gameObject.GetComponent<Rigidbody> ();
				if (parent == null) {
					Debug.Log ("Engine is not parented to an Aircraft!! Create a Kinematic Rigidbody if you're just testing the Engine");
				}
				SilantroTurboJet jet = Selection.activeGameObject.AddComponent<SilantroTurboJet> ();
				jet.Thruster = thruster.transform;
				jet.intakeFanPoint = fan.transform;
				if (parent != null) {
					jet.Parent = parent;
				}//
				AudioClip start = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Default Start.wav",typeof(AudioClip));
				AudioClip stop = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Default Shutdown.wav",typeof(AudioClip));
				//	AudioClip alert = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Fuel Alert.wav",typeof(AudioClip));
				AudioClip run = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Jet/Jet Running.wav",typeof(AudioClip));
				jet.EngineIdleSound = run;
				jet.EngineStartSound = start;jet.EngineShutdownSound = stop;
				//
				jet.diplaySettings = true;
				jet.exhaustSmoke = smokeEffect.GetComponent<ParticleSystem>();

			} else {
				Debug.Log ("Please Select GameObject to add Engine to..");
			}
		}
		//
		//SETUP ROCKET ENGINE
		[MenuItem("Oyedoyin/Propulsion System/Engines/Reaction Engines/Rocket Motor")]
		private static void AddRocketEngine()
		{
			if (Selection.activeGameObject != null) {
				GameObject thruster = new GameObject ();
				thruster.name = "Thruster";
				thruster.transform.parent = Selection.activeGameObject.transform;
				thruster.transform.localPosition = new Vector3 (0, 0, -2);
				//
				EditorSceneManager.MarkSceneDirty (thruster.gameObject.scene);
				//
				GameObject fan = new GameObject ();
				fan.name = "Fan";
				fan.transform.parent = Selection.activeGameObject.transform;
				fan.transform.localPosition = new Vector3 (0, 0, 2);
				Selection.activeGameObject.name = "Default Rocket Engine";
				//
				//
				GameObject effects = new GameObject("Engine Effects");
				effects.transform.parent = Selection.activeGameObject.transform;
				effects.transform.localPosition = new Vector3 (0, 0, -2);
			}
			else {
				Debug.Log ("Please Select GameObject to add Engine to..");
			}
		}
		//
		//SETUP TURBOFAN ENGINE
		[MenuItem("Oyedoyin/Propulsion System/Engines/Reaction Engines/TurboFan Engine")]
		private static void AddTurboFanEngine()
		{
			if (Selection.activeGameObject != null) {
				GameObject thruster = new GameObject ();
				thruster.name = "Thruster";
				thruster.transform.parent = Selection.activeGameObject.transform;
				thruster.transform.localPosition = new Vector3 (0, 0, -2);
				//
				EditorSceneManager.MarkSceneDirty (thruster.gameObject.scene);
				//
				GameObject fan = new GameObject ();
				fan.name= "Fan";
				fan.transform.parent = Selection.activeGameObject.transform;
				fan.transform.localPosition = new Vector3 (0, 0, 2);	Selection.activeGameObject.name = "Default TurboFan Engine";
				//
				//
				GameObject effects = new GameObject("Engine Effects");
				effects.transform.parent = Selection.activeGameObject.transform;
				effects.transform.localPosition = new Vector3 (0, 0, -2);
				//
				GameObject smoke = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Componets/Exhaust Smoke.prefab",typeof(GameObject));
				GameObject smokeEffect = GameObject.Instantiate (smoke, effects.transform.position, Quaternion.Euler(0,-180,0),effects.transform);
				//
				Rigidbody parent = Selection.activeGameObject.transform.root.gameObject.GetComponent<Rigidbody>();
				if (parent == null) {
					Debug.Log ("Engine is not parented to an Aircraft!! Create a default Rigidbody is you're just testing the Engine");
				}
				SilantroTurboFan jet =  Selection.activeGameObject.AddComponent<SilantroTurboFan> ();
				jet.Thruster = thruster.transform;
				jet.fan = fan.transform;
				if (parent != null) {
					jet.Parent = parent;
				}
				AudioClip start = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Default Start.wav",typeof(AudioClip));
				AudioClip stop = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Default Shutdown.wav",typeof(AudioClip));
				//AudioClip alert = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Fuel Alert.wav",typeof(AudioClip));
				AudioClip run = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Jet/Jet Running.wav",typeof(AudioClip));
				jet.EngineIdleSound = run;jet.EngineStartSound = start;jet.EngineShutdownSound = stop;
				//
				jet.diplaySettings = true;
				jet.exhaustSmoke = smokeEffect.GetComponent<ParticleSystem> ();
			} 
			else {
				Debug.Log ("Please Select GameObject to add Engine to..");
			}
			//
			//
		}
		//
		//
		//SETUP TURBOPROP ENGINE
		[MenuItem("Oyedoyin/Propulsion System/Engines/Drive Engines/TurboProp Engine")]
		private static void AddTurboPropEngine()
		{
			if (Selection.activeGameObject != null) {
				GameObject thruster = new GameObject ();
				thruster.name = "Thruster";
				thruster.transform.parent = Selection.activeGameObject.transform;
				thruster.transform.localPosition = new Vector3 (0, 0, -1);
				//
				EditorSceneManager.MarkSceneDirty (thruster.gameObject.scene);
				//
				Rigidbody parent = Selection.activeGameObject.transform.root.gameObject.GetComponent<Rigidbody>();
				if (parent == null) {
					Debug.Log ("Engine is not parented to an Aircraft!! Create a default Rigidbody is you're just testing the Engine");
				}
				SilantroTurboProp prop = Selection.activeGameObject.AddComponent<SilantroTurboProp> ();
				prop.Thruster = thruster.transform;
				prop.Parent = parent;	Selection.activeGameObject.name = "Default TurboProp Engine";
				//
				GameObject Props = new GameObject ("Default Propeller");
				//
				Props.transform.parent = Selection.activeGameObject.transform;
				SilantroBlade blade = Props.AddComponent<SilantroBlade> ();
				blade.bladeType = SilantroBlade.BladeType.Propeller;
				blade.engineType = SilantroBlade.EngineType.TurbopropEngine;
				blade.propEngine = prop;
				GameObject bladefoil = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Airfoils/Blades/BLD 08F5.prefab",typeof(GameObject));
				blade.bladefoil = bladefoil.GetComponent<SilantroBladefoil> ();
				//
				//
				//
				GameObject effects = new GameObject("Engine Effects");
				effects.transform.parent = Selection.activeGameObject.transform;
				effects.transform.localPosition = new Vector3 (0, 0, -2);
				//
				GameObject smoke = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Componets/Exhaust Smoke.prefab",typeof(GameObject));
				GameObject smokeEffect = GameObject.Instantiate (smoke, effects.transform.position, Quaternion.Euler(0,-180,0),effects.transform);
				//
				prop.diplaySettings = true;
				prop.exhaustSmoke = smokeEffect.GetComponent<ParticleSystem> ();
				//prop.fastPropeller = fastPropller.transform;prop.Propeller = normalPropeller.transform;
				AudioClip start = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Default Start.wav",typeof(AudioClip));
				AudioClip stop = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Default Shutdown.wav",typeof(AudioClip));
				//	AudioClip alert = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Fuel Alert.wav",typeof(AudioClip));
				AudioClip run = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Propeller/Propeller Running.wav",typeof(AudioClip));
				prop.EngineStartSound = start;prop.EngineShutdownSound = stop;prop.EngineIdleSound = run;
			} else {
				Debug.Log ("Please Select GameObject to add Engine to..");
			}
		}
		//
		//SETUP PiSTON ENGINE
		[MenuItem("Oyedoyin/Propulsion System/Engines/Drive Engines/Piston Engine")]
		private static void AddPistonEngine()
		{
			if (Selection.activeGameObject != null) {
				GameObject thruster = new GameObject ();
				thruster.name = "Thruster";
				thruster.transform.parent = Selection.activeGameObject.transform;
				thruster.transform.localPosition = new Vector3 (0, 0, -1);
				//
				EditorSceneManager.MarkSceneDirty (thruster.gameObject.scene);
				//
				Rigidbody parent = Selection.activeGameObject.transform.root.gameObject.GetComponent<Rigidbody>();
				if (parent == null) {
					Debug.Log ("Engine is not parented to an Aircraft!! Create a default Rigidbody is you're just testing the Engine");
				}
				SilantroPistonEngine prop = Selection.activeGameObject.AddComponent<SilantroPistonEngine> ();
				prop.Thruster = thruster.transform;
				prop.Parent = parent;	Selection.activeGameObject.name = "Default Piston Engine";
				//
				GameObject Props = new GameObject ("Default Propeller");
				//
				Props.transform.parent = Selection.activeGameObject.transform;
				SilantroBlade blade = Props.AddComponent<SilantroBlade> ();
				blade.bladeType = SilantroBlade.BladeType.Propeller;
				blade.engineType = SilantroBlade.EngineType.PistonEngine;
				blade.pistonEngine = prop;
				GameObject bladefoil = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Airfoils/Blades/BLD 08F5.prefab",typeof(GameObject));
				blade.bladefoil = bladefoil.GetComponent<SilantroBladefoil> ();
				//
				//
				//
				GameObject effects = new GameObject("Engine Effects");
				effects.transform.parent = Selection.activeGameObject.transform;
				effects.transform.localPosition = new Vector3 (0, 0, -2);
				//
				GameObject smoke = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Componets/Exhaust Smoke.prefab",typeof(GameObject));
				GameObject smokeEffect = GameObject.Instantiate (smoke, effects.transform.position, Quaternion.Euler(0,-180,0),effects.transform);
				//
				prop.diplaySettings = true;
				prop.exhaustSmoke = smokeEffect.GetComponent<ParticleSystem> ();
				AudioClip start = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Default Start.wav",typeof(AudioClip));
				AudioClip stop = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Default Shutdown.wav",typeof(AudioClip));
				//	AudioClip alert = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Fuel Alert.wav",typeof(AudioClip));
				AudioClip run = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Propeller/Merlin Running 1.wav",typeof(AudioClip));
				prop.EngineStartSound = start;prop.EngineShutdownSound = stop;prop.EngineIdleSound = run;
			} else {
				Debug.Log ("Please Select GameObject to add Engine to..");
			}
		}
		//
		//SETUP TURBOSHAFT ENGINE
		[MenuItem("Oyedoyin/Propulsion System/Engines/Drive Engines/TurboShaft Engine")]
		private static void AddTurboshaftEngine()
		{
			if (Selection.activeGameObject != null) {
				GameObject thruster = new GameObject ();
				thruster.name = "Thruster";
				thruster.transform.parent = Selection.activeGameObject.transform;
				thruster.transform.localPosition = new Vector3 (0, 0, -1);
				//
				EditorSceneManager.MarkSceneDirty (thruster.gameObject.scene);
				//
				Rigidbody parent = Selection.activeGameObject.transform.root.gameObject.GetComponent<Rigidbody>();
				if (parent == null) {
					Debug.Log ("Engine is not parented to an Aircraft!! Create a default Rigidbody is you're just testing the Engine");
				} 
				SilantroTurboShaft shaft = Selection.activeGameObject.AddComponent<SilantroTurboShaft> ();
				shaft.Thruster = thruster.transform;
				Selection.activeGameObject.name = "Default TurboShaft Engine";
				if(parent != null) {
					shaft.Parent = parent;	
				}
				GameObject fan = new GameObject ();
				fan.name= "Intake Fan";
				fan.transform.parent = Selection.activeGameObject.transform;
				fan.transform.localPosition = new Vector3 (0, 0, 2);
				shaft.intakeFan = fan.transform;
				//
				GameObject Props = new GameObject ("Default Rotor");
				//
				Props.transform.parent = Selection.activeGameObject.transform;
				SilantroBlade blade = Props.AddComponent<SilantroBlade> ();
				blade.bladeType = SilantroBlade.BladeType.Rotor;
				blade.engineType = SilantroBlade.EngineType.TurboshaftEngine;
				blade.shaftEngine = shaft;
				GameObject bladefoil = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Airfoils/Blades/BLD 08F5.prefab",typeof(GameObject));
				blade.bladefoil = bladefoil.GetComponent<SilantroBladefoil> ();
				//
				//
				//
				GameObject effects = new GameObject("Engine Effects");
				effects.transform.parent = Selection.activeGameObject.transform;
				effects.transform.localPosition = new Vector3 (0, 0, -2);
				//
				GameObject smoke = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Componets/Exhaust Smoke.prefab",typeof(GameObject));
				GameObject smokeEffect = GameObject.Instantiate (smoke, effects.transform.position, Quaternion.Euler(0,-180,0),effects.transform);
				//
				shaft.diplaySettings = true;
				shaft.exhaustSmoke = smokeEffect.GetComponent<ParticleSystem> ();
				//prop.fastPropeller = fastPropller.transform;prop.Propeller = normalPropeller.transform;
				AudioClip start = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Default Start.wav",typeof(AudioClip));
				AudioClip stop = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Default Shutdown.wav",typeof(AudioClip));
				//	AudioClip alert = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Fuel Alert.wav",typeof(AudioClip));
				AudioClip run = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Turboshaft/Multiple Blades.wav",typeof(AudioClip));
				shaft.EngineStartSound = start;shaft.EngineShutdownSound = stop;shaft.EngineIdleSound = run;
			} else {
				Debug.Log ("Please Select GameObject to add Engine to..");
			}
		}
		//
		//SETUP TURBOFAN ENGINE
		[MenuItem("Oyedoyin/Propulsion System/Engines/Drive Engines/LiftFan Engine")]
		private static void AddLiftFanEngine()
		{
			if (Selection.activeGameObject != null) {
				GameObject thruster = new GameObject ();
				thruster.name = "Thruster";
				thruster.transform.parent = Selection.activeGameObject.transform;
				thruster.transform.localPosition = new Vector3 (0, -1, 0);
				//
				EditorSceneManager.MarkSceneDirty (thruster.gameObject.scene);
				//
				GameObject fan = new GameObject ();
				fan.name= "Fan";
				fan.transform.parent = Selection.activeGameObject.transform;
				fan.transform.localPosition = new Vector3 (0, 1, 0);
				//
				Selection.activeGameObject.name = "Default LiftFan Engine";
				Rigidbody parent = Selection.activeGameObject.transform.root.gameObject.GetComponent<Rigidbody>();
				if (parent == null) {
					Debug.Log ("Engine is not parented to an Aircraft!! Create a default Rigidbody is you're just testing the Engine");
				}SilantroLiftFan liftfan =  Selection.activeGameObject.AddComponent<SilantroLiftFan> ();
				//
				liftfan.Thruster = thruster.transform;
				liftfan.fan = fan.transform;
				//
				if (parent != null) {
					liftfan.Parent = parent;
				}
				AudioClip start = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Default Start.wav",typeof(AudioClip));
				AudioClip stop = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Default Shutdown.wav",typeof(AudioClip));
				AudioClip run = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/LiftFan/Fan Running.wav",typeof(AudioClip));
				liftfan.fanStartClip = start;liftfan.fanShutdownClip = stop;liftfan.fanRunningClip = run;
			} else {
				Debug.Log ("Please Select GameObject to add Engine to..");
			}
		}
		//
		[MenuItem("Oyedoyin/Propulsion System/Fuel System/Fuel Distributor")]
		private static void AddFuelControl()
		{
			GameObject tank = new GameObject ("Fuel Distributor");
			SilantroFuelDistributor distributor = tank.AddComponent<SilantroFuelDistributor> ();
			AudioClip alert = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Default Fuel Alert.wav",typeof(AudioClip));
			distributor.fuelAlert = alert;
			EditorSceneManager.MarkSceneDirty (tank.gameObject.scene);
		}
		//
		//
		//
		//SETUP AEROFOILS
		[MenuItem("Oyedoyin/Aerofoil System/Structures/Controllable/Wing",false,1)]
		private static void AddWing()
		{
			GameObject wing;//EditorSceneManager.MarkSceneDirty (Selection.activeGameObject .gameObject.scene);
			if (Selection.activeGameObject != null )
			{
				wing = new GameObject ("Default Right Wing");
				wing.transform.parent = Selection.activeGameObject.transform;
				wing.transform.localPosition = new Vector3 (0, 0, 0);
			} else {
				wing = new GameObject ("Default Right Wing");
				GameObject parent = new GameObject ("Aerodynamics");
				wing.transform.parent = parent.transform;
			}
			EditorSceneManager.MarkSceneDirty (wing.gameObject.scene);
			SilantroAerofoil wingAerofoil = wing.AddComponent<SilantroAerofoil> ();wingAerofoil.AerofoilSubdivisions = 5;wingAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Wing;
			SilantroAirfoil wng = (SilantroAirfoil)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Airfoils/Legacy/Wing 23016.prefab",typeof(SilantroAirfoil));
			wingAerofoil.airfoil = wng;wingAerofoil.isDestructible = false;

		}
		//
		[MenuItem("Oyedoyin/Aerofoil System/Structures/Controllable/Rudder",false,3)]
		private static void AddRudder()
		{
			GameObject wing;
			if (Selection.activeGameObject != null )
			{
				wing = new GameObject ("Default Rudder");
				wing.transform.parent = Selection.activeGameObject.transform;wing.transform.localPosition = new Vector3 (0, 0, 0);
			} else {
				wing = new GameObject ("Default Rudder");GameObject parent = new GameObject ("Aerodynamics");wing.transform.parent = parent.transform;
			}
			//
			EditorSceneManager.MarkSceneDirty (wing.gameObject.scene);
			SilantroAerofoil wingAerofoil = wing.AddComponent<SilantroAerofoil> ();wingAerofoil.AerofoilSubdivisions = 4;wingAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Stabilizer;
			GameObject start = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Airfoils/Legacy/Control 0009.prefab",typeof(GameObject));
			wingAerofoil.airfoil = start.GetComponent<SilantroAirfoil> ();wingAerofoil.isDestructible = false;wingAerofoil.activeControlSurface = true;wingAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Rudder;
		}
		//
		//
		[MenuItem("Oyedoyin/Aerofoil System/Structures/Controllable/Stabilizer",false,2)]
		private static void AddTail()
		{
			GameObject wing;
			if (Selection.activeGameObject != null )
			{
				wing = new GameObject ("Default Right Stabilizer");
				wing.transform.parent = Selection.activeGameObject.transform;
				wing.transform.localPosition = new Vector3 (0, 0, 0);

			} else {
				wing = new GameObject ("Default Right Stabilizer");
				GameObject parent = new GameObject ("Aerodynamics");
				wing.transform.parent = parent.transform;
			}
			EditorSceneManager.MarkSceneDirty (wing.gameObject.scene);
			//
			SilantroAerofoil wingAerofoil = wing.AddComponent<SilantroAerofoil> ();wingAerofoil.AerofoilSubdivisions = 4;wingAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Stabilizer;
			SilantroAirfoil start = (SilantroAirfoil)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Airfoils/Legacy/Control 0009.prefab",typeof(SilantroAirfoil));
			wingAerofoil.airfoil = start;wingAerofoil.isDestructible = false;}
		//
		//
		//
		[MenuItem("Oyedoyin/Aerofoil System/Structures/Stationary/Stabilizer",false,4)]
		private static void AddStationaryTail()
		{
			GameObject wing;
			if (Selection.activeGameObject != null )
			{
				wing = new GameObject ("Default Right Stabilizer");
				wing.transform.parent = Selection.activeGameObject.transform;
				wing.transform.localPosition = new Vector3 (0, 0, 0);
				EditorSceneManager.MarkSceneDirty (Selection.activeGameObject .gameObject.scene);
			} else {
				wing = new GameObject ("Default Right Stabilizer");
				GameObject parent = new GameObject ("Aerodynamics");
				wing.transform.parent = parent.transform;
			}
			//
			SilantroAerofoil wingAerofoil = wing.AddComponent<SilantroAerofoil> ();wingAerofoil.AerofoilSubdivisions = 4;wingAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Stabilizer;
			SilantroAirfoil start = (SilantroAirfoil)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Airfoils/Legacy/Control 0009.prefab",typeof(SilantroAirfoil));
			wingAerofoil.airfoil = start;wingAerofoil.controlState = SilantroAerofoil.ControType.Stationary;wingAerofoil.isDestructible = false;
		}
		//
		//
		[MenuItem("Oyedoyin/Aerofoil System/Structures/Stationary/Wing")]
		private static void AddStationaryWing()
		{
			GameObject wing;
			if (Selection.activeGameObject != null )
			{
				wing = new GameObject ("Default Right Wing");
				wing.transform.parent = Selection.activeGameObject.transform;
				wing.transform.localPosition = new Vector3 (0, 0, 0);
				EditorSceneManager.MarkSceneDirty (Selection.activeGameObject .gameObject.scene);
			} else {
				wing = new GameObject ("Default Right Wing");
				GameObject parent = new GameObject ("Aerodynamics");
				wing.transform.parent = parent.transform;
			}
			//
			SilantroAerofoil wingAerofoil = wing.AddComponent<SilantroAerofoil> ();wingAerofoil.AerofoilSubdivisions = 4;wingAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Wing;
			SilantroAirfoil start = (SilantroAirfoil)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Airfoils/Legacy/Wing 23016.prefab",typeof(SilantroAirfoil));
			wingAerofoil.airfoil = start;wingAerofoil.controlState = SilantroAerofoil.ControType.Stationary;wingAerofoil.isDestructible = false;
		}
		//
		//
		[MenuItem("Oyedoyin/Aerofoil System/Structures/Left Structure/Bound",false,9)]
		private static void AddLeftWingMoving()
		{
			//
			//
			GameObject wingInstance;GameObject wing;
			if (Selection.activeGameObject != null)
			{
				wing = Selection.activeGameObject;
				EditorSceneManager.MarkSceneDirty (Selection.activeGameObject .gameObject.scene);
			} else {
				return;
			}
			SilantroAerofoil wingAerofoil = wing.GetComponent<SilantroAerofoil> ();
			if (wingAerofoil != null && wingAerofoil.gameObject.GetComponent<SilantroQuantum> () == null) {
				wingInstance = new GameObject ();
				wingInstance.transform.parent = Selection.activeGameObject.transform.parent;
				wingInstance.name = "Default Left " + wingAerofoil.aerofoilType.ToString ();
				SilantroAerofoil newwingAerofoil = wingInstance.AddComponent<SilantroAerofoil> ();
				newwingAerofoil.AerofoilSubdivisions = wingAerofoil.AerofoilSubdivisions;
				newwingAerofoil.aerofoilType = wingAerofoil.aerofoilType;newwingAerofoil.airfoil = wingAerofoil.airfoil;
				//
				newwingAerofoil.isDestructible = wingAerofoil.isDestructible;
				newwingAerofoil.controlState = wingAerofoil.controlState;
				newwingAerofoil.surfaceType = wingAerofoil.surfaceType;
				//
				SilantroQuantum mirror = wingInstance.AddComponent<SilantroQuantum> ();
				mirror.Instance = wing.transform;

			} 
			else if (wingAerofoil != null && wingAerofoil.gameObject.GetComponent<SilantroQuantum> () == null) {
				Debug.Log ("Selected GameObject has an attached Quantum Transformer");
			}
			else if (wingAerofoil == null) {
				Debug.Log ("Selected GameObject is not an Aerofoil! Create an Aerofoil and try again");
			} 

		}
		//
		//
		//
		[MenuItem("Oyedoyin/Aerofoil System/Structures/Left Structure/UnBound")]
		private static void AddLeftWing()
		{
			//
			//
			GameObject wingInstance;GameObject wing;EditorSceneManager.MarkSceneDirty (Selection.activeGameObject .gameObject.scene);
			if (Selection.activeGameObject != null)
			{
				wing = Selection.activeGameObject;
			} else {
				return;
			}
			SilantroAerofoil wingAerofoil = wing.GetComponent<SilantroAerofoil> ();
			if (wingAerofoil != null && wingAerofoil.gameObject.GetComponent<SilantroQuantum> () == null) {
				wingInstance = new GameObject ();
				wingInstance.transform.parent = Selection.activeGameObject.transform.parent;
				wingInstance.name = "Default Left " + wingAerofoil.aerofoilType.ToString ();	float x = wingAerofoil.transform.localPosition.x;
				float y = wingAerofoil.transform.localPosition.y;
				float z = wingAerofoil.transform.localPosition.z;
				wingInstance.transform.localPosition = new Vector3 (x-2, y, z);
				SilantroAerofoil newwingAerofoil = wingInstance.AddComponent<SilantroAerofoil> ();
				newwingAerofoil.AerofoilSubdivisions = wingAerofoil.AerofoilSubdivisions;
				newwingAerofoil.aerofoilType = wingAerofoil.aerofoilType;newwingAerofoil.airfoil = wingAerofoil.airfoil;
				//
				newwingAerofoil.isDestructible = wingAerofoil.isDestructible;
				newwingAerofoil.controlState = wingAerofoil.controlState;
				newwingAerofoil.surfaceType = wingAerofoil.surfaceType;
				//
				SilantroQuantum mirror = wingInstance.AddComponent<SilantroQuantum> ();
				mirror.Instance = wing.transform;
				//
				newwingAerofoil.transform.localScale = new Vector3 (-1, 1, 1);

			} 
			else if (wingAerofoil != null && wingAerofoil.gameObject.GetComponent<SilantroQuantum> () == null) {
				Debug.Log ("Selected GameObject has an attached Quantum Transformer");
			}
			else if (wingAerofoil == null) {
				Debug.Log ("Selected GameObject is not an Aerofoil! Create an Aerofoil and try again");
			} 

		}
		//
		//
		//
		[MenuItem("Oyedoyin/Aerofoil System/Controls/Combined/Ruddervator")]
		private static void AddRuddervator()
		{
			GameObject wing;
			if (Selection.activeGameObject != null)
			{
				wing = Selection.activeGameObject;
				EditorSceneManager.MarkSceneDirty (Selection.activeGameObject .gameObject.scene);
			} else {
				return;
			}
			SilantroAerofoil wingAerofoil = wing.GetComponent<SilantroAerofoil> ();

			if (wingAerofoil != null && wingAerofoil.aerofoilType == SilantroAerofoil.AerofoilType.Stabilizer) {
				//
				wingAerofoil.controlState = SilantroAerofoil.ControType.Controllable;
				wingAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Ruddervator;
				//
			} else if (wingAerofoil == null) {
				Debug.Log ("Selected GameObject is not an Aerofoil! Create an Aerofoil and try again");
			} else if (wingAerofoil.aerofoilType == SilantroAerofoil.AerofoilType.Stabilizer) {
				Debug.Log ("Ruddervator can only be used to control the Stabilzer!!!");
			}
		}
		//
		//[MenuItem("Oyedoyin/Aerofoil System/Controls/Combined/Flaperon")]
		//private static void AddFlaperon()
		//{
			//
		//	Debug.Log("Still in Development!!");
		//}
		//
		//
		//[MenuItem("Oyedoyin/Aerofoil System/Controls/Combined/Spoileron")]
		//private static void AddSpoileron()
		//{
			//
		//	Debug.Log("Still in Development!!");
		//}
		//
		[MenuItem("Oyedoyin/Aerofoil System/Controls/Combined/Elevon",false,1)]
		private static void AddElevon()
		{
			GameObject wing;
			if (Selection.activeGameObject != null)
			{
				EditorSceneManager.MarkSceneDirty (Selection.activeGameObject .gameObject.scene);
				wing = Selection.activeGameObject;
			} else {
				return;
			}
			SilantroAerofoil wingAerofoil = wing.GetComponent<SilantroAerofoil> ();

			if (wingAerofoil != null && wingAerofoil.aerofoilType == SilantroAerofoil.AerofoilType.Stabilizer) {
				//
				wingAerofoil.controlState = SilantroAerofoil.ControType.Controllable;
				wingAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Elevon;
				//
			} else if (wingAerofoil == null) {
				Debug.Log ("Selected GameObject is not an Aerofoil! Create an Aerofoil and try again");
			} else if (wingAerofoil.aerofoilType == SilantroAerofoil.AerofoilType.Stabilizer) {
				Debug.Log ("Elevon can only be used to control the Stabilizer!!!");
			}
		}
		//SETUP CONTROLS
		//
		[MenuItem("Oyedoyin/Aerofoil System/Controls/Primary/Aileron",false,2)]
		private static void AddAileron()
		{
			GameObject wing;
			if (Selection.activeGameObject != null)
			{
				EditorSceneManager.MarkSceneDirty (Selection.activeGameObject .gameObject.scene);
				wing = Selection.activeGameObject;
			} else {
				return;
			}
			SilantroAerofoil wingAerofoil = wing.GetComponent<SilantroAerofoil> ();

			if (wingAerofoil != null && wingAerofoil.aerofoilType == SilantroAerofoil.AerofoilType.Wing) {
				//
				wingAerofoil.controlState = SilantroAerofoil.ControType.Controllable;
				wingAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Aileron;
				//
			} else if (wingAerofoil == null) {
				Debug.Log ("Selected GameObject is not an Aerofoil! Create an Aerofoil and try again");
			} else if (wingAerofoil.aerofoilType == SilantroAerofoil.AerofoilType.Stabilizer) {
				Debug.Log ("Aileron can only be used to control the Wing!!!, Add Elevator or Rudder to the Tail");
			}
		}
		//
		[MenuItem("Oyedoyin/Aerofoil System/Controls/Secondary/Flaps")]
		private static void AddFlaps()
		{
			GameObject wing;
			if (Selection.activeGameObject != null)
			{
				wing = Selection.activeGameObject;
				EditorSceneManager.MarkSceneDirty (Selection.activeGameObject .gameObject.scene);
			} else {
				return;
			}
			SilantroAerofoil wingAerofoil = wing.GetComponent<SilantroAerofoil> ();

			if (wingAerofoil != null && wingAerofoil.aerofoilType == SilantroAerofoil.AerofoilType.Wing) {
				//
				wingAerofoil.controlState = SilantroAerofoil.ControType.Controllable;
				wingAerofoil.usesFlap = true;
				//
			} else if (wingAerofoil == null) {
				Debug.Log ("Selected GameObject is not an Aerofoil! Create an Aerofoil and try again");
			} else if (wingAerofoil.aerofoilType == SilantroAerofoil.AerofoilType.Stabilizer) {
				Debug.Log ("Flap can only be used to control the Wing!!!");
			}
		}
		//
		[MenuItem("Oyedoyin/Aerofoil System/Controls/Secondary/Slats")]
		private static void AddSlats()
		{
			GameObject wing;
			if (Selection.activeGameObject != null)
			{
				EditorSceneManager.MarkSceneDirty (Selection.activeGameObject .gameObject.scene);
				wing = Selection.activeGameObject;
			} else {
				return;
			}
			SilantroAerofoil wingAerofoil = wing.GetComponent<SilantroAerofoil> ();

			if (wingAerofoil != null && wingAerofoil.aerofoilType == SilantroAerofoil.AerofoilType.Wing) {
				//
				wingAerofoil.controlState = SilantroAerofoil.ControType.Controllable;
				wingAerofoil.usesSlats = true;
				//
			} else if (wingAerofoil == null) {
				Debug.Log ("Selected GameObject is not an Aerofoil! Create an Aerofoil and try again");
			} else if (wingAerofoil.aerofoilType == SilantroAerofoil.AerofoilType.Stabilizer) {
				Debug.Log ("Slats can only be used to control the Wing!!!");
			}
		}
		//
		//
		[MenuItem("Oyedoyin/Aerofoil System/Controls/Secondary/Spoilers")]
		private static void AddSpoilers()
		{
			GameObject wing;
			if (Selection.activeGameObject != null)
			{
				EditorSceneManager.MarkSceneDirty (Selection.activeGameObject .gameObject.scene);
				wing = Selection.activeGameObject;
			} else {
				return;
			}
			SilantroAerofoil wingAerofoil = wing.GetComponent<SilantroAerofoil> ();

			if (wingAerofoil != null && wingAerofoil.aerofoilType == SilantroAerofoil.AerofoilType.Wing) {
				//
				wingAerofoil.controlState = SilantroAerofoil.ControType.Controllable;
				wingAerofoil.usesSpoilers = true;
				//
			} else if (wingAerofoil == null) {
				Debug.Log ("Selected GameObject is not an Aerofoil! Create an Aerofoil and try again");
			} else if (wingAerofoil.aerofoilType == SilantroAerofoil.AerofoilType.Stabilizer) {
				Debug.Log ("Spoilers can only be used to control the Wing!!!");
			}
		}
		[MenuItem("Oyedoyin/Aerofoil System/Controls/Primary/Elevator")]
		private static void AddElevator()
		{
			GameObject wing;
			if (Selection.activeGameObject != null)
			{
				EditorSceneManager.MarkSceneDirty (Selection.activeGameObject .gameObject.scene);
				wing = Selection.activeGameObject;
			} else {
				return;
			}
			SilantroAerofoil wingAerofoil = wing.GetComponent<SilantroAerofoil> ();
			if (wingAerofoil != null&& wingAerofoil.aerofoilType == SilantroAerofoil.AerofoilType.Stabilizer) {
				//
				wingAerofoil.controlState = SilantroAerofoil.ControType.Controllable;
				wingAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Elevator;
				//
			} else if (wingAerofoil == null) {
				Debug.Log ("Selected GameObject is not an Aerofoil! Create an Aerofoil and try again");
			}
			else if (wingAerofoil.aerofoilType == SilantroAerofoil.AerofoilType.Wing) {
				Debug.Log ("Elevator can only be used to control the Stabilizer!!!, Add Aileron or Flap to the Wing");
			}
		}
		//
		[MenuItem("Oyedoyin/Aerofoil System/Controls/Primary/Rudder")]
		private static void AddRudderControl()
		{
			GameObject wing;
			if (Selection.activeGameObject != null)
			{
				wing = Selection.activeGameObject;
				EditorSceneManager.MarkSceneDirty (Selection.activeGameObject .gameObject.scene);
			} else {
				return;
			}
			SilantroAerofoil wingAerofoil = wing.GetComponent<SilantroAerofoil> ();
			if (wingAerofoil != null&& wingAerofoil.aerofoilType == SilantroAerofoil.AerofoilType.Stabilizer) {
				//
				wingAerofoil.controlState = SilantroAerofoil.ControType.Controllable;
				wingAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Rudder;
				//
			} else if (wingAerofoil == null) {
				Debug.Log ("Selected GameObject is not an Aerofoil! Create an Aerofoil and try again");
			}
			else if (wingAerofoil.aerofoilType == SilantroAerofoil.AerofoilType.Wing) {
				Debug.Log ("Rudder can only be used to control the Stabilizer!!!, Add Aileron or Flap to the Wing");
			}
		}
		//
		//
		[MenuItem("Oyedoyin/Propulsion System/Blade System/Propeller")]
		private static void AddPropellerControl()
		{
			if (Selection.activeGameObject.GetComponent<SilantroTurboProp> () ) {
				GameObject Props = new GameObject ("Default Propeller");
				//
				Props.transform.parent = Selection.activeGameObject.transform;
				SilantroBlade blade = Props.AddComponent<SilantroBlade> ();
				blade.bladeType = SilantroBlade.BladeType.Propeller;
				blade.engineType = SilantroBlade.EngineType.TurbopropEngine;
				blade.propEngine = Selection.activeGameObject.GetComponent<SilantroTurboProp> () ;
				GameObject bladefoil = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Airfoils/Blades/BLD 08F5.prefab", typeof(GameObject));
				blade.bladefoil = bladefoil.GetComponent<SilantroBladefoil> ();
				//
			} 
			if (Selection.activeGameObject.GetComponent<SilantroPistonEngine> () ) {
				GameObject Props = new GameObject ("Default Propeller");
				//
				Props.transform.parent = Selection.activeGameObject.transform;
				SilantroBlade blade = Props.AddComponent<SilantroBlade> ();
				blade.bladeType = SilantroBlade.BladeType.Rotor;
				blade.engineType = SilantroBlade.EngineType.PistonEngine;
				blade.pistonEngine = Selection.activeGameObject.GetComponent<SilantroPistonEngine> () ;
				GameObject bladefoil = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Airfoils/Blades/BLD 08F5.prefab", typeof(GameObject));
				blade.bladefoil = bladefoil.GetComponent<SilantroBladefoil> ();
				//
			} 
			{
				Debug.Log ("Please select a Suitable Engine to add Propeller to..");
			}
		}
		//
		[MenuItem("Oyedoyin/Propulsion System/Blade System/Rotor",false,50)]
		private static void AddRotorControl()
		{
			if (Selection.activeGameObject.GetComponent<SilantroTurboShaft> () ) {
				GameObject Props = new GameObject ("Default Rotor");
				//
				Props.transform.parent = Selection.activeGameObject.transform;
				SilantroBlade blade = Props.AddComponent<SilantroBlade> ();
				blade.bladeType = SilantroBlade.BladeType.Rotor;
				blade.engineType = SilantroBlade.EngineType.TurboshaftEngine;
				blade.shaftEngine = Selection.activeGameObject.GetComponent<SilantroTurboShaft> () ;
				GameObject bladefoil = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Airfoils/Blades/BLD 08F5.prefab", typeof(GameObject));
				blade.bladefoil = bladefoil.GetComponent<SilantroBladefoil> ();
				//
			} 
			if (Selection.activeGameObject.GetComponent<SilantroPistonEngine> () ) {
				GameObject Props = new GameObject ("Default Rotor");
				//
				Props.transform.parent = Selection.activeGameObject.transform;
				SilantroBlade blade = Props.AddComponent<SilantroBlade> ();
				blade.bladeType = SilantroBlade.BladeType.Rotor;
				blade.engineType = SilantroBlade.EngineType.PistonEngine;
				blade.pistonEngine = Selection.activeGameObject.GetComponent<SilantroPistonEngine> () ;
				GameObject bladefoil = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Airfoils/Blades/BLD 08F5.prefab", typeof(GameObject));
				blade.bladefoil = bladefoil.GetComponent<SilantroBladefoil> ();
				//
			} 
			else {
				Debug.Log ("Please select a Suitable to add Rotor to..");
			}
		}
		//
		[MenuItem("Oyedoyin/Propulsion System/Fuel System/Fuel Tanks/Internal",false,100)]
		private static void AddInternalTank()
		{
			GameObject tank;
			if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<SilantroFuelDistributor>())
			{
				tank = new GameObject ();
				tank.transform.parent = Selection.activeGameObject.transform;tank.transform.localPosition = new Vector3 (0, 0, 0);
			} else {
				tank = new GameObject ();
			}
			EditorSceneManager.MarkSceneDirty (tank.gameObject.scene);
			tank.name = "Internal Fuel Tank";
			SilantroFuelTank fuelTank = tank.AddComponent<SilantroFuelTank> ();fuelTank.Capacity = 1000f;fuelTank.tankType = SilantroFuelTank.TankType.Internal;
			GameObject smoke = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Effects/Explosion/Component Explosion.prefab",typeof(GameObject));
			fuelTank.ExplosionPrefab = smoke;
			EditorSceneManager.MarkSceneDirty (tank.gameObject.scene);
		}
		//
		[MenuItem("Oyedoyin/Propulsion System/Fuel System/Fuel Tanks/External")]
		private static void AddExternalTank()
		{
			GameObject tank;
			if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<SilantroFuelDistributor>())
			{
				tank = new GameObject ();
				tank.transform.parent = Selection.activeGameObject.transform;tank.transform.localPosition = new Vector3 (0, 0, 0);
			} else {
				tank = new GameObject ();
			}
			tank.name = "External Fuel Tank";
			EditorSceneManager.MarkSceneDirty (tank.gameObject.scene);
			SilantroFuelTank fuelTank = tank.AddComponent<SilantroFuelTank> ();fuelTank.Capacity = 400f;fuelTank.tankType = SilantroFuelTank.TankType.External;
			GameObject smoke = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Effects/Explosion/Component Explosion.prefab",typeof(GameObject));
			fuelTank.ExplosionPrefab = smoke;
			EditorSceneManager.MarkSceneDirty (tank.gameObject.scene);
		}
		//

		//SETUP DOOR HYDRAULICS
		//
		[MenuItem("Oyedoyin/Hydraulic System/Door System",false,21)]
		private static void AddDoorSystem()
		{
			GameObject door;door = new GameObject ("Door Hydraulics");
			//
			SilantroHydraulicSystem doorSystem = door.AddComponent<SilantroHydraulicSystem> ();
			AudioClip open = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Door/Door Open.wav",typeof(AudioClip));
			AudioClip close = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Door/Door Close.wav",typeof(AudioClip));
			doorSystem.openSound = open;doorSystem.closeSound = close;
			EditorSceneManager.MarkSceneDirty (door.gameObject.scene);
		}
		//
		//
		//SETUP WHEEL SYSTEM
		//
		[MenuItem("Oyedoyin/Hydraulic System/Gear System/Combined Hydraulics")]
		private static void AddCombinedGearSystem()
		{
			GameObject wheelSystem;
			wheelSystem = new GameObject ("Gear System");

			SilantroGearSystem gearSystem = wheelSystem.AddComponent<SilantroGearSystem> ();
			//SETUP WHEELS
			GameObject frontWheel = new GameObject("Front Wheel");frontWheel.transform.parent = wheelSystem.transform;frontWheel.transform.localPosition = new Vector3(0,-0.5f,0);frontWheel.AddComponent<WheelCollider>();frontWheel.GetComponent<WheelCollider>().radius = 0.2f;
			GameObject leftWheel = new GameObject ("Left Wheel");leftWheel.transform.parent = wheelSystem.transform;leftWheel.transform.localPosition = new Vector3 (-1, -0.5f, -2);leftWheel.AddComponent<WheelCollider>();leftWheel.GetComponent<WheelCollider>().radius = 0.2f;
			GameObject rightWheel = new GameObject ("Right Wheel");rightWheel.transform.parent = wheelSystem.transform;rightWheel.transform.localPosition = new Vector3 (1, -0.5f, -2);rightWheel.AddComponent<WheelCollider>();rightWheel.GetComponent<WheelCollider>().radius = 0.2f;
			//
			GameObject wheelBucket = new GameObject("Wheels");
			wheelBucket.transform.parent = gearSystem.transform;
			frontWheel.transform.parent = wheelBucket.transform;rightWheel.transform.parent = wheelBucket.transform;leftWheel.transform.parent = wheelBucket.transform;
			//
			SilantroGearSystem.WheelSystem frontGearSystem = new SilantroGearSystem.WheelSystem ();frontGearSystem.collider = frontWheel.GetComponent<WheelCollider>();frontGearSystem.Identifier = "Front Gear";frontGearSystem.steerable = true;
			SilantroGearSystem.WheelSystem leftGearSystem = new SilantroGearSystem.WheelSystem ();leftGearSystem.collider = leftWheel.GetComponent<WheelCollider>();leftGearSystem.Identifier = "Left Gear";leftGearSystem.attachedMotor = true;
			SilantroGearSystem.WheelSystem rightGearSystem = new SilantroGearSystem.WheelSystem ();rightGearSystem.collider = rightWheel.GetComponent<WheelCollider>();rightGearSystem.Identifier = "Right Gear";rightGearSystem.attachedMotor = true;
			//
			gearSystem.wheelSystem.Add(frontGearSystem);gearSystem.wheelSystem.Add(leftGearSystem);gearSystem.wheelSystem.Add(rightGearSystem);
			AudioClip open = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Gear/Gear Open.wav",typeof(AudioClip));
			AudioClip close = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Gear/Gear Close.wav",typeof(AudioClip));
			//
			//SETUP GEAR HYDRAULICS
			GameObject gearHydraulics = new GameObject ("Gear Hydraulics");gearHydraulics.transform.parent = wheelSystem.transform;
			SilantroHydraulicSystem hydraulics = gearHydraulics.AddComponent<SilantroHydraulicSystem> ();
			gearSystem.gearHydraulics = hydraulics;
			gearSystem.gearType = SilantroGearSystem.GearType.Combined;
			hydraulics.closeSound = close;hydraulics.openSound = open;
			EditorSceneManager.MarkSceneDirty (wheelSystem.gameObject.scene);
		}
		//
		//
		//
		[MenuItem("Oyedoyin/Hydraulic System/Gear System/Separate Hydraulics")]
		private static void AddSeparateGearSystem()
		{
			GameObject wheelSystem;
			wheelSystem = new GameObject ("Gear System");

			SilantroGearSystem gearSystem = wheelSystem.AddComponent<SilantroGearSystem> ();
			EditorSceneManager.MarkSceneDirty (wheelSystem.gameObject.scene);
			//SETUP WHEELS
			GameObject frontWheel = new GameObject("Front Wheel");frontWheel.transform.parent = wheelSystem.transform;frontWheel.transform.localPosition = new Vector3(0,-0.5f,0);frontWheel.AddComponent<WheelCollider>();frontWheel.GetComponent<WheelCollider>().radius = 0.2f;
			GameObject leftWheel = new GameObject ("Left Wheel");leftWheel.transform.parent = wheelSystem.transform;leftWheel.transform.localPosition = new Vector3 (-1, -0.5f, -2);leftWheel.AddComponent<WheelCollider>();leftWheel.GetComponent<WheelCollider>().radius = 0.2f;
			GameObject rightWheel = new GameObject ("Right Wheel");rightWheel.transform.parent = wheelSystem.transform;rightWheel.transform.localPosition = new Vector3 (1, -0.5f, -2);rightWheel.AddComponent<WheelCollider>();rightWheel.GetComponent<WheelCollider>().radius = 0.2f;
			//
			GameObject wheelBucket = new GameObject("Wheels");
			wheelBucket.transform.parent = gearSystem.transform;
			frontWheel.transform.parent = wheelBucket.transform;rightWheel.transform.parent = wheelBucket.transform;leftWheel.transform.parent = wheelBucket.transform;
			//
			SilantroGearSystem.WheelSystem frontGearSystem = new SilantroGearSystem.WheelSystem ();frontGearSystem.collider = frontWheel.GetComponent<WheelCollider>();frontGearSystem.Identifier = "Front Gear";frontGearSystem.steerable = true;
			SilantroGearSystem.WheelSystem leftGearSystem = new SilantroGearSystem.WheelSystem ();leftGearSystem.collider = leftWheel.GetComponent<WheelCollider>();leftGearSystem.Identifier = "Left Gear";leftGearSystem.attachedMotor = true;
			SilantroGearSystem.WheelSystem rightGearSystem = new SilantroGearSystem.WheelSystem ();rightGearSystem.collider = rightWheel.GetComponent<WheelCollider>();rightGearSystem.Identifier = "Right Gear";rightGearSystem.attachedMotor = true;
			//
			gearSystem.wheelSystem.Add(frontGearSystem);gearSystem.wheelSystem.Add(leftGearSystem);gearSystem.wheelSystem.Add(rightGearSystem);
			AudioClip open = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Gear/Gear Open.wav",typeof(AudioClip));
			AudioClip close = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Gear/Gear Close.wav",typeof(AudioClip));
			//
			//SETUP GEAR HYDRAULICS
			GameObject gearHydraulics = new GameObject ("Gear Hydraulics");gearHydraulics.transform.parent = wheelSystem.transform;
			GameObject doorHydraulics = new GameObject ("Door Hydraulics");doorHydraulics.transform.parent = wheelSystem.transform;
			SilantroHydraulicSystem hydraulics = gearHydraulics.AddComponent<SilantroHydraulicSystem> ();
			SilantroHydraulicSystem doorhydraulics = doorHydraulics.AddComponent<SilantroHydraulicSystem> ();
			gearSystem.gearHydraulics = hydraulics;gearSystem.doorHydraulics = doorhydraulics;hydraulics.playSound = true;doorhydraulics.playSound = false;
			gearSystem.gearType = SilantroGearSystem.GearType.Seperate;
			hydraulics.closeSound = close;hydraulics.openSound = open;
			EditorSceneManager.MarkSceneDirty (wheelSystem.gameObject.scene);
		}
		//
		[MenuItem("Oyedoyin/Camera System/Orbit Camera",false,200)]
		private static void Camera()
		{
			GameObject box;
			if (Selection.activeGameObject != null)
			{
				box = new GameObject ();
				box.transform.parent = Selection.activeGameObject.transform;
			} else {
				box = new GameObject ();
			}
			box.name = "Camera System";box.transform.localPosition = new Vector3 (0, 0, 0);
			GameObject focalPoint = new GameObject ("Camera Focus Point");focalPoint.transform.parent = box.transform;
			GameObject normalCam = new GameObject("Default Camera");
			normalCam.gameObject.transform.parent = box.transform;Camera comp = normalCam.AddComponent<Camera> ();
			comp.farClipPlane = 10000f;
			normalCam.gameObject.AddComponent<SilantroCamera> ();normalCam.GetComponent<SilantroCamera> ().FocusPoint = focalPoint;
			EditorSceneManager.MarkSceneDirty (box.gameObject.scene);
		}
		//
		//
		[MenuItem("Oyedoyin/Components/Radar/Civilian",false,411)]
		private static void CivilianRadar()
		{
			GameObject radome;
			if (Selection.activeGameObject != null && Selection.activeGameObject.name == "Avionics")
			{
				radome = new GameObject ("Radar");
				radome.transform.parent = Selection.activeGameObject.transform;
				//
				//
				SilantroRadar radar = radome.AddComponent<SilantroRadar>();
				SilantroController controller = radome.GetComponentInParent<SilantroController> ();
				if (controller != null) {
					radar.Aircraft = controller.gameObject;
				}
				radar.radarType = SilantroRadar.RadarType.Civilian;
				//Load Textures
				Texture background = (Texture)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Textures/Radar/Radar Background 1.png",typeof(Texture));
				Texture compass = (Texture)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Textures/Radar/middle Compass.png",typeof(Texture));
				//
				radar.background = background;radar.compass = compass;radar.active = true;radar.scale = 1;
				EditorSceneManager.MarkSceneDirty (radome.gameObject.scene);

			} else {
				Debug.Log ("Please Select 'Avionics' GameObject within the Aircraft, or create one if its's missing");
			}

		}
		//
		//
		[MenuItem("Oyedoyin/Components/Radar/Military")]
		private static void MilitaryRadar()
		{
			//
			Debug.Log("Coming Soon...");
		}
		//
		[MenuItem("Oyedoyin/Components/Black Box",false,511)]
		private static void AddBlackBox()
		{
			if (Selection.activeGameObject != null && Selection.activeGameObject.name == "Avionics") {
				GameObject box;
				box = new GameObject ("Black Box");
				box.transform.parent = Selection.activeGameObject.transform;
				SilantroDataLogger blackBox = box.AddComponent<SilantroDataLogger> ();
				//
				SilantroController controller = box.GetComponentInParent<SilantroController> ();
				if (controller != null) {
					blackBox.Aircraft = controller;
					blackBox.savefileName = controller.gameObject.name + " data";
					blackBox.logRate = 5f;
				}
				EditorSceneManager.MarkSceneDirty (blackBox.gameObject.scene);
			}
			else {
				Debug.Log ("Please Select 'Avionics' GameObject within the Aircraft, or create one if its's missing");
			}
		}
		//

		[MenuItem("Oyedoyin/Components/Gravity Center")]
		private static void COG()
		{
			GameObject box;
			if (Selection.activeGameObject != null)
			{
				box = new GameObject ();
				box.transform.parent = Selection.activeGameObject.transform;
			} else {
				box = new GameObject ();
			}
			box.name = "COG";box.AddComponent<SilantroGravityCenter> ();box.tag = "Brain";box.AddComponent<SilantroInstrumentation> ();
			box.transform.localPosition = new Vector3 (0, 0, 0);
			Rigidbody boxParent = box.transform.root.gameObject.GetComponent<Rigidbody> ();
			if (boxParent != null) {
				box.GetComponent<SilantroInstrumentation> ().airplane = boxParent;
			} else {
				Debug.Log ("Please parent Center of Gravity to Rigidbody Airplane");
			}
			AudioClip boom = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Extras/Sonic Boom.wav",typeof(AudioClip));
			GameObject flash = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Componets/Sonic Boom.prefab",typeof(GameObject));
			box.GetComponent<SilantroInstrumentation> ().sonicBoom = boom;box.GetComponent<SilantroInstrumentation> ().condenationEffect = flash.GetComponent<ParticleSystem>();
			EditorSceneManager.MarkSceneDirty (box.gameObject.scene);
		}
		//
		[MenuItem("Oyedoyin/Components/Controller/Conventional")]
		private static void AddSimpleController()
		{
			GameObject plane;
			if (Selection.activeGameObject != null) {
				plane = Selection.activeGameObject;
				if (plane.GetComponentInChildren<SilantroFuelDistributor> () && plane.GetComponentInChildren<SilantroGearSystem> ()) {
					SilantroController control = plane.AddComponent<SilantroController> ();
					control.gearHelper = plane.GetComponentInChildren<SilantroGearSystem> ();
					control.fuelsystem = plane.GetComponentInChildren<SilantroFuelDistributor> ();
					control.gearHelper.control = control;
					EditorSceneManager.MarkSceneDirty (Selection.activeGameObject .gameObject.scene);
				} else {
					Debug.Log ("Controller can't be added to this Object!!>..Vital Components are missing");
				}
			} else {
				Debug.Log ("Please Select an Aircraft to add Controller to!!");
			}
			//
		}
		//
		[MenuItem("Oyedoyin/Components/Controller/Complex/STOVL System",false,151)]
		private static void AddComplexController()
		{
			GameObject plane;
			if (Selection.activeGameObject != null) {
				plane = Selection.activeGameObject;
				if (plane.GetComponentInChildren<SilantroFuelDistributor> () && plane.GetComponentInChildren<SilantroGearSystem> ()) {
					SilantroController control = plane.AddComponent<SilantroController> ();
					control.gearHelper = plane.GetComponentInChildren<SilantroGearSystem> ();
					control.fuelsystem = plane.GetComponentInChildren<SilantroFuelDistributor> ();
					control.gearHelper.control = control;
					EditorSceneManager.MarkSceneDirty (Selection.activeGameObject .gameObject.scene);
				} else {
					Debug.Log ("Controller can't be added to this Object!!>..Vital Components are missing");
				}
			} else {
				Debug.Log ("Please Select an Aircraft to add Controller to!!");
			}
			//	//

		}
		//
		[MenuItem("GameObject/Oyedoyin/Weapon/Gatling Minigun",false,0)]
		static void CreateNewMinigun(MenuCommand command)
		{
			GameObject gun; 
			gun = new GameObject ();
			gun.name = "Default Gatling Gun";
			//
			EditorSceneManager.MarkSceneDirty (gun.gameObject.scene);
			GameObject shootSpot = new GameObject ("Shoot Spot");GameObject shellPoint = new GameObject ("Shell Ejection Point");shootSpot.transform.parent = gun.transform;shellPoint.transform.parent = gun.transform;
			SilantroMinigun minigun = gun.AddComponent<SilantroMinigun> ();minigun.shellEjectPoint = shellPoint.transform;//minigun.muzzles(shootSpot.transform);
			AudioClip shoot = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Weapons/Minigun/minigun_Fire.wav",typeof(AudioClip));
			minigun.fireSound = shoot;
			//
			minigun.muzzles = new Transform[1];
			GameObject muzzle = new GameObject ("Muzzle"); minigun.muzzles [0] = muzzle.transform;muzzle.transform.parent = gun.transform;
			GameObject flash = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Weapons/Effects/Flash/Default Muzzle Flash.prefab",typeof(GameObject));
			GameObject impact = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Weapons/Effects/Impacts/Ground Impact.prefab",typeof(GameObject));
			GameObject bcase = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Weapons/Cases/7.62x36mm.prefab",typeof(GameObject));
			//
			minigun.muzzleFlash = flash;minigun.groundHit = impact;minigun.metalHit = impact;minigun.woodHit = impact;minigun.bulletCase = bcase;

		}
		//
		[MenuItem("GameObject/Oyedoyin/Weapon/Bomb/Unguided",false,0)]
		static void CreateNewUnguidedBomb(MenuCommand command)
		{
			GameObject bomb = new GameObject ("Unguided Bomb");
			GameObjectUtility.SetParentAndAlign (bomb, command.context as GameObject);
			//
			Undo.RegisterCreatedObjectUndo(bomb,"Create " + bomb.name);
			Selection.activeObject = bomb;
			//
			//SETUP BOMB
			bomb.AddComponent<Rigidbody>();
			GameObject explosive = new GameObject ("Explosive Filling");
			GameObject model = new GameObject ("Model");
			//
			explosive.transform.parent = bomb.transform;model.transform.parent = bomb.transform;
			//
			CapsuleCollider collider = bomb.AddComponent<CapsuleCollider>();collider.radius =0.2f;collider.height =2f;collider.direction = 2;
			SilantroWarhead warhead = explosive.AddComponent<SilantroWarhead> ();
			SilantroBomb bomber = bomb.AddComponent<SilantroBomb> ();
			//
			GameObject explosion = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Effects/Explosion/Bomb Explosion.prefab",typeof(GameObject));
			warhead.explosionPrefab = explosion;
			//
			bomber.fillingWeight = 10f;bomber.filling = warhead;
			bomb.transform.position = new Vector3(0,2,0);
			//
			EditorSceneManager.MarkSceneDirty (bomb.gameObject.scene);
		}
		//
		//
		//
		//
		//
		//
		//
		//AIRCRAFTS
		//
		[MenuItem("GameObject/Oyedoyin/Aircraft/Jet Powered/TurboJet",false,0)]
		static void CreateNewTurbojetPlane(MenuCommand command)
		{
			GameObject plane = new GameObject ("TurboJet Plane");
			GameObjectUtility.SetParentAndAlign (plane, command.context as GameObject);
			//
			Undo.RegisterCreatedObjectUndo(plane,"Create " + plane.name);
			Selection.activeObject = plane;
			//

			//SETUP PLANE
			plane.AddComponent<Rigidbody>().mass = 1000f;
			GameObject cog = new GameObject ("COG");cog.transform.parent = plane.transform;cog.AddComponent<SilantroGravityCenter> ();cog.AddComponent<SilantroInstrumentation> ();cog.transform.localPosition = new Vector3 (0, 0, -1f);  cog.GetComponent<SilantroInstrumentation> ().aircraftType = SilantroInstrumentation.AircraftType.Propeller;cog.GetComponent<SilantroInstrumentation> ().airplane = plane.GetComponent<Rigidbody> ();cog.tag = "Brain";
			GameObject aerodynamics = new GameObject ("Aerodynamics");aerodynamics.transform.parent = plane.transform;
			GameObject body = new GameObject ("Body");body.transform.parent = plane.transform;
			GameObject engine = new GameObject ("TurboJet Engine");engine.transform.parent = plane.transform;
			GameObject wheelSystem = new GameObject ("Gear System");wheelSystem.transform.parent = plane.transform;
			SilantroGearSystem wheelSys = wheelSystem.AddComponent<SilantroGearSystem> ();
			GameObject weapons = new GameObject ("Weapon System");weapons.transform.parent = plane.transform;
			GameObject cameras = new GameObject ("Camera System");cameras.transform.parent = plane.transform;
			//
			//
			GameObject thruster = new GameObject ();
			thruster.name = "Thruster";
			thruster.transform.parent = engine.transform;
			thruster.transform.localPosition = new Vector3 (0, 0, -2);
			//
			GameObject fan = new GameObject ();
			fan.name= "Fan";
			fan.transform.parent = engine.transform;
			fan.transform.localPosition = new Vector3 (0, 0, 2);	//Selection.activeGameObject.name = "Default TurboFan Engine";
			//
			//
			GameObject effects = new GameObject("Engine Effects");
			effects.transform.parent = engine.transform;
			effects.transform.localPosition = new Vector3 (0, 0, -2);
			//
			GameObject smoke = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Componets/Exhaust Smoke.prefab",typeof(GameObject));
			GameObject smokeEffect = GameObject.Instantiate (smoke, effects.transform.position, Quaternion.Euler(0,-180,0),effects.transform);
			//
			Rigidbody parent = plane.GetComponent<Rigidbody>();
			if (parent == null) {
				Debug.Log ("Engine is not parented to an Aircraft!! Create a default Rigidbody is you're just testing the Engine");
			}
			SilantroTurboJet jet = engine.AddComponent<SilantroTurboJet> ();
			jet.Thruster = thruster.transform;
			jet.intakeFanPoint = fan.transform;
			if (parent != null) {
				jet.Parent = parent;
			}
			AudioClip start = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Default Start.wav",typeof(AudioClip));
			AudioClip stop = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Default Shutdown.wav",typeof(AudioClip));
			//AudioClip alert = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Fuel Alert.wav",typeof(AudioClip));
			AudioClip run = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Jet/Jet Running.wav",typeof(AudioClip));
			jet.EngineIdleSound = run;jet.EngineStartSound = start;jet.EngineShutdownSound = stop;
			//
			jet.diplaySettings = true;
			jet.exhaustSmoke = smokeEffect.GetComponent<ParticleSystem> ();
			//
			//
			GameObject fuelSystem = new GameObject ("Fuel System");fuelSystem.transform.parent = plane.transform;fuelSystem.AddComponent<SilantroFuelDistributor>();
			GameObject tank = new GameObject ("Main Tank");tank.transform.parent = fuelSystem.transform; SilantroFuelTank tnk = tank.AddComponent<SilantroFuelTank> ();tnk.Capacity = 1000f;tnk.tankType = SilantroFuelTank.TankType.Internal;
			fuelSystem.GetComponent<SilantroFuelDistributor> ().internalFuelTank = tnk;
			//
			//SETUP WHEELS
			GameObject frontWheel = new GameObject("Front Wheel");frontWheel.transform.parent = wheelSystem.transform;frontWheel.transform.localPosition = new Vector3(0,-0.5f,0);frontWheel.AddComponent<WheelCollider>();frontWheel.GetComponent<WheelCollider>().radius = 0.2f;
			GameObject leftWheel = new GameObject ("Left Wheel");leftWheel.transform.parent = wheelSystem.transform;leftWheel.transform.localPosition = new Vector3 (-1, -0.5f, -2);leftWheel.AddComponent<WheelCollider>();leftWheel.GetComponent<WheelCollider>().radius = 0.2f;
			GameObject rightWheel = new GameObject ("Right Wheel");rightWheel.transform.parent = wheelSystem.transform;rightWheel.transform.localPosition = new Vector3 (1, -0.5f, -2);rightWheel.AddComponent<WheelCollider>();rightWheel.GetComponent<WheelCollider>().radius = 0.2f;
			//
			GameObject wheelBucket = new GameObject("Wheels");
			wheelBucket.transform.parent = wheelSystem.transform;
			frontWheel.transform.parent = wheelBucket.transform;rightWheel.transform.parent = wheelBucket.transform;leftWheel.transform.parent = wheelBucket.transform;
			//

			//
			SilantroGearSystem.WheelSystem frontGearSystem = new SilantroGearSystem.WheelSystem ();frontGearSystem.collider = frontWheel.GetComponent<WheelCollider>();frontGearSystem.Identifier = "Front Gear";frontGearSystem.steerable = true;
			SilantroGearSystem.WheelSystem leftGearSystem = new SilantroGearSystem.WheelSystem ();leftGearSystem.collider = leftWheel.GetComponent<WheelCollider>();leftGearSystem.Identifier = "Left Gear";leftGearSystem.attachedMotor = true;
			SilantroGearSystem.WheelSystem rightGearSystem = new SilantroGearSystem.WheelSystem ();rightGearSystem.collider = rightWheel.GetComponent<WheelCollider>();rightGearSystem.Identifier = "Right Gear";rightGearSystem.attachedMotor = true;
			//
			wheelSys.wheelSystem.Add(frontGearSystem);wheelSys.wheelSystem.Add(leftGearSystem);wheelSys.wheelSystem.Add(rightGearSystem);
			AudioClip open = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Gear/Gear Open.wav",typeof(AudioClip));
			AudioClip close = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Gear/Gear Close.wav",typeof(AudioClip));
			//
			//SETUP GEAR HYDRAULICS
			GameObject gearHydraulics = new GameObject ("Gear Hydraulics");gearHydraulics.transform.parent = wheelSystem.transform;
			SilantroHydraulicSystem hydraulics = gearHydraulics.AddComponent<SilantroHydraulicSystem> ();
			wheelSys.gearHydraulics = hydraulics;
			wheelSys.gearType = SilantroGearSystem.GearType.Combined;
			hydraulics.closeSound = close;hydraulics.openSound = open;
			//
			SilantroController control = plane.AddComponent<SilantroController> ();
			control.gearHelper = wheelSys;
			control.fuelsystem = plane.GetComponentInChildren<SilantroFuelDistributor> ();
			control.gearHelper.control = control;control.engineType = SilantroController.AircraftType.TurboJet;
			//
			CapsuleCollider col =plane.AddComponent<CapsuleCollider>();
			col.height = 5f;
			col.radius = 0.5f;
			col.direction = 2;
			//
			//SETUP WINGS
			GameObject leftWing = new GameObject("Left Wing");leftWing.transform.parent = aerodynamics.transform;leftWing.transform.localPosition = new Vector3(-1,0,0);leftWing.transform.localScale = new Vector3(-1,1,1);
			GameObject rightWing = new GameObject("Right Wing");rightWing.transform.parent = aerodynamics.transform;rightWing.transform.localPosition = new Vector3(1,0,0);
			GameObject leftTail = new GameObject("Left Tail");leftTail.transform.parent = aerodynamics.transform;leftTail.transform.localPosition = new Vector3(-1,0,-2);leftTail.transform.localScale = new Vector3(-1,1,1);
			GameObject rightTail = new GameObject("Right Tail");rightTail.transform.parent = aerodynamics.transform;rightTail.transform.localPosition = new Vector3(1,0,-2);
			GameObject rudder = new GameObject("Rudder");rudder.transform.parent = aerodynamics.transform;rudder.transform.localPosition = new Vector3(0,0.5f,-2);rudder.transform.localRotation = Quaternion.Euler (0, 0, 90);
			//ADD WING COMPONENTS
			SilantroAerofoil rightWingAerofoil = rightWing.AddComponent<SilantroAerofoil>();rightWingAerofoil.AerofoilSubdivisions = 5;
			rightWingAerofoil.activeControlSurface = true;rightWingAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Aileron;
			//
			SilantroAerofoil leftWingAerofoil = leftWing.AddComponent<SilantroAerofoil>();leftWingAerofoil.AerofoilSubdivisions = 5;
			leftWingAerofoil.activeControlSurface = true;leftWingAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Aileron;
			//
			SilantroAerofoil leftTailAerofoil = leftTail.AddComponent<SilantroAerofoil>();leftTailAerofoil.AerofoilSubdivisions = 3;leftTailAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Stabilizer;
			leftTailAerofoil.activeControlSurface = true;leftTailAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Elevator;rightWingAerofoil.negativeDeflection = true;
			//
			SilantroAerofoil rightTailAerofoil = rightTail.AddComponent<SilantroAerofoil>();rightTailAerofoil.AerofoilSubdivisions = 3;rightTailAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Stabilizer;
			rightTailAerofoil.activeControlSurface = true;rightTailAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Elevator;
			//
			SilantroAerofoil rudderAerofoil = rudder.AddComponent<SilantroAerofoil>();rudderAerofoil.AerofoilSubdivisions = 3;rudderAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Stabilizer;
			rudderAerofoil.activeControlSurface = true;rudderAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Rudder;
			//
			SilantroAirfoil wng = (SilantroAirfoil)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Airfoils/Legacy/Wing 23016.prefab",typeof(SilantroAirfoil));
			SilantroAirfoil ctl = (SilantroAirfoil)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Airfoils/Legacy/Control 0009.prefab",typeof(SilantroAirfoil));
			rightTailAerofoil.airfoil = ctl;rudderAerofoil.airfoil = ctl;leftTailAerofoil.airfoil = ctl;leftWingAerofoil.airfoil = wng;rightWingAerofoil.airfoil = wng;
			//
			//
			GameObject minigunSystem = new GameObject("Minigun System");minigunSystem.transform.parent = weapons.transform;
			GameObject missileSystem = new GameObject("Missile System");missileSystem.transform.parent = weapons.transform;
			GameObject bombSystem = new GameObject("Bomb System");bombSystem.transform.parent = weapons.transform;
			//
			GameObject focalPoint = new GameObject ("Camera Focus Point");focalPoint.transform.parent = plane.transform;
			GameObject normalCam = new GameObject("Default Camera");
			normalCam.gameObject.transform.parent = cameras.transform;normalCam.AddComponent<Camera> ();
			normalCam.gameObject.AddComponent<SilantroCamera> ();normalCam.GetComponent<SilantroCamera> ().FocusPoint = focalPoint;
			//
			plane.transform.position = new Vector3(0,2,0);

		}
		//
		[MenuItem("GameObject/Oyedoyin/Aircraft/Rocket Powered",false,0)]
		static void CreateNewRocketPlane(MenuCommand command)
		{
			GameObject plane = new GameObject ("Rocket Plane");
			GameObjectUtility.SetParentAndAlign (plane, command.context as GameObject);
			//
			Undo.RegisterCreatedObjectUndo(plane,"Create " + plane.name);
			Selection.activeObject = plane;
			//

			//SETUP PLANE
			plane.AddComponent<Rigidbody>().mass = 1000f;
			GameObject cog = new GameObject ("COG");cog.transform.parent = plane.transform;cog.AddComponent<SilantroGravityCenter> ();cog.AddComponent<SilantroInstrumentation> ();cog.transform.localPosition = new Vector3 (0, 0, -1f);  cog.GetComponent<SilantroInstrumentation> ().aircraftType = SilantroInstrumentation.AircraftType.Propeller;cog.GetComponent<SilantroInstrumentation> ().airplane = plane.GetComponent<Rigidbody> ();cog.tag = "Brain";
			GameObject aerodynamics = new GameObject ("Aerodynamics");aerodynamics.transform.parent = plane.transform;
			GameObject body = new GameObject ("Body");body.transform.parent = plane.transform;
			GameObject engine = new GameObject ("Rocket Engine");engine.transform.parent = plane.transform;
			GameObject wheelSystem = new GameObject ("Gear System");wheelSystem.transform.parent = plane.transform;
			SilantroGearSystem wheelSys = wheelSystem.AddComponent<SilantroGearSystem> ();
			GameObject weapons = new GameObject ("Weapon System");weapons.transform.parent = plane.transform;
			GameObject cameras = new GameObject ("Camera System");cameras.transform.parent = plane.transform;
			//SETUP WINGS
			GameObject leftWing = new GameObject("Left Wing");leftWing.transform.parent = aerodynamics.transform;leftWing.transform.localPosition = new Vector3(-1,0,0);leftWing.transform.localScale = new Vector3(-1,1,1);
			GameObject rightWing = new GameObject("Right Wing");rightWing.transform.parent = aerodynamics.transform;rightWing.transform.localPosition = new Vector3(1,0,0);
			GameObject leftTail = new GameObject("Left Stabilizer");leftTail.transform.parent = aerodynamics.transform;leftTail.transform.localPosition = new Vector3(-1,0,-2);leftTail.transform.localScale = new Vector3(-1,1,1);
			GameObject rightTail = new GameObject("Right Stabilizer");rightTail.transform.parent = aerodynamics.transform;rightTail.transform.localPosition = new Vector3(1,0,-2);
			GameObject rudder = new GameObject("Rudder");rudder.transform.parent = aerodynamics.transform;rudder.transform.localPosition = new Vector3(0,0.5f,-2);rudder.transform.localRotation = Quaternion.Euler (0, 0, 90);
			//ADD WING COMPONENTS
			SilantroAerofoil rightWingAerofoil = rightWing.AddComponent<SilantroAerofoil>();rightWingAerofoil.AerofoilSubdivisions = 5;
			rightWingAerofoil.activeControlSurface = true;rightWingAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Aileron;
			//
			SilantroAerofoil leftWingAerofoil = leftWing.AddComponent<SilantroAerofoil>();leftWingAerofoil.AerofoilSubdivisions = 5;
			leftWingAerofoil.activeControlSurface = true;leftWingAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Aileron;
			//
			SilantroAerofoil leftTailAerofoil = leftTail.AddComponent<SilantroAerofoil>();leftTailAerofoil.AerofoilSubdivisions = 3;leftTailAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Stabilizer;
			leftTailAerofoil.activeControlSurface = true;leftTailAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Elevator;rightWingAerofoil.negativeDeflection = true;
			//
			SilantroAerofoil rightTailAerofoil = rightTail.AddComponent<SilantroAerofoil>();rightTailAerofoil.AerofoilSubdivisions = 3;rightTailAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Stabilizer;
			rightTailAerofoil.activeControlSurface = true;rightTailAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Elevator;
			//
			SilantroAerofoil rudderAerofoil = rudder.AddComponent<SilantroAerofoil>();rudderAerofoil.AerofoilSubdivisions = 3;rudderAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Stabilizer;
			rudderAerofoil.activeControlSurface = true;rudderAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Rudder;
			//
			SilantroAirfoil wng = (SilantroAirfoil)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Airfoils/Legacy/Wing 23016.prefab",typeof(SilantroAirfoil));
			SilantroAirfoil ctl = (SilantroAirfoil)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Airfoils/Legacy/Control 0009.prefab",typeof(SilantroAirfoil));
			rightTailAerofoil.airfoil = ctl;rudderAerofoil.airfoil = ctl;leftTailAerofoil.airfoil = ctl;leftWingAerofoil.airfoil = wng;rightWingAerofoil.airfoil = wng;
			//
		}
		//
		[MenuItem("GameObject/Oyedoyin/Aircraft/Jet Powered/TurboFan",false,0)]
		static void CreateNewTurbofanPlane(MenuCommand command)
		{
			GameObject plane = new GameObject ("Turbofan Plane");
			GameObjectUtility.SetParentAndAlign (plane, command.context as GameObject);
			//
			Undo.RegisterCreatedObjectUndo(plane,"Create " + plane.name);
			Selection.activeObject = plane;
			//

			//SETUP PLANE
			plane.AddComponent<Rigidbody>().mass = 1000f;
			GameObject cog = new GameObject ("COG");cog.transform.parent = plane.transform;cog.AddComponent<SilantroGravityCenter> ();cog.AddComponent<SilantroInstrumentation> ();cog.transform.localPosition = new Vector3 (0, 0, -1f);  cog.GetComponent<SilantroInstrumentation> ().aircraftType = SilantroInstrumentation.AircraftType.Propeller;cog.GetComponent<SilantroInstrumentation> ().airplane = plane.GetComponent<Rigidbody> ();cog.tag = "Brain";
			GameObject aerodynamics = new GameObject ("Aerodynamics");aerodynamics.transform.parent = plane.transform;
			GameObject body = new GameObject ("Body");body.transform.parent = plane.transform;
			GameObject engine = new GameObject ("TurboFan Engine");engine.transform.parent = plane.transform;
			GameObject wheelSystem = new GameObject ("Gear System");wheelSystem.transform.parent = plane.transform;
			SilantroGearSystem wheelSys = wheelSystem.AddComponent<SilantroGearSystem> ();
			GameObject weapons = new GameObject ("Weapon System");weapons.transform.parent = plane.transform;
			GameObject cameras = new GameObject ("Camera System");cameras.transform.parent = plane.transform;
			//
			GameObject thruster = new GameObject ();
			thruster.name = "Thruster";
			thruster.transform.parent = engine.transform;
			thruster.transform.localPosition = new Vector3 (0, 0, -2);
			//
			GameObject fan = new GameObject ();
			fan.name= "Fan";
			fan.transform.parent = engine.transform;
			fan.transform.localPosition = new Vector3 (0, 0, 2);	//Selection.activeGameObject.name = "Default TurboFan Engine";
			//
			//
			GameObject effects = new GameObject("Engine Effects");
			effects.transform.parent = engine.transform;
			effects.transform.localPosition = new Vector3 (0, 0, -2);
			//
			GameObject smoke = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Componets/Exhaust Smoke.prefab",typeof(GameObject));
			GameObject smokeEffect = GameObject.Instantiate (smoke, effects.transform.position, Quaternion.Euler(0,-180,0),effects.transform);
			//
			Rigidbody parent = plane.GetComponent<Rigidbody>();
			if (parent == null) {
				Debug.Log ("Engine is not parented to an Aircraft!! Create a default Rigidbody is you're just testing the Engine");
			}
			SilantroTurboFan jet = engine.AddComponent<SilantroTurboFan> ();
			jet.Thruster = thruster.transform;
			jet.fan = fan.transform;
			if (parent != null) {
				jet.Parent = parent;
			}
			AudioClip start = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Default Start.wav",typeof(AudioClip));
			AudioClip stop = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Default Shutdown.wav",typeof(AudioClip));
			//AudioClip alert = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Fuel Alert.wav",typeof(AudioClip));
			AudioClip run = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Jet/Jet Running.wav",typeof(AudioClip));
			jet.EngineIdleSound = run;jet.EngineStartSound = start;jet.EngineShutdownSound = stop;
			//
			jet.diplaySettings = true;
			jet.exhaustSmoke = smokeEffect.GetComponent<ParticleSystem> ();
			//
			//SETUP WINGS
			GameObject leftWing = new GameObject("Left Wing");leftWing.transform.parent = aerodynamics.transform;leftWing.transform.localPosition = new Vector3(-1,0,0);leftWing.transform.localScale = new Vector3(-1,1,1);
			GameObject rightWing = new GameObject("Right Wing");rightWing.transform.parent = aerodynamics.transform;rightWing.transform.localPosition = new Vector3(1,0,0);
			GameObject leftTail = new GameObject("Left Stabilizer");leftTail.transform.parent = aerodynamics.transform;leftTail.transform.localPosition = new Vector3(-1,0,-2);leftTail.transform.localScale = new Vector3(-1,1,1);
			GameObject rightTail = new GameObject("Right Stabilizer");rightTail.transform.parent = aerodynamics.transform;rightTail.transform.localPosition = new Vector3(1,0,-2);
			GameObject rudder = new GameObject("Rudder");rudder.transform.parent = aerodynamics.transform;rudder.transform.localPosition = new Vector3(0,0.5f,-2);rudder.transform.localRotation = Quaternion.Euler (0, 0, 90);
			//ADD WING COMPONENTS
			SilantroAerofoil rightWingAerofoil = rightWing.AddComponent<SilantroAerofoil>();rightWingAerofoil.AerofoilSubdivisions = 5;
			rightWingAerofoil.activeControlSurface = true;rightWingAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Aileron;
			//
			SilantroAerofoil leftWingAerofoil = leftWing.AddComponent<SilantroAerofoil>();leftWingAerofoil.AerofoilSubdivisions = 5;
			leftWingAerofoil.activeControlSurface = true;leftWingAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Aileron;
			//
			SilantroAerofoil leftTailAerofoil = leftTail.AddComponent<SilantroAerofoil>();leftTailAerofoil.AerofoilSubdivisions = 3;leftTailAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Stabilizer;
			leftTailAerofoil.activeControlSurface = true;leftTailAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Elevator;rightWingAerofoil.negativeDeflection = true;
			//
			SilantroAerofoil rightTailAerofoil = rightTail.AddComponent<SilantroAerofoil>();rightTailAerofoil.AerofoilSubdivisions = 3;rightTailAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Stabilizer;
			rightTailAerofoil.activeControlSurface = true;rightTailAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Elevator;
			//
			SilantroAerofoil rudderAerofoil = rudder.AddComponent<SilantroAerofoil>();rudderAerofoil.AerofoilSubdivisions = 3;rudderAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Stabilizer;
			rudderAerofoil.activeControlSurface = true;rudderAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Rudder;
			//
			SilantroAirfoil wng = (SilantroAirfoil)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Airfoils/Legacy/Wing 23016.prefab",typeof(SilantroAirfoil));
			SilantroAirfoil ctl = (SilantroAirfoil)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Airfoils/Legacy/Control 0009.prefab",typeof(SilantroAirfoil));
			rightTailAerofoil.airfoil = ctl;rudderAerofoil.airfoil = ctl;leftTailAerofoil.airfoil = ctl;leftWingAerofoil.airfoil = wng;rightWingAerofoil.airfoil = wng;
			//
			//
			GameObject fuelSystem = new GameObject ("Fuel System");fuelSystem.transform.parent = plane.transform;fuelSystem.AddComponent<SilantroFuelDistributor>();
			GameObject tank = new GameObject ("Main Tank");tank.transform.parent = fuelSystem.transform; SilantroFuelTank tnk = tank.AddComponent<SilantroFuelTank> ();tnk.Capacity = 1000f;tnk.tankType = SilantroFuelTank.TankType.Internal;
			fuelSystem.GetComponent<SilantroFuelDistributor> ().internalFuelTank = tnk;
			//
			//SETUP WHEELS
			GameObject frontWheel = new GameObject("Front Wheel");frontWheel.transform.parent = wheelSystem.transform;frontWheel.transform.localPosition = new Vector3(0,-0.5f,0);frontWheel.AddComponent<WheelCollider>();frontWheel.GetComponent<WheelCollider>().radius = 0.2f;
			GameObject leftWheel = new GameObject ("Left Wheel");leftWheel.transform.parent = wheelSystem.transform;leftWheel.transform.localPosition = new Vector3 (-1, -0.5f, -2);leftWheel.AddComponent<WheelCollider>();leftWheel.GetComponent<WheelCollider>().radius = 0.2f;
			GameObject rightWheel = new GameObject ("Right Wheel");rightWheel.transform.parent = wheelSystem.transform;rightWheel.transform.localPosition = new Vector3 (1, -0.5f, -2);rightWheel.AddComponent<WheelCollider>();rightWheel.GetComponent<WheelCollider>().radius = 0.2f;
			//
			GameObject wheelBucket = new GameObject("Wheels");
			wheelBucket.transform.parent = wheelSystem.transform;
			frontWheel.transform.parent = wheelBucket.transform;rightWheel.transform.parent = wheelBucket.transform;leftWheel.transform.parent = wheelBucket.transform;
			//
			SilantroGearSystem.WheelSystem frontGearSystem = new SilantroGearSystem.WheelSystem ();frontGearSystem.collider = frontWheel.GetComponent<WheelCollider>();frontGearSystem.Identifier = "Front Gear";frontGearSystem.steerable = true;
			SilantroGearSystem.WheelSystem leftGearSystem = new SilantroGearSystem.WheelSystem ();leftGearSystem.collider = leftWheel.GetComponent<WheelCollider>();leftGearSystem.Identifier = "Left Gear";leftGearSystem.attachedMotor = true;
			SilantroGearSystem.WheelSystem rightGearSystem = new SilantroGearSystem.WheelSystem ();rightGearSystem.collider = rightWheel.GetComponent<WheelCollider>();rightGearSystem.Identifier = "Right Gear";rightGearSystem.attachedMotor = true;
			//
			wheelSys.wheelSystem.Add(frontGearSystem);wheelSys.wheelSystem.Add(leftGearSystem);wheelSys.wheelSystem.Add(rightGearSystem);
			AudioClip open = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Gear/Gear Open.wav",typeof(AudioClip));
			AudioClip close = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Gear/Gear Close.wav",typeof(AudioClip));
			//
			//SETUP GEAR HYDRAULICS
			GameObject gearHydraulics = new GameObject ("Gear Hydraulics");gearHydraulics.transform.parent = wheelSystem.transform;
			SilantroHydraulicSystem hydraulics = gearHydraulics.AddComponent<SilantroHydraulicSystem> ();
			wheelSys.gearHydraulics = hydraulics;
			wheelSys.gearType = SilantroGearSystem.GearType.Combined;
			hydraulics.closeSound = close;hydraulics.openSound = open;
			//
			SilantroController control = plane.AddComponent<SilantroController> ();
			control.gearHelper = wheelSys;
			control.fuelsystem = plane.GetComponentInChildren<SilantroFuelDistributor> ();
			control.gearHelper.control = control;control.engineType = SilantroController.AircraftType.TurboFan;
			//
			CapsuleCollider col =plane.AddComponent<CapsuleCollider>();
			col.height = 5f;
			col.radius = 0.5f;
			col.direction = 2;
			//
			//
			GameObject minigunSystem = new GameObject("Minigun System");minigunSystem.transform.parent = weapons.transform;
			GameObject missileSystem = new GameObject("Missile System");missileSystem.transform.parent = weapons.transform;
			GameObject bombSystem = new GameObject("Bomb System");bombSystem.transform.parent = weapons.transform;
			//
			GameObject focalPoint = new GameObject ("Camera Focus Point");focalPoint.transform.parent = plane.transform;
			GameObject normalCam = new GameObject("Default Camera");
			normalCam.gameObject.transform.parent = cameras.transform;normalCam.AddComponent<Camera> ();
			normalCam.gameObject.AddComponent<SilantroCamera> ();normalCam.GetComponent<SilantroCamera> ().FocusPoint = focalPoint;
			//
			plane.transform.position = new Vector3(0,2,0);
		}
		//
		[MenuItem("GameObject/Oyedoyin/Aircraft/Propeller Powered/TurboProp",false,0)]
		static void CreateNewPropellerPlane(MenuCommand command)
		{
			GameObject plane = new GameObject ("Propeller Plane");
			GameObjectUtility.SetParentAndAlign (plane, command.context as GameObject);
			//
			Undo.RegisterCreatedObjectUndo(plane,"Create " + plane.name);
			Selection.activeObject = plane;
			//

			//SETUP PLANE
			plane.AddComponent<Rigidbody>().mass = 1000f;
			GameObject cog = new GameObject ("COG");cog.transform.parent = plane.transform;cog.AddComponent<SilantroGravityCenter> ();cog.AddComponent<SilantroInstrumentation> ();cog.transform.localPosition = new Vector3 (0, 0, -1f);  cog.GetComponent<SilantroInstrumentation> ().aircraftType = SilantroInstrumentation.AircraftType.Propeller;cog.GetComponent<SilantroInstrumentation> ().airplane = plane.GetComponent<Rigidbody> ();cog.tag = "Brain";
			GameObject aerodynamics = new GameObject ("Aerodynamics");aerodynamics.transform.parent = plane.transform;
			GameObject body = new GameObject ("Body");body.transform.parent = plane.transform;
			GameObject engine = new GameObject ("TurboProp Engine");engine.transform.parent = plane.transform;
			GameObject wheelSystem = new GameObject ("Gear System");wheelSystem.transform.parent = plane.transform;
			SilantroGearSystem wheelSys = wheelSystem.AddComponent<SilantroGearSystem> ();
			GameObject weapons = new GameObject ("Weapon System");weapons.transform.parent = plane.transform;
			GameObject cameras = new GameObject ("Camera System");cameras.transform.parent = plane.transform;


			//

			//ADD NECESSARY COMPONENTS
			SilantroTurboProp propEngine = engine.AddComponent<SilantroTurboProp> ();
			GameObject thruster = new GameObject ();
			thruster.name = "Thruster";
			thruster.transform.parent = engine.transform;
			thruster.transform.localPosition = new Vector3 (0, 0, -2);
			//
			propEngine.Thruster = thruster.transform;
			propEngine.Parent = plane.GetComponent<Rigidbody>();//Selection.activeGameObject.name = "Default TurboProp Engine";
			//
			GameObject Props = new GameObject ("Default Propeller");
			//
			Props.transform.parent = engine.transform;
			SilantroBlade blade = Props.AddComponent<SilantroBlade> ();
			blade.bladeType = SilantroBlade.BladeType.Propeller;
			blade.engineType = SilantroBlade.EngineType.TurbopropEngine;
			blade.propEngine = propEngine;
			GameObject bladefoil = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Airfoils/Blades/BLD 08F5.prefab",typeof(GameObject));
			blade.bladefoil = bladefoil.GetComponent<SilantroBladefoil> ();
			//
			//
			//
			GameObject effects = new GameObject("Engine Effects");
			effects.transform.parent = engine.transform;
			effects.transform.localPosition = new Vector3 (0, 0, -2);
			//
			GameObject smoke = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Componets/Exhaust Smoke.prefab",typeof(GameObject));
			GameObject smokeEffect = GameObject.Instantiate (smoke, effects.transform.position, Quaternion.Euler(0,-180,0),effects.transform);
			//
			propEngine.diplaySettings = true;
			propEngine.exhaustSmoke = smokeEffect.GetComponent<ParticleSystem> ();
			//prop.fastPropeller = fastPropller.transform;prop.Propeller = normalPropeller.transform;
			AudioClip start = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Default Start.wav",typeof(AudioClip));
			AudioClip stop = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Default Shutdown.wav",typeof(AudioClip));
			//	AudioClip alert = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Fuel Alert.wav",typeof(AudioClip));
			AudioClip run = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Propeller/Propeller Running.wav",typeof(AudioClip));
			propEngine.EngineStartSound = start;propEngine.EngineShutdownSound = stop;propEngine.EngineIdleSound = run;
			//
			//
			GameObject fuelSystem = new GameObject ("Fuel System");fuelSystem.transform.parent = plane.transform;fuelSystem.AddComponent<SilantroFuelDistributor>();
			GameObject tank = new GameObject ("Main Tank");tank.transform.parent = fuelSystem.transform; SilantroFuelTank tnk = tank.AddComponent<SilantroFuelTank> ();tnk.Capacity = 1000f;tnk.tankType = SilantroFuelTank.TankType.Internal;
			fuelSystem.GetComponent<SilantroFuelDistributor> ().internalFuelTank = tnk;
			//
			//SETUP WHEELS
			GameObject frontWheel = new GameObject("Front Wheel");frontWheel.transform.parent = wheelSystem.transform;frontWheel.transform.localPosition = new Vector3(0,-0.5f,0);frontWheel.AddComponent<WheelCollider>();frontWheel.GetComponent<WheelCollider>().radius = 0.2f;
			GameObject leftWheel = new GameObject ("Left Wheel");leftWheel.transform.parent = wheelSystem.transform;leftWheel.transform.localPosition = new Vector3 (-1, -0.5f, -2);leftWheel.AddComponent<WheelCollider>();leftWheel.GetComponent<WheelCollider>().radius = 0.2f;
			GameObject rightWheel = new GameObject ("Right Wheel");rightWheel.transform.parent = wheelSystem.transform;rightWheel.transform.localPosition = new Vector3 (1, -0.5f, -2);rightWheel.AddComponent<WheelCollider>();rightWheel.GetComponent<WheelCollider>().radius = 0.2f;
			//
			GameObject wheelBucket = new GameObject("Wheels");
			wheelBucket.transform.parent = wheelSystem.transform;
			frontWheel.transform.parent = wheelBucket.transform;rightWheel.transform.parent = wheelBucket.transform;leftWheel.transform.parent = wheelBucket.transform;
			//
			SilantroGearSystem.WheelSystem frontGearSystem = new SilantroGearSystem.WheelSystem ();frontGearSystem.collider = frontWheel.GetComponent<WheelCollider>();frontGearSystem.Identifier = "Front Gear";frontGearSystem.steerable = true;
			SilantroGearSystem.WheelSystem leftGearSystem = new SilantroGearSystem.WheelSystem ();leftGearSystem.collider = leftWheel.GetComponent<WheelCollider>();leftGearSystem.Identifier = "Left Gear";leftGearSystem.attachedMotor = true;
			SilantroGearSystem.WheelSystem rightGearSystem = new SilantroGearSystem.WheelSystem ();rightGearSystem.collider = rightWheel.GetComponent<WheelCollider>();rightGearSystem.Identifier = "Right Gear";rightGearSystem.attachedMotor = true;
			//
			wheelSys.wheelSystem.Add(frontGearSystem);wheelSys.wheelSystem.Add(leftGearSystem);wheelSys.wheelSystem.Add(rightGearSystem);
			AudioClip open = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Gear/Gear Open.wav",typeof(AudioClip));
			AudioClip close = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Gear/Gear Close.wav",typeof(AudioClip));
			//
			//SETUP GEAR HYDRAULICS
			GameObject gearHydraulics = new GameObject ("Gear Hydraulics");gearHydraulics.transform.parent = wheelSystem.transform;
			SilantroHydraulicSystem hydraulics = gearHydraulics.AddComponent<SilantroHydraulicSystem> ();
			wheelSys.gearHydraulics = hydraulics;
			wheelSys.gearType = SilantroGearSystem.GearType.Combined;
			hydraulics.closeSound = close;hydraulics.openSound = open;
			//
			SilantroController control = plane.AddComponent<SilantroController> ();
			control.gearHelper = wheelSys;
			control.fuelsystem = plane.GetComponentInChildren<SilantroFuelDistributor> ();
			control.gearHelper.control = control;control.engineType = SilantroController.AircraftType.TurboProp;
			//
			CapsuleCollider col =plane.AddComponent<CapsuleCollider>();
			col.height = 5f;
			col.radius = 0.5f;
			col.direction = 2;
			//
			//SETUP WINGS
			GameObject leftWing = new GameObject("Left Wing");leftWing.transform.parent = aerodynamics.transform;leftWing.transform.localPosition = new Vector3(-1,0,0);leftWing.transform.localScale = new Vector3(-1,1,1);
			GameObject rightWing = new GameObject("Right Wing");rightWing.transform.parent = aerodynamics.transform;rightWing.transform.localPosition = new Vector3(1,0,0);
			GameObject leftTail = new GameObject("Left Stabilizer");leftTail.transform.parent = aerodynamics.transform;leftTail.transform.localPosition = new Vector3(-1,0,-2);leftTail.transform.localScale = new Vector3(-1,1,1);
			GameObject rightTail = new GameObject("Right Stabilizer");rightTail.transform.parent = aerodynamics.transform;rightTail.transform.localPosition = new Vector3(1,0,-2);
			GameObject rudder = new GameObject("Rudder");rudder.transform.parent = aerodynamics.transform;rudder.transform.localPosition = new Vector3(0,0.5f,-2);rudder.transform.localRotation = Quaternion.Euler (0, 0, 90);
			//ADD WING COMPONENTS
			SilantroAerofoil rightWingAerofoil = rightWing.AddComponent<SilantroAerofoil>();rightWingAerofoil.AerofoilSubdivisions = 5;
			rightWingAerofoil.activeControlSurface = true;rightWingAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Aileron;
			//
			SilantroAerofoil leftWingAerofoil = leftWing.AddComponent<SilantroAerofoil>();leftWingAerofoil.AerofoilSubdivisions = 5;
			leftWingAerofoil.activeControlSurface = true;leftWingAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Aileron;
			//
			SilantroAerofoil leftTailAerofoil = leftTail.AddComponent<SilantroAerofoil>();leftTailAerofoil.AerofoilSubdivisions = 3;leftTailAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Stabilizer;
			leftTailAerofoil.activeControlSurface = true;leftTailAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Elevator;rightWingAerofoil.negativeDeflection = true;
			//
			SilantroAerofoil rightTailAerofoil = rightTail.AddComponent<SilantroAerofoil>();rightTailAerofoil.AerofoilSubdivisions = 3;rightTailAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Stabilizer;
			rightTailAerofoil.activeControlSurface = true;rightTailAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Elevator;
			//
			SilantroAerofoil rudderAerofoil = rudder.AddComponent<SilantroAerofoil>();rudderAerofoil.AerofoilSubdivisions = 3;rudderAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Stabilizer;
			rudderAerofoil.activeControlSurface = true;rudderAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Rudder;
			//
			SilantroAirfoil wng = (SilantroAirfoil)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Airfoils/Legacy/Wing 23016.prefab",typeof(SilantroAirfoil));
			SilantroAirfoil ctl = (SilantroAirfoil)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Airfoils/Legacy/Control 0009.prefab",typeof(SilantroAirfoil));
			rightTailAerofoil.airfoil = ctl;rudderAerofoil.airfoil = ctl;leftTailAerofoil.airfoil = ctl;leftWingAerofoil.airfoil = wng;rightWingAerofoil.airfoil = wng;
			//
			//
			GameObject minigunSystem = new GameObject("Minigun System");minigunSystem.transform.parent = weapons.transform;
			GameObject missileSystem = new GameObject("Missile System");missileSystem.transform.parent = weapons.transform;
			GameObject bombSystem = new GameObject("Bomb System");bombSystem.transform.parent = weapons.transform;
			//
			GameObject focalPoint = new GameObject ("Camera Focus Point");focalPoint.transform.parent = plane.transform;
			GameObject normalCam = new GameObject("Default Camera");
			normalCam.gameObject.transform.parent = cameras.transform;normalCam.AddComponent<Camera> ();
			normalCam.gameObject.AddComponent<SilantroCamera> ();normalCam.GetComponent<SilantroCamera> ().FocusPoint = focalPoint;
			//
			plane.transform.position = new Vector3(0,2,0);
		}
		//
		[MenuItem("GameObject/Oyedoyin/Aircraft/Propeller Powered/Piston",false,0)]
		static void CreateNewPistonPropellerPlane(MenuCommand command)
		{
			GameObject plane = new GameObject ("Propeller Plane");
			GameObjectUtility.SetParentAndAlign (plane, command.context as GameObject);
			//
			Undo.RegisterCreatedObjectUndo(plane,"Create " + plane.name);
			Selection.activeObject = plane;
			//

			//SETUP PLANE
			plane.AddComponent<Rigidbody>().mass = 1000f;
			GameObject cog = new GameObject ("COG");cog.transform.parent = plane.transform;cog.AddComponent<SilantroGravityCenter> ();cog.AddComponent<SilantroInstrumentation> ();cog.transform.localPosition = new Vector3 (0, 0, -1f);  cog.GetComponent<SilantroInstrumentation> ().aircraftType = SilantroInstrumentation.AircraftType.Propeller;cog.GetComponent<SilantroInstrumentation> ().airplane = plane.GetComponent<Rigidbody> ();cog.tag = "Brain";
			GameObject aerodynamics = new GameObject ("Aerodynamics");aerodynamics.transform.parent = plane.transform;
			GameObject body = new GameObject ("Body");body.transform.parent = plane.transform;
			GameObject engine = new GameObject ("TurboProp Engine");engine.transform.parent = plane.transform;
			GameObject wheelSystem = new GameObject ("Gear System");wheelSystem.transform.parent = plane.transform;
			SilantroGearSystem wheelSys = wheelSystem.AddComponent<SilantroGearSystem> ();
			GameObject weapons = new GameObject ("Weapon System");weapons.transform.parent = plane.transform;
			GameObject cameras = new GameObject ("Camera System");cameras.transform.parent = plane.transform;

			//ADD NECESSARY COMPONENTS
			SilantroPistonEngine propEngine = engine.AddComponent<SilantroPistonEngine> ();
			GameObject thruster = new GameObject ();
			thruster.name = "Thruster";
			thruster.transform.parent = engine.transform;
			thruster.transform.localPosition = new Vector3 (0, 0, -2);
			//
			propEngine.Thruster = thruster.transform;
			propEngine.Parent = plane.GetComponent<Rigidbody>();Selection.activeGameObject.name = "Default Piston Engine";
			//
			GameObject Props = new GameObject ("Default Propeller");
			//
			Props.transform.parent = engine.transform;
			SilantroBlade blade = Props.AddComponent<SilantroBlade> ();
			blade.bladeType = SilantroBlade.BladeType.Propeller;
			blade.engineType = SilantroBlade.EngineType.PistonEngine;
			blade.pistonEngine = propEngine;
			GameObject bladefoil = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Airfoils/Blades/BLD 08F5.prefab",typeof(GameObject));
			blade.bladefoil = bladefoil.GetComponent<SilantroBladefoil> ();
			//
			//
			//
			GameObject effects = new GameObject("Engine Effects");
			effects.transform.parent = engine.transform;
			effects.transform.localPosition = new Vector3 (0, 0, -2);
			//
			GameObject smoke = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Componets/Exhaust Smoke.prefab",typeof(GameObject));
			GameObject smokeEffect = GameObject.Instantiate (smoke, effects.transform.position, Quaternion.Euler(0,-180,0),effects.transform);
			//
			propEngine.diplaySettings = true;
			propEngine.exhaustSmoke = smokeEffect.GetComponent<ParticleSystem> ();
			//prop.fastPropeller = fastPropller.transform;prop.Propeller = normalPropeller.transform;
			AudioClip start = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Default Start.wav",typeof(AudioClip));
			AudioClip stop = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Engines/Default Shutdown.wav",typeof(AudioClip));
			//	AudioClip alert = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Fuel Alert.wav",typeof(AudioClip));
			AudioClip run = (AudioClip)AssetDatabase.LoadAssetAtPath ("ssets/Silantro Simulator/Sounds/Default/Engines/Propeller/Merlin Running 1.wav",typeof(AudioClip));
			propEngine.EngineStartSound = start;propEngine.EngineShutdownSound = stop;propEngine.EngineIdleSound = run;
			//
			//
			GameObject fuelSystem = new GameObject ("Fuel System");fuelSystem.transform.parent = plane.transform;fuelSystem.AddComponent<SilantroFuelDistributor>();
			GameObject tank = new GameObject ("Main Tank");tank.transform.parent = fuelSystem.transform; SilantroFuelTank tnk = tank.AddComponent<SilantroFuelTank> ();tnk.Capacity = 1000f;tnk.tankType = SilantroFuelTank.TankType.Internal;
			fuelSystem.GetComponent<SilantroFuelDistributor> ().internalFuelTank = tnk;
			//
			//SETUP WHEELS
			GameObject frontWheel = new GameObject("Front Wheel");frontWheel.transform.parent = wheelSystem.transform;frontWheel.transform.localPosition = new Vector3(0,-0.5f,0);frontWheel.AddComponent<WheelCollider>();frontWheel.GetComponent<WheelCollider>().radius = 0.2f;
			GameObject leftWheel = new GameObject ("Left Wheel");leftWheel.transform.parent = wheelSystem.transform;leftWheel.transform.localPosition = new Vector3 (-1, -0.5f, -2);leftWheel.AddComponent<WheelCollider>();leftWheel.GetComponent<WheelCollider>().radius = 0.2f;
			GameObject rightWheel = new GameObject ("Right Wheel");rightWheel.transform.parent = wheelSystem.transform;rightWheel.transform.localPosition = new Vector3 (1, -0.5f, -2);rightWheel.AddComponent<WheelCollider>();rightWheel.GetComponent<WheelCollider>().radius = 0.2f;
			//
			GameObject wheelBucket = new GameObject("Wheels");
			wheelBucket.transform.parent = wheelSystem.transform;
			frontWheel.transform.parent = wheelBucket.transform;rightWheel.transform.parent = wheelBucket.transform;leftWheel.transform.parent = wheelBucket.transform;
			//
			SilantroGearSystem.WheelSystem frontGearSystem = new SilantroGearSystem.WheelSystem ();frontGearSystem.collider = frontWheel.GetComponent<WheelCollider>();frontGearSystem.Identifier = "Front Gear";frontGearSystem.steerable = true;
			SilantroGearSystem.WheelSystem leftGearSystem = new SilantroGearSystem.WheelSystem ();leftGearSystem.collider = leftWheel.GetComponent<WheelCollider>();leftGearSystem.Identifier = "Left Gear";leftGearSystem.attachedMotor = true;
			SilantroGearSystem.WheelSystem rightGearSystem = new SilantroGearSystem.WheelSystem ();rightGearSystem.collider = rightWheel.GetComponent<WheelCollider>();rightGearSystem.Identifier = "Right Gear";rightGearSystem.attachedMotor = true;
			//
			wheelSys.wheelSystem.Add(frontGearSystem);wheelSys.wheelSystem.Add(leftGearSystem);wheelSys.wheelSystem.Add(rightGearSystem);
			AudioClip open = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Gear/Gear Open.wav",typeof(AudioClip));
			AudioClip close = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Sounds/Default/Gear/Gear Close.wav",typeof(AudioClip));
			//
			//SETUP GEAR HYDRAULICS
			GameObject gearHydraulics = new GameObject ("Gear Hydraulics");gearHydraulics.transform.parent = wheelSystem.transform;
			SilantroHydraulicSystem hydraulics = gearHydraulics.AddComponent<SilantroHydraulicSystem> ();
			wheelSys.gearHydraulics = hydraulics;
			wheelSys.gearType = SilantroGearSystem.GearType.Combined;
			hydraulics.closeSound = close;hydraulics.openSound = open;
			//
			SilantroController control = plane.AddComponent<SilantroController> ();
			control.gearHelper = wheelSys;
			control.fuelsystem = plane.GetComponentInChildren<SilantroFuelDistributor> ();
			control.gearHelper.control = control;control.engineType = SilantroController.AircraftType.Piston;
			//
			CapsuleCollider col =plane.AddComponent<CapsuleCollider>();
			col.height = 5f;
			col.radius = 0.5f;
			col.direction = 2;
			//
			//SETUP WINGS
			GameObject leftWing = new GameObject("Left Wing");leftWing.transform.parent = aerodynamics.transform;leftWing.transform.localPosition = new Vector3(-1,0,0);leftWing.transform.localScale = new Vector3(-1,1,1);
			GameObject rightWing = new GameObject("Right Wing");rightWing.transform.parent = aerodynamics.transform;rightWing.transform.localPosition = new Vector3(1,0,0);
			GameObject leftTail = new GameObject("Left Tail");leftTail.transform.parent = aerodynamics.transform;leftTail.transform.localPosition = new Vector3(-1,0,-2);leftTail.transform.localScale = new Vector3(-1,1,1);
			GameObject rightTail = new GameObject("Right Tail");rightTail.transform.parent = aerodynamics.transform;rightTail.transform.localPosition = new Vector3(1,0,-2);
			GameObject rudder = new GameObject("Rudder");rudder.transform.parent = aerodynamics.transform;rudder.transform.localPosition = new Vector3(0,0.5f,-2);rudder.transform.localRotation = Quaternion.Euler (0, 0, 90);
			//ADD WING COMPONENTS
			SilantroAerofoil rightWingAerofoil = rightWing.AddComponent<SilantroAerofoil>();rightWingAerofoil.AerofoilSubdivisions = 5;
			rightWingAerofoil.activeControlSurface = true;rightWingAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Aileron;
			//
			SilantroAerofoil leftWingAerofoil = leftWing.AddComponent<SilantroAerofoil>();leftWingAerofoil.AerofoilSubdivisions = 5;
			leftWingAerofoil.activeControlSurface = true;leftWingAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Aileron;
			//
			SilantroAerofoil leftTailAerofoil = leftTail.AddComponent<SilantroAerofoil>();leftTailAerofoil.AerofoilSubdivisions = 3;leftTailAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Stabilizer;
			leftTailAerofoil.activeControlSurface = true;leftTailAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Elevator;rightWingAerofoil.negativeDeflection = true;
			//
			SilantroAerofoil rightTailAerofoil = rightTail.AddComponent<SilantroAerofoil>();rightTailAerofoil.AerofoilSubdivisions = 3;rightTailAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Stabilizer;
			rightTailAerofoil.activeControlSurface = true;rightTailAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Elevator;
			//
			SilantroAerofoil rudderAerofoil = rudder.AddComponent<SilantroAerofoil>();rudderAerofoil.AerofoilSubdivisions = 3;rudderAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Stabilizer;
			rudderAerofoil.activeControlSurface = true;rudderAerofoil.surfaceType = SilantroAerofoil.SurfaceType.Rudder;
			//
			SilantroAirfoil wng = (SilantroAirfoil)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Airfoils/Legacy/Wing 23016.prefab",typeof(SilantroAirfoil));
			SilantroAirfoil ctl = (SilantroAirfoil)AssetDatabase.LoadAssetAtPath ("Assets/Silantro Simulator/Prefabs/Default/Airfoils/Legacy/Control 0009.prefab",typeof(SilantroAirfoil));
			rightTailAerofoil.airfoil = ctl;rudderAerofoil.airfoil = ctl;leftTailAerofoil.airfoil = ctl;leftWingAerofoil.airfoil = wng;rightWingAerofoil.airfoil = wng;
			//
			//
			GameObject minigunSystem = new GameObject("Minigun System");minigunSystem.transform.parent = weapons.transform;
			GameObject missileSystem = new GameObject("Missile System");missileSystem.transform.parent = weapons.transform;
			GameObject bombSystem = new GameObject("Bomb System");bombSystem.transform.parent = weapons.transform;
			//
			GameObject focalPoint = new GameObject ("Camera Focus Point");focalPoint.transform.parent = plane.transform;
			GameObject normalCam = new GameObject("Default Camera");
			normalCam.gameObject.transform.parent = cameras.transform;normalCam.AddComponent<Camera> ();
			normalCam.gameObject.AddComponent<SilantroCamera> ();normalCam.GetComponent<SilantroCamera> ().FocusPoint = focalPoint;
			//
			plane.transform.position = new Vector3(0,2,0);

		}
		//

	}
}
