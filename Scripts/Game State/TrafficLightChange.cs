using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Class <c>TrafficLightChange</c> OnClick events for traffic lights</summary>
public class TrafficLightChange : MonoBehaviour
{
    [SerializeField] TrafficStates localTrafficLights = default; //gather states

    #region 'OnClick' methods for changing lights
    //*OnClick Methods are called on indivdual traffic buttons*
    public void OnClickChangeLightToGreen() => localTrafficLights.ChangeLightToGreen();

    public void OnClickChangeLightToAmber() => localTrafficLights.ChangeLightToAmber();

    public void OnClickChangeLightToRed() => localTrafficLights.ChangeLightToRed();
    #endregion
}
