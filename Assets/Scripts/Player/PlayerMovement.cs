using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    // Components
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Timer timer;

    // Movement Parameters
    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpHeight = 5;
    [SerializeField] private float jumpBuffer = 0.2f;
    [SerializeField] private float coyoteTime = 0.2f;

    [SerializeField] private Sprite cubeOff;
    [SerializeField] private Sprite cubeOn;

    public bool controlled = false;

    // Internal movement variables
    private float xInput;

    private float lastJumpTime;
    private bool jumpConsumed = true;
    private bool jumpTriggered => !jumpConsumed && Time.time - lastJumpTime <= jumpBuffer;

    private float lastGroundedTime;
    private bool grounded = true;
    private bool canJump => grounded || Time.time - lastGroundedTime <= coyoteTime;

    // Movement replay
    private Vector3 startPosition;
    private List<Vector2> positions = new List<Vector2>();
    private List<Vector2>.Enumerator iterator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        iterator = positions.GetEnumerator();
        startPosition = transform.position;
        
        timer = Timer.instance;
        timer.OnTimerEnd += Reset;
    }

    private void Reset()
    {
        iterator = positions.GetEnumerator();
        transform.position = startPosition;
        grounded = true;
        InputManager.instance.DisableMovement(0.5f);
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void Activate()
    {
        controlled = true;
        //rb.bodyType = RigidbodyType2D.Dynamic;
        sr.sprite = cubeOn;
    }

    public void Deactivate()
    {
        controlled = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        sr.sprite = cubeOff;
    }

    void FixedUpdate()
    {
        if (timer.active)
        {
            if (controlled)
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
                positions.Add(transform.position);
            }
            else if (iterator.MoveNext())
            {
                transform.position = iterator.Current;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger == false)
        {
            lastGroundedTime = Time.time;
            grounded = true;
        }
    }

    public void OnJump()
    {
        StartTimer();
        lastJumpTime = Time.time;
        jumpConsumed = false;
    }

    public void OnMove(InputValue value)
    {
        StartTimer();
        xInput = value.Get<Vector2>().x;
    }

    private void StartTimer()
    {
        if (!timer.active)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            positions.Clear();
            timer.StartTime();
        }
    }
}
