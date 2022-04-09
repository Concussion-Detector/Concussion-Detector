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

        foreach(var patient in allPatients.Children)
        {
            Debug.Log($"Patient: {patient.Key}");

            if(patient.Key != fullName)
                continue;

            Debug.Log("Found Desired Patient!");

            uuid = patient.Child("uuid").Value as string;
            Debug.Log("uuid" +uuid);
            Data.uuid = uuid;
            uuidReceived = true;

            foundPatient.Invoke();
            break;
        }


        //.ContinueWith(task => 
        // {
        //     if(task.IsCompleted)
        //     {
        //         Debug.Log("Succesful");
        //         DataSnapshot snapshot = task.Result;
        //         IEnumerable <DataSnapshot> children = snapshot.Children;

        //         // Loop over the child elements of a snapshot
        //         foreach(var c in children)
        //         {
        //             //Debug.Log(c.Key + ", " + fullName);
        //             if(c.Key == fullName)
        //             {
        //                 Debug.Log("Found!");

        //                 uuid = c.Child("uuid").Value as string;
        //                 Data.uuid = uuid;
        //                 uuidReceived = true;

        //                 Debug.Log("Invoked found patient event!");
                        
        //                 // foreach(var child in c.Children)
        //                 // {
        //                 //     if(child.Key == "uuid")
        //                 //     {
        //                 //         // Patients uuid
        //                 //         uuid = child.Value as string;
        //                 //         foundPatient.Invoke(uuid);
        //                 //         Debug.Log("The UUID for fb is " + uuid);
        //                 //         //SetUUID("codes", uuid);
        //                 //         Data.uuid = uuid;
        //                 //         uuidReceived = true;
        //                 //         return;
        //                 //     }
        //                 // }
        //             }     
        //         }  
        //         Debug.Log("Could not find " + fullName);
        //         Data.uuid = null;
        //         uuidReceived = true;
        //     }
        //     //else{Debug.Log("Could not find patient");}
        // });

        //Debug.Log("uuid is" +uuid);

    }

    private string GetUUID()
    {
        Guid myuuid = Guid.NewGuid();
        string uuid = myuuid.ToString();

        return uuid;
    }
}