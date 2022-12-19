//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
//
public class SilantroMinigun : MonoBehaviour {

	[HideInInspector]public float damage=10f;
	//
	[HideInInspector]public float rateOfFire = 5;
	[HideInInspector]public float actualRate;
	[HideInInspector]public float fireTimer;
	//float actualFireTimer;
	//
	[HideInInspector]public float accuracy = 80f;
	[HideInInspector]public float currentAccuracy;
	[HideInInspector]public float accuracyDrop = 0.2f;
	[HideInInspector]public float accuracyRecover = 0.5f;
	float acc;
	//
	[HideInInspector]public float range = 500f;
	[HideInInspector]public float rangeRatio = 1f;
	[HideInInspector ]public bool isOnline;
	//
	[HideInInspector]public int ammoCapacity = 100;
	[HideInInspector]public int currentAmmo;
	[HideInInspector]public bool unlimitedAmmo;
	[HideInInspector]public bool advancedSettings;
	//
	[HideInInspector]public int muzzleCount;
	[HideInInspector]public Transform[] muzzles;
	[HideInInspector]public Transform shellEjectPoint;
	//
	[HideInInspector]public bool useTrails;
	[HideInInspector]public float muzzleVelocity;
	[HideInInspector]public GameObject bulletTrails;
	//
	[HideInInspector]public Transform barrel;
	[HideInInspector]private float barrelRPM;
	[HideInInspector]public float currentRPM;
	//
	[HideInInspector]public bool ejectShells = false;
	//
	public enum RotationAxis
	{
		X,
		Y,
		Z
	}
	//[HideInInspector]
	[HideInInspector]public RotationAxis rotationAxis = RotationAxis.X;
	//
	public enum RotationDirection
	{
		CW,
		CCW
	}
	//[HideInInspector]
	[HideInInspector]public RotationDirection rotationDirection = RotationDirection.CCW;
	//
	//
	[HideInInspector]public GameObject muzzleFlash;
	[HideInInspector]public GameObject bulletCase;
	private float shellSpitForce = 1.5f;					
	private float shellForceRandom = 1.5f;
	private float shellSpitTorqueX = 0.5f;
	private float shellSpitTorqueY = 0.5f;
	private float shellTorqueRandom = 1.0f;
	[HideInInspector]public bool isControllable = true;
	//
	[HideInInspector]public GameObject groundHit;
	[HideInInspector]public GameObject metalHit;
	[HideInInspector]public GameObject woodHit;
	//
	private int muzzle = 0;
	private Transform currentMuzzle;
	//
	Vector3 planeVelocity;
	//
	[HideInInspector]public AudioClip fireSound;
	[HideInInspector]public float soundVolume = 0.75f;
	//public AudioClip dryFireSound;
	//
	bool canFire = true;
	//
	// Use this for initialization
	void Start () {
		//
		if (rateOfFire != 0) {
			actualRate = 1.0f / rateOfFire;
		} else {
			actualRate = 0.01f;
		}
		fireTimer = 0.0f;
		//
		currentMuzzle = muzzles [muzzle];
		//Add Audio Source
		if (GetComponent<AudioSource> () == null) {
			gameObject.AddComponent (typeof(AudioSource));
			gameObject.GetComponent<AudioSource> ().volume = soundVolume;
		}
		currentAmmo = ammoCapacity;
		barrelRPM = rateOfFire * 5.1f * 60f;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		//
		currentAccuracy = Mathf.Lerp(currentAccuracy, accuracy, accuracyRecover * Time.deltaTime);
		// Update the fireTimer
		fireTimer += Time.deltaTime;
		//
		//actualFireTimer = fireTimer;
		//
		if (currentAmmo < 0)
		{ 
			currentAmmo = 0;
		}
		if (currentAmmo == 0) {
			canFire = false;
		}
		//
		if (isControllable && isOnline) {
			if (Input.GetButton ("Fire") && (fireTimer >= actualRate) && canFire) {
				Fire ();
				currentRPM = Mathf.Lerp (currentRPM, barrelRPM, actualRate);
			} else {
				currentRPM = Mathf.Lerp (currentRPM, 0.0f, actualRate);
			}
		}
	}
	//
	//DESTRUCTION SYSTEM
	public void Disintegrate()
	{
		Destroy (this.gameObject);
	}
	//

	void Fire()
	{
		muzzle += 1;
		if (muzzle > (muzzles.Length -1)) {
			muzzle = 0;
		}
		currentMuzzle = muzzles [muzzle];
		//
		//Reset
		fireTimer = 0.0f;
		//
		if (!unlimitedAmmo) {
			currentAmmo--;
		}
		//
		//Actually Shoot weapon
		Vector3 direction = currentMuzzle.forward;
		//
		Ray rayout = new Ray (currentMuzzle.position, direction);
		RaycastHit hitout;
		//
		if (Physics.Raycast (rayout, out hitout, range / rangeRatio)) {
			//Debug.Log (hitout.distance);
			acc = 1 - ((hitout.distance) / (range / rangeRatio));
		}
		//Calculate accuracy for current shot
		float accuracyVary = (100 - currentAccuracy) / 1000;

		direction.x += UnityEngine.Random.Range (-accuracyVary, accuracyVary);
		direction.y += UnityEngine.Random.Range (-accuracyVary, accuracyVary);
		direction.z += UnityEngine.Random.Range (-accuracyVary, accuracyVary);
		currentAccuracy -= accuracyDrop;
		if (currentAccuracy <= 0.0f)
			currentAccuracy = 0.0f;
		//
		Ray ray = new Ray (currentMuzzle.position, direction);
		RaycastHit hit;
		//
		//IMPACT ON HIT
		if (Physics.Raycast (ray, out hit, range / rangeRatio)) {
			// Warmup heat
			float damageeffect = damage * acc;//
			hit.collider.gameObject.SendMessage ("SilantroDamage", -damageeffect,SendMessageOptions.DontRequireReceiver);
			//
			if (hit.collider.tag == "Ground") {
				Instantiate(groundHit, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
			}
			//
		}
		///
		//
		if (useTrails) {
			GameObject trail = GameObject.Instantiate (bulletTrails, currentMuzzle.position, Quaternion.identity);
			trail.transform.forward = direction;
			if (trail.GetComponent < Rigidbody> ()) {
				trail.GetComponent<Rigidbody> ().velocity = trail.transform.forward * (muzzleVelocity/1.5f);
			}
		}
		//
		if (transform.root.GetComponent<Rigidbody> ()) {
			planeVelocity = transform.root.GetComponent<Rigidbody> ().velocity;
		}
		if (muzzleFlash != null) {
			GameObject flash = Instantiate (muzzleFlash, currentMuzzle.position, currentMuzzle.rotation);
			flash.transform.position = currentMuzzle.position;
			flash.transform.parent = currentMuzzle.transform;
		}
		//
		if(ejectShells){
		GameObject shellGO = Instantiate(bulletCase, shellEjectPoint.position, shellEjectPoint.rotation) as GameObject;
		shellGO.GetComponent<Rigidbody> ().velocity = planeVelocity;
		shellGO.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(shellSpitForce + UnityEngine.Random.Range(0, shellForceRandom), 0, 0), ForceMode.Impulse);
		shellGO.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(shellSpitTorqueX + UnityEngine.Random.Range(-shellTorqueRandom, shellTorqueRandom), shellSpitTorqueY + UnityEngine.Random.Range(-shellTorqueRandom, shellTorqueRandom), 0), ForceMode.Impulse);
		//
		}
		//
		GetComponent<AudioSource>().PlayOneShot(fireSound);
	}
	//
	void FixedUpdate()
	{
		//
		if (currentRPM <= 0f)
		{
			currentRPM = 0f;
		}
		//
		if (barrel)
		{
			if (rotationDirection == RotationDirection.CCW) {
				if (rotationAxis == RotationAxis.X) {
					barrel.Rotate (new Vector3 (currentRPM * Time.deltaTime, 0, 0));
				}
				if (rotationAxis == RotationAxis.Y) {
					barrel.Rotate (new Vector3 (0, currentRPM * Time.deltaTime, 0));
				}
				if (rotationAxis == RotationAxis.Z) {
					barrel.Rotate (new Vector3 (0, 0, currentRPM * Time.deltaTime));
				}
			}
			//
			if (rotationDirection == RotationDirection.CW) {
				if (rotationAxis == RotationAxis.X) {
					barrel.Rotate (new Vector3 (-1f *currentRPM * Time.deltaTime, 0, 0));
				}
				if (rotationAxis == RotationAxis.Y) {
					barrel.Rotate (new Vector3 (0, -1f *currentRPM * Time.deltaTime, 0));
				}
				if (rotationAxis == RotationAxis.Z) {
					barrel.Rotate (new Vector3 (0, 0, -1f *currentRPM * Time.deltaTime));
				}
			}
		}
	}

}
//
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroMinigun))]
public class MinigunEditor: Editor
{
	Color backgroundColor;
	Color silantroColor = Color.cyan;
	//

	public override void OnInspectorGUI()
	{
		backgroundColor = GUI.backgroundColor;
		//
		DrawDefaultInspector ();serializedObject.Update();
		//
		SilantroMinigun gun = (SilantroMinigun)target;
		//
		GUILayout.Space(10f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("System Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		//
		GUILayout.Space(3f);
		gun.damage = EditorGUILayout.FloatField ("Damage", gun.damage);
		GUILayout.Space(10f);
		gun.rateOfFire = EditorGUILayout.FloatField ("Rate Of Fire", gun.rateOfFire);
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Actual Rate", gun.actualRate.ToString ("0.00"));
		//GUILayout.Space(2f);
		EditorGUILayout.LabelField("Fire TImer",gun.fireTimer.ToString ("0.00"));
		//
		GUILayout.Space(10f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Accuracy Settings", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		gun.accuracy = EditorGUILayout.FloatField ("Accuracy", gun.accuracy);
		GUILayout.Space(2f);
		EditorGUILayout.LabelField ("Current Accuracy", gun.currentAccuracy.ToString ("0.00"));
		GUILayout.Space(2f);
		gun.advancedSettings = EditorGUILayout.Toggle ("Advanced Settings", gun.advancedSettings);
		if (gun.advancedSettings) {
			GUILayout.Space(3f);
			gun.accuracyDrop = EditorGUILayout.FloatField ("Drop Per Shot", gun.accuracyDrop);
			GUILayout.Space(3f);
			gun.accuracyRecover = EditorGUILayout.FloatField ("Recovery Per Shot", gun.accuracyRecover);
		}
		//
		GUILayout.Space(10f);
		gun.range = EditorGUILayout.FloatField ("Range", gun.range);
		//
		GUILayout.Space(3f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Eject Spent Cases", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		gun.ejectShells = EditorGUILayout.Toggle ("Eject Cases", gun.ejectShells);
		//
		GUILayout.Space(25f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Connections", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Barrels", MessageType.None);
		GUI.color = backgroundColor;
		//

		//gun.muzzleCount = EditorGUILayout.IntField("Barrels",gun.muzzleCount);
		GUILayout.Space(5f);
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.BeginVertical ();
		//
		SerializedProperty muzs = this.serializedObject.FindProperty("muzzles");
		GUIContent barrelLabel = new GUIContent ("Barrel Count");
		//
		EditorGUILayout.PropertyField (muzs.FindPropertyRelative ("Array.size"),barrelLabel);
		GUILayout.Space(5f);
		for (int i = 0; i < muzs.arraySize; i++) {
			GUIContent label = new GUIContent("Barrel " +(i+1).ToString ());
			EditorGUILayout.PropertyField (muzs.GetArrayElementAtIndex (i), label);
		}
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.EndVertical ();
		//

		//
		GUILayout.Space(10f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Revolver", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		gun.barrel = EditorGUILayout.ObjectField ("Revolver", gun.barrel, typeof(Transform), true) as Transform;
		GUILayout.Space(5f);
		gun.rotationAxis = (SilantroMinigun.RotationAxis)EditorGUILayout.EnumPopup("Rotation Axis",gun.rotationAxis);
		GUILayout.Space(3f);
		gun.rotationDirection = (SilantroMinigun.RotationDirection)EditorGUILayout.EnumPopup("Rotation Direction",gun.rotationDirection);
		//
		if (gun.ejectShells) {
			GUILayout.Space(5f);
			gun.shellEjectPoint = EditorGUILayout.ObjectField ("Shell Ejection Point", gun.barrel, typeof(Transform), true) as Transform;
		}
	
		GUILayout.Space(20f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Ammo Settings", MessageType.None);
		GUI.color = backgroundColor;
		//
		GUILayout.Space(3f);
		gun.unlimitedAmmo = EditorGUILayout.Toggle ("Infinite Ammo", gun.unlimitedAmmo);
		if (!gun.unlimitedAmmo) {
			GUILayout.Space(5f);
			gun.ammoCapacity = EditorGUILayout.IntField ("Capacity", gun.ammoCapacity);
			GUILayout.Space(3f);
			EditorGUILayout.LabelField ("Current Ammo", gun.currentAmmo.ToString ());
		}
		//
		GUILayout.Space(20f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Effects Configuration", MessageType.None);
		GUI.color = backgroundColor;
		//
		GUILayout.Space(3f);
		gun.muzzleFlash = EditorGUILayout.ObjectField ("Muzzle Flash", gun.muzzleFlash, typeof(GameObject), true) as GameObject;
		if (gun.ejectShells) {
			GUILayout.Space(3f);
			gun.bulletCase = EditorGUILayout.ObjectField ("Bullet Case", gun.bulletCase, typeof(GameObject), true) as GameObject;
		}
		//
		//
		GUILayout.Space(10f);
		gun.useTrails = EditorGUILayout.Toggle ("Visualize Trails", gun.useTrails);
		if(gun.useTrails){
		GUILayout.Space(3f);
		gun.bulletTrails = EditorGUILayout.ObjectField ("Bullet Trail", gun.bulletTrails, typeof(GameObject), true) as GameObject;
		GUILayout.Space(3f);
		gun.muzzleVelocity = EditorGUILayout.FloatField ("Muzzle Velocity", gun.muzzleVelocity);
		//
		}
		//
		GUILayout.Space(10f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Impact Effects", MessageType.None);
		GUI.color = backgroundColor;
		//
		GUILayout.Space(3f);
		//
		gun.groundHit = EditorGUILayout.ObjectField ("Ground Hit", gun.groundHit, typeof(GameObject), true) as GameObject;
		GUILayout.Space(3f);
		gun.metalHit = EditorGUILayout.ObjectField ("Metal Hit", gun.metalHit, typeof(GameObject), true) as GameObject;
		GUILayout.Space(3f);
		gun.woodHit = EditorGUILayout.ObjectField ("Wood Hit", gun.woodHit, typeof(GameObject), true) as GameObject;
		//
		//
		//
		GUILayout.Space(20f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Sound Configuration", MessageType.None);
		GUI.color = backgroundColor;
		//
		GUILayout.Space(3f);
		gun.fireSound = EditorGUILayout.ObjectField ("Fire Sound", gun.fireSound, typeof(AudioClip), true) as AudioClip;
		GUILayout.Space(3f);
		gun.soundVolume = EditorGUILayout.FloatField ("Sound Volume", gun.soundVolume);
		//
		//
		if (GUI.changed) {
			EditorUtility.SetDirty (gun);
			EditorSceneManager.MarkSceneDirty (gun.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
	}
}
#endif