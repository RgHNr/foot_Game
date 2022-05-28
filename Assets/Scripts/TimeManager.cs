using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static float slowDownFactor = 0.05f;

    public static void doSlowmotion() {

        Time.timeScale = slowDownFactor;
        

        Time.fixedDeltaTime = Time.deltaTime * 0.02f;
    }

    public static void stopSlowmotion() {

        Time.timeScale = 1;
        
        Time.fixedDeltaTime = 0.02f;
    }

    public static void pauseGame() {

        Time.timeScale = 0;
        Debug.Log("timescale= "+Time.timeScale);
    }
}
