using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Skillsets : MonoBehaviour
{
    PhysicsEvent physicsEvent;
    Transform NormalAttackGO;

    void Start()
    {
    physicsEvent = GetComponent<PhysicsEvent>();
    NormalAttackGO = transform.Find("pointerGO/NormalAttackGO");
    }

    // Update is called once per frame
    void Update()
    {
        if (NormalAttackGO.gameObject.activeSelf) { ResetColliders(); }
    }

    //bool isNormalAttack = false;
    public void NormalAttack()
    {
        NormalAttackGO.gameObject.SetActive(true);
        currentSkillDamage = 25;
        CinemachineEffect.Instance.ShakeVCam(0.2f, .1f);
        physicsEvent.PushSelf();
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
