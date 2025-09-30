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

    private float sprintTimer;
    private float cooldownTimer;
    private bool isCoolingDown = false;

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
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        moveDirection = (transform.right * moveX + transform.forward * moveZ).normalized;
    }

    void MovePlayer()
    {
        rb.velocity = moveDirection * moveSpeed + new Vector3(0, rb.velocity.y, 0);
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
}
