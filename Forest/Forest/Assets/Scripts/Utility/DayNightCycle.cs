using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public SpriteRenderer dayBackGround;
    public SpriteRenderer nightBackGround;
    public Transform sunRotator;
    public Transform sun;
    public LayerMask leafLayer;
    float t = -3;
    public float timeSpeed = 0.1f;   
    static DayNightCycle instance;
    private void Start()
    {
        instance = this;
    }
    private void Update()
    {
        t += timeSpeed * Time.deltaTime;
        Color c = dayBackGround.color;      
        Color c2 = nightBackGround.color;
        //c.a = SigmoidSine(t);
        //c2.a = 1f - SigmoidSine(t);
        c.a = TimeRatio();
        c2.a = (1f - TimeRatio()) * 0.75f;
        nightBackGround.color = c2;
        dayBackGround.color = c;
        
        sunRotator.eulerAngles = new Vector3(0, 0, t * 30);     
    }
    float TimeRatio()
    {
        float returnable = 0f;
        if(sun.transform.position.y > sunRotator.transform.position.y)
        {
            returnable = 0.5f + ((sun.transform.position.y / 12f) / 2f);           
        }
        else if(sun.transform.position.y < sunRotator.transform.position.y)
        {
            returnable = 0.5f + ((sun.transform.position.y / 12f) / 2f);
        }


        return returnable;
    }
    float SigmoidSine(float val)
    {
        return (Mathf.Sin(val) / 2f) + 0.5f;
    }
    public static float SunStrenght
    {
        get
        {
            return instance.TimeRatio();
        }
    }
}
