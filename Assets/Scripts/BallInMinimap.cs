using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInMinimap : MonoBehaviour
{
    // Start is called before the first frame update
    Quaternion rot;
    void Start()
    {
        rot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = rot;
    }
}
