using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f; // The force applied when jumping
    public float gravitySwitchDuration = 1f; // Duration for gravity switch
    public float rotationSpeed = 100f; // Speed at which the player rotates
    public float moveSpeed = 5f; // Speed at which the player moves left and right
    private bool isGravitySwitched = false; // Flag for gravity switch state
    private float gravitySwitchTime = 0f; // Time when gravity switch was activated
    private bool isGrounded = true; // Check if the player is on the ground
    private Rigidbody2D rb; // Reference to the player's Rigidbody2D

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }

    void Update()
    {
        // Jumping behavior when gravity is not switched
        if (isGrounded && Input.GetKeyDown(KeyCode.Space) && !isGravitySwitched)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Apply jump force upward
            isGrounded = false; // The player is no longer grounded after jumping
        }

        // Gravity switch behavior
        if (Input.GetKeyDown(KeyCode.G) && !isGravitySwitched)
        {
            isGravitySwitched = true;
            gravitySwitchTime = Time.time;
            rb.gravityScale = -rb.gravityScale; // Flip gravity direction
        }

        // Reset gravity after duration
        if (isGravitySwitched && Time.time - gravitySwitchTime > gravitySwitchDuration)
        {
            isGravitySwitched = false;
            rb.gravityScale = -rb.gravityScale; // Reset gravity direction
        }

        // Rotate left (Q) and right (E)
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime); // Rotate counter-clockwise
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime); // Rotate clockwise
        }

        // Move left (A) and right (D)
        float moveDirection = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            moveDirection = -1f; // Move left
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDirection = 1f; // Move right
        }

        // Apply movement
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        // If gravity is switched, jump will be inverted: down then up
        if (isGravitySwitched && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, -jumpForce); // Apply a "downward" jump force
        }
    }

    // Detect ground collision to reset jump
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // The player is back on the ground
        }
    }
}
