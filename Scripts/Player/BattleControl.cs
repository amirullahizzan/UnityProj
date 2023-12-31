using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using Cinemachine;

public class BattleControl : MonoBehaviour
{

    Rigidbody rb;
    PlayerControl playerControl;
    public IAction currentAttack;
    public IAction currentDefence;

    enum PlayerState
    {
        NoAttack,
        NormalAttack,
    };

    Skillsets skillsets;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerControl = GetComponent<PlayerControl>();
        skillsets = GetComponent<Skillsets>();
        //AssignSkill_Attack(GetComponent<Slash>());
        //AssignSkill_Attack(GetComponent<SlamGround>());
        //AssignSkill_Def(GetComponent<Block>());
    }

    PlayerState currentState = PlayerState.NoAttack;
    private void FixedUpdate()
    {
        if (GameManager.Instance.GetGameplayFrozen()) { currentState = PlayerState.NoAttack; }
        switch (currentState)
        {
            case PlayerState.NoAttack:
                break;

            case PlayerState.NormalAttack:
                skillsets.NormalAttack();
                ResetVelocity();
                currentState = PlayerState.NoAttack;
                break;
        }
    }
    bool isResetVelocityTrigger = false;
    void ResetVelocity()
    {
        isResetVelocityTrigger = true;
    }
    
    float buttonhold_timer = 0f;
    bool IsHold_J()
    {
        const float HOLD_DUR = 0.6f;
        if(Input.GetKey(KeyCode.J))
        {
        buttonhold_timer += Time.deltaTime;
        }
        return buttonhold_timer >= HOLD_DUR;
    }
    bool J_Pressed = false;
    bool iscontrolFrozen = false;

    void HoldAttack()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            J_Pressed = true;
        }
        if (J_Pressed && Input.GetKeyUp(KeyCode.J))
        {
            currentState = PlayerState.NormalAttack;
            buttonhold_timer = 0;
            J_Pressed = false;
        }
        else if (J_Pressed && IsHold_J())
        {
            print("SHOCKWAVE");
            buttonhold_timer = 0;
            J_Pressed = false;
        }
    }
    void Update()
    {
        if (!iscontrolFrozen)
        {
            HoldAttack();

            if (Input.GetKeyDown(KeyCode.L))
            {
                rb.velocity = Vector3.zero;
            }

        }

        if (isResetVelocityTrigger)
        {
            rb.velocity = Vector3.zero;
            isResetVelocityTrigger = false;
        }
        CancelGravityOnMove();
    }

    int currentSkillDamage = 0;
    public int GetDamage()
    {
        return currentSkillDamage;
    }

    void CancelGravityOnMove()
    {
        if (playerControl.IsMoving())
        {
            rb.useGravity = true;
        }
    }

}
