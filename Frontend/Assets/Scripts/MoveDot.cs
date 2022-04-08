using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveDot : MonoBehaviour
{
    [SerializeField]
    private GameObject sceneManager;

    [SerializeField]
    private Transform[] waypoints;

    [SerializeField]
    private float moveSpeed = 2f;

    private int waypointIndex = 0;
    private bool dotMoving = false;

    private UdpSocket udpSocket;

    
    // Start is called before the first frame update
    void Start()
    {
        udpSocket = sceneManager.GetComponent<UdpSocket>();
        transform.position = waypoints[waypointIndex].transform.position;
        StartCoroutine(StartMovingDot());
    }

    // Update is called once per frame
    void Update()
    {
        if(dotMoving)
        {
            Move();
        }
    }

    private IEnumerator StartMovingDot()
    {
        yield return new WaitForSeconds(2);
        udpSocket.SendData("true");
        dotMoving = true;
    }

    private IEnumerator StopMovingDot()
    {
        yield return new WaitForSeconds(2);

        if(Data.test == 1) {
            SceneManager.LoadScene("Main");
        } else {
            yield return new WaitForSeconds(2);
            SceneManager.LoadScene("Report");
        }
    }

    private void Move()
    {
        if(waypointIndex <= waypoints.Length - 1)
        {
            transform.position = Vector2.MoveTowards(transform.position,
            waypoints[waypointIndex].transform.position,
            moveSpeed * Time.deltaTime);

            if(transform.position == waypoints[waypointIndex].transform.position)
            {
                waypointIndex += 1;
            }
        }
        else if (waypointIndex == waypoints.Length)
        {
            // Stop reading data from python
            udpSocket.SendData("false");
            StartCoroutine(StopMovingDot());
        }
    }
}
