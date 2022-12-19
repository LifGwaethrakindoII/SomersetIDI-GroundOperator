using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Supercargo
{
[CustomEditor(typeof(ApplicationData))]
public class ApplicationDataEditor : Editor
{
	private ApplicationData data;      /// <summary>Main Application's Data.</summary>
    private User user;

    private void OnEnable()
    {
        data = target as ApplicationData;
        SceneView.onSceneGUIDelegate += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
    }

	public override void OnInspectorGUI()
    {
        if(GUILayout.Button(!data.toggle ? "Hide All Classifications" : "Show All Classifications")) data.toggle = !data.toggle;

    	if(!data.toggle) DrawDefaultInspector();
        DrawStartingPointsConfigurations();
        DrawGizmosConfigurations();
        if(data.toggle)
        {
            DrawIndexPagingconfigurations();
            DrawCurrentPatternClassification();
        }
        DrawXMLConfigurations();

        if(GUI.changed) EditorUtility.SetDirty(data);
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if(user != null)
        {
            DrawStartingPointGizmos();
            if(data.classifications != null && data.classifications.Length > 0 )
            {
                DrawWaypointsGizmos();
            }

            SceneView.RepaintAll();
        }
    }

    private void DrawStartingPointsConfigurations()
    {
        EditorExtensions.Spaces(2);
        data.startingPoint.position = EditorGUILayout.Vector3Field("Position: ", data.startingPoint.position);
        data.startingPoint.eulerRotation = EditorGUILayout.Vector3Field("Euler Rotation: ", data.startingPoint.eulerRotation);
        data.startingPoint.scale = Vector3.one;
        data.startingPoint.rotation = Quaternion.Euler(data.startingPoint.eulerRotation);
    }

    private void DrawGizmosConfigurations()
    {
        EditorExtensions.Spaces(4);
        data.color = EditorGUILayout.ColorField("Gizmos Color: ", data.color);
    }

    private void DrawIndexPagingconfigurations()
    {
        if(user == null) user = Object.FindObjectOfType<User>();
        else if(data.classifications != null && data.classifications.Length > 0 )
        {
            EditorExtensions.Spaces(4);
            GUILayout.Label("Pattern Classification Paging: ");
            EditorExtensions.DrawArraySizeConfiguration(ref data._classifications, "Pattern Classifications: ");
            GUILayout.Label("Current Index: " + data.index);
            GUILayout.Label("Current Command: " + data.classifications[data.index].command.ToString());
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("-")) data.index = (data.index - 1 > -1) ? data.index - 1 : 0; 
            if(GUILayout.Button("+")) data.index = (data.index + 1 < (data.classifications.Length - 1)) ? data.index + 1 : (data.classifications.Length - 1);
            EditorGUILayout.EndHorizontal();
        }
    }

    private void DrawCurrentPatternClassification()
    {
        PatternClassification classification = data.classifications[data.index];

        EditorExtensions.Spaces(4);
        classification.command = (Command)EditorGUILayout.EnumPopup("Command: ", classification.command);
        EditorGUILayout.Space();
        EditorExtensions.DrawArraySizeConfiguration(ref classification.waypoints, "Waypoints: ");
        for(int i = 0; i < classification.waypoints.Length; i++)
        {

            UserPatternWaypoints userWaypoints = classification.waypoints[i];
            PatternWaypoint leftWaypoint = userWaypoints.leftWaypoint;
            PatternWaypoint rightWaypoint = userWaypoints.rightWaypoint;

            EditorExtensions.DrawHorizontalLine();
            GUILayout.Label("Waypoint Pair " + i + ": ");
            EditorGUILayout.Space();
            GUILayout.Label("Left: ");
            leftWaypoint.offsetPoint = EditorGUILayout.Vector3Field("Offset Point: ", leftWaypoint.offsetPoint);
            leftWaypoint.toleranceRadius = EditorGUILayout.FloatField("Tolerance Radius: ", leftWaypoint.toleranceRadius);
            classification.waypoints[i].leftWaypoint = leftWaypoint;
            EditorGUILayout.Space();
            GUILayout.Label("Right: ");
            rightWaypoint.offsetPoint = EditorGUILayout.Vector3Field("Offset Point: ", rightWaypoint.offsetPoint);
            rightWaypoint.toleranceRadius = EditorGUILayout.FloatField("Tolerance Radius: ", rightWaypoint.toleranceRadius);
            classification.waypoints[i].rightWaypoint = rightWaypoint;
            if(i < (classification.waypoints.Length - 1))EditorExtensions.Spaces(2);
            else EditorExtensions.DrawHorizontalLine();   
        }

        data.classifications[data.index] = classification;
    }

    private void DrawXMLConfigurations()
    {
        EditorExtensions.Spaces(4);
        GUILayout.Label("XML Serialization: ");
        data.dataName = EditorGUILayout.TextField("XML Name: ", data.dataName);
        EditorGUILayout.Space();
        if(GUILayout.Button("Serialize"))
        {
            data.SerializeData(data.dataName);
            UnityEditor.AssetDatabase.Refresh();
        }
        if(GUILayout.Button("Deserialize")) data.DeserializeData(data.dataName);
        if(Application.isPlaying && GUILayout.Button("Update Data")) data.UpdateData();
    }

    private void DrawStartingPointGizmos()
    {
        TransformData transformData = data.startingPoint;
        float length = 5.0f;

        Handles.color = Color.red;
        Handles.DrawLine(transformData.position, transformData.position + transformData.rotation * (Vector3.right * length));
        Handles.color = Color.green;
        Handles.DrawLine(transformData.position, transformData.position + transformData.rotation * (Vector3.up * length));
        Handles.color = Color.blue;
        Handles.DrawLine(transformData.position, transformData.position + transformData.rotation * (Vector3.forward * length));
    }

    private void DrawWaypointsGizmos()
    {
        if(user != null)
        {
            PatternClassification classification = data.classifications[data.index];

            Handles.color = data.color;
            Handles.Label(user.eye.position, classification.command.ToString());
            
            if(data.classifications != null && data.classifications.Length > 0)
            {
                if(classification.waypoints != null)
                for(int i = 0; i < classification.waypoints.Length; i++)
                {
                    UserPatternWaypoints waypoint = classification.waypoints[i];
                    UserPatternWaypoints previousWaypoint = i > 0 ? classification.waypoints[i - 1] : default(UserPatternWaypoints);
                    Vector3 leftPoint = user.eye.TransformPoint(waypoint.leftWaypoint.offsetPoint);
                    Vector3 rightPoint = user.eye.TransformPoint(waypoint.rightWaypoint.offsetPoint);
                    Quaternion leftRotation = Quaternion.LookRotation(Camera.current.transform.position - leftPoint);
                    Quaternion rightRotation = Quaternion.LookRotation(Camera.current.transform.position - rightPoint);

                    Handles.Label(leftPoint, "Left " + i.ToString());
                    Handles.Label(rightPoint, "Right " + i.ToString());
                    Handles.CircleCap(0, leftPoint, leftRotation, waypoint.leftWaypoint.toleranceRadius);
                    Handles.CircleCap(0, rightPoint, rightRotation, waypoint.rightWaypoint.toleranceRadius);
                    Handles.DrawLine(i == 0 ? user.eye.transform.position : user.eye.TransformPoint(previousWaypoint.leftWaypoint.offsetPoint), leftPoint);
                    Handles.DrawLine(i == 0 ? user.eye.transform.position : user.eye.TransformPoint(previousWaypoint.rightWaypoint.offsetPoint), rightPoint);
                }
            }
        }
    }
}
}