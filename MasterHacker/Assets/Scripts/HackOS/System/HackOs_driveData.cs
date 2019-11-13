using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class HackOs_driveData
{
    public string name;

    public abstract HackOs_driveData Copy(TimeStamp time);

    public struct FileSize
    {
        float size;
        string unit;

        public FileSize(float size, string unit)
        {
            this.size = size;
            this.unit = unit;
        }

        public float Size { get => size; }
        public string Unit { get => unit; }
    }

    public virtual float SizeInBytes ()
    {
        return name.Length * 8;
    }

    public virtual FileSize Size ()
    {
        float size = SizeInBytes();
        string[] units = GenericFunctions.FileSizeUnits();

        int u = 0;
        float s = size;
        if (s > 1000)
        {
            while (s > 1000 && u < units.Length)
            {
                size = size / 1000f;
                u++;
            }
        }

        return new FileSize(s, units[u]);
    }
}
