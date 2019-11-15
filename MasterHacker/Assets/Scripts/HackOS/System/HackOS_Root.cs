using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HackOS_Root
{
    public string userName;
    string password;

    public HackOS_directory rootDirectory;
    
    public HackOS_Root(string user, string pswrd, HackOS_directory root)
    {
        userName = user;
        password = pswrd;
        rootDirectory = root;
    }

    public bool CheckPassword (string input)
    {
        if (input.Equals(password))
            return true;

        return false;
    }

    public HackOS_directory CheckDirectory (string[] directories)
    {
        HackOS_directory currentDir;
        if (directories[0].ToLower() == rootDirectory.name.ToLower())
        {
            currentDir = rootDirectory;
            for (int i = 1; i < directories.Length; i++)
            {
                bool found = false;
                foreach(HackOs_driveData data in currentDir.content)
                {
                    if(data.name.ToLower()==directories[i].ToLower() && data is HackOS_directory)
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
        } else
        {
            return null;
        }
    }

    public HackOS_file GetFile(CommandParser.SystemPath path)
    {
        HackOS_directory directory = CheckDirectory(path.directories.ToArray());
        if(directory!=null && path.file!="")
        {
            foreach(HackOs_driveData data in directory.content)
            {
                if(data is HackOS_file)
                {
                    HackOS_file file = (HackOS_file)data;
                    if (file.name.ToLower().Equals(path.file.ToLower()) && (file.extension.ToLower().Equals(path.fileExtention.ToLower()) || path.fileExtention == ""))
                        return file;
                }
            }
        }

        return null;
    }
    
}
