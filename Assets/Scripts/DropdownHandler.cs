using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownHandler : MonoBehaviour
{
    public GameObject menu;

    public void activateMenu()
    {
        menu.SetActive(true);
    }

    public void deactivateMenu()
    {
        menu.SetActive(false);
    }

    public void optionSelected()
    {
        menu.SetActive(false);
    }
}
