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

    public static string RGBAtoHex (Color rgba)
    {
        return "#" + ColorUtility.ToHtmlStringRGBA(rgba);
    }

    public static Color HexToRGBA (string hex)
    {
        Color color = new Color();
        if (hex.StartsWith("#"))
        {
            ColorUtility.TryParseHtmlString(hex, out color);
            return color;
        }
        else
            return Color.black;
    }

    public static string RemoveChar (char charToDelete, string text)
    {
        string t = "";
        foreach(char c in text)
        {
            if (c != charToDelete)
                t += c;
        }
        return t;
    }

    public static string RemoveChars (List<char> charsToDelete, string text)
    {
        string t = "";
        foreach (char c in text)
        {
            if (!charsToDelete.Contains(c))
                t += c;
        }
        return t;
    }
}

[System.Serializable]
public struct RangeConverter
{
    [System.Serializable]
    public struct RangeItem
    {
        public float value, output;
    }

    public List<RangeItem> items;

    public float Evaluate (float input)
    {
        for(int i = 0; i < items.Count; i++)
        {
            if (input <= items[i].value)
                return items[i].output;
        }
        return items[items.Count - 1].output;
    }
}