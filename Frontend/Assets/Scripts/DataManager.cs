using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{

    [SerializeField] 
    private GameObject baselineToggle;

    private UdpSocket udpSocket;
    private int testType = 1;

    // Start is called before the first frame update
    void Start()
    {
        udpSocket = gameObject.GetComponent<UdpSocket>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendPatientData(string uuid)
    {
        string msg = testType + " " + uuid;
        udpSocket.SendData(msg);
    }

    public void GetToggle()
    {
        if(baselineToggle.GetComponent<Toggle>().isOn) {
            testType = 1;
        } else {
            testType = 2;
        }
    }
}
