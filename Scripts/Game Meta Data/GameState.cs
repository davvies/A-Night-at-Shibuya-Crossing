using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Class <c>GameState</c> Manage game state behaviour</summary>
public class GameState : MonoBehaviour
{
    [SerializeField] PossibleStates currentGameState;
    [SerializeField] Canvas pauseMenu = default;
    [SerializeField] RoundStatTracker gameFinishedData = default;
    [SerializeField] GameObject endScreen = default;
    [SerializeField] Animator endAnimator = default;
    [SerializeField] Text reasonForCrash = default;

    public bool lightsExpiredRed = false; 

    RoundData roundData;

    bool DEBUGSHOWNONCE = false; //aux method for displaying scores in debug

    void Start()
    {
        currentGameState = PossibleStates.RUNNING; //by default the game will be running
        roundData = new RoundData(); //struct container for storing end of round data
    }

    void Update()
    {
        #region Game state machine
        switch (currentGameState) {


            case PossibleStates.RUNNING:

                if (Input.GetKeyDown(KeyCode.Escape) || pauseMenu.isActiveAndEnabled)
                {
                    currentGameState = PossibleStates.PAUSED;
                    PauseGame();
                }

            break;

            case PossibleStates.PAUSED:

                if (Input.GetKeyDown(KeyCode.Escape) || !pauseMenu.isActiveAndEnabled)
                {
                    currentGameState = PossibleStates.RUNNING;
                    ResumeGame();
                    
                }

            break;

            case PossibleStates.ENDED:

                if (DEBUGSHOWNONCE == false)
                {
                    ShowGameOverLeaderboard();
                    DEBUGSHOWNONCE = true; //stop repeating data within the console
                    roundData.highScore = gameFinishedData.totalCarsPassed;
                    roundData.highScoreTime = gameFinishedData.totalNormalisedLevelTime; 
                    SaveWrapper.EvalData(roundData); //pass data to leaderboards
                }
                break; 
        }
        #endregion
    }

    enum PossibleStates { 
    
        RUNNING, PAUSED, ENDED //possible game states

    }

    #region State Aux Methods
    public void RunGame() => currentGameState = PossibleStates.RUNNING;

    public bool IsGameRunning() => currentGameState == PossibleStates.RUNNING;

    public void EndGame() => currentGameState = PossibleStates.ENDED;
    #endregion

    #region 'On Click' state methods
    void PauseGame()
    {
        pauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0; 
    }

    void ResumeGame()
    {
        pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1; 
    }

    void ShowGameOverLeaderboard()
    {
        pauseMenu.gameObject.SetActive(false);
        reasonForCrash.text = lightsExpiredRed == false ? "You caused a crash!" : "You left a red light on!";
        StartCoroutine(FadeAnim());
        Debug.Log("Cars passed: " + gameFinishedData.totalCarsPassed + ", Final Level Time: " + gameFinishedData.totalNormalisedLevelTime);
    }

    IEnumerator FadeAnim()
    {
        //Time.timeScale = 0.5f;
        endScreen.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        endAnimator.gameObject.SetActive(true);
        endAnimator.SetTrigger("FadeIn");
    }
    #endregion
}
