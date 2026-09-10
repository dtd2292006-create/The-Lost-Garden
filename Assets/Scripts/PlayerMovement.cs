using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator animator;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastDirection = Vector2.down;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    private void Update()
    {
        moveInput = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed ||
                Keyboard.current.upArrowKey.isPressed)
            {
                moveInput.y += 1;
            }

            if (Keyboard.current.sKey.isPressed ||
                Keyboard.current.downArrowKey.isPressed)
            {
                moveInput.y -= 1;
            }

            if (Keyboard.current.aKey.isPressed ||
                Keyboard.current.leftArrowKey.isPressed)
            {
                moveInput.x -= 1;
            }

            if (Keyboard.current.dKey.isPressed ||
                Keyboard.current.rightArrowKey.isPressed)
            {
                moveInput.x += 1;
            }
        }

        moveInput = moveInput.normalized;

        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void UpdateAnimation()
    {
        if (animator == null)
        {
            return;
        }

        bool isMoving = moveInput.sqrMagnitude > 0.01f;
        Vector2 animationDirection = moveInput;

        if (isMoving)
        {
            if (Mathf.Abs(animationDirection.x) >
                Mathf.Abs(animationDirection.y))
            {
                animationDirection = new Vector2(
                    Mathf.Sign(animationDirection.x),
                    0
                );
            }
            else
            {
                animationDirection = new Vector2(
                    0,
                    Mathf.Sign(animationDirection.y)
                );
            }

            lastDirection = animationDirection;

            animator.SetFloat("MoveX", animationDirection.x);
            animator.SetFloat("MoveY", animationDirection.y);
        }

        animator.SetFloat("LastX", lastDirection.x);
        animator.SetFloat("LastY", lastDirection.y);
        animator.SetFloat("Speed", isMoving ? 1f : 0f);
    }
}