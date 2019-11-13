using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachJack : MonoBehaviour
{
    HackJack jack;
    JackSocket s;
    public float speed = 1f;

    int attachLayer = 0;

    // Start is called before the first frame update
    void Start()
    {
        jack = GetComponent<HackJack>();
    }

    public void Attach (JackSocket socket)
    {
        s = socket;

        Transform[] g = GetComponentsInChildren<Transform>();
        foreach(Transform obj in g)
        {
            obj.gameObject.layer = attachLayer;
        }

        StartCoroutine("SlideInSocket");
    }

    IEnumerator SlideInSocket ()
    {
        Vector3 line = (s.GetJack.Position - s.GetHover.Position);
        Vector3 dir = line.normalized;
        float lenght = line.magnitude;
        float moved = 0;

        while(moved < lenght)
        {
            float spd = speed * Time.deltaTime;
            if (spd > lenght - moved)
                spd = lenght - moved;

            moved += spd;
            transform.position += dir * spd;
            yield return null;
        }

        s.PluggedJack = jack;
        s = null;
    }
}
