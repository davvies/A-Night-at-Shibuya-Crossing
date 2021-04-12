using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>Class <c>PauseMenuFunctionality</c> Handle pause functionality</summary>
public class PauseMenuFunctionality : MonoBehaviour
{

    [SerializeField] GameObject mainPauseScreen = default;
    [SerializeField] float standardFontSize = 61.72f;
    [SerializeField] float inflatedFontSize = 75f;

    #region 'On Click' events
    public void OnClickResumeGame() => mainPauseScreen.SetActive(false);

    public void OnClickExitToMenu()
    {
        Time.timeScale = 1; //handle edge case
        SceneManager.LoadScene(0);
    }
    #endregion

    #region 'Mouse Hover' events
    public void OnMouseOver() => transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = inflatedFontSize;

    public void OnMouseExit() => transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = standardFontSize;
    #endregion
}
