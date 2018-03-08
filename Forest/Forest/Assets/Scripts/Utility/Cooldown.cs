using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cooldown
{
    public float d;
    float c;
    public Cooldown(float _d)
    {
        d = _d;
    }
    public bool CountDown(bool r=false, float n=0f)
    {
        c -= Time.deltaTime;
        if(c <= 0)
        {
            if (!r)
            {
                c = d;
            }
            else { c = n; }
            
            return true;
        }
        return false;
    }

}
