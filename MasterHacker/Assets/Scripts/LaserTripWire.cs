using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserTripWire : MonoBehaviour
{
    LineRenderer lineRenderer;
    public Transform laserStart, laserEnd;
    public Transform reciever;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Setup ()
    {
        Ray ray = new Ray(laserStart.position, laserStart.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 10))
        {
            Vector3 pos = transform.InverseTransformPoint(hit.point);
            reciever.localPosition = new Vector3(reciever.localPosition.x, reciever.localPosition.y, pos.z);
        }

        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(new Vector3[] { laserStart.position, laserEnd.position });
    }
}
