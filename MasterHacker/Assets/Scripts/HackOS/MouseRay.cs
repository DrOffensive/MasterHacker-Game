using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRay : MonoBehaviour
{
    public Mouse mouse;
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(ray.origin, ray.direction * 2, Color.white, .5f);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            if(hit.collider.tag == "LaptopScreen")
            {
                MeshCollider col = (MeshCollider)hit.collider;
                if (col == null)
                    return;

                mouse.SetMousePosition(hit.textureCoord);
            }
        }
    }
}
