using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>Class <c>MainMenuFunctionality</c> Functionality for the main menu</summary>
[RequireComponent (typeof(Button))] [RequireComponent(typeof(TextMeshProUGUI))]
public class MainMenuFunctionality : MonoBehaviour
{
    [SerializeField] Animator tranisationAnim = default;

    const float standardTextSize = 81.6f;
    const float expandedTextSize = 88f;
    const float animationLength = 1.25f;

    #region 'Mouse Hover' events
    /// <summary>method <c>TextEnterScaleUp</c> Scales text up </summary>
    public void OnTextEnterScaleUp() => GetComponent<TextMeshProUGUI>().fontSize = expandedTextSize;

    /// <summary>method <c>TextEnterScaleDown</c> Scale text down </summary>
    public void OnTextExitScaleDown() => GetComponent<TextMeshProUGUI>().fontSize = standardTextSize;
    #endregion

    #region 'On Click' events
    public void StartGame() {
        QualitySettings.vSyncCount = 0; //stop object stuttering with physics
        StartCoroutine(LaunchStartScreen()); //begin transition to first level
    }

    public void EndGame() => Application.Quit();

    public void LaunchTutorial() => StartCoroutine(LaunchTutorialAnim());
    #endregion

    public void OnEnable() => Time.timeScale = 1;

    IEnumerator LaunchTutorialAnim()
    {
        MakeOtherButtonsNonInteractable();
        tranisationAnim.SetTrigger("playPreFade");
        yield return new WaitForSeconds(animationLength);
        SceneManager.LoadScene(2);
    }

    IEnumerator LaunchStartScreen()
    {
        MakeOtherButtonsNonInteractable();
        tranisationAnim.SetTrigger("playPreFade");
        yield return new WaitForSeconds(animationLength);
        SceneManager.LoadScene(1);
    }

    /// <summary>method <c>MakeOtherButtonsNonInteractable</c> Stop Button Clicking when transition</summary>
    void MakeOtherButtonsNonInteractable()
    {
        foreach(Transform childObject in transform.parent.transform)
        {
            if (childObject.name.Contains("BTN"))
            {
                childObject.gameObject.GetComponent<Button>().interactable = false;
            }
        }
       
    }
}
