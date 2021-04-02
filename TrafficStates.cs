using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Class <c>TrafficStates</c> Manage individual traffic
/// lights and their states.</summary>
public class TrafficStates : MonoBehaviour
{
    #region Serial Private Objects
    [SerializeField] Image red = default, amber = default, green = default; //traffic image states
    [SerializeField] Light reflectiveLight = default; //light beam on road (symbolise colour)
    [SerializeField] float redLightWaitTime = 3; //time to wait on red before a countdown begins
    [SerializeField] GameObject timerHUD = default; //time countdown OBJ
    [SerializeField] GameState globalGameState = default; //traffic lights directly affect state
    #endregion

    #region Other declaration
    Color redC, amberC, greenC; //colours required
    TrafficLightColour currentLightColour = TrafficLightColour.GREEN; //by default game is ran with green lights
    public float redLightCounter = 0; // beginning trigger time for light
    public Slider slider; //reference to value of countdown timer
    int sliderIndex=0; //used in O(1) lookup time using .find functions
    #endregion

    void Start()
    {
        //Colour declaration
        redC = Color.red;
        amberC = new Color(0.976f, 1, 0f);
        greenC = new Color(0.490f, 1, 0);

        slider = timerHUD.transform.GetChild(sliderIndex).GetComponent<Slider>();
    }

    void Update() => LightStateLogic();

    #region Finite state logic
    public bool IsTrafficLightRed() => currentLightColour == TrafficLightColour.RED; //Cars require traffic data

    void LightStateLogic()
    {
        switch (currentLightColour)
        {

            case TrafficLightColour.RED:
                amber.color = new Color(amber.color.r, amber.color.g, amber.color.b, 0f);
                green.color = new Color(green.color.r, green.color.g, green.color.b, 0f);
                red.color = new Color(red.color.r, red.color.g, red.color.b, 1f);
                reflectiveLight.color = redC;
                redLightCounter += Time.deltaTime;

                break;

            case TrafficLightColour.AMBER:
                amber.color = new Color(amber.color.r, amber.color.g, amber.color.b, 1f);
                green.color = new Color(green.color.r, green.color.g, green.color.b, 0f);
                red.color = new Color(red.color.r, red.color.g, red.color.b, 0f);
                reflectiveLight.color = amberC;
                redLightCounter = 0;
                break;

            case TrafficLightColour.GREEN:
                amber.color = new Color(amber.color.r, amber.color.g, amber.color.b, 0f);
                green.color = new Color(green.color.r, green.color.g, green.color.b, 1f);
                red.color = new Color(red.color.r, red.color.g, red.color.b, 0f);
                reflectiveLight.color = greenC;
                redLightCounter = 0; 
                
                break;

        }

        if(redLightCounter > redLightWaitTime)
        {
            if (!timerHUD.gameObject.activeSelf) //stop continuous draw calls
            {
                timerHUD.SetActive(true);
            }
            slider.value -= Time.deltaTime; //slowly reduce time on timer while active

            if (slider.value <= 0)
            {
                globalGameState.EndGame(); //signify end of game globally
                globalGameState.lightsExpiredRed = true; //set global reason for disruption
            }

        }
        else
        {
            timerHUD.SetActive(false);
            slider.value = slider.maxValue;
        }
    }

    public enum TrafficLightColour
    {

        RED, AMBER, GREEN

    }

    public void ChangeLightToRed() => StartCoroutine(ChangeToColour(TrafficLightColour.RED));

    public void ChangeLightToAmber() => currentLightColour = TrafficLightColour.AMBER;

    public void ChangeLightToGreen() => StartCoroutine(ChangeToColour(TrafficLightColour.GREEN));

    const float amberWaitTime = 0.30f; 

    IEnumerator ChangeToColour(TrafficLightColour givenColour)
    {
        currentLightColour = TrafficLightColour.AMBER;
        yield return new WaitForSeconds(amberWaitTime);
        currentLightColour = givenColour;
    }

    #endregion
}
