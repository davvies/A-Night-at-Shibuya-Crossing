using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Class <c>MiscBillboardColourLerp</c> Background Animation handled
/// through code, to take load off animator</summary>
[RequireComponent(typeof(Light))]
public class MiscBillboardColourLerp : MonoBehaviour
{
    //Lerping colours
    Color original = new Color(0.85f, 0.85f, 0.93f); 
    Color flicker = new Color(0.17f, 0.22f, 0.97f);

    Light billboardLight =default; //reflection light

    [SerializeField] float lightSpeed = default; //flicker
    const float pingPongMax = 1f; //"bounce" speed of flicker

    void Start() => billboardLight = GetComponent<Light>();

    void Update()
    {
        //ping pong values to create a TV flicker effect
        float t = Mathf.PingPong(Time.time * lightSpeed, pingPongMax);
        billboardLight.color = (Color.Lerp(original,flicker,t));
    }
}
