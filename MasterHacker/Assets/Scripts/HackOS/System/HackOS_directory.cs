using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HackOS_directory : HackOs_driveData
{
    public bool hidden;
    public bool locked;

    public List<HackOs_driveData> content;
    
    public HackOS_directory (string n, bool h)
    {
        name = n;
        hidden = h;
        content = new List<HackOs_driveData>();
    }


    public HackOS_directory(string n, bool h, List<HackOs_driveData> c)
    {
        name = n;
        hidden = h;
        content = c;
    }

    public HackOS_directory (HackOS_directory dir)
    {
        name = dir.name;
        hidden = dir.hidden;
        content = dir.content;
    }

    public override HackOs_driveData Copy (TimeStamp stamp)
    {
        return new HackOS_directory(this);
    }

    public string[] ListContent ()
    {
        List<HackOS_directory> directories = new List<HackOS_directory>();
        List<HackOs_driveData> items = new List<HackOs_driveData>();
        foreach (HackOs_driveData data in content)
        {
            if (data is HackOS_directory)
                directories.Add((HackOS_directory)data);
            else
                items.Add(data);
        }

        List<string> prints = new List<string>();
        foreach(HackOS_directory dir in directories)
        {
            prints.Add(PromptScreen.StepIn("- " + dir.name + "/", 15, 4) + "[Directory]");
        }
        foreach (HackOs_driveData data in items)
        {
            FileSize size = data.Size();
            int decimals = 0;
            if (size.Size < 100)
                decimals = 1;
            if (size.Size < 10)
                decimals = 2;

            prints.Add(PromptScreen.StepIn("- " + data.name + (data is HackOS_file ? "." + ((HackOS_file)data).extension : "") , 20, 4) + GenericFunctions.LimitDecimals(size.Size, decimals) + " " + size.Unit);
        }
        if (prints.Count > 0)
            return prints.ToArray();
        else
            return new List<string>() { PromptScreen.StepIn("", 5, 0) + "[Empty Directory]" }.ToArray();
    }
}
