using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id = 0;
    public Renderer visibilityObject;
    public TrailRenderer trail;
    private int points = 0;
    public Controls controls;

    public void Update()
    {
        if (controls.IsBoosting())
        {
            SetTrailWidth(2.0f);
        }
        else
        {
            SetTrailWidth(1.0f);
        }
    }

    public int GetId()
    {
        return id;
    }

    // Checks if the player is visible on the screen
    public bool CheckVisibility()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        bool onScreen = screenPos.x > 0f && screenPos.x < Screen.width && screenPos.y > 0f && screenPos.y < Screen.height;

        return onScreen && visibilityObject.isVisible;
    }

    public void SetTrailWidth(float width)
    {
        trail.widthMultiplier = width;
    }

    public void ClearTrail()
    {
        trail.Clear();
    }

    public void AddPoint()
    {
        points++;
    }

    public int GetPoints()
    {
        return points;
    }

}
