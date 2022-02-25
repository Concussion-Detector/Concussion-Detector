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
    public Camera cam;

    void Start () {
        //Invoke("MoveDot", 3f);
    }

    void Update () {
        //Debug.Log(transform.position);
        if(moveDot) {
            transform.position = startPos + (Vector3.right * Mathf.Sin(Time.timeSinceLevelLoad/2*speed)*xScale - Vector3.up * Mathf.Sin(Time.timeSinceLevelLoad * speed)*yScale);
            Vector3 screenPos = cam.WorldToScreenPoint(transform.position);
            //Debug.Log(screenPos);
        }
    }

    public void SetBool()
    {
        transform.position = new Vector3(0f, 0f, 0f);
        moveDot = !moveDot;
    }
}
