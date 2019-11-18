using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateHackOSFile : EditorWindow
{
    string extention = "rf", path = "StandardFiles", filename = "default", hOSfilename = "";

    int year = 1982, month = 7, day = 13, hour = 23, minute = 52, second = 9;
    FileCreationMode mode = FileCreationMode.InputBased;

    GUIStyle bold = new GUIStyle(), standard = new GUIStyle();
    Object target;

    Object o;
    string txt;

    bool timeSetOpen = false, fileSetOpen = false;

    string audioKey = "";
    float sizeLengthMod = 1;


    public enum FileCreationMode
    {
        InputBased, CustomFile, AudioFile
    }

    // Add menu named "My Window" to the Window menu
    [MenuItem("HackOS/Create Hack OS File")]
    static void Init()
    {
        GUIStyle bold = new GUIStyle();
        GUIStyle standard = new GUIStyle();
        standard.fontStyle = FontStyle.Normal;
        bold.fontStyle = FontStyle.Bold;
        // Get existing open window or if none, make a new one:
        CreateHackOSFile window = (CreateHackOSFile)EditorWindow.GetWindow(typeof(CreateHackOSFile));
        window.name = "HackOS File Converter";
        window.Show();
    }



    string FormatImage (Texture2D image)
    {
        string serial = "[" + image.width + "x" + image.height + "];";
        Color[] pixels = image.GetPixels();
        foreach (Color c in pixels)
            serial += GenericFunctions.RGBAtoHex(c) + ";";

        return serial;
    }

    string FormatImage(Sprite image)
    {
        Texture2D img = image.texture;
        Rect imgSize = image.textureRect;
        Vector2 imgOffset = image.textureRectOffset;
        string serial = "[" + imgSize.width + "x" + imgSize.height + "];";
        Color[] pixels = new Color[(int)imgSize.width * (int)imgSize.height];
        for (int y = 0; y < imgSize.height; y++)
        {
            for (int x = 0; x < imgSize.width; x++)
            {
                int i = (int)((y * imgSize.width) + x);
                Color c = img.GetPixel((int)imgOffset.x + x, (int)imgOffset.y + y);
                serial += GenericFunctions.RGBAtoHex(c) + ";";
            }
        }
        
        return serial;
    }

    private void OnGUI()
    {
        int ind = EditorGUI.indentLevel;
        EditorGUI.indentLevel++;
        GUILayout.Space(8);
        mode = (FileCreationMode)EditorGUILayout.EnumPopup(mode);
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
        EditorGUILayout.LabelField("Asset name:", bold);
        filename = GenericFunctions.TextField(filename);
        GUILayout.Space(3);
        EditorGUILayout.LabelField("System File name (incl. extention):", bold);
        hOSfilename = GenericFunctions.TextField(hOSfilename);
        if (mode == FileCreationMode.InputBased)
        {
            o = EditorGUILayout.ObjectField("File: ", o, typeof(Object), false);

            if (o != null)
            {
                string[] fullFileName = hOSfilename.Split('.');
                Texture2D img = o as Texture2D;
                if (img != null)
                {
                    if (GUILayout.Button("Convert Image"))
                    {
                        if (img.isReadable)
                        {
                            string content = FormatImage(img);
                            HackOS_file file = new HackOS_file(fullFileName[0], fullFileName[1], content, new TimeStamp(year, month, day, hour, minute, second));
                            SerializeUtility.Save<HackOS_file>(file, "/" + path, filename, extention);
                            o = null;
                            AssetDatabase.Refresh();
                        }
                        else
                        {
                            Debug.LogWarning(img.name + " not set to readable");
                        }
                    }
                }
                Sprite spr = o as Sprite;
                if (spr != null)
                {
                    if (GUILayout.Button("Convert Sprite"))
                    {
                        string content = FormatImage(spr);
                        HackOS_file file = new HackOS_file(fullFileName[0], fullFileName[1], content, new TimeStamp(year, month, day, hour, minute, second));
                        SerializeUtility.Save<HackOS_file>(file, "/" + path, filename, extention);
                        o = null;
                        AssetDatabase.Refresh();
                    }
                }
                TextAsset txt = o as TextAsset;
                if (txt != null)
                {
                    if (GUILayout.Button("Convert Text"))
                    {
                        string content = txt.text;
                        HackOS_file file = new HackOS_file(fullFileName[0], fullFileName[1], content, new TimeStamp(year, month, day, hour, minute, second));
                        SerializeUtility.Save<HackOS_file>(file, "/" + path, filename, extention);
                        o = null;
                        AssetDatabase.Refresh();
                    }
                }
            }

            EditorGUI.indentLevel = ind;
        } else if(mode == FileCreationMode.AudioFile)
        {
            string[] fullFileName = hOSfilename.Split('.');
            EditorGUILayout.LabelField("Sound Library key:", bold);
            GUILayout.BeginHorizontal();
            audioKey = GUILayout.TextField(audioKey);

            if(GUILayout.Button("Paste"))
            {
                audioKey = GUIUtility.systemCopyBuffer;
            }
            GUILayout.EndHorizontal();

            sizeLengthMod = EditorGUILayout.FloatField("File size (MB): ", sizeLengthMod);
            if(GUILayout.Button("Generate AudioFile"))
            {
                HackOS_audioFile audio = new HackOS_audioFile(fullFileName[0], fullFileName[1], audioKey, sizeLengthMod, new TimeStamp(year, month, day, hour, minute, second));
                SerializeUtility.Save<HackOS_audioFile>(audio, "/" + path, filename, extention);
                audioKey = "";
                AssetDatabase.Refresh();
            }
        }
    }

    
}
