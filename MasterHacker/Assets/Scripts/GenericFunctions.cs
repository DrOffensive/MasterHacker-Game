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
}
