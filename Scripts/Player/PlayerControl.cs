using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour
{
    public GameObject pointerGO;
    public Transform playerChild;
    
    private Animator animator;
    Rigidbody rb;
    SpriteRenderer sprite;
    GameManager gamemanager;
    void Start()
    {
    rb = GetComponent<Rigidbody>();
    animator = playerChild.GetComponent<Animator>();    
    sprite = playerChild.GetComponent<SpriteRenderer>();
    if(!gamemanager) gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void FixedUpdate()
    {
        if (!gamemanager.GetGameplayFrozen())
        {
        Move();
        Jump();
        }
    }

    private void Update()
    {
        if(!gamemanager.GetGameplayFrozen())
        {
        //MoveInput();
        Sprint();
        Interact();
        AnimationManager();
        }

        
    }

    const float BASE_movementSpeed = 1.6f;
    const float SPRINT_movementSpeed = 2.5f;
    float movementSpeed = BASE_movementSpeed;
    float horizontalInput = 0f;
    float verticalInput = 0f;
    float jumpForce = 3.0f;

    void AnimationManager()
    {
        animator.SetBool("ismoving", IsMoving());
        animator.SetBool("issprinting", IsSprinting());

        if (horizontalInput > 0)
        {
            sprite.flipX = true;
        }
        else if (horizontalInput < 0)
        {
            sprite.flipX = false;
        }
    }

    bool isSprinting = false;
    void Sprint()
    {
        if( Input.GetKey(KeyCode.LeftShift) )
        {
            movementSpeed = SPRINT_movementSpeed;
            isSprinting = true;
        }
        else
        {
            movementSpeed = BASE_movementSpeed;
            isSprinting = false;
        }
    }
   
    public bool IsMoving()
    {
        if (horizontalInput != 0f || verticalInput != 0f)
        {
           return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsSprinting()
    {
        return isSprinting;
    }
    //private Quaternion lastRotation = Quaternion.identity; 
    private Quaternion lastRotation = Quaternion.Euler(0,180.0f,0); 

    public Vector3 GetPointerDirection()
    {
        return pointerGO.transform.forward;
    }
    void SetPointerRotation(Vector3 moveDir)
    {
        if (moveDir != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            lastRotation = Quaternion.Euler(0f, angle, 0f);
        }
            pointerGO.transform.rotation = lastRotation;
    }
    void Move()
   {
        horizontalInput = 0f;
        verticalInput = 0f;
        //rb.velocity = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput -= 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            horizontalInput += 1f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            verticalInput += 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            verticalInput -= 1f;
        }
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        SetPointerRotation(moveDirection);
        Vector3 targetPosition = transform.position + (moveDirection * movementSpeed * Time.deltaTime);
        rb.MovePosition(targetPosition);
    }

    public LayerMask groundLayerMask;
    float groundRaycastDistance = 0.1f;

    public bool IsGrounded()
    {
        Vector3 raycastOrigin = transform.position + Vector3.up * 0.1f;
        return Physics.Raycast(raycastOrigin, Vector3.down, groundRaycastDistance, groundLayerMask);
    }

    void Jump()
    {
        Debug.DrawRay(transform.position, Vector3.down * groundRaycastDistance, Color.red);
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    void Interact()
    {

    }

   
}
