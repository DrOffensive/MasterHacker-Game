using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(HackOSFile))]
public class HackOSFileDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        EditorGUI.ObjectField(position, property.FindPropertyRelative("file"), GUIContent.none);

        Object o = property.FindPropertyRelative("file").objectReferenceValue;
        if (o != null)
        {
            string path = AssetDatabase.GetAssetPath(o);
            if(!path.EndsWith(".rf"))
            {
                Debug.Log(property.FindPropertyRelative("file").objectReferenceValue.name + " is not a valid filetype");
                property.FindPropertyRelative("file").objectReferenceValue = null;

            }
        }
        EditorGUI.EndProperty();
    }
}
