using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaptopScreenHinge : MonoBehaviour
{
    public float openAngle = -110, closedAngle = 0, openSpeed, closeSpeed;

    int state = 0;
    float moved, maxMove;

    bool open;

    public bool openOnStart = true;
    // Start is called before the first frame update
    void Start()
    {
        maxMove = Mathf.Abs(closedAngle - openAngle);
        if(openAngle < 0)
        {
            openAngle = openAngle + 360;
        }
        if(openOnStart)
            Open();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 1)
        {
            Vector3 r = transform.rotation.eulerAngles;
            float m = openSpeed * Time.deltaTime;
            if (moved + m > maxMove)
            {
                m = maxMove - moved;
                state = 0;
                open = true;
            }
            moved += m;

            r.z -= m;
            transform.rotation = Quaternion.Euler(r);
        }
        else if (state == -1)
        {
            Vector3 r = transform.rotation.eulerAngles;
            float m = closeSpeed * Time.deltaTime;
            if(moved + m > maxMove)
            {
                m = maxMove - moved;
                state = 0;
                open = false;
            }
            moved += m;

            r.z += m;
            transform.rotation = Quaternion.Euler(r);
        }

        if(state ==0)
        {
            /*if(Input.GetKeyDown(KeyCode.Escape))
            {
                Close();
            }*/
        }
    }

    public void Open()
    {
        if (!open && state == 0)
        {
            state = 1;
            moved = 0;
        }
    }

    public void Close ()
    {
        if (open && state == 0)
        {
            state = -1;
            moved = 0;
        }
    }
}
