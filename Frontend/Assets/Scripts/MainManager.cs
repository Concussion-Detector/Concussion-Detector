using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public GameObject patientDetails;

    public void FollowDot()
    {
        StartCoroutine(StartDelay("DotFollow"));
    }

     public void GoToReports()
    {
        StartCoroutine(StartDelay("Report"));
    }
    
    IEnumerator StartDelay(string scene)
    {
        yield return new WaitForSeconds(0.2f);

        SceneManager.LoadScene(scene);
    }

    public void showPatientDetails()
    {
        patientDetails.SetActive(true);
    }
}
