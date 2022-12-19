using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
//
public class SilantroLight : MonoBehaviour {
	//
	float blinkRate = 0.05f;
	float timer;
	//
	public enum LightType
	{
		Navigation,
		Strobe,
		Beacon,
		Landing,
		Airport
	}
	//
	public enum Position
	{
		Left,
		Right,
	}
	[HideInInspector]public Position location = Position.Left;
	//
	[HideInInspector]public LightType lightType = LightType.Navigation;
	[HideInInspector]public GameObject bulb;
	//
	public enum CurrentState
	{
		On,
		Off
	}
	[HideInInspector]public CurrentState state = CurrentState.On;
	[HideInInspector]public bool active = true;
	[HideInInspector]public SilantroSapphire weather;
	//
	void Start()
	{
		GameObject lighter = GameObject.FindGameObjectWithTag ("Weather");
		if (lighter != null) {
			weather = lighter.GetComponent<SilantroSapphire> ();
		}
		//
		//
		if (lightType != LightType.Airport) {
			state = CurrentState.Off;
			bulb.SetActive (false);
			active = false;
			//
		}
		//
		if (lightType == LightType.Strobe) {
			blinkRate = 1f;
		}
		if (lightType == LightType.Beacon) {
			blinkRate = 0.75f;
		}
		if (lightType == LightType.Navigation) {
			if (location == Position.Left) {
				blinkRate = 0.45f;
			}
			if (location == Position.Right) {
				blinkRate = 0.8f;
			}
		}
	}
	//SWITCH OFF THE LIGHT
	public void TurnOff()
	{	//
		if(bulb != null){
		bulb.SetActive (false);
		state = CurrentState.Off;
		}
	}
	//SWITCH ON THE LIGHT
	public void TurnOn()
	{
		if (bulb != null) {
			bulb.SetActive (true);
			state = CurrentState.On;
		}
	}
	//
	//
	void Update()
	{
		if (state == CurrentState.On) {
			if (lightType == LightType.Navigation || lightType == LightType.Strobe || lightType == LightType.Beacon) {
				timer += Time.deltaTime;
				//
				if (timer >= blinkRate) {
					Blink ();
				}
				//
				if (active) {
					bulb.SetActive (true);
				} else {
					bulb.SetActive (false);
				}
			}
		}
		if (lightType == LightType.Airport) {
			//if(
		}
	}
	//
	void Blink()
	{
		timer = 0f;
		active = !active;
	}
}
//
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroLight))]
[CanEditMultipleObjects]
public class LightEditor: Editor
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
		SilantroLight light = (SilantroLight)target;
		//
		GUILayout.Space(3f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Bulb", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		light.bulb = EditorGUILayout.ObjectField ("Light Bulb", light.bulb, typeof(GameObject), true) as GameObject;
		//
		GUILayout.Space(3f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		light.lightType = (SilantroLight.LightType)EditorGUILayout.EnumPopup("Type",light.lightType);
		//
		if (light.lightType == SilantroLight.LightType.Navigation) {
			GUILayout.Space(3f);
			light.location = (SilantroLight.Position)EditorGUILayout.EnumPopup("Location",light.location);
		}
		//
		GUILayout.Space(3f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		light.state = (SilantroLight.CurrentState)EditorGUILayout.EnumPopup("State",light.state);
		//
		//
		if (GUI.changed) {
			EditorUtility.SetDirty (light);
			EditorSceneManager.MarkSceneDirty (light.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();

	}
}
#endif