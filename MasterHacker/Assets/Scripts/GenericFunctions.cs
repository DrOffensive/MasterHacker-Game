using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenericFunctions
{
    public static void ShuffleList<T>(ref List<T> list)
    {
        for(int i = list.Count-1; i >= 0; i--)
        {
            int random = Random.Range(0, i);
            T stored = list[i];
            list[i] = list[random];
            list[random] = stored;
        }
    }

    public static float LimitDecimals (float value, int decimals)
    {
        float pow = Mathf.Pow(10, decimals);
        return (Mathf.Floor(value * pow) / pow);
    }

    public static string[] FileSizeUnits()
    {
        return new string[] { "Byte(s)", "Kilobyte(s)", "Megabyte(s)", "Gigabyte(s)", "Terabyte(s)", "Petabyte(s)" };
    }
}
