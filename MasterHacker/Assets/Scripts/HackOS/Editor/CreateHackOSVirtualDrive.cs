using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateHackOSVirtualDrive : EditorWindow
{
    GUIStyle bold = new GUIStyle(), standard = new GUIStyle();

    public HackOS_directory selectedDir = null, parentDir = null;
    public CommandParser.SystemPath selectedPath;

    HackOS_directory rootDir = new HackOS_directory("root", false);
    public string username, password, savePath, filename;

    // Add menu named "My Window" to the Window menu
    [MenuItem("HackOS/Create Hack OS Virtual Drive")]
    static void Init()
    {
        GUIStyle bold = new GUIStyle();
        GUIStyle standard = new GUIStyle();
        // Get existing open window or if none, make a new one:
        CreateHackOSVirtualDrive window = (CreateHackOSVirtualDrive)GetWindow(typeof(CreateHackOSVirtualDrive));
        window.name = "HackOS File Converter";
        window.Show();
    }

    private void OnGUI()
    {
        standard.fontStyle = FontStyle.Normal;
        bold.fontStyle = FontStyle.Bold;
        Rect area = new Rect(10f, 10f, position.width - 20f, position.height - 120f);
        GUILayout.BeginArea(area);

        GUILayout.Label("Create Hack OS Virtual Drive", bold);
        GUILayout.Space(4);

        GUILayout.Label("Root settings:", bold);
        username = EditorGUILayout.TextField("Username: ", username);
        password = EditorGUILayout.TextField("Password: ", password);

        GUILayout.Space(4);
        GUILayout.Label("File Structure:", bold);

        ShowDirectory(rootDir, null, 1, new List<string>());
        EditorGUI.indentLevel = 0;

        GUILayout.Space(4);
        if (selectedDir != null && selectedPath.directories != null)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add sub folder"))
            {
                int inc = 0;
                foreach(HackOS_directory dir in selectedDir.content)
                {
                    string[] dirName = dir.name.Split('_');
                    if(dirName.Length == 2 || dirName.Length == 3)
                    {
                        if (dirName[0] == "New" && dirName[1] == "Folder")
                            inc++;
                    }
                }


                selectedDir.content.Add(new HackOS_directory("New_Folder" + (inc > 0 ? "_" + inc.ToString() : ""), false));
            }
            if (parentDir != null)
            {
                if (GUILayout.Button("Delete folder"))
                {
                    RemoveDirectiory(selectedDir, parentDir);
                    selectedDir = null;
                }
            }

            EditorGUILayout.EndHorizontal();
            /*string p = "/";
            foreach (string s in selectedPath.directories)
            {
                p += s + "/";
            }
            GUILayout.Label(p);*/
            //GUILayout.Label("Selected Dir: " + vRoot.selectedDir.name);
        }
        GUILayout.EndArea();
        area = new Rect(10f, position.height - 80f, position.width - 20f, 80f);
        GUILayout.BeginArea(area);

        GUILayout.Space(4);
        GUILayout.Label("Save Virtual Root:", bold);
        savePath = EditorGUILayout.TextField("Save Path (asset/...): ", savePath);
        filename = EditorGUILayout.TextField("File name: ", filename);
        if (GUILayout.Button("Create Asset"))
        {
            if (savePath != "" && filename != "")
            {
                SO_virtualroot asset = new SO_virtualroot();
                asset.userName = username;
                asset.password = password;
                asset.rootDirectory = rootDir;
                //string path = Application.dataPath + "/" + savePath + "/" + filename + ".asset";
                //Debug.Log(path);         
                string path = AssetDatabase.GetAssetPath (Selection.activeObject);
                //Debug.Log(path);         
                AssetDatabase.CreateAsset(asset, path + "/" + filename + ".asset" );
                AssetDatabase.SaveAssets();
            }
        }
        GUILayout.EndArea();
    }

    void RemoveDirectiory (HackOS_directory directory, HackOS_directory parent)
    {
        parent.content.Remove(directory);
    }

    void ShowDirectory(HackOs_driveData directory, HackOS_directory parent, int indent, List<string> adress)
    {
        HackOS_directory cDir = (HackOS_directory)directory;
        if (cDir != null)
        {
            EditorGUI.indentLevel = indent;
            EditorGUILayout.BeginHorizontal();
            List<string> adr = adress;
            bool selected = selectedDir == null ? false : selectedDir.Equals(cDir);
            cDir.name = EditorGUILayout.TextField(cDir.name, selected ? bold : standard);
            bool sel = selected;
            sel = EditorGUILayout.Toggle(sel);

            EditorGUILayout.EndHorizontal();
            if (sel != selected)
            {
                if (sel)
                {
                    selectedDir = (HackOS_directory)cDir;
                    parentDir = parent;
                    selectedPath = new CommandParser.SystemPath(adr);
                }
                else
                {
                    selectedDir = null;
                    parentDir = null;
                    selectedPath.directories = null;
                }
            }
            adr.Add(cDir.name);
            foreach (HackOs_driveData dir2 in cDir.content)
            {
                if (dir2 != null)
                    ShowDirectory(dir2, cDir, indent + 1, adr);
                else
                    cDir.content.Remove(dir2);
            }
        }
    }
}
