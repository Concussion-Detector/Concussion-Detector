/*
Idea implemented from Two-way communication between Python 3 and Unity (C#) - Y. T. Elashry
*/
using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UdpSocket : MonoBehaviour
{
    [HideInInspector] public bool isTxStarted = false;

    [SerializeField] string IP = "127.0.0.1"; // local host
    [SerializeField] int rxPort = 8000; // port to receive data from Python on
    [SerializeField] int txPort = 8001; // port to send data to Python on
    [SerializeField] private GameObject leftDot;
    [SerializeField] private GameObject rightDot;
    public Camera cam;

    private float x, y;
    private Boolean leftEyeDetected = false;
    private Boolean rightEyeDetected = false;

    // Create necessary UdpClient objects
    UdpClient client;
    IPEndPoint remoteEndPoint;
    Thread receiveThread; // Receiving Thread

    void Awake()
    {
        // Create remote endpoint (to Matlab) 
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), txPort);

        // Create local client
        client = new UdpClient(rxPort);

        // local endpoint define (where messages are received)
        // Create a new thread for reception of incoming messages
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();

        // Initialize (seen in comments window)
        print("UDP Comms Initialised");
        print(Screen.height + " " + Screen.width);
    }

    void Update()
    {
        if(leftEyeDetected)
        {
            Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(x, y, 10f));
            Instantiate(leftDot, worldPos, Quaternion.identity);
            leftEyeDetected = false;
        } else if (rightEyeDetected)
        {
            Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(x, y, 10f));
            Instantiate(rightDot, worldPos, Quaternion.identity);
            rightEyeDetected = false;
        }
    }

    public void SendData(string message) // Use to send data to Python
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.Send(data, data.Length, remoteEndPoint);
        }
        catch (Exception err)
        {
            print(err.ToString());
        }
    }


    // Receive data, update packets received
    private void ReceiveData()
    {
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);
                string text = Encoding.UTF8.GetString(data);
                print(text);
                // Split the received text by a comma
                string[] coords = text.Split(',');
                string eye = "";
                // The first string is 'l' or 'r' to distinguish
                // the left from the right eye
                // The x coord is the second string after split
                // y coord is third. Parse both of these to floats
                if(coords[0] == "l")
                {
                    eye = "l";
                } else if(coords[0] == "r")
                {
                    eye = "r";
                }
                x = float.Parse(coords[1]);
                y = float.Parse(coords[2]);
        //         //print(eye + "=> x:" + x + ", y: " + y);
                x = x / 640;
                y = y / 480;

                x *= Screen.width;
                y *= Screen.height;
                if(eye == "l")
                {
                    leftEyeDetected = true;
                } else if(eye == "r") {
                    rightEyeDetected = true;
                }
                //print(eye + "=> x:" + x + ", y: " + y);

                ProcessInput(text);
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
            }
    }

    private void ProcessInput(string input)
    {
        // PROCESS INPUT RECEIVED STRING HERE

        if (!isTxStarted) // First data arrived so tx started
        {
            isTxStarted = true;
        }
    }

    //Prevent crashes - close clients and threads properly!
    void OnDisable()
    {
        if (receiveThread != null)
            receiveThread.Abort();

        client.Close();
    }
}
