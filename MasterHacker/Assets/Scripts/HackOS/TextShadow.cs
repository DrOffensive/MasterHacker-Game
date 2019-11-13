using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextShadow : MonoBehaviour
{
    Text t, parent;

    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Text>();
        parent = transform.parent.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        t.text = parent.text;
    }
}
