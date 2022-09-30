using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(PlayerInput)), RequireComponent(typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpHeight = 5;
    [SerializeField] private float jumpBuffer = 0.2f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float raycastDistance = 0.1f;

    private float xInput;

    private float lastJumpTime;
    private bool jumpConsumed = true;
    private bool jumpTriggered => !jumpConsumed && Time.time - lastJumpTime <= jumpBuffer;

    private float lastGroundedTime;
    private bool grounded = false;
    private bool canJump => grounded || Time.time - lastGroundedTime <= coyoteTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        float x = xInput * speed;
        float y;
        if (jumpTriggered && canJump)
        {
            y = jumpHeight;
            jumpConsumed = true;
            grounded = false;
        }
        else
        {
            y = rb.velocity.y;
        }

        rb.velocity = new Vector2(x, y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger == false)
        {
            lastGroundedTime = Time.time;
            grounded = true;
        }
    }

    void OnJump()
    {
        lastJumpTime = Time.time;
        jumpConsumed = false;
    }

    void OnMove(InputValue value) => xInput = value.Get<Vector2>().x;
}
