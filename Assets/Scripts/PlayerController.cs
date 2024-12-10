using UnityEngine;
using TMPro; // Add this for TextMeshPro support
using UnityEngine.SceneManagement; // For reloading the scene (to restart the game)
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f; // The force applied when jumping
    public float gravitySwitchDuration = 1f; // Duration for gravity switch
    public float rotationSpeed = 200f; // Speed at which the player rotates
    public float moveSpeed = 5f; // Speed at which the player moves left and right
    private bool isGravitySwitched = false; // Flag for gravity switch state
    private float gravitySwitchTime = 0f; // Time when gravity switch was activated
    private bool isGrounded = true; // Check if the player is on the ground
    private Rigidbody2D rb; // Reference to the player's Rigidbody2D

    // AudioSource to play and stop sound
    public AudioSource gravitySound; // Reference to the AudioSource component for the gravity sound
    public AudioSource jumpSound; // Reference to the AudioSource component for the jump sound

    // Game Over UI elements
    public GameObject gameOverUI; // Reference to the Game Over UI panel
    public TextMeshProUGUI timerText; // Reference to the TextMeshProUGUI element for the timer
    public TextMeshProUGUI gameOverTimeText; // Reference to the TextMeshProUGUI for displaying time on Game Over screen
    public TextMeshProUGUI startText; // Reference to the TextMeshProUGUI for showing the start text

    private float timer = 0f; // Store the elapsed time
    private float gameOverTime = 0f; // Store the time when the player hit an obstacle

    private SpriteRenderer spriteRenderer; // Reference to the player's SpriteRenderer

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        gameOverUI.SetActive(false); // Hide the game over UI at the start

        // Start the coroutine to display the text for a given time
        StartCoroutine(DisplayStartText());
    }

    void Update()
    {
        // If the game is paused, listen for Spacebar to restart
        if (Time.timeScale == 0f && Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame(); // Restart the game when Spacebar is pressed
        }

        // Update the timer only when the game is not paused
        if (Time.timeScale != 0f)
        {
            timer += Time.deltaTime; // Increment the timer
            int seconds = Mathf.FloorToInt(timer); // Convert the timer to whole seconds
            timerText.text = "Score: " + seconds.ToString(); // Update the timer text on the UI
        }

        // Jumping behavior when gravity is not switched
        if (isGrounded && Input.GetKeyDown(KeyCode.Space) && !isGravitySwitched)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Apply jump force upward
            isGrounded = false; // The player is no longer grounded after jumping

            // Play jump sound when the player jumps
            jumpSound.Play(); // Play the jump sound
        }

        // Gravity switch behavior
        if (Input.GetKeyDown(KeyCode.G) && !isGravitySwitched)
        {
            isGravitySwitched = true;
            gravitySwitchTime = Time.time;
            rb.gravityScale = -rb.gravityScale; // Flip gravity direction
            gravitySound.Play(); // Play the sound when gravity is switched

            // Flip the sprite upside down
            spriteRenderer.flipY = true;
        }

        // Reset gravity after duration
        if (isGravitySwitched && Time.time - gravitySwitchTime > gravitySwitchDuration)
        {
            isGravitySwitched = false;
            rb.gravityScale = -rb.gravityScale; // Reset gravity direction
            gravitySound.Stop(); // Stop the sound when gravity switch ends

            // Flip the sprite back to its original position
            spriteRenderer.flipY = false;
        }

        // Rotate left (Q) and right (E) OR Left Arrow and Right Arrow
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime); // Rotate counter-clockwise
        }
        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.RightArrow))
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

    // Coroutine to display start text for a given time
    private IEnumerator DisplayStartText()
    {
        startText.gameObject.SetActive(true); // Make the start text visible
        yield return new WaitForSeconds(5f); // Wait for 5 seconds
        startText.gameObject.SetActive(false); // Hide the start text after 5 seconds
    }

    // Detect ground collision to reset jump
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // The player is back on the ground
        }

        // Detect collision with obstacles and trigger game over
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            gameOverTime = Mathf.FloorToInt(timer); // Capture the time when the player hit the obstacle
            GameOver(); // Call the Game Over function when the player hits an obstacle
        }
    }

    // Game Over Logic
    void GameOver()
    {
        gameOverUI.SetActive(true); // Show the Game Over UI
        gameOverTimeText.text = "Your Score: " + gameOverTime.ToString(); // Display the time on the Game Over screen
        Time.timeScale = 0f; // Pause the game (freeze all movement)
    }

    // Restart the game (this can be called via a button in the Game Over UI or Spacebar)
    public void RestartGame()
    {
        Time.timeScale = 1f; // Unpause the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }
}
