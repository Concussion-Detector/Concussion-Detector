using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public GameObject patientDetails;
    public bool patientNotFound = false;

    public void FollowDot()
    {
        StartCoroutine(StartDelay("DotFollow"));
    }

     public void GoToReports()
    {
        StartCoroutine(StartDelay("Report"));
    }

    public void GoToMain()
    {
        SceneManager.LoadScene("Main");
    }
    
    IEnumerator StartDelay(string scene)
    {
        yield return new WaitForSeconds(0.2f);
        Debug.Log("Player is not found " + patientNotFound);
        if(patientNotFound == false) {
            SceneManager.LoadScene(scene);
        }
    }

    public void showPatientDetails()
    {
        patientDetails.SetActive(true);
    }
}
