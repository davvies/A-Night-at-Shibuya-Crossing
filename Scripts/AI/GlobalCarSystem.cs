using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>Class <c>GlobalCarSystem</c> A management class for handling all cars</summary>
public class GlobalCarSystem : MonoBehaviour
{
    #region Internal serialize data for cars 
    [SerializeField] GameObject[] CarTypes = default; //can contain duplicates
    [SerializeField] GameObject[] Lanes = default;
    [SerializeField] GlobalNavigationSystem navigationSystem = default;
    [SerializeField] float deltaSpawnTime = 1.3f; //rate at which cars are brought back off-screen
    #endregion

    public List<int> AvailableCarsForSpawn; //list of all non-active cars

    float internalClock = 0; //handle spawn in a clock pattern
    float startClock = 0;  //start of game delay

    void Start()
    {
        AvailableCarsForSpawn = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }; //all cars are available at start
    }

    #region Per-frame car clock
    void Update() => ExecuteCarClockCycle();

    /// <summary>Method <c>AddCarBackToQueue</c> After car is off-screen add back to queue</summary>
    public void AddCarBackToQueue(int ID)
    {
        if (!AvailableCarsForSpawn.Contains(ID)) //defensive programming
        {
            AvailableCarsForSpawn.Add(ID); //add car back to available lists
        }
    }

    void ExecuteCarClockCycle()
    {
        internalClock += Time.deltaTime;
        startClock += Time.deltaTime;

        if (startClock > 5) //initial starting delay
        {
            if (internalClock >= deltaSpawnTime) //fixed time delay
            {
                if (AvailableCarsForSpawn.Count > 0)
                {
                    int randomCarIndex = Random.Range((AvailableCarsForSpawn.Count - 1) - (AvailableCarsForSpawn.Count - 1), AvailableCarsForSpawn.Count - 1);
                    int randomLaneIndex = Random.Range(0, Lanes.Length);
                    GameObject car = CarTypes[AvailableCarsForSpawn[randomCarIndex]]; //get a random available car
                    car.SetActive(true);
                    Transform lane = Lanes[randomLaneIndex].transform; //get a random lane (1-8)
                    car.GetComponent<LocalCarNavigation>().activeRoute = navigationSystem.GetNavigationPath(lane.name);

                    //Car transform data relative to lane
                    car.transform.position = lane.gameObject.transform.position;
                    car.transform.rotation = lane.gameObject.transform.rotation;

                    AvailableCarsForSpawn.Remove(AvailableCarsForSpawn[randomCarIndex]); //remove car availability after its left
                }
                internalClock = 0; //clock cycle reset
                deltaSpawnTime -= 0.045f; //rate of car decrease
            }
        }
    }
    #endregion

}
