using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private DataManager dataManager;
    Patient patient = new Patient();

    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        dataManager = sceneManager.GetComponent<DataManager>();
    }

    public void SaveData()
    {
        string fullName = " ";
        string json = " ";
        Debug.Log("Saving Data to Firebase");
        // Baseline data
        if(dataManager.GetTestType() == 1)
        {
            savingData = true;
            GetPatientData();
            if(UUID.uuid == null)
            {
                patient.uuid = GetUUID();
            } else {
                patient.uuid = UUID.uuid;
            }
            
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
        } else if(dataManager.GetTestType() == 2)
        {
            savingData = true;
            GetPatientData();
            patient.uuid = UUID.uuid;
            patient.firstName = firstName.text;
            patient.lastName = lastName.text;
            dataManager.SendPatientData(patient.uuid);
        }
    }

    // Retrieves first and last name
    public void GetPatientData()
    {
        var uuid = "";
        string fullName;

        if(savingData == true) 
        {
            fullName = patient.firstName + " " + patient.lastName;
        } else {
            fullName = searchFirstName.text + " " + searchLastName.text;
        }

        reference.Child("Patients").GetValueAsync().ContinueWith(task => 
        {
            if(task.IsCompleted)
            {
                Debug.Log("Succesful");
                DataSnapshot snapshot = task.Result;
                IEnumerable <DataSnapshot> children = snapshot.Children;

                // Loop over the child elements of a snapshot
                foreach(var c in children)
                {
                    //Debug.Log(c.Key + ", " + fullName);
                    if(c.Key == fullName)
                    {
                        Debug.Log("Found!");
                        foreach(var child in c.Children)
                        {
                            if(child.Key == "uuid")
                            {
                                // Patients uuid
                                uuid = child.Value as string;
                                Debug.Log("The UUID for fb is " + uuid);
                                //SetUUID("codes", uuid);
                                UUID.uuid = uuid;
                            }
                        }
                    }     
                }  
                //Debug.Log("Could not find " + fullName); 
            }
            //else{Debug.Log("Could not find patient");}
        });

        //Debug.Log("uuid is" +uuid);

    }

    private string GetUUID()
    {
        Guid myuuid = Guid.NewGuid();
        string uuid = myuuid.ToString();

        return uuid;
    }
}
