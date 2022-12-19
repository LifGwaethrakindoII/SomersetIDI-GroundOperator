using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SafeHandles;

[CustomPropertyDrawer(typeof(VisualizableVector))]
public class VisualizableVectorDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // First get the attribute since it contains the range for the slider
        VisualizableVector range = attribute as VisualizableVector;

        // Now draw the property as a Slider or an IntSlider based on whether it's a float or integer.
        if (property.propertyType == SerializedPropertyType.Vector3)
        {
            property.vector3Value = EditorGUILayout.Vector3Field(label, property.vector3Value);
            property.vector3Value = HandlesHelper.PositionHandle (property.propertyPath, property.vector3Value, Quaternion.identity);
            property.serializedObject.ApplyModifiedProperties();
        }
        else
            EditorGUI.LabelField(position, label.text, "Use Attribute with Vector3.");
    }
}