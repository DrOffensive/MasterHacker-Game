using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskMountSystem : HackOSSystem
{

    public string rootDrive, floppyDrive, jacklink, check;

    public enum HackOSMountPoint
    {
        Root, Floppy, Jack
    }

    public override IEnumerator ExecuteCommand(string[] commandLine, PromptScreen screen, CommandParser.SystemPath targetPath)
    {
        if (commandLine.Length == 2)
        {
            string[] commands = new string[commandLine.Length - 1];
            for (int i = 1; i < commandLine.Length - 1; i++)
                commands[i - 1] = commandLine[i].ToLower();

            foreach (string command in commands)
            {
                if (command.Equals(rootDrive.ToLower()))
                    Mount(HackOSMountPoint.Root, screen, screen.parser.OS);
                else if (command.Equals(floppyDrive.ToLower()))
                    Mount(HackOSMountPoint.Floppy, screen, screen.parser.OS);
                else if (command.Equals(jacklink.ToLower()))
                    Mount(HackOSMountPoint.Jack, screen, screen.parser.OS);
                else if (command.Equals(check.ToLower()))
                    Check(screen, screen.parser.OS);
                else
                {
                    screen.InsertLine("Unknown keyword " + command);
                    break;
                }
            }
        } else
        {
            if(commandLine.Length > 2)
            {
                screen.InsertLine("Unknown keyword " + commandLine[2]);
            } else
            {
                screen.InsertLine("Specify mountpoint");
            }
        }
        SetComplete();
        yield return null;
    }

    public void Mount (HackOSMountPoint mountPoint, PromptScreen screen, HackOS_Base OS)
    {
        switch (mountPoint)
        {
            case HackOSMountPoint.Root:
                OS.Root.rootDirectory = OS.Root.InternalDrive;
                screen.SetMountedDir(new List<string>() { OS.Root.rootDirectory.name });
                break;

            case HackOSMountPoint.Floppy:
                if (OS.Root.FloppyDrive != null)
                {
                    OS.Root.rootDirectory = OS.Root.FloppyDrive;
                    screen.SetMountedDir(new List<string>() { OS.Root.rootDirectory.name });
                }
                break;
        }
    }

    public void Check(PromptScreen screen, HackOS_Base OS)
    {
        screen.InsertLine(PromptScreen.StepIn("Internal Drive:", 20, 3) + "Available");
        screen.InsertLine(PromptScreen.StepIn("Floppy Drive:", 20, 3) + (OS.Root.FloppyDrive == null ? "Not Available" : "Available"));
        screen.InsertLine(PromptScreen.StepIn("Internal Drive:", 20, 3) + (OS.Root.JackConnection == null ? "Not Available" : "Available"));
    }
}
