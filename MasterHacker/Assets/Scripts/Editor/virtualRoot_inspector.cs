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

        vRoot = (SO_virtualroot)target;
    }

    public override void OnInspectorGUI()
    {
        //serializedObject.Update();
        base.OnInspectorGUI();
        HackOS_directory root = vRoot.rootDirectory;
        vRoot.userName = EditorGUILayout.TextField("Username: ", vRoot.userName);
         /*if(root==null || root.content == null)
         {
             root = new HackOS_directory("root", false);
             root.content = new List<HackOs_driveData>();
             vRoot.rootDirectory = root;
         }*/

         GUILayout.Label("File Structure:", bold);

        ShowDirectory(root, 1, new List<string>());
        EditorGUI.indentLevel = 0;

        if (vRoot.selectedDir != null && vRoot.selectedPath.directories != null)
        {
            if (GUILayout.Button("Add sub folder"))
            {
                Undo.RecordObject(vRoot, "added folder");
                vRoot.selectedDir.content.Add(new HackOS_directory("New Folder", false));
            }
            if (GUILayout.Button("Delete folder"))
            {
                GetParent(vRoot.selectedPath.directories, vRoot.rootDirectory).content.Remove(vRoot.selectedDir);
                vRoot.selectedDir = null;
            }
            string p = "/";
            foreach (string s in vRoot.selectedPath.directories)
            {
                p += s + "/";
            }
            GUILayout.Label(p);
            GUILayout.Label("Selected Dir: " + vRoot.selectedDir.name);
        }

        /*if (GUILayout.Button("Save asset"))
        {
            Repaint();
            //AssetDatabase.SaveAssets();
        }
        serializedObject.ApplyModifiedProperties();*/
        EditorUtility.SetDirty(vRoot);
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

    void ShowDirectory (HackOs_driveData directory, int indent, List<string> adress)
    {
        HackOS_directory cDir = (HackOS_directory)directory;
        if (cDir != null)
        {
            EditorGUI.indentLevel = indent;
            EditorGUILayout.BeginHorizontal();
            List<string> adr = adress;
            bool selected = vRoot.selectedDir == null ? false : vRoot.selectedDir.Equals(cDir);
            cDir.name = EditorGUILayout.TextField(cDir.name, selected ? bold : standard);
            bool sel = selected;
            sel = EditorGUILayout.Toggle(sel);

            EditorGUILayout.EndHorizontal();
            if (sel != selected)
            {
                if (sel)
                {
                    vRoot.selectedDir = (HackOS_directory)cDir;
                    vRoot.selectedPath = new CommandParser.SystemPath(adr);
                } else
                {
                    vRoot.selectedDir = null;
                    vRoot.selectedPath.directories = null;
                }
            }
            adr.Add(cDir.name);
            foreach (HackOs_driveData dir2 in cDir.content)
            {
                ShowDirectory(dir2, indent + 1, adr);
            }
        }
    }

    bool ComparePaths(List<string> pathA, List<string> pathB)
    {
        if (pathA == null || pathB == null)
            return false;

        if (pathA.Count != pathB.Count)
            return false;

        for (int i = 0; i < pathA.Count; i++)
        {
            if (!pathA[i].Equals(pathB[i]))
                return false;
        }

        return true;
    }
}
