using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    public Camera osCam;
    public void SetMousePosition (Vector2 position)
    {
        Vector2 screenSize = new Vector2(osCam.pixelWidth, osCam.pixelHeight);
        Vector2 screenPos = new Vector2(screenSize.x * position.x, screenSize.y * position.y);
        transform.localPosition = new Vector2(screenPos.x - (screenSize.x *.5f), screenPos.y - (screenSize.y * .5f));
    }
}
