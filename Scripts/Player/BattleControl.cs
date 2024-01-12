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
    public Slash slash_abil; //unity can only display monobehaviour inside inspector
    GameObject PointerGO;
    enum PlayerState
    {
        NoAttack,
        NormalAttack,
    };

    Skillsets skillsets;

    private void Awake()
    {
        playerControl = GetComponent<PlayerControl>();
        physicsEvent = GetComponent<PhysicsEvent>();
        rb = physicsEvent.playerRb;
        skillsets = GetComponent<Skillsets>();
        PointerGO = playerControl.PointerGO;
    }
    void Start()
    {
        
    }

    PlayerState currentState = PlayerState.NoAttack;

    void Update()
    {
        if (IsControllable() && !playerControl.GetVoidboardMode())
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
        RestoreGravityOnMove();
        TickSlashState();

        switch (currentState)
        {
            case PlayerState.NoAttack:
                break;
                case PlayerState.NormalAttack:
                slashingstate++;
                slashdelay = BASE_slashdelay;
                if (slashingstate > 2)
                {
                    slashingstate = 1;
                }
                playerControl.DisableMove();
                SlashingAnimation();
                SpawnSlashVFX();
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
                currentState = PlayerState.NoAttack;
                break;
        }
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

    void SpawnSlashVFX()
    {
        Vector3 slashpos = PointerGO.transform.position;
        Quaternion slashrot = PointerGO.transform.rotation;
        switch(GetSlashingSequence())
        {
            case 1:
                slashpos = PointerGO.transform.position;
                slashrot = PointerGO.transform.rotation;
                break;
            case 2:
                slashpos = PointerGO.transform.position;
                Quaternion slashrot_offset = Quaternion.Euler(0, 0, 180);
                slashrot = PointerGO.transform.rotation * slashrot_offset;
                break;
        }
        if (playerControl.sprite.flipX)
        {
            Quaternion slashrot_spriteflip = Quaternion.Euler(0, 0, 180);
            slashrot *= slashrot_spriteflip;
        }

        Destroy(Instantiate(slash_abil.SlashVFX, slashpos, slashrot), 1f);
    }

    int slashingstate = 0;
    const float BASE_slashstatetimer = 0.8f;
    float slashstatetimer = BASE_slashstatetimer;
    const float BASE_slashdelay = 0.13f;
    float slashdelay = BASE_slashdelay;
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

            if(slashdelay <= 0)
            {
                //playerControl.EnableMove();
            }
        }

        if (slashdelay > 0)
        {
            slashdelay -= Time.deltaTime;
        }

    }

    public int GetSlashingSequence()
    {
        return slashingstate;
    }

    //public List<Slash> slashes;
    //IEnumerator ISlashVFX()
    //{
    //    for(int i = 0; i<slashes.Count ; i++)
    //    {
    //    yield return new WaitForSeconds(slashes[i].delay);
    //        slashes[i].SlashVFX.SetActive(true);
    //    }
    //    yield return new WaitForSeconds(1f);
    //    for (int i = 0; i < slashes.Count; i++)
    //    {
    //        slashes[i].SlashVFX.SetActive(false);
    //    }
    //}


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

    void HoldAttack()
    {
        if (Input.GetKeyDown(KeyCode.J) && slashdelay <= 0 )
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
[System.Serializable]
public class Slash
{
    public GameObject SlashVFX;
    public float delay;

}