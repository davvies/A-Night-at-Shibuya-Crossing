using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

/// <summary>Class <c>SaveWrapper</c> Handles saving functionality</summary>
public static class SaveWrapper
{
    //Arbitrary directory data
    public static string saveDir = "/SaveData/"; 
    public static string fileName = "HighScores.txt";

    public static List<RoundData> highestScores; //use in the sorting process 

    /// <summary>Method <c>Save</c> Save to file</summary>
    public static void Save(SeralizeableScore score)
    {
        string dir = Application.persistentDataPath + saveDir;

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        string jsonToParse = JsonUtility.ToJson(score);

        File.WriteAllText(dir + fileName, jsonToParse); //write all scores to file
    }

    /// <summary>Method <c>Load</c> Load the currently stored data</summary>
    public static SeralizeableScore Load()
    {
        string fullPath = Application.persistentDataPath + saveDir + fileName; //acquire path for retrieval 
        SeralizeableScore score = new SeralizeableScore();

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            score = JsonUtility.FromJson<SeralizeableScore>(json);
        } else 
        {
            //if data does not exist init data with some default values
            score.roundOne.highScore = int.MinValue;
            score.roundTwo.highScore = int.MinValue;
            score.roundThree.highScore = int.MinValue;
            score.roundOne.highScoreTime = string.Empty;
            score.roundTwo.highScoreTime = string.Empty;
            score.roundThree.highScoreTime = string.Empty;
            Save(score);
        }

        return score;
    }

    /// <summary>Method <c>EvalData</c> There are times where data won't be stored, but needs to be 
    /// analysed</summary>
    public static void EvalData(RoundData score)
    {
        SeralizeableScore temp = Load();

        #region Check each slot for availability 
        if (temp.isRoundOneFree())
        {
            temp.roundOne = score;
            Save(temp);
            return;
        } 
        if (temp.isRoundTwoFree())
        {

            temp.roundTwo = score;
            Save(temp);
            return;
        }

        if (temp.isRoundThreeFree())
        {
            temp.roundThree = score;
            Save(temp);
            return;
        }
        #endregion

        //Acquire smallest
        int smallest = Mathf.Min(temp.roundOne.highScore, temp.roundTwo.highScore, temp.roundThree.highScore);

        //if the current smallest is to small for the score then don't save
        if (score.highScore < smallest) return;

        #region Replace the smallest value
        if (temp.roundOne.highScore == smallest)
        {
            temp.roundOne = score;
            Save(temp);
            return;
        }

        if (temp.roundTwo.highScore == smallest)
        {
            temp.roundTwo = score;
            Save(temp);
            return;
        }

        if (temp.roundThree.highScore == smallest)
        {
            temp.roundThree = score;
            Save(temp);
            return; 
        }
        #endregion

    }

    /// <summary>Method <c>SortData</c> Stored data is sorted</summary>
    public static List<RoundData> SortData()
    {
        highestScores = new List<RoundData>();
        SeralizeableScore score = Load();

        List<RoundData> temp = new List<RoundData>() { score.roundOne, score.roundTwo, score.roundThree }; //temp list for sorting

        highestScores = temp.OrderBy(w => w.highScore).ToList(); //sort using LINQ lib functionality
        return highestScores;
    }

}
