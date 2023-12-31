using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsEvent : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerControl playerControl;
    Rigidbody playerRb;
    Coroutine gravityCoroutine;

    private void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        playerRb = GetComponent<Rigidbody>();
    }
    public void PerformAction()
    {
        Debug.Log("Performing action...");
        // Your specific action logic goes here
    }
    public void PushSelf()
    {
        if (playerControl.IsGrounded())
        {
            float forceAmount = 5.0f;
            playerRb.AddForce(playerControl.GetPointerDirection() * forceAmount, ForceMode.Impulse);
        }
        else
        {
            if (gravityCoroutine != null)
            {
                StopCoroutine(gravityCoroutine);
            }
            gravityCoroutine = StartCoroutine(SelfPauseGravity(0.2f));
        }
    }

    private IEnumerator SelfPauseGravity(float duration)
    {
        playerRb.useGravity = false;
        yield return new WaitForSeconds(duration);
        playerRb.useGravity = true;
    }


}
