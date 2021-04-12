using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>Class <c>EndScreenOverlay</c> Functionality for the end screen</summary>
[RequireComponent(typeof(Button))]
public class EndScreenOverlay : MonoBehaviour
{
    [SerializeField] Animator levelTransition = default;
    [SerializeField] Animator shrinkGrow = default;
    [SerializeField] float scaleFactor = 1.2f; //amount to scale up/down on hover 

    //Endscreen objects are split to allow for modularity in animations
    [SerializeField] GameObject mainEndScreen = default;
    [SerializeField] GameObject leaderboard = default; 

    [SerializeField] GameObject score1 = default; //highest score
    [SerializeField] GameObject score2 = default; //mid score
    [SerializeField] GameObject score3 = default; //lowest score

    [SerializeField] Button[] LocalButtons = default;

    const float transitionLength = 1.25f; //time for endscreen fade
    const float currentOverlayFadeTime = 1f; //standard overlay time for start transition
    const float timeToDisable = currentOverlayFadeTime * 2;
    bool shouldBeInteractable = true;

    private void OnEnable()
    {
        shouldBeInteractable = true;
    }

    IEnumerator stopButtonInteractivity()
    {
        foreach(Button button in LocalButtons)
        {
            button.interactable = false;
        }
        yield return new WaitForSeconds(timeToDisable);
        foreach(Button button in LocalButtons)
        {
            button.interactable = true;
        }
        shouldBeInteractable = true;
    }

    #region 'OnClick' methods
    public void OnClickShowStats() => StartCoroutine(GoToLeaderboard());

    public void OnClickGoBackToEndScreen() => StartCoroutine(GoBackToExitScreen());

    public void OnClickBackToMenu() => SceneManager.LoadScene(0);

    public void OnClickResetLevel() => StartCoroutine(PlayTransition());
    #endregion

    #region 'OnMouseHover' events
    public void OnHoverExpandBTN() => transform.localScale *= scaleFactor;

    public void OnExitExpandBTN() => transform.localScale /= scaleFactor;
    #endregion

    private void Update()
    {
        if (shouldBeInteractable == false && GetComponent<Button>().interactable == true) StartCoroutine(stopButtonInteractivity());
    }

    /// <summary>Coroutine <c>PlayTransition</c> Play fade out animation</summary>
    IEnumerator PlayTransition()
    {
        shouldBeInteractable = false;
        shrinkGrow.SetTrigger("FadeOut");
        yield return new WaitForSeconds(currentOverlayFadeTime);     
        StopScreenRenderAndKeepActive(); //stops strange bug where objects need to be still active
        levelTransition.SetTrigger("playPreFade");
        yield return new WaitForSeconds(transitionLength);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>Coroutine <c>PlayTransition</c> Play fade out animation</summary>
    void StopScreenRenderAndKeepActive() //this function is needed as setting the object to be non-active won't run the script
    {
        shouldBeInteractable = false;
        gameObject.GetComponent<Image>().enabled = false; 
        foreach(Transform givenChild in mainEndScreen.transform)
        {
            if (givenChild != gameObject.transform) givenChild.gameObject.SetActive(false); //this call is justified as not only is the runtime O(n) where n = 11, but it does not matter about draw calls as the scene is reset
        }
        return; 
    }

    /// <summary>Coroutine <c>GoBackToExitScreen</c> Async back to the main end game overlay</summary>
    IEnumerator GoBackToExitScreen()
    {
        shrinkGrow.SetTrigger("FadeOut");
        shouldBeInteractable = false;
        yield return new WaitForSeconds(currentOverlayFadeTime);
        mainEndScreen.SetActive(true);
        leaderboard.SetActive(false);
        shrinkGrow.SetTrigger("FadeIn");
    }

    /// <summary>Coroutine <c>GoBackToLeaderboard</c> Async to the leaderboard overlay</summary>
    IEnumerator GoToLeaderboard()
    {
        shrinkGrow.SetTrigger("FadeOut");
        shouldBeInteractable = false;
        yield return new WaitForSeconds(currentOverlayFadeTime);
        mainEndScreen.SetActive(false);
        leaderboard.SetActive(true);
        CacheHighscores();
        shrinkGrow.SetTrigger("FadeIn");
    }

    void CacheHighscores()
    {
        List<RoundData> sortedData = SaveWrapper.SortData(); //temp sorted data

        //store it in the leaderboard
        AppendText(score1, 2, ref sortedData);
        AppendText(score2, 1, ref sortedData);
        AppendText(score3, 0, ref sortedData);
        
    }

    void AppendText(GameObject givenScore, int index, ref List<RoundData> data)
    {
        givenScore.transform.GetChild(0).GetComponent<Text>().text = data[index].highScore == int.MinValue ? "N/A" : "Cars Passed: " + data[index].highScore;
        givenScore.transform.GetChild(1).GetComponent<Text>().text = data[index].highScoreTime == string.Empty ? "N/A" : "Total Time: " + data[index].highScoreTime;
    }

    
}
