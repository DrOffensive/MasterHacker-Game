using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMouseControl : MonoBehaviour
{

    public bool mouseLocked;
    public PlayerController controller;
    public Image crosshair;
    public KeyCode freezeKey;
    public bool locked;

    public bool Locked { set => Locked = value; get => Locked; }

    // Start is called before the first frame update
    void Start()
    {
        if (controller == null)
            controller = GetComponent<PlayerController>();

        MouseLocked = mouseLocked;
    }

    private void Update()
    {
        /*if (!locked)
            if (Input.GetKeyDown(freezeKey))
                MouseLocked = !mouseLocked;*/
    }

    bool MouseLocked
    {
        set
        {
            Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !value;
            crosshair.enabled = value;
            controller.Frozen = !value;
            mouseLocked = value;
        }
        get => mouseLocked;
    }
}
