using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour
{
    public GameObject PointerGO;
    public Transform playerAssetChild;
    
    [HideInInspector] public Animator animator;
    Rigidbody rb;
    PhysicsEvent physicsEvent;
    [HideInInspector] public SpriteRenderer sprite;
    GameManager gamemanager;
    BattleControl battlecontrol;
    
    private void Awake()
    {
        physicsEvent = GetComponent<PhysicsEvent>();
        rb = physicsEvent.playerRb;

        battlecontrol = GetComponent<BattleControl>();

        animator = playerAssetChild.GetComponent<Animator>();
        sprite = playerAssetChild.GetComponent<SpriteRenderer>();
        if (!gamemanager) gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Start()
    {
  
    }

    void FixedUpdate()
    {
        if (!gamemanager.GetGameplayFrozen())
        {
                Jump();
                Move();
        }
    }

    private void Update()
    {
       
        if (!gamemanager.GetGameplayFrozen())
        {
        //if (IsGrounded())   {                EnableMove();            }
        MoveInput();
        AnimationManager();

            VoidGlider();
            if(!isVoidGliderMode)
            {
            Sprint();
            Interact();
            }
        }
    }
    public void DisableMove()
    {
        isCanMove = false;
    }
    public void EnableMove()
    {
        isCanMove = true;
    }

    bool isVoidGliderMode = false;
    public bool GetVoidboardMode()
    {
        return isVoidGliderMode;
    }
    void VoidGlider()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isVoidGliderMode = !isVoidGliderMode;
            //TODO StartCoroutine pause move?
            if (isVoidGliderMode)
            {
                //VGBoard
                movementSpeed = AIRBOARD_movementSpeed;
            }
            else
            {
                movementSpeed = WALK_movementSpeed;
            }
        }
        
    }

    const float WALK_movementSpeed = 1.6f;
    const float SPRINT_movementSpeed = 2.5f;
    const float AIRBOARD_movementSpeed = 5.0f;
    float movementSpeed = WALK_movementSpeed;
    float horizontalInput = 0f;
    float verticalInput = 0f;
    float jumpForce = 35.0f;
    void AnimationManager()
    {
        animator.SetBool("ismoving", IsMoving());
        animator.SetBool("issprinting", IsSprinting());
        animator.SetInteger("slashingseq", battlecontrol.GetSlashingSequence());

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
            movementSpeed = WALK_movementSpeed;
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
        return PointerGO.transform.forward;
    }
    void SetPointerRotation(Vector3 moveDir)
    {
        if (moveDir != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            lastRotation = Quaternion.Euler(0f, angle, 0f);
        }
            PointerGO.transform.rotation = lastRotation;
    }

    public bool isCanMove = true;
    void MoveInput()
    {
        horizontalInput = 0f;
        verticalInput = 0f;
        //rb.velocity = Vector3.zero;
        if (!isCanMove) { return; }
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
    }
    void Move()
   {
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
