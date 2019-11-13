using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SO_virtualroot))]
public class virtualRoot_inspector : Editor
{
    GUIStyle bold = new GUIStyle(), standard = new GUIStyle();
    SO_virtualroot vRoot;

    private void OnEnable()
    {
        standard.fontStyle = FontStyle.Normal;
        bold.fontStyle = FontStyle.Bold;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        vRoot = (SO_virtualroot)target;
        HackOS_directory root = vRoot.rootDirectory;
        if(root==null || root.content == null)
        {
            root = new HackOS_directory("root", false);
            root.content = new List<HackOs_driveData>();
            root.content.Add(new HackOS_directory("root", false));
        }

        GUILayout.Label(root.name);

        foreach(HackOs_driveData data in root.content)
        {
            HackOS_directory dir = (HackOS_directory)data;
            if (data != null) {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.BeginHorizontal();
                List<string> adr = new List<string>();
                adr.Add(dir.name);
                bool selected = adr.Equals(vRoot.selectedPath.directories);
                Debug.Log(dir.name + " selected = " + selected);
                bool sel = selected;
                dir.name = EditorGUILayout.TextField(dir.name, selected ? bold : standard);
                selected = EditorGUILayout.Toggle(selected);

                EditorGUILayout.EndHorizontal();
                if (sel != selected)
                {
                    Debug.Log(sel);
                    if (selected)
                    {
                        vRoot.selectedDir = dir;
                        vRoot.selectedPath = new CommandParser.SystemPath(adr);
                        string path = "/";
                        foreach (string s in vRoot.selectedPath.directories) {
                            path += s + "/";
                        }
                        Debug.Log(vRoot.selectedDir.name + ", " + path);
                    }
                }
                ShowDirectory(dir, 2, new List<string>());
            }
        }
        EditorGUI.indentLevel = 0;

        if (vRoot.selectedDir != null)
        {
            if (GUILayout.Button("Add sub folder"))
            {
                vRoot.selectedDir.content.Add(new HackOS_directory("New Folder", false));
            }
            if (GUILayout.Button("Delete folder"))
            {
                GetParent(vRoot.selectedPath.directories, vRoot.rootDirectory).content.Remove(vRoot.selectedDir);
                vRoot.selectedDir = null;
            }
        }

        string p = "/";
        foreach (string s in vRoot.selectedPath.directories)
        {
            p += s + "/";
        }
        GUILayout.Label(p);
    }

    HackOS_directory GetParent (List<string> dirs, HackOS_directory directory)
    {
        HackOS_directory cDir;
        if (dirs[0].ToLower() == directory.name.ToLower())
        {
            cDir = directory;
            for (int i = 1; i < dirs.Count-1; i++)
            {
                bool found = false;
                foreach (HackOs_driveData data in cDir.content)
                {
                    if (data.name.ToLower() == dirs[i].ToLower() && data is HackOS_directory)
                    {
                        cDir = (HackOS_directory)data;
                        found = true;
                        break;
                    }
                }
                if (!found)
                    return null;
            }
            return cDir;
        }
        else
        {
            return null;
        }
    }

    void ShowDirectory (HackOS_directory directory, int indent, List<string> adress)
    {
        foreach (HackOS_directory dir in directory.content)
        {
            Debug.Log(dir.name);
            EditorGUI.indentLevel = indent;
            EditorGUILayout.BeginHorizontal();
            List<string> adr = adress;
            adr.Add(dir.name);
            bool selected = vRoot.selectedPath.Equals(adr);
            dir.name = EditorGUILayout.TextField(dir.name, selected ? bold : standard);
            bool sel = selected;
            sel = EditorGUILayout.Toggle(sel);

            EditorGUILayout.EndHorizontal();
            if (sel!=selected)
            {
                if (sel)
                {
                    vRoot.selectedDir = dir;
                    vRoot.selectedPath = new CommandParser.SystemPath(adr);
                }
            }


            foreach(HackOS_directory dir2 in dir.content)
            {
                ShowDirectory(dir2, indent + 1, adr);
            }

        }
    }
}
