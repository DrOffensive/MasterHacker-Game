using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractToast : MonoBehaviour
{
    public GameObject pickUpToast, useToast;

    public void Set (bool pickUp, bool use)
    {
        if (pickUp)
            pickUpToast.SetActive(true);
        else
            pickUpToast.SetActive(false);

        if (use)
            useToast.SetActive(true);
        else
            useToast.SetActive(false);
    }

    private void Start()
    {
        Set(false, false);
    }
}
