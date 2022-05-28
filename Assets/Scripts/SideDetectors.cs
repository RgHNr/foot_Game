using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideDetectors : MonoBehaviour
{
    // Start is called before the first frame update

    LevelChanger levelChanger;
    void Start()
    {
        levelChanger = GameObject.Find("LevelChanger").GetComponent<LevelChanger>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (SphereMove.IsInGoal)
            return;
        if (other.gameObject.name == "Sphere (1)") {
            levelChanger.outsideAnimation();
            SphereMove.IsInGoal = true;
        }
    }
}
