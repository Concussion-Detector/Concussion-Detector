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
            //Debug.Log("Concussion");
            savingData = true;
            GetPatientData();
            StartCoroutine(Wait(2));
        }
    }

    IEnumerator Wait(int num)
    {
        yield return new WaitUntil(UUIDReceived);
        Debug.Log("UUID received");
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

            // Removed a callback function from here which was causing an issue with method not being invoked.
            reference.Child("Patients").Child(fullName).SetRawJsonValueAsync(json);
  
            Debug.Log("Successfully added to firebase");
            dataManager.SendPatientData(patient.uuid);
                
        } 
        // Concussion
        else 
        {
            Debug.Log("Concussion");
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

        //Debug.Log(savingData);

        if((searchFirstName.text == String.Empty || searchLastName.text == String.Empty) && savingData != true)
        {
            dataManager.SearchErrorMessage("Please fill in all fields");
            return;
        } 
        else if ((firstName.text == String.Empty || lastName.text == String.Empty) && savingData == true) 
        {
            dataManager.DetailsErrorMessage("Please fill in all fields");
            return;
        }

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
            //Debug.Log("Patients:"+ patient.Key);

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
            if(savingData != true)
            {
                // Invoke redirection to different scene
                foundPatient.Invoke();
            }
            break;
        }
        // If no patient is found but it is baseline data to be recorded,
        // set the uuidReceived to true to allow the wait method to complete
        if(dataManager.GetTestType() == 1) {
            uuidReceived = true;
        }

        /*if(dataManager.GetTestType() == 2) {
            data
        }*/

        if((searchFirstName.text == String.Empty || searchLastName.text == String.Empty) && savingData != true)
        {
            dataManager.SearchErrorMessage("Could not find " + fullName);
            return;
        } 
    }

    private string GetUUID()
    {
        Guid myuuid = Guid.NewGuid();
        string uuid = myuuid.ToString();

        return uuid;
    }
}