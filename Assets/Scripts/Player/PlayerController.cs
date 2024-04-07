
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float normalMoveSpeed = 5f;
    public float tentacleMoveSpeed = 3f;
    public float jumpForce = 15f;
    public float jumpTime = 0.5f;
    public float coyoteTime = 0.2f;
    public float dashForce = 10f;
    public float dashingVelocity = 14f;
    public float dashingTime = 0.5f;

    public LayerMask ground;
    public Transform feetPos;

    private bool isTentacleMode = false;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isJumping;
    private float jumpCounter;
    private bool canDoubleJump = true;
    private float currentCoyoteTime = 0.2f;
    private Vector2 dashDirection;
    private bool isDashing;
    private bool canDash = true;


    [Header("Abilities")]
    public bool doubleJump = true;
    public bool dash = true;
    public bool sprint;
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

        // Dash Input
        var dashInput = Input.GetKeyDown(KeyCode.X);

        if(dash == true)
        {
            dashingTime = 0.5f;
        }
        else
        {
            dashingTime = 0f;
        }

        if(dashInput && canDash)
        {
            isDashing = true;
            canDash = false;
            dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if(dashDirection == Vector2.zero)
            {
                dashDirection = new Vector2(transform.localScale.x, 0);
            }

            StartCoroutine(StopDashing());
        }

        if (isDashing)
        {
            rb.velocity = dashDirection.normalized * dashingVelocity;
            return;
        }

        if (isGrounded)
        {
            canDash = true;
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
                jumpCounter = 0f;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                canDoubleJump = true;
                currentCoyoteTime = 0;
            }
            else if (doubleJump && canDoubleJump)
            {
                isJumping = true;
                jumpCounter = 0f;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                canDoubleJump = false;
            }
        }

        // Variable jump height logic
        if (isJumping)
        {
            if (Input.GetButton("Jump") && jumpCounter < jumpTime)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpCounter += Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
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

    private void ToggleAIObjectsAndSelectors(bool enable)
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

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
    }
}