using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Class <c>TrafficLightSystem</c> Handle mouse over event raycasting 
/// for traffic lights</summary>
public class TrafficLightSystem : MonoBehaviour
{
    #region Declarations
    RaycastHit hit;
    const int lightPopupIndex = 5;
    GameObject cached = null;
    public List<int> activeTrafficLights;
    [SerializeField] GameState gameState = default;
    #endregion

    private void Start() => activeTrafficLights = new List<int>();

    void Update()
    {
        if(gameState.IsGameRunning()) OnMouseTrafficLightHover();
    }

    #region 'MouseOver' - traffic light event logic 
    /// <summary>method <c>OnMouseTrafficLightHover</c> Handles mouse over events </summary>
    void OnMouseTrafficLightHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //gather mouse data

        if (Physics.Raycast(ray, out hit))
        {

            if (hit.transform.name.Contains("Light"))
            {
                if (cached == null) cached = hit.transform.gameObject; //keep a log of the last light raycast
                else if (!cached.name.Equals(hit.transform.name)) cached = hit.transform.gameObject;

                hit.transform.GetChild(lightPopupIndex).gameObject.SetActive(true); //activate lights

            }
            else if (cached != null && !hit.transform.name.Contains("Overlay"))
            {
                cached.transform.GetChild(lightPopupIndex).gameObject.SetActive(false); //on mouse off remove lights
            }

        }
    }
    #endregion

}
