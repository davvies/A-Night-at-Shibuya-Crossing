using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>Class <c>TutorialGoToMenu</c> A script to handle a button edge case
/// within the tutorial screen</summary>
public class TutorialGoToMenu : MonoBehaviour
{
    const float scaleFactor = 1.2f;

    public void OnClickHeadToMenu() => SceneManager.LoadScene(0);

    public void OnHoverExpandBTN() => transform.localScale *= scaleFactor;

    public void OnHoverShrinkBTN() => transform.localScale /= scaleFactor;
}
