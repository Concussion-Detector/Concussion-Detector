using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.UI;
using TMPro;

public class FirebaseManager : MonoBehaviour
{
    [Header("Firebase")]
    public DatabaseReference reference;

    [Header("Patient")]
    public TMP_InputField firstName;
    public TMP_InputField lastName;

    public GameObject sceneManager;

    private DataManager dataManager;

    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        dataManager = sceneManager.GetComponent<DataManager>();
    }

    public void SaveData()
    {
        Debug.Log("Saving Data to Firebase");
        Patient patient = new Patient();
        patient.uuid = GetUUID();
        patient.firstName = firstName.text;
        patient.lastName = lastName.text;
        string fullName = firstName.text + " " + lastName.text;
        string json = JsonUtility.ToJson(patient);

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

    private string GetUUID()
    {
        Guid myuuid = Guid.NewGuid();
        string uuid = myuuid.ToString();

        return uuid;
    }
}
