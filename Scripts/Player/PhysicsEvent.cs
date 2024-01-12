using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PhysicsEvent : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerControl playerControl;
    [HideInInspector] public Rigidbody playerRb;

    private void Awake()
    {
        playerControl = GetComponent<PlayerControl>();
        playerRb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        
    }
    public void PerformAction()
    {
        Debug.Log("Performing action...");
        // Your specific action logic goes here
    }
    public void PushSelf(float forceAmount = 5.0f)
    {
        if (playerControl.IsGrounded())
        {
            playerRb.AddForce(playerControl.GetPointerDirection() * forceAmount, ForceMode.Impulse);
        }
        else
        {
           
        }
    }

    public IEnumerator IPauseMove(float duration)
    {
        playerControl.DisableMove();
        float timer = 0;
        while ( timer < duration)
        {
        timer += Time.deltaTime;
        yield return null;
        }
        playerControl.EnableMove();
        
    }

    public IEnumerator ISelfPauseGravity(float duration)
    {
        float timer = 0;
        playerRb.useGravity = false;
        while(timer < duration)
        {
        timer += Time.deltaTime;
        yield return null;
        }
        playerRb.useGravity = true;
    }

    public void SelfDisableGravity()
    {
        playerRb.useGravity = false;
    }

    public void SelfEnableGravity()
    {
        playerRb.useGravity = true;
    }


}
