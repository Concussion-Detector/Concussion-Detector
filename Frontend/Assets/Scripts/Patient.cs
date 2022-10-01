using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient
{
    public string uuid;
    public string firstName;
    public string lastName;
    public string dob;
    public bool previousConcussion;


    // Converts to a Json String
    public string Stringify()
    {
        return JsonUtility.ToJson(this);
    }

    // Takes Json string and convert it back to an object
    public static Patient Parse(string json)
    {
        return JsonUtility.FromJson<Patient>(json);
    }
}
