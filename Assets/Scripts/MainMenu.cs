using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playBall;
    void Start()
    {
        Time.timeScale = 1;
        LeanTween.rotateZ(playBall, 720, 2f).setLoopType(LeanTweenType.easeInOutSine);
        LeanTween.moveLocalY(playBall, playBall.transform.position.y + 3f, 1f).setEaseInBounce().setLoopPingPong();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
