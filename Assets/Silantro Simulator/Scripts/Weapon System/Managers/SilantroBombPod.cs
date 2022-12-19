using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class SilantroBombPod : MonoBehaviour {

	[HideInInspector]public SilantroBomb[] availableBombs;
	[HideInInspector]public List<SilantroBomb> unguidedBombs;
	[HideInInspector]public List<SilantroBomb> guidedBombs;
	[HideInInspector]public Transform target;
	[HideInInspector]public float totalWeight;
	[HideInInspector]public SilantroControls controlBoard;
	[HideInInspector]public string bombDrop;
	[HideInInspector]public bool isOnline = true;
	[HideInInspector]public bool isControllable = true;
	//
	[HideInInspector]public float dropInterval = 1f;
	[HideInInspector]public float minimumDropHeight = 200f;
	[HideInInspector]public Transform Aircraft;
	// Use this for initialization
	void Start () {
		bombDrop = controlBoard.BombDrop;
		CountBombs ();
	}
	//
	//
	public void CountBombs()
	{
		availableBombs = new SilantroBomb[0];
		availableBombs = GetComponentsInChildren<SilantroBomb> ();
		//
		totalWeight = 0;
		//
		guidedBombs = new List<SilantroBomb>();
		unguidedBombs = new List<SilantroBomb> ();
		foreach (SilantroBomb bomb in availableBombs) {
			//
			if(bomb != null){

				totalWeight += bomb.weight;
				//
				if (bomb.bombType == SilantroBomb.BombType.Guided) {
					guidedBombs.Add (bomb);
				}
				if (bomb.bombType == SilantroBomb.BombType.Unguided) {
					unguidedBombs.Add (bomb);
				}
			}
			//
		}
	}
	// Update is called once per frame
	void Update () {
		if (isControllable && isOnline) {
			if (Input.GetButtonDown (bombDrop) && (Aircraft.position.y * 3.286f) > minimumDropHeight) {
				StartBombDrop ();
			}
		}
	}
	//
	void StartBombDrop()
	{
		if (availableBombs.Length > 0) {
			int index = Random.Range (0, availableBombs.Length);
			if (availableBombs [index] != null) {
				availableBombs [index].DropBomb ();
			}
			StartCoroutine (WaitForNextDrop ());
		}
	}
	//
	IEnumerator WaitForNextDrop()
	{
		yield return new WaitForSeconds (dropInterval);
		StartBombDrop ();
	}
}
//
//
//
#if UNITY_EDITOR
[CustomEditor(typeof(SilantroBombPod))]
[CanEditMultipleObjects]
public class BombPodEditor: Editor
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
		SilantroBombPod pod = (SilantroBombPod)target;
		//
		GUILayout.Space(3f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Available Bombs", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.BeginVertical ();
		//
		SerializedProperty engine = this.serializedObject.FindProperty("availableBombs");
		//EditorGUILayout.PropertyField (engine.FindPropertyRelative ("Array.size"),engineLabel);
		GUILayout.Space(5f);
		for (int i = 0; i < engine.arraySize; i++) {
			GUIContent label = new GUIContent("Bomb " +(i+1).ToString ());
			EditorGUILayout.PropertyField (engine.GetArrayElementAtIndex (i), label);
		}
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.EndVertical ();
		//
		GUILayout.Space(3f);
		EditorGUILayout.LabelField ("Total Weight", pod.totalWeight.ToString ("0.0"));

		//
		GUILayout.Space(10f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Drop Settings", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space(3f);
		pod.dropInterval = EditorGUILayout.FloatField("Drop Interval",pod.dropInterval);
		GUILayout.Space(3f);
		pod.minimumDropHeight = EditorGUILayout.FloatField ("Minimum Drop Height", pod.minimumDropHeight);
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