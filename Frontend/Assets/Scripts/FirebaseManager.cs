using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Firebase;
using Firebase.Database;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class FirebaseManager : MonoBehaviour
{
    [Header("Firebase")]
    public DatabaseReference reference;

    [Header("Patient")]
    public TMP_InputField firstName;
    public TMP_InputField lastName;
    
    [Header("Search Patient")]
    public TMP_InputField searchFirstName;
    public TMP_InputField searchLastName;

    public GameObject sceneManager;
    private bool savingData = false;
    private bool uuidReceived = false;

    private DataManager dataManager;
    Patient patient = new Patient();

    // Event which invokes redirecto to Reports page
    public UnityEvent foundPatient;

    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        dataManager = sceneManager.GetComponent<DataManager>();
    }

    public void SaveData()
    {
        Debug.Log("Saving Data to Firebase");
        // Baseline data
        if(dataManager.GetTestType() == 1)
        {
            savingData = true;
            GetPatientData();
            StartCoroutine(Wait(1));
        } 
        else if(dataManager.GetTestType() == 2)
        {
            Debug.Log("Concussion");
            savingData = true;
            GetPatientData();
            StartCoroutine(Wait(2));
        }
    }

    IEnumerator Wait(int num)
    {
        yield return new WaitUntil(UUIDReceived);

        // Baseline
        if(num == 1) {
            string fullName = " ";
            string json = " ";

            if(Data.uuid == null) {
                patient.uuid = GetUUID();
                Data.uuid = patient.uuid;
            } else {
                patient.uuid = Data.uuid;
            }

            dataManager.PatientNotFoundGUI(false);
            patient.firstName = firstName.text;
            patient.lastName = lastName.text;
            fullName = firstName.text + " " + lastName.text;
            json = JsonUtility.ToJson(patient);

            reference.Child("Patients").Child(fullName).SetRawJsonValueAsync(json).ContinueWith(task => {
                if(task.IsCompleted)
                {
                    Debug.Log("Successfully added to firebase");
                    dataManager.SendPatientData(patient.uuid);
                }
                else
                {
                    Debug.Log("Not successfull");
                }
            });
        } 
        // Concussion
        else 
        {
            if(Data.uuid != null) {
                patient.uuid = Data.uuid;
                Debug.Log(patient.uuid);
                dataManager.SendPatientData(patient.uuid);
                //UUID.uuid = null;
            } else {
                dataManager.PatientNotFoundGUI(true);
            }
        }
    }

    bool UUIDReceived()
    {
        return uuidReceived;
    }

    // Retrieves first and last name
    public async void GetPatientData()
    {
        var uuid = "";
        string fullName = String.Empty;

        if(savingData == true) 
        {
            fullName = firstName.text + " " + lastName.text;
        } else {
            fullName = searchFirstName.text + " " + searchLastName.text;
        }

        var allPatients = await reference.Child("Patients").GetValueAsync();

        // Loop over the collection
        foreach(var patient in allPatients.Children)
        {
            Debug.Log("Patients:"+ patient.Key);

            // Continue if desired patient not found.
            if(patient.Key != fullName)
                //dataManager.PatientNotFoundGUI(true);
                continue;

            Debug.Log("Found Desired Patient!");
            //dataManager.PatientNotFoundGUI(false);
            uuid = patient.Child("uuid").Value as string;
            Debug.Log("uuid" +uuid);
            Data.uuid = uuid;
            uuidReceived = true;
            // Invoke redirection to different scene
            foundPatient.Invoke();
            break;
        }

        dataManager.ErrorMessage("Please fill in all fields");
    }

    private string GetUUID()
    {
        Guid myuuid = Guid.NewGuid();
        string uuid = myuuid.ToString();

        return uuid;
    }
}