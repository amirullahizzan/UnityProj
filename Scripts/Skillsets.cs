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
    public Slash slash_abil;
    //public Blink blink_abil;
    GameObject PointerGO;
    private void Awake()
    {
        playerControl = GetComponent<PlayerControl>();
        physicsEvent = GetComponent<PhysicsEvent>();
        battleControl = GetComponent<BattleControl>();
        PointerGO = playerControl.PointerGO;
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
        SpawnSlashVFX();
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
    }
    public void BlinkFixed()
    {
        physicsEvent.PushSelf(30.0f);
    }
    public void ResetColliders()
    {
        NormalAttackGO.gameObject.SetActive(false);
    }

    void SpawnSlashVFX()
    {
        Vector3 slashpos = PointerGO.transform.position;
        Quaternion slashrot = PointerGO.transform.rotation;
        switch (battleControl.GetSlashingSequence())
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

        Destroy(Instantiate(slash_abil.VFX, slashpos, slashrot), 1f);
    }


    int currentSkillDamage = 0;
    public int GetDamage()
    {
        return currentSkillDamage;
    }
}


[System.Serializable]
public class Slash
{
    public GameObject VFX;
    public static readonly float BASE_cooldown = 0.13f;
    [HideInInspector] public float cooldown = BASE_cooldown;
}
[System.Serializable]
public class Blink
{
    public GameObject VFX;
    public static readonly float BASE_cooldown = 0.15f;
    [HideInInspector] public float cooldown = BASE_cooldown;
}