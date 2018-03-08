using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GradientObject
{
    public Color color1;
    public Color color2;
    public GradientObject(Color c1, Color c2)
    {
        color1 = c1;
        color2 = c2;
    }
    public Color GetGradientColor(float val)
    {
        return Color.Lerp(color1, color2, val);
    }
	
}
