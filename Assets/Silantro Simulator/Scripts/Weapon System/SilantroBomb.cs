
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class SilantroBomb : MonoBehaviour {

	public enum BombType
	{
		Guided,
		Unguided
	}
	[HideInInspector]public BombType bombType = BombType.Unguided;
	//
	public enum TriggerMechanism
	{
		Proximity,
		ImpactForce
	}
	[HideInInspector]public TriggerMechanism triggerMechanism = TriggerMechanism.ImpactForce;
	[HideInInspector]public float detonationDistance;
	//
	[HideInInspector]public string name;
	[HideInInspector]public float weight = 900;
	[HideInInspector]public float length = 2f;
	[HideInInspector]public float diameter = 0.1f;
	//
	[HideInInspector]public float CDCoefficient;
	[HideInInspector]public float surfaceArea;
	[HideInInspector]public float percentageSkinning = 70f;
	[HideInInspector]float airDensity;
	[HideInInspector]float dragForce;
	[HideInInspector]public float ambientTemperature;
	[HideInInspector]public float ambientPressure;
	//
	[HideInInspector]public SilantroWarhead filling;
	[HideInInspector]public float fillingWeight = 10f;
	//
	//
	[HideInInspector]public bool activated = false;
	[HideInInspector]public bool falling;
	//
	private Rigidbody bombBody;
	[HideInInspector]public SilantroBombPod manager;
	//
	[HideInInspector]public float speed;
	[HideInInspector]public float distanceToTarget;
	[HideInInspector]public float fallTime;
	//
	[HideInInspector]public float startingHealth = 100.0f;
	[HideInInspector]public float currentHealth;	

	//
	Vector3 dropVelocity;
	Transform parentAircraft;
	// Use this for initialization
	void Start () {
		//Set Initial Parameters
		parentAircraft = transform.root;
		manager = GetComponentInParent<SilantroBombPod> ();
		bombBody = GetComponent<Rigidbody> ();
		bombBody.mass = weight;
		//Hold Bomb in Position++
		bombBody.isKinematic = true;
		///
		//Calcualte surface Area
		float radius = diameter/2;
		float a1 = 3.142f*radius*radius;
		//
		float error = UnityEngine.Random.Range((percentageSkinning-4.5f),(percentageSkinning +5f));
		surfaceArea = (a1 )*(error/100f);
		//
		currentHealth = startingHealth;
	}
	//
	//HIT SYSTEM
	public void SilantroDamage(float amount)
	{
		currentHealth += amount;

		// If the health runs out, then Die.
		if (currentHealth < 0)
		{
			currentHealth = 0;
		}

		//Die Procedure
		if (currentHealth == 0 )
			Explode(this.transform.position);
	}
	//
	//
	void Update()
	{
		if (activated) {
			//
			speed = bombBody.velocity.magnitude;
			distanceToTarget = bombBody.transform.position.y * 3.28f;
			//
			//Calculate Fall time
			if (speed > 0 && falling) {
				fallTime += Time.deltaTime;
			}
		}
		//
		if(falling){
			//Rotate Bomb Towards falling direction
			bombBody.transform.forward = Vector3.Slerp (bombBody.transform.forward, bombBody.velocity.normalized, Time.deltaTime);
			//Calculate current density at this height
			CalculateFlightData(transform.position.y);
			//CALCULATE REYNOLDS NUMBER
			float viscocity = (0.00000009f*ambientTemperature)+0.00001f;
			float reynolds = (airDensity * speed * length) / viscocity;
			//Calculate Drag coefficient;
			if (reynolds < 1000000) {
				CDCoefficient = (0.1f * Mathf.Log10 (reynolds)) - 0.4f;
			} else {
				CDCoefficient = 0.19f - (80000 / reynolds);
			}
			//Calculate drag force
			dragForce = (0.5f * airDensity * CDCoefficient * surfaceArea * speed*speed);
		}
	}
	//
	//APPLY PHYSICS
	void FixedUpdate()
	{
		Vector3 force = -bombBody.velocity.normalized * dragForce;
		bombBody.AddForce (force);
	}
	//
	void CalculateFlightData(float altitude)
	{
		float altiKmeter = altitude / 3280.84f;
		//
		float a =  0.0025f * Mathf.Pow(altiKmeter,2f);
		float b = 0.106f * altiKmeter;
		//
		airDensity = a -b +1.2147f;
		//Calculate Temperature
		float a1 = 0.000000003f * altitude * altitude;
		float a2 = 0.0021f * altitude;
		ambientTemperature = a1-a2+15.443f;//
		//Calculate Pressure
		float a3 = 0.0000004f * altitude * altitude;
		float b3 = (0.0351f*altitude);
		ambientPressure =( a3 - b3 + 1009.6f)/10f;
	}
	///
	//
	public void DropBomb()
	{
		//
		if (manager) {
			manager.CountBombs ();
		}
		//
		dropVelocity = bombBody.transform.root.gameObject.GetComponent<Rigidbody> ().velocity;
		// Drop
		bombBody.isKinematic = false;
		bombBody.transform.parent = null;
		bombBody.velocity = dropVelocity;
		activated = true;
		falling = true;
	}
	//
	//DAMAGE
	void OnCollisionEnter(Collision col)
	{
		if (col.transform.root != parentAircraft) {
			if (activated && triggerMechanism == TriggerMechanism.ImpactForce) {
				StartCoroutine (WaitForMomentumShead ());
			}
		}
	}
	//
	IEnumerator WaitForMomentumShead()
	{
		yield return new WaitForSeconds (0.07f);
		Explode (transform.position);
	}
	//EXPLODE
	public void Explode(Vector3 position)
	{
		//
		//REMOVE BOMB FROM AVAILABLE BOMBS?? DOUBLE CHECK
		if (manager != null) {
			manager.CountBombs ();
		}
		//
		if (filling) {
			filling.Explode (position);
		}
		//
		//float impactForce = weight *(speed/fallTime);

		//Delete this gameObject
		Destroy (gameObject);
	}
}
//
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroBomb))]
public class BombEditor: Editor
{
	Color backgroundColor;
	Color silantroColor = Color.cyan;
	//

	public override void OnInspectorGUI()
	{
		backgroundColor = GUI.backgroundColor;
		//
		DrawDefaultInspector ();serializedObject.Update ();
		//
		SilantroBomb bomb = (SilantroBomb)target;
		//
		GUILayout.Space(3f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Setup", MessageType.None);
		GUI.color = backgroundColor;
		//
		GUILayout.Space(3f);
		bomb.name = EditorGUILayout.TextField ("Identifier", bomb.name);
		GUILayout.Space(3f);
		bomb.bombType = (SilantroBomb.BombType)EditorGUILayout.EnumPopup ("Bomb Type", bomb.bombType);
		//
		GUILayout.Space(10f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Bomb Dimensions", MessageType.None);
		GUI.color = backgroundColor;
		//
		GUILayout.Space(5f);
		bomb.weight = EditorGUILayout.FloatField ("Weight", bomb.weight);
		GUILayout.Space(3f);
		bomb.length = EditorGUILayout.FloatField ("Length", bomb.length);
		GUILayout.Space(3f);
		bomb.diameter = EditorGUILayout.FloatField ("Diameter", bomb.diameter);
		GUILayout.Space(3f);
		bomb.percentageSkinning = EditorGUILayout.Slider ("Skinning",bomb.percentageSkinning, 0f, 100f);
		//
		GUILayout.Space(20f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Filling Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		bomb.filling = EditorGUILayout.ObjectField ("Filling", bomb.filling, typeof(SilantroWarhead), true) as SilantroWarhead;
		GUILayout.Space(3f);
		bomb.fillingWeight = EditorGUILayout.FloatField ("Filling Weight", bomb.fillingWeight);
		GUILayout.Space(5f);
		bomb.triggerMechanism = (SilantroBomb.TriggerMechanism)EditorGUILayout.EnumPopup ("Fuze", bomb.triggerMechanism);
		if (bomb.triggerMechanism == SilantroBomb.TriggerMechanism.Proximity) {
			GUILayout.Space(3f);
			bomb.detonationDistance = EditorGUILayout.FloatField ("Detonation Distance", bomb.detonationDistance);
		}
		//
		GUILayout.Space(10f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Health Settings", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		bomb.startingHealth = EditorGUILayout.FloatField ("Starting Health", bomb.startingHealth);
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Current Health", bomb.currentHealth.ToString ("0.0"));
		//
		GUILayout.Space(20f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Display", MessageType.None);
		GUI.color = backgroundColor;
		EditorGUILayout.LabelField ("Current Speed", bomb.speed.ToString ("0.0") + " m/s");
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Distance To Target", (bomb.distanceToTarget/3.286f).ToString ("0.0") + " m");
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Fall Time", bomb.fallTime.ToString ("0.0") + " s");
		//
		//
		if (GUI.changed) {
			EditorUtility.SetDirty (bomb);
			EditorSceneManager.MarkSceneDirty (bomb.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
	}
}
#endif