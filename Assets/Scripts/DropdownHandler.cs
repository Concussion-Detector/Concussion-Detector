using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownHandler : MonoBehaviour
{
    public GameObject menu;
    public GameObject settings;
    public GameObject patientDetails;

    public void activateMenu()
    {
        menu.SetActive(true);
    }

    public void deactivateMenu()
    {
        menu.SetActive(false);
        settings.SetActive(false);
        patientDetails.SetActive(false);
    }

    public void optionSelected()
    {
        menu.SetActive(false);
    }

    public void showSettings()
    {
        settings.SetActive(true);
        optionSelected();
    }

    public void showPatientDetails()
    {
        patientDetails.SetActive(true);
        optionSelected();
    }
}
