using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownHandler : MonoBehaviour
{
    public GameObject menu;
    public GameObject patientDetails;

    public void activateMenu()
    {
        menu.SetActive(true);
    }

    public void deactivateMenu()
    {
        menu.SetActive(false);
        patientDetails.SetActive(false);
    }

    public void showPatientDetails()
    {
        patientDetails.SetActive(true);
        menu.SetActive(false);
    }

    public void exit()
    {
        Application.Quit();
    }
}
