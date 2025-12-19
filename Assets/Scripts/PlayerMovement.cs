using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private Vector2 moveDirection;
    private Vector2 lastMoveDir = new Vector2(1, 0);
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float wallJummpUpForce;
    [SerializeField] private float wallJummpSideForce;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform leftWallCheck;
    [SerializeField] private Transform rightWallCheck;
    [SerializeField] private LayerMask wallLayer;
    private bool isGrounded;
    private bool onLeftWall;
    private bool onRightWall;
    private bool isWallJumping;
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
        CheckWallStatus();
    }

    void Update()
    {
        if (!isWallJumping)
        {
            MovementHandler();
        }


    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            Debug.Log("player jump triggered");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        else if (context.performed && (onLeftWall || onRightWall) && !isGrounded)
        {
            StartCoroutine(WallJump());
        }
    }
    IEnumerator WallJump()
    {
        isWallJumping = true;
        float jumpDirection = onLeftWall ? 1 : -1;
        rb.linearVelocity = new Vector2(jumpDirection * wallJummpSideForce, wallJummpUpForce);
        Debug.Log("wall jump");
        yield return new WaitForSeconds(0.2f);
        isWallJumping = false;
    }

    private void CheckGroundStatus()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    private void CheckWallStatus()
    {
        onLeftWall = Physics2D.OverlapCircle(leftWallCheck.position, 0.2f, wallLayer);
        onRightWall = Physics2D.OverlapCircle(rightWallCheck.position, 0.2f, wallLayer);
    }


    private void MovementHandler()
    {
        moveDirection = playerInput.actions["Movement"].ReadValue<Vector2>();

        isIdle = moveDirection == Vector2.zero;

        if (isIdle)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
            anim.SetBool("isRunning", false);
        }
        else
        {
            anim.SetBool("isRunning", true);
            lastMoveDir = moveDirection;
            rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocityY);
            anim.SetFloat("horizontalMovement", moveDirection.x);
        }
    }
}
