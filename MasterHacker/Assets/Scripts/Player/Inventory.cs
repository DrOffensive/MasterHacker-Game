using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool hasPc;
    [Range(0,25)]
    public int hackJacks;

    public bool HasItem (Equipment item)
    {
        switch(item)
        {
            case Equipment.Nothing:
                return true;

            case Equipment.Laptop:
                return hasPc;

            case Equipment.HackJack:
                return hackJacks > 0;
        }

        return false;
    }

    public void UseHackJack()
    {
        hackJacks = Mathf.Clamp(hackJacks - 1, 0, 25);
    }

    public void Add (Equipment item)
    {
        switch (item)
        {
            case Equipment.HackJack:
                hackJacks++;
                break;

            case Equipment.Laptop:
                hasPc = true;
                break;
        }
    }
}
