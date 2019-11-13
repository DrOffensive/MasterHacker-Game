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
        HackOS_directory cDir;
        if (directories[0].ToLower() == rootDirectory.name.ToLower())
        {
            cDir = rootDirectory;
            for (int i = 1; i < directories.Length; i++)
            {
                bool found = false;
                foreach(HackOs_driveData data in cDir.content)
                {
                    if(data.name.ToLower()==directories[i].ToLower() && data is HackOS_directory)
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
        } else
        {
            return null;
        }
    }
    
}
