using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private Vector2 moveDirection;
    private Vector2 lastMoveDir = new Vector2(1, 0);
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;
    private Animator anim;
    bool isIdle;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        CheckGroundStatus();
    }

    void Update()
    {
        MovementHandler();


    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            Debug.Log("player jump triggered");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void CheckGroundStatus()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }


    private void MovementHandler()
    {
        moveDirection = playerInput.actions["Movement"].ReadValue<Vector2>();

        isIdle = moveDirection == Vector2.zero;

        if (isIdle)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
            anim.SetBool("isRunning", false);
            Debug.Log("not running");
        }
        else
        {
            anim.SetBool("isRunning", true);
            lastMoveDir = moveDirection;
            rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocityY);
            anim.SetFloat("horizontalMovement", moveDirection.x);
            Debug.Log("running");
        }
    }
}
