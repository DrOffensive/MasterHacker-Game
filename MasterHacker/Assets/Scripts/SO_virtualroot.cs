using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Virtual_root", menuName = "HackOS/VirtualFiles", order = 1)]
public class SO_virtualroot : ScriptableObject
{
    public string userName;
    public string password;

    public HackOS_directory selectedDir = null;
    public CommandParser.SystemPath selectedPath;

    public HackOS_directory rootDirectory = new HackOS_directory("root",false);
}