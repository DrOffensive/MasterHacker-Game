using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackSocket : MonoBehaviour
{
    public Transform jackPosition, hoverPosition;
    TransformData jack, hover;
    public MeshRenderer jackLight;
    public Material onMaterial, offMaterial;
    public Light jackLightLight;

    HackJack pluggedJack;

    // Start is called before the first frame update
    void Start()
    {
        jack = new TransformData(jackPosition);
        hover = new TransformData(hoverPosition);

        Flip(pluggedJack != null);
    }

    public TransformData GetJack
    {
        get => jack;
    }

    public TransformData GetHover
    {
        get => hover;
    }

    public HackJack PluggedJack { get => pluggedJack;
        set
        {
            Flip(value != null);
            pluggedJack = value;
        }
    }

    void Flip (bool on)
    {
        if(on)
        {
            jackLight.material = onMaterial;
            jackLightLight.enabled = true;
        } else
        {
            jackLight.material = offMaterial;
            jackLightLight.enabled = false;
        }
    }
}
