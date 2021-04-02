using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Class <c>RoundStatTracker</c> Tracking round stats</summary>
public class RoundStatTracker : MonoBehaviour
{
    [SerializeField] GameState currentState = default;
    [SerializeField] RoundData roundData = default;

    void Start() => roundData = new RoundData();

    void Update()
    {
        if (currentState.IsGameRunning()) roundData.levelTime += Time.deltaTime; //track level time
    }

    /// <summary>method <c>FormatTimeData</c> Format floating point data to a usable time</summary>
    string FormatTimeData(float timeInSeconds)
    {
        int minutes = (int)timeInSeconds / 60;
        int seconds = (int)timeInSeconds - 60 * minutes;
        int milliseconds = (int)(1000 * (timeInSeconds - minutes * 60 - seconds));
        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    public void IncrementCarCount() => roundData.carsPassed += 1; //when a car has gone off-screen

    public int totalCarsPassed => roundData.carsPassed; //keep track of data

    public string totalNormalisedLevelTime => FormatTimeData(roundData.levelTime); //store conversion

    /// <summary>Struct <c>RoundData</c> Temp datatype of storage</summary>
    public struct RoundData
    {
        public int carsPassed { get; set; }
        public float levelTime { get; set; }
    }

}
