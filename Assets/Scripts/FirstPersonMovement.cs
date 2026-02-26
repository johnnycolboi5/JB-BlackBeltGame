using UnityEngine;
using UnityEngine.UI; // Needed for UI elements

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float sprintSpeed = 15f;

    public float maxSprintTime = 5f; // seconds you can sprint
    public float sprintCooldown = 5f; // seconds to recover before sprinting again

    public Slider sprintBar; // Assign your UI slider in Inspector

    public float jumpHeight = 2f; // Height of the jump
    public LayerMask groundLayer; // Ground detection layer

    private float sprintTimer;
    private float cooldownTimer;
    private bool isCoolingDown = false;
    private bool isGrounded;

    private float moveSpeed;
    private Rigidbody rb;
    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveSpeed = walkSpeed;
        sprintTimer = maxSprintTime;
        cooldownTimer = 0f;

        if (sprintBar != null)
            sprintBar.value = 1f; // Start full
    }

    void Update()
    {
        ProcessInputs();
        HandleSprint();
        UpdateSprintBar();
        HandleJump(); // Check for jump input
    }

    void FixedUpdate()
    {
        MovePlayer();
        CheckGroundStatus(); // Update whether player is grounded
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        moveDirection = (transform.right * moveX + transform.forward * moveZ).normalized;
    }

    void MovePlayer()
    {
        Debug.Log(rb == null);
        Debug.Log("Direction: " + moveDirection + " , Speed: " + moveSpeed);
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);
    }

    void HandleSprint()
    {
        if (isCoolingDown)
        {
            cooldownTimer -= Time.deltaTime;
            moveSpeed = walkSpeed;

            // Recharge sprint slowly
            sprintTimer = Mathf.Min(sprintTimer + Time.deltaTime, maxSprintTime);

            if (cooldownTimer <= 0f)
            {
                isCoolingDown = false;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift) && sprintTimer > 0f)
            {
                moveSpeed = sprintSpeed;
                sprintTimer -= Time.deltaTime;

                if (sprintTimer <= 0f)
                {
                    isCoolingDown = true;
                    cooldownTimer = sprintCooldown;
                }
            }
            else
            {
                moveSpeed = walkSpeed;
                // Recharge sprint when not sprinting
                sprintTimer = Mathf.Min(sprintTimer + Time.deltaTime, maxSprintTime);
            }
        }
    }

    void UpdateSprintBar()
    {
        if (sprintBar != null)
        {
            sprintBar.value = sprintTimer / maxSprintTime;
        }
    }

    // Handle Jump logic
    void HandleJump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space)) // Jump when space is pressed
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
    }

    // Check if the player is grounded
    void CheckGroundStatus()
    {
        // Cast a ray down to check if the player is on the ground
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1f, groundLayer);
    }
}