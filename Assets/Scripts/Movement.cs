using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class Movement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;

    [Header("Footsteps")]
    public AudioClip[] walkClips;      // Drag walk footstep clips here
    public AudioClip[] runClips;       // Drag run footstep clips here
    public float walkStepInterval = 0.5f;  // Seconds between walk steps
    public float runStepInterval = 0.3f;   // Seconds between run steps
    [Range(0f, 1f)] public float footstepVolume = 0.5f;

    private CharacterController controller;
    private AudioSource audioSource;
    private float stepTimer = 0f;
    private int lastClipIndex = -1; // Prevents same clip playing twice in a row

    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        bool isMoving = direction.magnitude >= 0.1f;

        float speed = isRunning ? runSpeed : walkSpeed;

        if (isMoving)
        {
            controller.Move(direction.normalized * speed * Time.deltaTime);
            HandleFootsteps(isRunning);
        }
        else
        {
            stepTimer = 0f; // Reset so first step plays immediately on next move
        }
    }

    void HandleFootsteps(bool isRunning)
    {
        float interval = isRunning ? runStepInterval : walkStepInterval;
        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0f)
        {
            PlayFootstep(isRunning ? runClips : walkClips);
            stepTimer = interval;
        }
    }

    void PlayFootstep(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return;

        // Pick a random clip, avoiding repeating the same one twice
        int index;
        do
        {
            index = Random.Range(0, clips.Length);
        } while (clips.Length > 1 && index == lastClipIndex);

        lastClipIndex = index;
        audioSource.PlayOneShot(clips[index], footstepVolume);
    }
}