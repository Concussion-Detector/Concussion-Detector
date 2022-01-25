using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public GameObject patientDetails;

    public void FollowDot()
    {
        SceneManager.LoadScene("DotFollow");
    }

    public void showPatientDetails()
    {
        patientDetails.SetActive(true);
    }
}
