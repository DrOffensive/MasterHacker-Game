using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HackOS_Base : MonoBehaviour
{
    [Header("Load Settings")]
    public string profile = "Guest";
    public string password = "guest";
    public string loadRoot = "";
    public string profilesPath = "/Profiles/";
    public string rootFileExtension = "root";

    [Header("FileSystem Settings")]
    public string txtExtension;
    public string imgExtension, sfxExtension, exeExtension, shortcutExtension;

    [Header("Installed Systems")]
    [Tooltip("Dos = Text prompt only\nPanels1 = Old Windows Clone\nPanels2 = Modern Windows Clone")]
    public UISystem gui;
    public bool ImageViewer, TextEditor, Calculator, PanelExplorer;

    public List<DefaultFile> defaultFiles;

    [System.Serializable]
    public struct DefaultFile
    {
        public string path;
        public HackOSFile file;

        public HackOS_file GetFile
        {
            get
            {
                if (file == null)
                    return null;

                HackOS_file f = SerializeUtility.Load<HackOS_file>(AssetDatabase.GetAssetPath(file.file), false);
                return f;
            }
        }

        public string[] GetPath
        {
            get
            {
                return path.Split('/');
            }
        }
    }

    HackOS_Root root;

    public HackOS_Root Root { get => root; }

    public enum UISystem
    {
        Dos, Panels1, Panels2
    }

    private void Start()
    {
        if(loadRoot == "")
        {
            Debug.Log(Application.dataPath + profilesPath + "/" + profile + "-Profile." + rootFileExtension);
            CreateRootDirectory(profile, password);
        }
    }

    void CreateRootDirectory (string user, string pswrd)
    {
        HackOS_directory rootDir = new HackOS_directory("root", false);
        root = new HackOS_Root(user, pswrd, rootDir);

        HackOS_directory homeDir = new HackOS_directory("Home", false);
        HackOS_directory devicesDir = new HackOS_directory("Devices", false);
        rootDir.content.Add(homeDir);
        rootDir.content.Add(devicesDir);

        HackOS_directory desktopDir = new HackOS_directory("Desktop", false);
        HackOS_directory documentsDir = new HackOS_directory("Documents", false);
        HackOS_directory picturesDir = new HackOS_directory("Pictures", false);

        homeDir.content.Add(devicesDir);
        homeDir.content.Add(documentsDir);
        homeDir.content.Add(picturesDir);

        foreach(DefaultFile file in defaultFiles)
        {
            HackOS_directory dir = root.CheckDirectory(file.GetPath);
            dir.content.Add(file.GetFile);
        }

        //SerializeUtility.FolderCheck(Application.dataPath + profilesPath, true);
        SerializeUtility.Save<HackOS_Root>(root, profilesPath, user + "-Profile", rootFileExtension);
        Debug.Log("Created Profile: " + user + " at " + Application.dataPath + profilesPath + "/" + user + "-Profile." + rootFileExtension);
    }
}

