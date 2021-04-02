using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Class <c>LevelFadeTransition</c> Handle black screen effect throughout game</summary>
public class LevelFadeTransition : MonoBehaviour
{
    [SerializeField] Animator anim = default;

    void Start()
    {
        anim.SetTrigger("playPostFade");
        StartCoroutine(TurnOffIntroFade());
    }

    IEnumerator TurnOffIntroFade()
    {
        yield return new WaitForEndOfFrame(); //avoid any anomalies by waiting for the end of a frame to launch another anim
        anim.ResetTrigger("playPostFade");
       
    }
 
}
