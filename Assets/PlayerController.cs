using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f;
    public float gravitySwitchDuration = 1f;
    private bool isGravitySwitched = false;
    private float gravitySwitchTime = 0f;
    private bool isGrounded = true;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Jumping
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        }

        // Gravity switch
        if (Input.GetKeyDown(KeyCode.G) && !isGravitySwitched)
        {
            isGravitySwitched = true;
            gravitySwitchTime = Time.time;
            rb.gravityScale = -rb.gravityScale;
        }

        // Reset gravity after duration
        if (isGravitySwitched && Time.time - gravitySwitchTime > gravitySwitchDuration)
        {
            isGravitySwitched = false;
            rb.gravityScale = -rb.gravityScale;
        }
    }

    // Detect ground collision to reset jump
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
