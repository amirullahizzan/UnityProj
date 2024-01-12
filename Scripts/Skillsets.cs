using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Skillsets : MonoBehaviour
{
    PhysicsEvent physicsEvent;
    public Transform NormalAttackGO;
    PlayerControl playerControl;
    BattleControl battleControl;

    private void Awake()
    {
        playerControl = GetComponent<PlayerControl>();
        physicsEvent = GetComponent<PhysicsEvent>();
        battleControl = GetComponent<BattleControl>();
        // = playerControl.PointerGO.transform.Find("NormalAttackGO");
    }
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        if (NormalAttackGO.gameObject.activeSelf) { ResetColliders(); }
    }
    Coroutine gravityCoroutine;
    Coroutine moveCoroutine;

    void DealDamage(int damage)
    {
        currentSkillDamage = damage;
    }

    //bool isNormalAttack = false;
    public void NormalAttack()
    {

    }
    public void NormalAttackFixed()
    {
        if (!playerControl.IsGrounded() && gravityCoroutine != null) { StopCoroutine(gravityCoroutine); }
        gravityCoroutine = StartCoroutine(physicsEvent.ISelfPauseGravity(0.3f));
        if (!playerControl.IsGrounded() && moveCoroutine != null) { StopCoroutine(moveCoroutine); }
        physicsEvent.StartCoroutine(physicsEvent.IPauseMove(0.2f));
        switch (battleControl.GetSlashingSequence())
        {
            case 1:
                if (playerControl.IsSprinting()) { physicsEvent.PushSelf(30.0f); }
                break;
            case 2:
                if (playerControl.IsSprinting()) { physicsEvent.PushSelf(50.0f); }
                break;
        }

        NormalAttackGO.gameObject.SetActive(true);
        DealDamage(25);
        CinemachineEffect.Instance.ShakeVCam(0.2f, .1f);
        physicsEvent.PushSelf(5.0f);
        
    }
    public void Blink()
    {
        physicsEvent.PushSelf(10.0f);
    }
    public void ResetColliders()
    {
        NormalAttackGO.gameObject.SetActive(false);
    }

    int currentSkillDamage = 0;
    public int GetDamage()
    {
        return currentSkillDamage;
    }
}
