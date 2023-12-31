using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BattleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void GetCardList()
    {

    }

    public GameObject CanvasWorldGO;
    public CinemachineVirtualCamera CMCamera;
    public CinemachineVirtualCamera BattleCMCamera;
    void Start()
    {
        BattleUI_T = CanvasWorldGO.transform.Find("BattleUIGO");
        CMCamera = GetComponent<CinemachineVirtualCamera>();
        InitBattle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //bool isInBattle = false;
    public ShardsReflection glassshard_SCR;

    public void StartRandomEncounter()
    {
        if (!glassshard_SCR) glassshard_SCR = GameObject.Find("glasspaneGO").GetComponent<ShardsReflection>();
        glassshard_SCR.StartRandomEncounterFX();
        //displayrandomencounter behind UI
    }
    
    private Transform BattleUI_T;
    public GameObject player;

    public Transform camPos;
    public Transform enemyPos;
    public Transform playerPos;
    public Transform mobPos;
    void InitBattle()
    {
        //isInBattle = true;
        BattleCMCamera.enabled = true;
        CMCamera.enabled = false; 
        player.GetComponent<Transform>().transform.position = playerPos.position;
        //if animation is done
        BattleUI_T.gameObject.SetActive(true);
    }

}
