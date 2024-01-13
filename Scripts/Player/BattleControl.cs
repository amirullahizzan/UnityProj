using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using Cinemachine;
using Cinemachine.Utility;

public class BattleControl : MonoBehaviour
{
    Rigidbody rb;
    PlayerControl playerControl;
    PhysicsEvent physicsEvent;
    Slash slash_abil;
    enum PlayerState
    {
        NoAttack,
        NormalAttack,
        Blink,
    };

    Skillsets skillsets;

    private void Awake()
    {
        playerControl = GetComponent<PlayerControl>();
        physicsEvent = GetComponent<PhysicsEvent>();
        skillsets = GetComponent<Skillsets>();

        rb = physicsEvent.playerRb;
        slash_abil = skillsets.slash_abil;
    }
    void Start()
    {
        
    }

    PlayerState currentState = PlayerState.NoAttack;

    void Key_K()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            currentState = PlayerState.Blink;
            if (Input.GetKey(KeyCode.LeftControl))
            {

                return;
            }
        }
    }
    void Update()
    {
        if (IsControllable() && !playerControl.GetVoidboardMode())
        {
            Key_J();
            Key_K();
            if(Input.GetKeyDown(KeyCode.I))
            {

            }
        }

        if (isResetVelocityTrigger)
        {
            rb.velocity = Vector3.zero;
            isResetVelocityTrigger = false;
        }
        RestoreGravityOnMove();
        TickSlashState();

        switch (currentState)
        {
            case PlayerState.NoAttack:
                break;

            case PlayerState.NormalAttack:
            slashingstate++;
            slash_abil.cooldown = Slash.BASE_cooldown;
            if (slashingstate > 2)
            {
                slashingstate = 1;
            }
            playerControl.DisableMove();
            skillsets.NormalAttack();
            SlashingAnimation();
                break;

                case PlayerState.Blink:
                skillsets.Blink();
                break;
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.GetGameplayFrozen()) { currentState = PlayerState.NoAttack; }
        switch (currentState)
        {
            case PlayerState.NoAttack:
                
                break;

            case PlayerState.NormalAttack:
                skillsets.NormalAttackFixed();
                
                ResetVelocity();
                break;

            case PlayerState.Blink:
                skillsets.BlinkFixed();
                break;
        }
        currentState = PlayerState.NoAttack;

    }

    void SlashingAnimation()
    {
        switch (GetSlashingSequence())
        {
            case 1:
                playerControl.animator.SetTrigger("slash1");
                break;
            case 2:
                playerControl.animator.SetTrigger("slash2");
                break;
        }
    }

    int slashingstate = 0;
    const float BASE_slashstatetimer = 0.8f;
    float slashstatetimer = BASE_slashstatetimer;
   
    void TickSlashState()
    {
        if (slashingstate > 0)
        {
            slashstatetimer -= Time.deltaTime;
            if (slashstatetimer <= 0)
            {
                slashstatetimer = BASE_slashstatetimer;
                slashingstate = 0;
            }

            if(slash_abil.cooldown <= 0)
            {
                //playerControl.EnableMove();
            }
        }

        if (slash_abil.cooldown > 0)
        {
            slash_abil.cooldown -= Time.deltaTime;
        }

    }

    public int GetSlashingSequence()
    {
        return slashingstate;
    }

    bool isResetVelocityTrigger = false;
    void ResetVelocity()
    {
        isResetVelocityTrigger = true;
    }

    float buttonhold_timer = 0f;
    bool OnHoldTimer(float HOLD_DUR = 0.6f)
    {
        if (Input.GetKey(KeyCode.J))
        {
            buttonhold_timer += Time.deltaTime;
        }
        return buttonhold_timer >= HOLD_DUR;
    }
    bool J_Pressed = false;

    void Key_J()
    {
        if (Input.GetKeyDown(KeyCode.J) && slash_abil.cooldown <= 0 )
        {
            J_Pressed = true;
        }
        if (J_Pressed)
        {
            if (Input.GetKeyUp(KeyCode.J))
            {
                currentState = PlayerState.NormalAttack;
                buttonhold_timer = 0;
                J_Pressed = false;
            }
            else if (OnHoldTimer(0.15f))
            {
                playerControl.DisableMove();
                physicsEvent.SelfDisableGravity();
                if (OnHoldTimer(0.9f))
                {
                    playerControl.EnableMove();
                    print("SHOCKWAVE");
                    physicsEvent.SelfEnableGravity();
                    buttonhold_timer = 0;
                    J_Pressed = false;
                }
            }
        }
    }

    bool isAnimLock = false;
    bool iscontrolFrozen = false;
    private bool IsControllable()
    {
        return !iscontrolFrozen && !isAnimLock;
    }

    int currentSkillDamage = 0;
    public int GetDamage()
    {
        return currentSkillDamage;
    }
    void RestoreGravityOnMove()
    {
        if (playerControl.IsMoving())
        {
            rb.useGravity = true;
        }
    }

}
