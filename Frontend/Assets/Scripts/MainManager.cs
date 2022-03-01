using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public GameObject patientDetails;

    public void FollowDot()
    {
       
        //Debug.Log("Load Dot Scene");
        //SceneManager.LoadSceneAsync("DotFollow");
        StartCoroutine(StartDelay());

    }


    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(0.2f);

        SceneManager.LoadScene("DotFollow");

    }

    public void showPatientDetails()
    {
        patientDetails.SetActive(true);
    }
}
