using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    // Time in seconds to delay (five seconds for example):
    public float delayTime = 3f;
    public Image black;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DelayedAction", delayTime);
    }

    void DelayedAction(){
        StartCoroutine(Fading());
    }

    IEnumerator Fading()
    {
        anim.SetBool("Fade", true);
        yield return new WaitUntil(()=>black.color.a == 1);
        SceneManager.LoadScene("Main");
    }
}
