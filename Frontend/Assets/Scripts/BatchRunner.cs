using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatchRunner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Starts an external process, in this case it's a batch file to run python script
        Process.Start("C:\Users\Wojtek Pogorzelski\Desktop\FourthYearProject\Concussion-Detector\Backend\run.bat");
    }
}
