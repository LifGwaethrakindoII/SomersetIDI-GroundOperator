using System.Collections;
using UnityEditor;
using UnityEngine;
using System.IO;

public class SilantroControls : EditorWindow {
	Color backgroundColor;
	Color silantroColor = Color.cyan;
	//
	//
	[HideInInspector]public int toolbarTab;
	[HideInInspector]public string currentTab;

	//
	[MenuItem ("Oyedoyin/Miscellaneous/Setup Input/Quick Setup",false,301)]
	public static void InitializeQuick ()
	{
		string sourcePath = Application.dataPath + "/Silantro Simulator/Data/Input/Keyboard/SilantroKeyboard.dat";
		string destPath = Application.dataPath + "/../ProjectSettings/InputManager.asset";
		string defaultPath = Application.dataPath + "/Silantro Simulator/Data/Input/Default/SilantroDefault.dat";
		//
		if (!File.Exists (sourcePath)) {
			Debug.LogError ("Source File missing....Please Reimport file");
		}
		if (!File.Exists (destPath)) {
			Debug.LogError ("Destination input manager missing");
		}
		if (File.Exists (destPath) && File.Exists (sourcePath)) {
			File.Copy (defaultPath, destPath, true);
			File.Copy (sourcePath, destPath, true);
			AssetDatabase.Refresh ();
			Debug.Log ("Keyboard Input Setup Successful!");

		}
	}
	//
	[MenuItem ("Oyedoyin/Miscellaneous/Setup Input/Detailed Setup",false,301)]
	public static void InitializeAdvanced ()
	{
		EditorWindow.GetWindow<SilantroControls> ("Input Configuration");
	}
	//
	//
	//
	//
	[MenuItem ("Oyedoyin/Miscellaneous/Help/Forum Page",false,401)]
	public static void ForumPage ()
	{
		Application.OpenURL("https://forum.unity.com/threads/released-silantro-flight-simulator.522642/");
	}
	[MenuItem ("Oyedoyin/Miscellaneous/Help/Youtube Channel",false,452)]
	public static void YoutubeChannel ()
	{
		Application.OpenURL("https://www.youtube.com/channel/UCYXrhYRzY11qokg59RFg7gQ/videos");
	}
	[MenuItem ("Oyedoyin/Miscellaneous/Help/Update Log",false,501)]
	public static void UpdateLog ()
	{
		Application.OpenURL("http://unity3d.com/");
	}


	//
	string sourcePath ;
	string destPath;
	//
	//
	void OnGUI()
	{
		//
		backgroundColor = GUI.backgroundColor;
		//
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Control Buttons Setup", MessageType.Info);
		GUI.color = backgroundColor;
		GUILayout.Space (5f);
		toolbarTab = GUILayout.Toolbar(toolbarTab, new string[]{"Keyboard","Logitech X3D","PS4"});
		//
		switch (toolbarTab) {
		case 0:
			//
			currentTab = "Keyboard";
			break;
		case 1:
			//
			currentTab = "Logitech X3D";
			break;
		case 2:
			//
			currentTab = "PS4";
			break;
		}
		//
		switch (currentTab) {
		case "Keyboard":
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("Propulsion System", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (5f);
			EditorGUILayout.LabelField ("Start Engine", "F1");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Stop Engine", "F2");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Throttle Up", "Alpha 1");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Throttle Down", "Alpha 2");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Engage Afterburner", "F12");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Engage Reverse Thrust", "F11");
			GUILayout.Space (10f);
			//
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("Fuel System", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (5f);
			EditorGUILayout.LabelField ("Dump Fuel", "Alpha 5");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Refill Tank", "Alpha 6");
			//GUILayout.Space (3f);
			GUILayout.Space (10f);
			//
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("Actuators", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (5f);
			//GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Actuate Gear", "Alpha 0");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Parking Brake", "X");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Brake Lever", "Space Bar");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Actuate Spoiler", "G");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Actuate Slat", "K");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Extend Flap", "Alpha 3");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Retract Flap", "Alpha 4");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Light Switch", "V");
			GUILayout.Space (10f);
			//
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("STOVL System", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (5f);
			EditorGUILayout.LabelField ("Transition to VTOL", "Alpha 7");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Transition to STOL", "Alpha 8");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Transition to Normal", "Alpha 9");
			GUI.color = silantroColor;
			//
			GUILayout.Space (10f);
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("Weapon System", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (5f);
			EditorGUILayout.LabelField ("Bomb Drop Switch", "Back Space");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Minigun Fire", "Left Ctrl");
			//
			GUILayout.Space (10f);
			GUI.color = Color.yellow;
			EditorGUILayout.HelpBox ("Control Levers", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (5f);
			EditorGUILayout.LabelField ("Pitch", "Up and Down");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Roll", "Left and Right");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Rudder", "< and >");
			GUILayout.Space(5f);
			GUI.color = Color.yellow;
			EditorGUILayout.HelpBox ("Note: This will reset all input values (axis) to these values, PLEASE BACKUP YOUR CURRENT INPUT MANAGER", MessageType.Info);
			GUI.color = backgroundColor;
			//
			GUILayout.Space(20f);
			if (GUILayout.Button ("Configure Keyboard Control")) {
				string sourcePath = Application.dataPath + "/Silantro Simulator/Data/Input/Keyboard/SilantroKeyboard.dat";
				string destPath = Application.dataPath + "/../ProjectSettings/InputManager.asset";
				string defaultPath = Application.dataPath + "/Silantro Simulator/Data/Input/Default/SilantroDefault.dat";
				//
				if (!File.Exists (sourcePath)) {
					Debug.LogError ("Source File missing....Please Reimport file");
				}
				if (!File.Exists (destPath)) {
					Debug.LogError ("Destination input manager missing");
				}
				if (File.Exists (destPath) && File.Exists (sourcePath)) {
					File.Copy (defaultPath, destPath, true);
					File.Copy (sourcePath, destPath, true);
					AssetDatabase.Refresh ();
					Debug.Log ("Keyboard Input Setup Successful!");

				}
			}
			//
			break;
		case "PS4":
			//
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("Propulsion System", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (5f);
			EditorGUILayout.LabelField ("Start Engine", "Button 8");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Stop Engine", "Button 9");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Throttle Up", "Alpha 1 (Keyboard)");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Throttle Down", "Alpha 2 (Keyboard)");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Engage Afterburner", "Button 1");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Engage Reverse Thrust", "F11 (Keyboard)");
			GUILayout.Space (10f);
			//
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("Fuel System", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (5f);
			EditorGUILayout.LabelField ("Dump Fuel", "Alpha 5 (Keyboard)");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Refill Tank", "Alpha 6 (Keyboard)");
			//GUILayout.Space (3f);
			GUILayout.Space (10f);
			//
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("Actuators", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (5f);
			//GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Actuate Gear", "Button 5");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Parking Brake", "Button 2");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Brake Lever", "Button 4");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Actuate Spoiler", "G (Keyboard)");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Actuate Slat", "K (Keyboard))");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Extend Flap", "Button 6");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Retract Flap", "Button 7");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Light Switch", "V (Keyboard)");
			GUILayout.Space (10f);
			//
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("STOVL System", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (5f);
			EditorGUILayout.LabelField ("Transition to VTOL", "Alpha 7 (Keyboard)");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Transition to STOL", "Alpha 8 (keyboard)");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Transition to Normal", "Alpha 9 (Keyboard)");
			GUI.color = silantroColor;
			//
			GUILayout.Space (10f);
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("Weapon System", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (5f);
			EditorGUILayout.LabelField ("Bomb Drop Switch", "Button 3");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Minigun Fire", "Button 0");

			//
			GUILayout.Space (10f);
			GUI.color = Color.yellow;
			EditorGUILayout.HelpBox ("Control Levers", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (5f);
			EditorGUILayout.LabelField ("Pitch", "Axis 0");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Roll", "Axis 1");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Rudder", "< and > (Keyboard)");
			GUILayout.Space(5f);
			GUI.color = Color.yellow;
			EditorGUILayout.HelpBox ("Note: This will reset all input values (axis) to these values, PLEASE BACKUP YOUR CURRENT INPUT MANAGER", MessageType.Info);
			GUI.color = backgroundColor;
			//
			GUILayout.Space(20f);
			if (GUILayout.Button ("Configure PS4 Control")) {
				string sourcePath = Application.dataPath + "/Silantro Simulator/Data/Input/PS4/SilantroPS4.dat";
				string destPath = Application.dataPath + "/../ProjectSettings/InputManager.asset";
				string defaultPath = Application.dataPath + "/Silantro Simulator/Data/Input/Default/SilantroDefault.dat";
				//
				if (!File.Exists (sourcePath)) {
					Debug.LogError ("Source File missing....Please Reimport file");
				}
				if (!File.Exists (destPath)) {
					Debug.LogError ("Destination input manager missing");
				}
				if (File.Exists (destPath) && File.Exists (sourcePath)) {
					File.Copy (defaultPath, destPath, true);
					File.Copy (sourcePath, destPath, true);
					AssetDatabase.Refresh ();
					Debug.Log ("Playstation Input Setup Successful!");

				}
			}
			break;
		case "Logitech X3D":
			//
			//
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("Propulsion System", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (5f);
			EditorGUILayout.LabelField ("Start Engine", "Button 9");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Stop Engine", "Button 8");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Engage Afterburner", "Button 3");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Engage Reverse Thrust", "F11 (Keyboard)");
			GUILayout.Space (10f);
			//
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("Fuel System", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (5f);
			EditorGUILayout.LabelField ("Dump Fuel", "Alpha 5 (Keyboard)");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Refill Tank", "Alpha 6 (Keyboard)");
			//GUILayout.Space (3f);
			GUILayout.Space (10f);
			//
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("Actuators", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (5f);
			//GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Actuate Gear", "Button 10");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Parking Brake", "Button 11");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Brake Lever", "Button 1");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Actuate Spoiler", "G (Keyboard)");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Actuate Slat", "K (Keyboard))");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Extend Flap", "Button 4");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Retract Flap", "Button 5");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Light Switch", "V (Keyboard)");
			GUILayout.Space (10f);
			//
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("STOVL System", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (5f);
			EditorGUILayout.LabelField ("Transition to VTOL", "Button 6");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Transition to STOL", "Button 7");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Transition to Normal", "Alpha 9 (Keyboard)");
			GUI.color = silantroColor;
			//
			GUILayout.Space (10f);
			GUI.color = silantroColor;
			EditorGUILayout.HelpBox ("Weapon System", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (5f);
			EditorGUILayout.LabelField ("Bomb Drop Switch", "Button 2");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Minigun Fire", "Button 0");

			//
			GUILayout.Space (10f);
			GUI.color = Color.yellow;
			EditorGUILayout.HelpBox ("Control Levers", MessageType.None);
			GUI.color = backgroundColor;
			GUILayout.Space (5f);
			EditorGUILayout.LabelField ("Pitch", "Axis 1");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Roll", "Axis 0");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Rudder", "Axis 2");
			GUILayout.Space (3f);
			EditorGUILayout.LabelField ("Throttle", "Axis 3");
			GUILayout.Space(5f);
			GUI.color = Color.yellow;
			EditorGUILayout.HelpBox ("Note: This will reset all input values (axis) to these values, PLEASE BACKUP YOUR CURRENT INPUT MANAGER", MessageType.Info);
			GUI.color = backgroundColor;
			//
			GUILayout.Space(20f);
			if (GUILayout.Button ("Configure X3D Control")) {
				
				string sourcePath = Application.dataPath + "/Silantro Simulator/Data/Input/X3D/SilantroX3D.dat";
				string destPath = Application.dataPath + "/../ProjectSettings/InputManager.asset";
				string defaultPath = Application.dataPath + "/Silantro Simulator/Data/Input/Default/SilantroDefault.dat";
				//
				if (!File.Exists (sourcePath)) {
					Debug.LogError ("Source File missing....Please Reimport file");
				}
				if (!File.Exists (destPath)) {
					Debug.LogError ("Destination input manager missing");
				}
				if (File.Exists (destPath) && File.Exists (sourcePath)) {
					File.Copy (defaultPath, destPath, true);
					File.Copy (sourcePath, destPath, true);
					AssetDatabase.Refresh ();
					Debug.Log ("Joystick Input Setup Successful!");

				}
			}
			break;
		}


	}
}
