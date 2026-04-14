using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class FirstPersonMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 15f;
    public float acceleration = 15f;
    public float deceleration = 20f;
    public float maxSprintTime = 5f;
    public float sprintCooldown = 5f;
    public Slider sprintBar;
    public float jumpHeight = 2f;
    public LayerMask groundLayer;

    [Header("Footsteps")]
    public AudioClip[] walkClips;
    public AudioClip[] sprintClips;
    public float walkStepInterval = 0.5f;
    public float sprintStepInterval = 0.3f;
    [Range(0f, 1f)] public float footstepVolume = 0.5f;

    private float sprintTimer;
    private float cooldownTimer;
    private bool isCoolingDown = false;
    private bool isGrounded;
    private float moveSpeed;
    private Rigidbody rb;
    private Vector3 moveDirection;
    private Vector3 currentVelocity;

    private AudioSource audioSource;
    private float stepTimer = 0f;
    private int lastClipIndex = -1;

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

        audioSource = GetComponent<AudioSource>();

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
        HandleFootsteps();
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

    void HandleFootsteps()
    {
        bool isMoving = moveDirection.magnitude > 0.1f;
        bool isSprinting = moveSpeed == sprintSpeed;

        if (isMoving && isGrounded)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                PlayFootstep(isSprinting ? sprintClips : walkClips);
                stepTimer = isSprinting ? sprintStepInterval : walkStepInterval;
            }
        }
        else
        {
            stepTimer = 0f; // Reset so first step plays immediately on next move
        }
    }

    void PlayFootstep(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return;

        int index;
        do
        {
            index = Random.Range(0, clips.Length);
        } while (clips.Length > 1 && index == lastClipIndex);

        lastClipIndex = index;
        audioSource.PlayOneShot(clips[index], footstepVolume);
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