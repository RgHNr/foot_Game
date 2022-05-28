using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamShake : MonoBehaviour
{
    // Start is called before the first frame update
    private CinemachineVirtualCamera myCam;


    public static CamShake Instance { get; private set; }
    void Awake()
    {
        Instance = this;
        myCam = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    public void shakeCamera() {

        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannel = myCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannel.m_AmplitudeGain = 0.5f;
    }

    public void StopCameraShake() {

        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannel = myCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannel.m_AmplitudeGain = 0;
    }
}
