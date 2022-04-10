using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DataManager : MonoBehaviour
{

    [SerializeField] 
    private GameObject baselineToggle;
    [SerializeField]
    private GameObject playerNotFound;
    [SerializeField]
    private GameObject searchError;
    [SerializeField]
    private GameObject detailsError;
    [SerializeField]
    private TextMeshProUGUI searchErrorMessage;

    [SerializeField]
    private TextMeshProUGUI detailsErrorMessage;

    [SerializeField]
    private MainManager mainManager;

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

    public int GetTestType()
    {
        return testType;
    }

    public void SendPatientData(string uuid)
    {
        string msg = testType + " " + uuid;
        Data.test = testType;
        udpSocket.SendData(msg);
        mainManager.FollowDot();
    }

    public void PatientNotFoundGUI(bool notFound)
    {
        mainManager.patientNotFound = notFound;
        playerNotFound.SetActive(notFound);
        baselineToggle.GetComponent<Toggle>().isOn = notFound;
    }

    public void SearchErrorMessage(string errorMsg)
    {
        searchErrorMessage.text = errorMsg;
        searchError.SetActive(true);
    }

     public void DetailsErrorMessage(string errorMsg)
    {
        detailsErrorMessage.text = errorMsg;
        detailsError.SetActive(true);
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
