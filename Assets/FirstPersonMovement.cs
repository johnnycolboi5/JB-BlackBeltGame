using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
    }

    void Update()
    {
        ProcessInputs();
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

    void OnButtonDown() 
    {
       
    } 
}
