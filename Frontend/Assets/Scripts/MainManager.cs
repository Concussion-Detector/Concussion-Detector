using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public GameObject patientDetails;

    public void FollowDot()
    {
        Debug.Log("Load Dot Scene");
        SceneManager.LoadScene("DotFollow");
    }

    public void showPatientDetails()
    {
        patientDetails.SetActive(true);
    }
}
