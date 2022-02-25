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

    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SaveData()
    {
        Patient patient = new Patient();
        patient.firstName = firstName.text;
        patient.lastName = lastName.text;
        string fullName = firstName.text + " " + lastName.text;
        string json = JsonUtility.ToJson(patient);

        reference.Child("Patients").Child(fullName).SetRawJsonValueAsync(json).ContinueWith(task => {
            if(task.IsCompleted)
            {
                Debug.Log("Successfully added to firebase");
            }
            else
            {
                Debug.Log("Not successfull");
            }
        });

    }
}
