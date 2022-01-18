using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infinitesign : MonoBehaviour
{
    public float speed = 3;
    public float xScale = 4;
    public float yScale = 2;
    private Vector3 startPos;
    private bool moveDot = true;

    void Start () {
        //Invoke("MoveDot", 3f);
    }

    void Update () {
        Debug.Log(transform.position);
        if(moveDot) {
            transform.position = startPos + (Vector3.right * Mathf.Sin(Time.timeSinceLevelLoad/2*speed)*xScale - Vector3.up * Mathf.Sin(Time.timeSinceLevelLoad * speed)*yScale);
        }
    }

    void MoveDot()
    {
        startPos = transform.position;
        moveDot = true;
    }
}
