using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
//
public class SilantroRocketPod : MonoBehaviour {

	[HideInInspector]public SilantroRocket[] availableRockets;
	[HideInInspector]public float totalWeight;
	[HideInInspector]public float minimumLaunchHeight = 100f;
	[HideInInspector]public SilantroControls controlBoard;
	[HideInInspector]public string rocketFire;
	[HideInInspector]public bool isControllable = true;
	[HideInInspector]public bool isOnline = true;
	//
	[HideInInspector]public float rateOfFire = 5;
	[HideInInspector]public float actualRate;
	[HideInInspector]public float fireTimer;
	//
	[HideInInspector]public AudioClip fireSound;
	[HideInInspector]public float soundVolume = 0.75f;
	AudioSource rocketSound;
	//
	bool canFire = true;
	// Use this for initialization
	void Start () {
		//
		GameObject speaker = new GameObject("Launch Sound Point");
		speaker.transform.parent = this.transform;
		speaker.transform.localPosition = new Vector3 (0, 0, 0);
		rocketSound = speaker.AddComponent<AudioSource> ();
		//
		rocketSound.loop = false;
		rocketSound.dopplerLevel = 0f;
		rocketSound.spatialBlend = 1f;
		rocketSound.rolloffMode = AudioRolloffMode.Custom;
		rocketSound.maxDistance = 300f;
		rocketSound.volume = soundVolume;
		//
		if (rateOfFire != 0) {
			actualRate = 1.0f / rateOfFire;
		} else {
			actualRate = 0.01f;
		}
		fireTimer = 0.0f;
		//
		CountRockets ();
	}
	
	public void CountRockets()
	{
		availableRockets = new SilantroRocket[0];
		availableRockets = GetComponentsInChildren<SilantroRocket> ();
		//
		totalWeight = 0;
		foreach (SilantroRocket rocket in availableRockets) {
			//
			if (rocket != null) {
				totalWeight += rocket.weight;
				//
			}
		}
		if (availableRockets.Length <= 0) {
			canFire = false;
		}
	}
	//
	void Update()
	{
		fireTimer += Time.deltaTime;
		//
		//
		if (isControllable && isOnline) {
			if (Input.GetButton (rocketFire)&& (fireTimer >= actualRate) && canFire) {
				Fire ();
			}
		}
	}
	//
	void Fire()
	{
		//Reset
		fireTimer = 0.0f;
		//
		int index = Random.Range (0, availableRockets.Length);
		//
		if (availableRockets [index] != null) {
			availableRockets [index].Launch ();
		}
		CountRockets ();
		rocketSound.PlayOneShot(fireSound);
		//

	}
}
//
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroRocketPod))]
public class RocketPodEditor: Editor
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
		SilantroRocketPod pod = (SilantroRocketPod)target;
		//
		GUILayout.Space(10f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("System Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		pod.rateOfFire = EditorGUILayout.FloatField ("Launch Rate", pod.rateOfFire);
		GUILayout.Space(3f);
		pod.rocketFire = EditorGUILayout.TextField ("Launch Button", pod.rocketFire);
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Available Rockets", pod.availableRockets.Length.ToString ());
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Total Weight", pod.totalWeight.ToString ("0.0") + " kg");
		//
		GUILayout.Space(10f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Sound Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		pod.fireSound = EditorGUILayout.ObjectField ("Launch Sound", pod.fireSound, typeof(AudioClip), true) as AudioClip;
		GUILayout.Space(3f);
		pod.soundVolume = EditorGUILayout.FloatField ("Sound Volume", pod.soundVolume);
		//
		//
		if (GUI.changed) {
			EditorUtility.SetDirty (pod);
			EditorSceneManager.MarkSceneDirty (pod.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
	}
}
#endif