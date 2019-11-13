using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentController : MonoBehaviour
{
    public Inventory inventory;
    public List<EquippedItem> equippedItems = new List<EquippedItem>() { new EquippedItem(Equipment.Nothing) };

    public List<Equipment> equipped = new List<Equipment>() { Equipment.Nothing };
    int eqIndex = 0;

    public KeyCode weaponSlot1Key = KeyCode.Alpha1, weaponSlot2Key = KeyCode.Alpha2, weaponSlot3Key = KeyCode.Alpha3, weaponSlot4Key = KeyCode.Alpha4, weaponSlot5Key = KeyCode.Alpha5;

    [System.Serializable]
    public struct EquippedItem
    {
        public Equipment equipment;
        public GameObject obj;

        public EquippedItem (Equipment eq)
        {
            equipment = eq;
            obj = null;
        }
    }

    public void Equip (Equipment equipment)
    {
        if (inventory.HasItem(equipment))
        {
            HideAll();
            GameObject g = null;
            foreach (EquippedItem e in equippedItems)
            {
                if (e.equipment.Equals(equipment))
                {
                    g = e.obj;
                    break;
                }
            }
            if (g != null)
                g.SetActive(true);

            Debug.Log("Equipped " + equipment.ToString());
        } else { Equip(Equipment.Nothing); }
    }

    void HideAll ()
    {
        GameObject[] g = new GameObject[equippedItems.Count];
        for(int i = 0; i < g.Length; i++)
        {
            g[i] = equippedItems[i].obj;
        }
        foreach(GameObject o in g)
        {
            if(o!=null)
                o.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (inventory == null)
            GetComponent<Inventory>();
        Equip(equipped[eqIndex]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(weaponSlot1Key) && eqIndex != 0)
        {
            if (equipped.Count > 0)
            {
                Equip(equipped[0]);
                eqIndex = 0;
            }
        }
        if (Input.GetKeyDown(weaponSlot2Key) && eqIndex != 1)
        {
            if (equipped.Count > 1)
            {
                Equip(equipped[1]);
                eqIndex = 1;
            }
        }
        if (Input.GetKeyDown(weaponSlot3Key) && eqIndex != 2)
        {
            if (equipped.Count > 2)
            {
                Equip(equipped[2]);
                eqIndex = 2;
            }
        }
        if (Input.GetKeyDown(weaponSlot4Key) && eqIndex != 3)
        {
            if (equipped.Count > 3)
            {
                Equip(equipped[3]);
                eqIndex = 3;
            }
        }
        if (Input.GetKeyDown(weaponSlot5Key) && eqIndex != 4)
        {
            if (equipped.Count > 4)
            {
                Equip(equipped[4]);
                eqIndex = 4;
            }
        }
    }
}

public enum Equipment
{
    Nothing, Laptop, HackJack
}
