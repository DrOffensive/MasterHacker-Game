using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SoundLibrary))]
public class SoundLib_Inspector : Editor
{
    bool overwriteOldKeys = false;
    int keyLength = 20;
    string defaultPrefix = "SND-";
    List<int> openItems = new List<int>();


    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        SoundLibrary library = (SoundLibrary)target;
        GUILayout.Space(4);
        
        for(int i = 0; i< library.sounds.Count; i++)
        {
            bool open = openItems.Contains(i);

            if(EditorGUILayout.DropdownButton(new GUIContent((open ? "v" : ">") + library.sounds[i].name == "" ? "Element" + i : library.sounds[i].name), FocusType.Keyboard, new GUILayoutOption[0])) {
                if (open)
                    openItems.Remove(i);
                else
                    openItems.Add(i);

                open = !open;
            }
            int indent = EditorGUI.indentLevel;
            if(open)
            {
                SoundLibrary.HasckOSSound sound = library.sounds[i];
                EditorGUI.indentLevel += 1;
                sound.name = EditorGUILayout.TextField("Name:", sound.name);
                sound.clip = (AudioClip)EditorGUILayout.ObjectField("Clip:", sound.clip, typeof(AudioClip), false);
                sound.key = EditorGUILayout.TextField("Key:", sound.key);
                EditorGUILayout.BeginHorizontal();

                if(GUILayout.Button("Generate key"))
                {
                    sound.key = GenerateKey();
                }
                if (GUILayout.Button("Copy key"))
                {
                    GUIUtility.systemCopyBuffer = sound.key;
                }
                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button("Remove"))
                {
                    library.sounds.RemoveAt(i);
                }
            }
        }

        GUILayout.Space(4);
        if (GUILayout.Button("Add Sound"))
        {
            library.sounds.Add(new SoundLibrary.HasckOSSound());
        }

        GUILayout.Space(4);
        GUIStyle bold = new GUIStyle();
        bold.fontStyle = FontStyle.Bold;
        EditorGUILayout.LabelField("Sound Library Key Generation:", bold);
        overwriteOldKeys = EditorGUILayout.Toggle("Overwrite Old Keys ", overwriteOldKeys);
        keyLength = EditorGUILayout.IntField("Key Length", keyLength);
        defaultPrefix = EditorGUILayout.TextField("Key Prefix", defaultPrefix);

        if(GUILayout.Button("Generate new keys"))
        {
            for(int i = 0; i < library.sounds.Count; i++)
            {
                if (library.sounds[i].key == "" || overwriteOldKeys)
                    library.sounds[i].key = GenerateKey();
            }
        }

    }

    string GenerateKey()
    {
        List<char> chars = new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '#', '$', '£', '&', '%' };
        string key = defaultPrefix;

        int keys = keyLength - key.Length;

        for (int i = 0; i < keys; i++)
            key += chars[Random.Range(0, chars.Count)];

        return key;
    }
}
