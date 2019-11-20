using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TerminalSpool : MonoBehaviour
{

    LineRenderer LineRenderer;
    public Vector2 range = new Vector2(.28f, .35f);
    public Transform leftEnd, rightEnd;
    public List<Transform> path;
    public float speed = 1;
    float t = 0;


    // Start is called before the first frame update
    void Start()
    {
        LineRenderer = GetComponent<LineRenderer>();
        SetPositions();


    }

    // Update is called once per frame
    void Update()
    {
        t += speed * Time.deltaTime;
        SetEndPositions();
        SetPositions();
    }

    void SetEndPositions ()
    {
        float time = Mathf.PingPong(t, 1f);

        //left side
        float leftOffset = ((range.y - range.x)) * time;
        leftEnd.localPosition = new Vector3(leftEnd.localPosition.x, leftEnd.localPosition.y, range.x + leftOffset);

        float rightOffset = ((range.y - range.x)) * (1f-time);
        rightEnd.localPosition = new Vector3(rightEnd.localPosition.x, rightEnd.localPosition.y, -(range.x + rightOffset));
    }

    public void SetPositions ()
    {
        if(LineRenderer.positionCount != path.Count + 2)
            LineRenderer.positionCount = path.Count + 2;

        List<Vector3> points = new List<Vector3>() { leftEnd.position };
        foreach(Transform t in path)
        {
            points.Add(t.position);
        }
        points.Add(rightEnd.position);
        LineRenderer.SetPositions(points.ToArray());
    }
}
