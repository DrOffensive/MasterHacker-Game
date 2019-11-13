using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHackJacks : MonoBehaviour
{
    public HackJack equippedJack;
    HackJack jackA;
    public LayerMask jackLayer;
    public Transform equippedPosition;
    Transform parent;
    public Inventory inventory;
    public EquipmentController equipmentController;
    public float reach;
    public Camera cam;
    JackSocket activeSocket;
    bool usedA;
    public GameObject hackJackPrefab;

    public int notAttachedLayer = 0, attachedLayer = 10;

    // Start is called before the first frame update
    void Start()
    {

    }

    public TransformData EquippedPosition
    {
        get => new TransformData(equippedPosition);
    }

    // Update is called once per frame
    void Update()
    {
        Scan();
        if(activeSocket!=null)
        {
            if(Input.GetMouseButtonDown(0))
            {
                AttachJack();
            }
        }
    }

    void Scan ()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, reach, jackLayer, QueryTriggerInteraction.Collide))
        {
            if(hit.collider.GetComponent<JackSocket>() && hit.collider.GetComponent<JackSocket>().PluggedJack == null)
            {
                activeSocket = hit.collider.GetComponent<JackSocket>();
                TransformData hover = activeSocket.GetHover;
                equippedJack.transform.parent = activeSocket.transform;
                equippedJack.transform.localPosition = activeSocket.hoverPosition.localPosition;
                equippedJack.transform.localRotation = activeSocket.hoverPosition.localRotation;
                SwitchLayer(attachedLayer);
            } else
            {
                activeSocket = null;
            }
        }
        else
        {
            activeSocket = null;
        }

        if (activeSocket==null)
        {
            equippedJack.transform.parent = this.transform;
            equippedJack.transform.position = EquippedPosition.Position;
            equippedJack.transform.rotation = EquippedPosition.Rotation;
            SwitchLayer(notAttachedLayer);
        }
    }

    void SwitchLayer(int newLayer)
    {
        Transform[] g = equippedJack.GetComponentsInChildren<Transform>();
        foreach (Transform obj in g)
        {
            obj.gameObject.layer = newLayer;
        }
    }

    void AttachJack ()
    {
        if(jackA == null)
        {
            jackA = Instantiate(hackJackPrefab, new Vector3(0,-250,0), Quaternion.identity).GetComponent<HackJack>();
            jackA.transform.parent = activeSocket.transform;
            jackA.transform.localPosition = activeSocket.hoverPosition.localPosition;
            jackA.transform.localRotation = activeSocket.hoverPosition.localRotation;
            jackA.lineTarget = false;
            jackA.otherEnd = equippedJack;
            jackA.GetComponent<AttachJack>().Attach(activeSocket);
            activeSocket = null;
        } else
        {
            HackJack jack = Instantiate(hackJackPrefab, new Vector3(0, -250, 0), Quaternion.identity).GetComponent<HackJack>();
            jack.transform.parent = activeSocket.transform;
            jack.transform.localPosition = activeSocket.hoverPosition.localPosition;
            jack.transform.localRotation = activeSocket.hoverPosition.localRotation;
            jack.lineTarget = true;
            jack.otherEnd = jackA;
            jack.GetComponent<AttachJack>().Attach(activeSocket);
            jackA.otherEnd = jack;
            jackA = null;
            activeSocket = null;
            equippedJack.transform.parent = this.transform;
            equippedJack.transform.position = EquippedPosition.Position;
            equippedJack.transform.rotation = EquippedPosition.Rotation;
            SwitchLayer(notAttachedLayer);
            inventory.UseHackJack();

            equipmentController.Equip(Equipment.HackJack);
        }
    }
}

public struct TransformData
{
    Vector3 position;
    Quaternion rotation;

    public TransformData(Transform transform)
    {
        position = transform.position;
        rotation = transform.rotation;
    }

    public Transform Set
    {
        set
        {
            value.position = position;
            value.rotation = rotation;
        }
    }

    public Vector3 Position { get => position; }
    public Quaternion Rotation { get => rotation; }
}