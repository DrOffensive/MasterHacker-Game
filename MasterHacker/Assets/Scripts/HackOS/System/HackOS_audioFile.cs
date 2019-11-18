using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HackOS_audioFile : HackOS_file
{
    public float sizeInMb;


    public HackOS_audioFile(string n, string e, string c, float size, TimeStamp stamp)
    {
        name = n;
        extension = e;
        content = c;
        sizeInMb = size;
        creationTime = stamp;
    }

    public HackOS_audioFile(HackOS_audioFile file, TimeStamp stamp)
    {
        name = file.name;
        extension = file.extension;
        content = file.content;
        sizeInMb = file.sizeInMb;
        creationTime = stamp;
    }

    public override float SizeInBytes()
    {
        return sizeInMb * 1000000;
    }

    public override HackOs_driveData Copy(TimeStamp time)
    {
        return new HackOS_audioFile(this, time);
    }
}
