using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
//

public class SilantroRadar : MonoBehaviour {
	//
	public enum RadarType
	{
		Military,
		Civilian
	}
	//
	//RADAR CONFIGURAATION
	[HideInInspector]public float range = 1000f;
	[HideInInspector]public float pingRate;
	[HideInInspector]public float actualPingRate;
	[HideInInspector]public Collider[] visibleObjects;
	[HideInInspector]public List<GameObject> processedObjects;
	float closest;[HideInInspector]public float pingTime;
	//
	[HideInInspector]public string[] targetDeclaration;
	//
	[HideInInspector]public RadarType radarType = RadarType.Civilian;
	[HideInInspector]public float size = 250f;

	[HideInInspector]public float Transparency = 0.9f;
	[HideInInspector]public Texture background;
	[HideInInspector]public Texture compass;
	[HideInInspector]public float scale;
	[HideInInspector]public bool rotate;
	[HideInInspector]public GameObject Aircraft;
	[HideInInspector]public bool active;
	[HideInInspector]public Color generalColor = Color.white;
	[HideInInspector]private Vector2 radarPosition;
	[HideInInspector]public Transform radarIndicator;
	//
	void Start()
	{
		radarPosition = Vector2.zero;
		//
		actualPingRate = (1f/pingRate);
		pingTime = 0.0f;
		//
		scale = range/100f;
	}
	// Update is called once per frame
	void Update () {
		//MAKE SURE AIRCRAFT IS NOT NUll
		if (Aircraft == null) {
			Aircraft = this.gameObject;
		}
		//
		//PREVENT NEGATIVE SCALE
		if (scale <= 0) {
			scale = 1;
		}
		//
		//SEND OUT PING
		pingTime += Time.deltaTime;
		//
		if (pingTime >= actualPingRate) {
			Ping ();
		}
	}
	//
	void Ping()
	{
		pingTime = 0.0f;
		//
		visibleObjects = Physics.OverlapSphere (transform.position, range);
		//FILTER OUT OBJECTS
		processedObjects = new List<GameObject>();
		foreach (Collider col in visibleObjects) {
			if (col.gameObject.GetComponent<SilantroMarker> ()) {
				processedObjects.Add (col.gameObject);
			}
		}
	}
	//
	//CONVERT VECTOR3 position to VECTOR2
	Vector2 GetPosition(Vector3 position)
	{
		Vector2 resultant = Vector2.zero;
		//
		if (Aircraft) {
			resultant.x = radarPosition.x + (((position.x - Aircraft.transform.position.x) + (size * scale) / 2) / scale);
			resultant.y = radarPosition.y + ((-(position.z -Aircraft.transform.position.z)+(size * scale) / 2) / scale);
		}
		return resultant;
	}
	//
	//DRAW TARGET ON RADAR SCREEN
	void DrawTargetPosition(List<GameObject> currentTargets, Texture2D radarTexture)
	{
		if (Aircraft != null) {
			//
			for (int a = 0; a < currentTargets.Count; a++) {
				if (currentTargets [a] != null && Vector3.Distance (Aircraft.transform.position, currentTargets [a].transform.position) <= (range)) {
					//
					//CONVERT THE TARGET POSITION TO A 2D DISTANCE
					Vector2 targetPosition = GetPosition(currentTargets[a].transform.position);
					//
						float displayScale = scale;
						if (displayScale < 1) {
							displayScale = 1;
						}
						//DRAW ACTUAL TEXTURE
						GUI.DrawTexture(new Rect(targetPosition.x-(radarTexture.width/displayScale)/2f,targetPosition.y-(radarTexture.height/displayScale)/2,radarTexture.width/displayScale,radarTexture.height/displayScale),radarTexture);
						//
					//}
				}
			}
		}
	}
	//
	void OnGUI()
	{
		if (!active)
			return;
		//
		GUI.color = new Color (generalColor.r,generalColor.g,generalColor.b,Transparency);
		//
		//ROTATE MAP 
		if (rotate) {
			GUIUtility.RotateAroundPivot (-(this.transform.eulerAngles.y), radarPosition + new Vector2 (size / 2f, size / 2f)); 
		}
		//
		//Debug.Log(processedObjects);
		//DRAW TARGETS
		if (processedObjects.Count > 0) {
			//
			for (int i = 0; i < processedObjects.Count; i++) {
				//
				for (int o = 0; o < targetDeclaration.Length; o++) {
					//
					List<GameObject> targets = new List<GameObject>();
					//
					if(processedObjects[i].GetComponent<SilantroMarker>().silantoTag == targetDeclaration[o] && !targets.Contains(processedObjects[i]))
						{
						targets.Add (processedObjects [i]);
						//
						DrawTargetPosition(targets,processedObjects[i].GetComponent<SilantroMarker>().silantroTexture);
					}
				}
			}
		}
		//
		//DRAW RADAR MAP
		if (background)
			GUI.DrawTexture (new Rect (radarPosition.x, radarPosition.y, size, size), background);
		GUIUtility.RotateAroundPivot ((this.transform.eulerAngles.y), radarPosition + new Vector2 (size / 2f, size / 2f)); 
		if (compass)
			GUI.DrawTexture (new Rect (radarPosition.x + (size / 2f) - (compass.width / 2f), radarPosition.y + (size / 2f) - (compass.height / 2f), compass.width, compass.height), compass);
		
	}
}
//
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroRadar))]
public class RadarEditor: Editor
{
	Color backgroundColor;
	Color silantroColor = Color.blue;
	//
	public override void OnInspectorGUI ()
	{
		backgroundColor = GUI.backgroundColor;
		//
		DrawDefaultInspector ();
		SilantroRadar radar = (SilantroRadar)target;
		//
		serializedObject.Update ();
		//
		GUILayout.Space (2f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Radar Setup", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space (3f);
		radar.Aircraft = EditorGUILayout.ObjectField ("Aircraft", radar.Aircraft, typeof(GameObject), true) as GameObject;
		GUILayout.Space (5f);
		radar.radarType = (SilantroRadar.RadarType)EditorGUILayout.EnumPopup("Type",radar.radarType);
		//
		if (radar.radarType == SilantroRadar.RadarType.Civilian) {
			//
			GUILayout.Space (20f);
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("Radar Configuration", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (3f);
			radar.range = EditorGUILayout.FloatField ("Effective Range", radar.range);
			GUILayout.Space (3f);
			radar.pingRate = EditorGUILayout.FloatField ("Ping Rate", radar.pingRate);
			GUILayout.Space (5f);
			EditorGUILayout.LabelField ("Last Ping",(radar.pingTime).ToString ("0.0") + " s");
		//
			GUILayout.Space (10f);
			GUI.color = Color.white;
			EditorGUILayout.HelpBox ("Object Identification", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (5f);
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.BeginVertical ();
			//
			GUIContent partlabel = new GUIContent ("Identifiable Objects");
			SerializedProperty part = this.serializedObject.FindProperty ("targetDeclaration");
			EditorGUILayout.PropertyField (part.FindPropertyRelative ("Array.size"), partlabel);
			GUILayout.Space (5f);
			for (int i = 0; i < part.arraySize; i++) {
				GUIContent label = new GUIContent ("Declaration " + (i + 1).ToString ());
				EditorGUILayout.PropertyField (part.GetArrayElementAtIndex (i), label);
			}
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.EndVertical ();
			//
			GUILayout.Space (5f);
			GUI.color = Color.white;
			EditorGUILayout.HelpBox ("Radar Return", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (5f);
			EditorGUILayout.LabelField ("Visible Objects",(radar.visibleObjects.Length).ToString ());
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Identified Objects",(radar.processedObjects.Count).ToString ());
			//
			GUILayout.Space (20f);
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("Display Configuration", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (3f);
			radar.size = EditorGUILayout.FloatField ("Radar Size", radar.size);
			GUILayout.Space (3f);
			radar.scale = EditorGUILayout.FloatField ("Detection Scale", radar.scale);
			//
			GUILayout.Space (5f);
			GUI.color = Color.white;
			EditorGUILayout.HelpBox ("Texture Settings", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (3f);
			radar.background = EditorGUILayout.ObjectField ("Radar Background", radar.background, typeof(Texture), true) as Texture;
			GUILayout.Space (5f);
			radar.compass = EditorGUILayout.ObjectField ("Compass", radar.compass, typeof(Texture), true) as Texture;
			GUILayout.Space (15f);
			radar.Transparency = EditorGUILayout.Slider ("Transparency", radar.Transparency, 0f, 1f);
			GUILayout.Space (3f);
			radar.generalColor = EditorGUILayout.ColorField ("Radar Color", radar.generalColor);
			//
			GUILayout.Space (5f);
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("Extra Settings", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (3f);
			radar.active = EditorGUILayout.Toggle ("Engaged", radar.active);
			GUILayout.Space (3f);
			radar.rotate = EditorGUILayout.Toggle ("Pivolted", radar.rotate);
		} else {
			GUILayout.Space (20f);
			EditorGUILayout.LabelField ("Coming Soon...");
		}
		//
		//
		//
		if (GUI.changed) {
			EditorUtility.SetDirty (radar);
			EditorSceneManager.MarkSceneDirty (radar.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
	}
}
#endif