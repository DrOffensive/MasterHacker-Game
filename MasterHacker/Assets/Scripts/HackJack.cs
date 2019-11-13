using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class HackJack : MonoBehaviour
{
    LineRenderer lr;
    public bool lineTarget;
    public HackJack otherEnd;
    public Transform coordPosition;
    public float maxCoordLength = 3f, dropMod = .5f;
    float yDrop = 1f;
    public int verts;
    public AnimationCurve dropCurve;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = verts;
    }

    // Update is called once per frame
    void Update()
    {
        if(!lineTarget)
        {

            Vector3 otherPos = otherEnd.coordPosition.position;

            float dist = (otherPos - coordPosition.position).magnitude;
            if (dist < maxCoordLength)
            {
                if (!lr.enabled)
                    lr.enabled = true;

                lr.SetPositions(UpdateLinePositions(otherPos));
            } else
            {
                if (lr.enabled)
                    lr.enabled = false;
            }
        }
    }

    Vector3[] UpdateLinePositions (Vector3 end)
    {
        /*if (verts % 2 == 0)
            verts++;

        Vector3[] positions = new Vector3[verts];
        Vector3 dir = end - coordPosition.position;
        float halfLength = new Vector3(dir.x, 0, dir.z).magnitude * .5f;
        float centerY = end.y - yDrop < coordPosition.position.y - yDrop ? end.y - yDrop : coordPosition.position.y - yDrop;
        float dropLength = (Mathf.Abs(centerY) - Mathf.Abs(coordPosition.position.y)) / ((verts - 1) * .5f);*/


        Vector3[] positions = new Vector3[verts];
        Vector3 dir = end - coordPosition.position;
        float step = dir.magnitude / verts-1;
        yDrop = (maxCoordLength - dir.magnitude) * .5f;
        for(int i = 0; i < verts-1; i++)
        {
            float p = (1f / (float)verts) * i;
            positions[i] = coordPosition.position + (dir * p) + new Vector3(0, -dropCurve.Evaluate(p) * yDrop, 0); 
        }
        positions[verts - 1] = end;
        return positions;
    }
}
