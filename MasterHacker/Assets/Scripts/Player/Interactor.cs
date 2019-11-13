using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class Interactor : MonoBehaviour
{
    Camera cam;
    public float reach = 2;
    public LayerMask interactionLayers;
    public InteractToast toast;
    public PlayerController playerController;
    public KeyCode pickUpKey = KeyCode.E, useKey = KeyCode.F;
    Interactable cInteractable;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!playerController.freezed)
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward * reach);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, reach, interactionLayers, QueryTriggerInteraction.Collide))
            {
                if(hit.collider.GetComponent<Interactable>())
                {
                    cInteractable = hit.collider.GetComponent<Interactable>();
                    toast.Set(cInteractable.canPickUp, cInteractable.canUse);
                } else
                {
                    cInteractable = null;
                    toast.Set(false, false);
                }
            } else
            {
                cInteractable = null;
                toast.Set(false, false);
            }


            if(cInteractable!=null)
            {
                if (Input.GetKeyDown(pickUpKey))
                    cInteractable.OnPickup();

                if (Input.GetKeyDown(useKey))
                    cInteractable.OnUse();
            }
        } else
        {
            cInteractable = null;
            toast.Set(false, false);
        }
    }
}
