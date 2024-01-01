using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineEffect : MonoBehaviour
{

    public static CinemachineEffect Instance { get; private set; }
    CinemachineVirtualCamera CMCam;
    GameObject player;

    private void Awake()
    {
        Instance = this;
        CMCam = GetComponent<CinemachineVirtualCamera>();
        if(!player) player = GameObject.Find("Player");
        CMCam.m_Follow = player.transform;
    }
    void Start()
    {
        
    }

    public void ZoomIn()
    {

    }
    public void ZoomOut()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                CinemachineBasicMultiChannelPerlin CMBasicMultiChannelPerlin =
                CMCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                CMBasicMultiChannelPerlin.m_AmplitudeGain = 0;
            }
        }
    }

    float shakeTimer = 0;
    public void ShakeVCam(float magnitude, float duration)
    {
        CinemachineBasicMultiChannelPerlin CMBasicMultiChannelPerlin =
        CMCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        CMBasicMultiChannelPerlin.m_AmplitudeGain = magnitude;
        shakeTimer = duration;
    }



}
