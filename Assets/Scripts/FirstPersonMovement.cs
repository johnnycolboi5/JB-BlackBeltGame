using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float sprintSpeed = 15f;
    public float acceleration = 15f;  
    public float deceleration = 20f;   
    public float maxSprintTime = 5f;
    public float sprintCooldown = 5f;
    public Slider sprintBar;
    public float jumpHeight = 2f;
    public LayerMask groundLayer;

    private float sprintTimer;
    private float cooldownTimer;
    private bool isCoolingDown = false;
    private bool isGrounded;
    private float moveSpeed;
    private Rigidbody rb;
    private Vector3 moveDirection;
    private Vector3 currentVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

      
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            PhysicMaterial frictionless = new PhysicMaterial();
            frictionless.dynamicFriction = 0f;
            frictionless.staticFriction = 0f;
            frictionless.frictionCombine = PhysicMaterialCombine.Minimum;
            col.material = frictionless;
        }

        moveSpeed = walkSpeed;
        sprintTimer = maxSprintTime;
        cooldownTimer = 0f;

        if (sprintBar != null)
            sprintBar.value = 1f;
    }

    void Update()
    {
        ProcessInputs();
        HandleSprint();
        UpdateSprintBar();
        HandleJump();
    }

    void FixedUpdate()
    {
        MovePlayer();
        CheckGroundStatus();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        moveDirection = (transform.right * moveX + transform.forward * moveZ).normalized;
    }

    void MovePlayer()
    {
        Vector3 targetVelocity = moveDirection * moveSpeed;

       
        float rate = moveDirection.magnitude > 0.1f ? acceleration : deceleration;

        currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, rate * Time.fixedDeltaTime);

        rb.velocity = new Vector3(currentVelocity.x, rb.velocity.y, currentVelocity.z);
    }

    void HandleSprint()
    {
        bool isMoving = moveDirection.magnitude > 0.1f; 

        if (isCoolingDown)
        {
            cooldownTimer -= Time.deltaTime;
            moveSpeed = walkSpeed;
            sprintTimer = Mathf.Min(sprintTimer + Time.deltaTime, maxSprintTime);
            if (cooldownTimer <= 0f)
                isCoolingDown = false;
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift) && sprintTimer > 0f && isMoving)
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
                sprintTimer = Mathf.Min(sprintTimer + Time.deltaTime, maxSprintTime);
            }
        }
    }

    void UpdateSprintBar()
    {
        if (sprintBar != null)
            sprintBar.value = sprintTimer / maxSprintTime;
    }

    void HandleJump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
    }

    void CheckGroundStatus()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1f, groundLayer);
    }
}