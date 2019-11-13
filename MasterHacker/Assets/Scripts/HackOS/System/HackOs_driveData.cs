using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class HackOs_driveData
{
    public string name;

    public abstract HackOs_driveData Copy(TimeStamp time);

    public virtual int Size ()
    {
        return name.Length;
    }
}
