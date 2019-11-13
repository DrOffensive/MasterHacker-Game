using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HackOS_compressedDir : HackOs_driveData
{
    public string name, extension;
    public bool decompressable;
    public bool locked;

    public List<HackOs_driveData> content;

    public TimeStamp creationTime;

    public HackOS_compressedDir (HackOS_directory dir, string ext, TimeStamp stamp)
    {
        name = dir.name;
        extension = ext;
        content = dir.content;
        creationTime = stamp;
    }

    public HackOS_compressedDir(HackOS_compressedDir dir, TimeStamp stamp)
    {
        name = dir.name;
        extension = dir.extension;
        content = dir.content;
        creationTime = stamp;
    }


    public override HackOs_driveData Copy(TimeStamp stamp)
    {
        return new HackOS_compressedDir(this, stamp);
    }

    public HackOS_directory Decompress (bool force)
    {
        if (!decompressable || force)
        {
            HackOS_directory dir = new HackOS_directory(name, false, content);
            return dir;
        } else
        {
            return null;
        }
    }

    void ExecuteCompressed()
    {
        HackOS_file init;
        foreach (HackOs_driveData f in content)
        {
            HackOS_file file = f as HackOS_file;
            if(file != null) { 
                if (file.name.Equals("init") && file.extension.Equals("bat"))
                {
                    init = file;
                    break;
                }
            }
        }
    }
}
