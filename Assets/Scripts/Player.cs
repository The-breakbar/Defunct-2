using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id = 0;
    public Renderer visibilityObject;
    public TrailRenderer trail;

    public int GetId()
    {
        return id;
    }

    public bool CheckVisibility()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        bool onScreen = screenPos.x > 0f && screenPos.x < Screen.width && screenPos.y > 0f && screenPos.y < Screen.height;

        return onScreen && visibilityObject.isVisible;
    }

    public void ClearTrail()
    {
        trail.Clear();
    }

}
