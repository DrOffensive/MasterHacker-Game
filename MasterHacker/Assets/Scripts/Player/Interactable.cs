using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool canPickUp, canUse;

    public Equipment pickupItem;

    public virtual void OnPickup ()
    {
        if (canPickUp)
        {
            FindObjectOfType<Inventory>().Add(pickupItem);
            Destroy(this.gameObject);
        }
    }

    public virtual void OnUse()
    {

    }

}
