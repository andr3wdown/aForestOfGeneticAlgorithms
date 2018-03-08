using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liner : MonoBehaviour
{
    public float lineLenght;
   
    private void OnDrawGizmos()
    {
        Debug.DrawLine(new Vector3(-lineLenght, transform.position.y, 0), new Vector3(lineLenght, transform.position.y, 0));
    }

}
