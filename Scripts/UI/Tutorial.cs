using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>Class <c>Tutorial</c> Tutorial functionality</summary>
public class Tutorial : MonoBehaviour
{
    [SerializeField] Animator shrinkGrow = default; //tutorial UI overlay
    [SerializeField] Animator levelTransition = default; //level fading

    int tutorialIndex = 0; //index to manage images
    Dictionary<int, string> tutorialDialogue; //dict for updating text relative to index
    float scaleFactor = 1.2f; //text scalar factor
    const float transitionLength = 1.25f; //menu transition length 
    const float currentOverlayFadeTime = 1f; //length of animation for fade out
    const float animIntroDelay = 0.5f;
    bool clickableButtons = true;
    const float fullAnimLength = currentOverlayFadeTime * 2; 

    //Image relative objects
    [SerializeField] Sprite crossingFirst = default, trafficLightImage = default, countdownImage = default, finalImage=default;
    [SerializeField] Image primaryBackground = default;
    [SerializeField] Text displayText = default;
    [SerializeField] Button buttonsClickable = default;

    void Start()
    {
        StartCoroutine(PlayIntroAnim()); //fade in

        //All dialog options within the tutorial window 
        tutorialDialogue = new Dictionary<int, string>() {
            {0, "Welcome traffic warden!"},
            {1, "It seems quiet at the moment!"},
            {2, "Your goal is to keep traffic flowing by using your mouse cursor to turn on/off lights"},
            {3, "Don't keep the lights on too long!" },
            {4, "Hurry now, it's getting busy!" }
            
        };

        primaryBackground.sprite = crossingFirst;
    }

    IEnumerator MakeButtonsInactive()
    {
        gameObject.GetComponent<Button>().interactable = false;
        buttonsClickable.interactable = false;
        yield return new WaitForSeconds(fullAnimLength);
        buttonsClickable.interactable = true;
        gameObject.GetComponent<Button>().interactable = true;
        clickableButtons = true;
    }

    IEnumerator PlayIntroAnim()
    {
        levelTransition.SetTrigger("playPostFade");
        clickableButtons = false;
        yield return new WaitForSeconds(animIntroDelay);
        shrinkGrow.SetTrigger("GuideGrow");
    }

    IEnumerator PlayOutroAnim()
    {
        levelTransition.SetTrigger("playPreFade");
        yield return new WaitForSeconds(transitionLength);
        SceneManager.LoadScene(1);
    }

    /// <summary>Method <c>ShrinkGrow</c> Play text transition animation</summary>
    IEnumerator ShrinkGrow()
    {
        shrinkGrow.SetTrigger("GuideShrink");
        clickableButtons = false;
        yield return new WaitForSeconds(currentOverlayFadeTime);
        tutorialIndex++; 
        displayText.text = tutorialDialogue[tutorialIndex]; //update image relative to state
        shrinkGrow.SetTrigger("GuideGrow");
    }

     void Update()
    {
        if (clickableButtons == false && GetComponent<Button>().interactable == true) StartCoroutine(MakeButtonsInactive());

        #region Background image states
        switch (tutorialIndex)
        {
            case 0:
            case 1:
                if(primaryBackground.sprite!=crossingFirst) primaryBackground.sprite = crossingFirst;
                break;

            case 2:
                if(primaryBackground.sprite!=trafficLightImage) primaryBackground.sprite = trafficLightImage;
                break;

            case 3:
                if(primaryBackground.sprite!=countdownImage) primaryBackground.sprite = countdownImage;
                break;

            case 4:
                primaryBackground.sprite = finalImage;
                break;
        }
        #endregion
    }

    public void OnClickNextSlide()
    {      
        StartCoroutine(ShrinkGrow());
        if (tutorialIndex == 4) StartCoroutine(PlayOutroAnim()); //at the end play animation to transition to playing
    }


    #region 'Hover over' events 
    public void OnHoverExpandBTN() => transform.localScale *= scaleFactor;

    public void OnHoverShrinkBTN() => transform.localScale /= scaleFactor;
    #endregion
}
