using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorChanger : MonoBehaviour
{
    public Renderer[] MainRenderers;
    public Renderer[] SecondaryRenderers;
    
    public void SetMainColor(Color color)
    {
        foreach (Renderer renderer in MainRenderers)
        {
            renderer.material.color = color;
        }
    }
    public void SetSecondaryColor(Color color)
    {
        foreach (Renderer renderer in SecondaryRenderers)
        {
            renderer.material.color = color;
        }
    }
    
    public void SetColors(Color mainColor, Color secondaryColor)
    {
        SetMainColor(mainColor);
        SetSecondaryColor(secondaryColor);
    }
    
    public Color GetMainColor()
    {
        return MainRenderers[0].material.color;
    }
    public Color GetSecondaryColor()
    {
        return SecondaryRenderers[0].material.color;
    }
}
