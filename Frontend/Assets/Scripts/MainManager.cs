using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public GameObject patientDetails;
    public GameObject patientSearchDetails;
    public bool patientNotFound = false;

    public static bool valid = false;

    public void FollowDot()
    {
        Debug.Log("Follow dot");
        StartCoroutine(StartDelay("DotFollow"));
    }

     public void GoToReports()
    {
        Debug.Log("Going to reports!");

        StartCoroutine(StartDelay("Report"));
    }

    public void GoToMain()
    {
        SceneManager.LoadScene("Main");
    }
    
    IEnumerator StartDelay(string scene)
    {
        Debug.Log("Wait ");
        yield return new WaitForSeconds(0.2f);
        //Debug.Log("Player is not found " + valid);
        // if(valid == false) {
            
        // }
        Debug.Log("Load " + scene);
        SceneManager.LoadScene(scene);
    }

    public void showPatientDetails()
    {
        patientDetails.SetActive(true);
    }

    public void hidePatientDetails()
    {
        patientDetails.SetActive(false);
    }

    public void hideSearchPatientDetails()
    {
        patientSearchDetails.SetActive(false);
    }
}