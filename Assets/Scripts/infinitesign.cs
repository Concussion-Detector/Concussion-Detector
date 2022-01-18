using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infinitesign : MonoBehaviour
{
   public float speed = 4;
    public float xScale = 2;
    public float yScale = 2;
    private Vector3 startPos;

    void Start () {
        startPos = transform.position;
    }

    void Update () {
        transform.position = startPos + (Vector3.right * Mathf.Sin(Time.timeSinceLevelLoad/2*speed)*xScale - Vector3.up * Mathf.Sin(Time.timeSinceLevelLoad * speed)*yScale);
    }
}
