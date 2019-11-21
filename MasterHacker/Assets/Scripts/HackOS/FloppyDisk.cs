using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloppyDisk : MonoBehaviour
{
    public string diskName;

    public HackOS_directory floppyRoot;

    public TextAsset fileStructure;

    public List<HackOS_Base.DefaultFile> defaultFiles;

    private void Start()
    {
        if (fileStructure != null)
        {
            BuildFilesStructure();
        }
        else
        {
            floppyRoot = new HackOS_directory(diskName, false);
            floppyRoot.content = new List<HackOs_driveData>();
        }
    }


    void BuildFilesStructure ()
    {
        string raw = fileStructure.text.Replace("<diskName>", diskName);

        floppyRoot = new HackOS_directory(diskName, false);
        floppyRoot.content = new List<HackOs_driveData>();
        string[] folders = raw.Split('\n'); 

        for(int i = 0; i < folders.Length;i++)
        {
            HackOS_directory currentDirectory = null;
            string[] structure = folders[i].Split('/');
            for(int o = 0; o < structure.Length; o++)
            {
                Debug.Log(structure[o]);
                if(o==0)
                {
                    if (structure[o].Equals(diskName))
                    {
                        currentDirectory = floppyRoot;
                    }
                    else break;
                } else
                {
                    bool found = false;
                    foreach(HackOS_directory data in currentDirectory.content)
                    {
                        if(data.name == structure[o])
                        {
                            found = true;
                            currentDirectory = (HackOS_directory)data;
                            break;
                        }
                    } 
                    if(!found)
                    {
                        HackOS_directory newDir = new HackOS_directory(structure[o], false);
                        newDir.content = new List<HackOs_driveData>();
                        currentDirectory.content.Add(newDir);
                        currentDirectory = newDir;
                    }
                }

            }
        }

        foreach (HackOS_Base.DefaultFile file in defaultFiles)
        {
            HackOS_directory dir = CheckDirectory(file.GetPath);
            if (dir != null)
                dir.content.Add(file.GetFile);
            else
                Debug.LogWarning("Unknown directory: " + file.path + "/");
        }
    }

    public HackOS_directory CheckDirectory(string[] directories)
    {
        HackOS_directory currentDir;
        if (directories[0].ToLower() == floppyRoot.name.ToLower())
        {
            currentDir = floppyRoot;
            for (int i = 1; i < directories.Length; i++)
            {
                bool found = false;
                foreach (HackOs_driveData data in currentDir.content)
                {
                    if (data.name.ToLower() == directories[i].ToLower() && data is HackOS_directory)
                    {
                        currentDir = (HackOS_directory)data;
                        found = true;
                        break;
                    }
                }
                if (!found)
                    return null;
            }
            return currentDir;
        }
        else
        {
            return null;
        }
    }
}
