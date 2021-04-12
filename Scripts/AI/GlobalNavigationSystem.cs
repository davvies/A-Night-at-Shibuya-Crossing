using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Class <c>GlobalNavigationSystem</c> Handle container data for routes</summary>
public class GlobalNavigationSystem : MonoBehaviour
{
    #region Routes
    [SerializeField] GameObject[] RouteOne = default;
    [SerializeField] GameObject[] RouteTwo = default;
    [SerializeField] GameObject[] RouteThree = default;
    [SerializeField] GameObject[] RouteFour = default;
    [SerializeField] GameObject[] RouteFive = default;
    [SerializeField] GameObject[] RouteSix = default;
    [SerializeField] GameObject[] RouteSeven = default;
    [SerializeField] GameObject[] RouteEight = default;
    [SerializeField] GameObject[] RouteNine = default;
    [SerializeField] GameObject[] RouteTen = default;
    [SerializeField] GameObject[] RouteEleven = default;
    [SerializeField] GameObject[] RouteTweleve = default;
    [SerializeField] GameObject[] RouteThirteen = default;
    [SerializeField] GameObject[] RouteFourteen = default;
    [SerializeField] GameObject[] RouteFifteen = default;
    [SerializeField] GameObject[] RouteSixteen = default;
    #endregion

    Dictionary<string, GameObject[]> pathKV; //list of available paths, relative to lane

    void Start()
    {
        pathKV = new Dictionary<string, GameObject[]>
        {
            {"L1", RouteOne },
            {"L1ALTERNATIVE", RouteTwo },
            {"L2", RouteThree},
            {"L2ALTERNATIVE", RouteFour },
            {"L3", RouteFive },
            {"L3ALTERNATIVE", RouteSix },
            {"L4", RouteSeven },
            {"L4ALTERNATIVE", RouteEight},
            {"L5", RouteNine },
            {"L5ALTERNATIVE", RouteTen },
            {"L6", RouteEleven },
            {"L6ALTERNATIVE", RouteTweleve },
            {"L7", RouteThirteen },
            {"L7ALTERNATIVE", RouteFourteen },
            {"L8", RouteFifteen },
            {"L8ALTERNATIVE", RouteSixteen }
        }; //Init Routes via nodes
    }

    /// <summary>method <c>GetNavigationPath</c> returns a path given a spawned lane</summary>
    public GameObject[] GetNavigationPath(string laneKey) {

        string key = Random.Range(0f, 1f) > 0.5f ? laneKey += "ALTERNATIVE" : laneKey; //get a 50/50 chance of a route relative to lane

        return pathKV[key]; //return route
    }

}
