using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateHackOSFile : EditorWindow
{
    string extention = "rf", path = "StandardFiles", filename = "default";

    int year = 1982, month = 7, day = 13, hour = 23, minute = 52, second = 9;

    Object target;

    Object o;
    string txt;

    bool timeSetOpen = false, fileSetOpen = false;

    // Add menu named "My Window" to the Window menu
    [MenuItem("HackOS/Create Hack OS File")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        CreateHackOSFile window = (CreateHackOSFile)EditorWindow.GetWindow(typeof(CreateHackOSFile));
        window.name = "HackOS File Converter";
        window.Show();
    }



    void FormatImage (Texture2D image)
    {
        string serial = "[" + image.width + "x" + image.height + "];";
        Color[] pixels = image.GetPixels();
        foreach (Color c in pixels)
            serial += c.r + "," + c.g + "," + c.b + "," + c.a + ";";
    }

    void FormatImage(Sprite image)
    {

    }

    private void OnGUI()
    {
        int ind = EditorGUI.indentLevel;
        EditorGUI.indentLevel++;
        GUIStyle bold = new GUIStyle();
        bold.fontStyle = FontStyle.Bold;
        GUILayout.Space(8);

        EditorGUILayout.LabelField(".../Assets/" + path + "/" + filename + "." + extention, bold);
        GUILayout.Space(2);
        if (EditorGUILayout.DropdownButton(new GUIContent((fileSetOpen ? "▼" : "►") + " File Settings"), FocusType.Keyboard, bold))
        {
            fileSetOpen = !fileSetOpen;
        }
        if (fileSetOpen)
        {
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel++;
            path = EditorGUILayout.TextField("Path: ", path);
            extention = EditorGUILayout.TextField("File Extention: ", extention);
            EditorGUI.indentLevel = indent;
        }
        GUILayout.Space(4);
        EditorGUILayout.LabelField("File Creation Time: " + hour + ":" + (minute < 10 ? "0" + minute : minute.ToString()) + ":" + (second < 10 ? "0" + second : second.ToString()) + " " + month + "/" + day + " - " + year, bold);

        GUILayout.Space(2);
        if (EditorGUILayout.DropdownButton(new GUIContent((timeSetOpen ? "▼" : "►" ) + " Time Settings"), FocusType.Keyboard, bold))
        {
            timeSetOpen = !timeSetOpen;
        }
        if (timeSetOpen)
        {
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel++;
            year = (int)EditorGUILayout.Slider("Year: ", year, 1976, 1995);
            month = (int)EditorGUILayout.Slider("Month: ", month, 1, 12);
            int[] monthLengths = new int[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            Debug.Log(monthLengths.Length + " " + month);
            day = (int)EditorGUILayout.Slider("Day: ", day, 1, monthLengths[month - 1]);
            GUILayout.Space(2);

            hour = (int)EditorGUILayout.Slider("Hour: ", hour, 0, 23);
            minute = (int)EditorGUILayout.Slider("Minute: ", minute, 0, 59);
            second = (int)EditorGUILayout.Slider("Second: ", second, 0, 59);
            EditorGUI.indentLevel = indent;
        }

        GUILayout.Space(8);
        o = EditorGUILayout.ObjectField("File: ", o, typeof(Object), false);
        filename = EditorGUILayout.TextField("File Name: ", filename);

        if (o!=null)
        {
            Texture2D img = o as Texture2D;
            if( img!=null)
            {
                if(GUILayout.Button("Convert Image"))
                {
                    FormatImage(img);
                    o = null;
                }
            }
            Sprite spr = o as Sprite;
            if (spr != null)
            {
                if (GUILayout.Button("Convert Sprite"))
                {
                    FormatImage(spr);
                    o = null;
                }
            }
            TextAsset txt = o as TextAsset;
            if (txt != null)
            {
                if (GUILayout.Button("Convert Text"))
                {
                    o = null;
                }
            }
        }

        EditorGUI.indentLevel = ind;
    }
}
