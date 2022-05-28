using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalChecker : MonoBehaviour
{
    LevelChanger levelChanger;
    // Start is called before the first frame update
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
        if (other.name == "Sphere (1)") {
            Debug.Log("goooooooooal");
            levelChanger.goalAnimation();
            SphereMove.IsInGoal = true;
        }
    }
}
