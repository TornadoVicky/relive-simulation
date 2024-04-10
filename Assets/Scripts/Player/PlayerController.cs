
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float normalMoveSpeed = 5f;
    public float tentacleMoveSpeed = 3f;
    public float jumpForce = 15f;
    public float coyoteTime = 0.2f;

    public LayerMask ground;
    public Transform feetPos;

    public bool isTentacleMode = false;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isJumping;
    private float currentCoyoteTime = 0.2f;
    public float jumpGravityScale = 2f;
    public AudioSource moveAudioSource;

    [Header("Abilities")]
    public bool coyoteJump = true;

    public float tentacleSpreadAngle = 180f;
    public GameObject[] idealPositionObjects;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        DisableAIObjectsAndSelectors();
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, 0.1f, ground);
        float moveSpeed = isTentacleMode ? tentacleMoveSpeed : normalMoveSpeed;

        if (isTentacleMode && (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f))
        {
            if (!moveAudioSource.isPlaying)
            {
                moveAudioSource.Play();
            }
        }
        else
        {
            moveAudioSource.Stop(); // Stop the audio if not moving or not in tentacle mode
        }

        // Decrease coyote time
        if (!isGrounded)
        {
            currentCoyoteTime -= Time.deltaTime;
        }
        else
        {
            currentCoyoteTime = coyoteTime;
        }

        // Check for jump input
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || currentCoyoteTime > 0)
            {
                isJumping = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                currentCoyoteTime = 0;
            }
        }

        // Variable jump height logic
        if (isJumping && !Input.GetButton("Jump"))
        {
            isJumping = false;
        }

        if (!isTentacleMode)
        {
            // Normal Mode
            float horizontalInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        }
        else
        {
            // Tentacle Mode
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(horizontalInput * moveSpeed, verticalInput * moveSpeed);
        }

        // Adjust gravity for faster descent
        if (rb.velocity.y < 0) // Check if the player is falling
        {
            rb.gravityScale = 5.5f; // Increase gravity scale for faster descent
        }
        else
        {
            rb.gravityScale = 2.5f; // Reset gravity scale to normal if ascending or grounded
        }

        if (!isTentacleMode)
        {
            // Normal Mode
            float horizontalInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        }
        else
        {
            // Tentacle Mode
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(horizontalInput * moveSpeed, verticalInput * moveSpeed);
        }

        // Toggle Tentacle Mode
        if (Input.GetKeyDown(KeyCode.T))
        {
            isTentacleMode = !isTentacleMode;
            ToggleAIObjectsAndSelectors(isTentacleMode);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerPosition = transform.position;
            Vector2 direction = mousePosition - playerPosition;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float radius = direction.magnitude;
            float angleIncrement = tentacleSpreadAngle / 2f;

            for (int i = 0; i < idealPositionObjects.Length; i++)
            {
                float offsetAngle = angle + (i - 1.5f) * angleIncrement;
                float x = playerPosition.x + radius * Mathf.Cos(offsetAngle * Mathf.Deg2Rad);
                float y = playerPosition.y + radius * Mathf.Sin(offsetAngle * Mathf.Deg2Rad);

                if (i < idealPositionObjects.Length)
                {
                    idealPositionObjects[i].transform.position = new Vector3(x, y, idealPositionObjects[i].transform.position.z);
                }
            }
        }
    }

    private void DisableAIObjectsAndSelectors()
    {
        RandomPointSelector[] selectors = FindObjectsOfType<RandomPointSelector>();

        foreach (RandomPointSelector selector in selectors)
        {
            if (selector.aiGameObject != null)
            {
                selector.aiGameObject.SetActive(false);
            }
            selector.enabled = false;
        }
    }

    public void ToggleAIObjectsAndSelectors(bool enable)
    {
        RandomPointSelector[] selectors = FindObjectsOfType<RandomPointSelector>();

        foreach (RandomPointSelector selector in selectors)
        {
            if (selector.aiGameObject != null)
            {
                selector.aiGameObject.SetActive(enable);
            }
            selector.enabled = enable;
        }
    }
}