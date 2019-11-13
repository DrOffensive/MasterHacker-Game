using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_Laptop : Interactable
{
    public Transform camPosition;

    public override void OnUse()
    {
        FindObjectOfType<ZoomToPC>().ZoomToTarget(camPosition);
    }
}
