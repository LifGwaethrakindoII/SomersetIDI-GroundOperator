using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
//
public class SilantroRocket : MonoBehaviour {
	//
	public enum BoosterType
	{
		Weapon,
		Booster
	}
	[HideInInspector]public BoosterType boosterType = BoosterType.Booster;
	//
	[HideInInspector]public float rocketDiameter;
	[HideInInspector]public float nozzleDiameterPercentage;
	[HideInInspector]public float nozzleDiameter;
	[HideInInspector]public float nozzleArea;[HideInInspector]public float demoArea;//To be displayed
	[HideInInspector]public float weight;
	[HideInInspector]public float overallLength;
	[HideInInspector]public float exhaustConeLength;
	[HideInInspector]public float fuelLength;
	//
	[HideInInspector]public string Identifier;
	//
	[HideInInspector]public Rigidbody Aircraft;
	[HideInInspector]public Rigidbody Rocket;
	[HideInInspector]public SilantroRocketMotor MotorEngine;
	[HideInInspector]public SilantroInstrumentation boardControl;
	//
	[HideInInspector]public SilantroProfile profile;
	[HideInInspector]public bool armed;
	//
	public enum FuzeType
	{
		MK352,
		M423,
		M427,
		MK191,
		MK193Mod0,
		MK93
	}
	[HideInInspector]public FuzeType fuzeType = FuzeType.M423;
	[HideInInspector]public string detonationMechanism = "Default";
	//[HideInInspector]public float impactVelocity;
	[HideInInspector]public float timer;
	[HideInInspector]public float proximity;
	float selfDestructTimer;
	//
	void Start()
	{
		nozzleDiameter = rocketDiameter * nozzleDiameterPercentage/100f;
		demoArea = (3.142f * nozzleDiameter * nozzleDiameter) / 4f;
		nozzleArea = demoArea;
		//
		Rocket = GetComponent<Rigidbody> ();
		if(Rocket != null){
		Rocket.mass = weight;
		Rocket.isKinematic = true;
		//
		MotorEngine.Rocket = Rocket;
		}
		MotorEngine.nozzleArea = nozzleArea;
		MotorEngine.InitialCalculations ();
		MotorEngine.booster = GetComponent<SilantroRocket> ();
		//
		if (boardControl != null) {
			MotorEngine.control = boardControl;
		}
		armed = false;
		//
	}
	//
	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		//
		nozzleDiameter = rocketDiameter * nozzleDiameterPercentage/100f;
		demoArea = (3.142f * nozzleDiameter * nozzleDiameter) / 4f;
		//
		Handles.color = Color.red;
		if(MotorEngine != null){
			Handles.DrawWireDisc (MotorEngine.transform.position, MotorEngine.transform.forward, (nozzleDiameter/2f));
		}
		//
		//
		Vector3 throatDistance = MotorEngine.transform.position + new Vector3 (0,0,exhaustConeLength);
		Vector3 fuelDistance = throatDistance + new Vector3 (0, 0, fuelLength);
		//
		Handles.color = Color.blue;
		Handles.DrawWireDisc (fuelDistance, this.transform.forward, (rocketDiameter / 2f));
		//
		Handles.color = Color.red;
		Handles.DrawLine (fuelDistance, throatDistance);

	}
	#endif
	//
	//DAMAGE
	void OnCollisionEnter(Collision col)
	{
			if (armed && fuzeType == FuzeType.MK352){
				Explode (transform.position);
			}
	}
	//
	//EXPLODE
	public void Explode(Vector3 position)
	{
		SilantroWarhead filling = GetComponentInChildren<SilantroWarhead> ();
		if (filling != null) {
			filling.Explode (position);
		}
		//
		Destroy (gameObject);
	}
	//
	public void Launch()
	{
		Vector3 dropVelocity = Rocket.transform.root.gameObject.GetComponent<Rigidbody> ().velocity;
		//
		MotorEngine.active = true;
		Rocket.isKinematic = false;
		Rocket.transform.parent = null;
		Rocket.velocity = dropVelocity;
		//armed = true;
		StartCoroutine (ClearAircraft ());
	}
	//
	void Update()
	{
		if (armed) {
			selfDestructTimer += Time.deltaTime;
			if (selfDestructTimer > 15) {
				Explode (transform.position);
			}
		}
	}
	//
	IEnumerator ClearAircraft()
	{
		yield return new WaitForSeconds (0.2f);
		armed = true;
	}
	//
}
//
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroRocket))]
public class RocketEditor: Editor
{
	Color backgroundColor;
	Color silantroColor = Color.cyan;
	//

	public override void OnInspectorGUI()
	{
		backgroundColor = GUI.backgroundColor;
		//
		DrawDefaultInspector ();
		//
		//SilantroTurboJet kk;
		SilantroRocket rocket = (SilantroRocket)target;
		//
		GUILayout.Space(2f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Rocket Type", MessageType.None);
		GUI.color = backgroundColor;
		//
		GUILayout.Space(2f);
		rocket.boosterType = (SilantroRocket.BoosterType)EditorGUILayout.EnumPopup("Rocket Type",rocket.boosterType);
		GUILayout.Space(2f);
		if (rocket.boosterType == SilantroRocket.BoosterType.Booster) {
			GUILayout.Space(2f);
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("Booster System", MessageType.None);
			GUI.color = backgroundColor;
			//
			GUILayout.Space(2f);
			rocket.Aircraft = EditorGUILayout.ObjectField (" ", rocket.Aircraft, typeof(Rigidbody), true) as Rigidbody;
			GUILayout.Space(5f);
			rocket.boardControl = EditorGUILayout.ObjectField ("Onboard Computer", rocket.boardControl, typeof(SilantroInstrumentation), true) as SilantroInstrumentation;
		}
		if (rocket.boosterType == SilantroRocket.BoosterType.Weapon) {
			GUILayout.Space(5f);
			rocket.boardControl = EditorGUILayout.ObjectField ("Onboard Computer", rocket.boardControl, typeof(SilantroInstrumentation), true) as SilantroInstrumentation;
			//
			GUILayout.Space(2f);
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("Detonation System", MessageType.None);
			GUI.color = backgroundColor;
			//
			GUILayout.Space(2f);
			rocket.fuzeType = (SilantroRocket.FuzeType)EditorGUILayout.EnumPopup ("Fuze Type", rocket.fuzeType);
			//
			GUILayout.Space(2f);
			EditorGUILayout.LabelField ("Trigger System", rocket.detonationMechanism);
			//
			GUILayout.Space(2f);
			EditorGUILayout.LabelField ("Armed", rocket.armed.ToString ());
			GUILayout.Space(3f);
			if (rocket.fuzeType == SilantroRocket.FuzeType.M423 || rocket.fuzeType == SilantroRocket.FuzeType.MK352 || rocket.fuzeType == SilantroRocket.FuzeType.M427) {
				rocket.detonationMechanism = "Nose Impact";
				GUILayout.Space(2f);
				//rocket.impactVelocity = EditorGUILayout.FloatField ("Impact Velocity", rocket.impactVelocity);
			} 
			else if (rocket.fuzeType == SilantroRocket.FuzeType.MK191) {
				rocket.detonationMechanism = "Base Detonation Impact";
				GUILayout.Space(2f);
				//rocket.impactVelocity = EditorGUILayout.FloatField ("Impact Velocity", rocket.impactVelocity);
			} 
			else if (rocket.fuzeType == SilantroRocket.FuzeType.MK193Mod0) 
			{
				rocket.detonationMechanism = "Mechanical Time";
				GUILayout.Space(2f);
				rocket.timer = EditorGUILayout.FloatField ("Trigger Timer", rocket.timer);
			}
			else if (rocket.fuzeType == SilantroRocket.FuzeType.MK93) 
			{
				rocket.detonationMechanism = "Proximity";
				GUILayout.Space(2f);
				rocket.proximity = EditorGUILayout.FloatField ("Detonation Proximity", rocket.proximity);
			}

			//
		}
		//
		GUILayout.Space(20f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Rocket Dimensions", MessageType.None);
		GUI.color = backgroundColor;
		//
		GUILayout.Space(2f);
		//
		rocket.rocketDiameter = EditorGUILayout.FloatField("Diameter",rocket.rocketDiameter);
		GUILayout.Space(2f);
		rocket.nozzleDiameterPercentage = EditorGUILayout.Slider ("Nozzle Diameter Percentage",rocket.nozzleDiameterPercentage,0,100);
		//
		GUILayout.Space(2f);
		EditorGUILayout.LabelField ("Nozzle Diameter", rocket.nozzleDiameter.ToString ("0.00") + " m");
		GUILayout.Space(1f);
		EditorGUILayout.LabelField ("Nozzle Area", rocket.demoArea.ToString ("0.00") + " m2");
		//
		GUILayout.Space(5f);
		rocket.weight = EditorGUILayout.FloatField("Weight",rocket.weight);
		rocket.overallLength = EditorGUILayout.FloatField("Overall Length",rocket.overallLength);
		//
		GUILayout.Space(3f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Fuel Dimensions", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		rocket.exhaustConeLength = EditorGUILayout.FloatField ("Exhaust Cone Length", rocket.exhaustConeLength);
		GUILayout.Space(2f);
		rocket.fuelLength = EditorGUILayout.FloatField ("Fuel Length", rocket.fuelLength);
		GUILayout.Space (3f);
		rocket.MotorEngine = EditorGUILayout.ObjectField ("Rocket Motor", rocket.MotorEngine, typeof(SilantroRocketMotor), true) as SilantroRocketMotor;
		//
		//
		if (GUI.changed) {
			EditorUtility.SetDirty (rocket);
			EditorSceneManager.MarkSceneDirty (rocket.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
	}
}
#endif