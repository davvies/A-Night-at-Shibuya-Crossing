using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Struct <c>SeralizeableScore</c> Score containers</summary>
[System.Serializable]
public struct SeralizeableScore {

    //Three highest scores are kept
    public RoundData roundOne;
    public RoundData roundTwo;
    public RoundData roundThree;

    #region Auxiliary functions
    public bool isRoundOneFree() => roundOne.highScoreTime == string.Empty;

    public bool isRoundTwoFree() => roundTwo.highScoreTime == string.Empty;

    public bool isRoundThreeFree() => roundThree.highScoreTime == string.Empty;
    #endregion
}

/// <summary>Struct <c>RoundData</c> Containers for individual rounds</summary>
[System.Serializable]
public struct RoundData 
{
    public int highScore; //highest cars passed
    public string highScoreTime; //highest time kept

}
