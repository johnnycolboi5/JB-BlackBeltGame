using UnityEngine;

public class DoubleGateDoor : MonoBehaviour
{
    public Transform leftHinge;     // Empty GameObject at left hinge
    public Transform rightHinge;    // Empty GameObject at right hinge
    public Transform player;        // Player object
    public float openAngle = 90f;   // How wide the left door swings
    public float speed = 2f;        // Swing speed
    public float interactDistance = 20f; // Max distance from gate to interact

    private bool isOpen = false;
    private float leftTargetAngle = 0f;
    private float rightTargetAngle = 0f;

    public bool HasKey1;
    public bool HasKey2;

    public bool NeedsKey1;
    public bool NeedsKey2;
    void Start ()
    {
        HasKey1 = false;
        HasKey2 = false;
    }
    void Update()
    {
        // Distance from player to gate (use midpoint between hinges)
        Vector3 gateCenter = (leftHinge.position + rightHinge.position) / 2f;
        float distance = Vector3.Distance(player.position, gateCenter);


        if (NeedsKey1 == true) {
            // Press E if close enough
            if (distance <= interactDistance && Input.GetKeyDown(KeyCode.E) && HasKey1 == true)
            {
                isOpen = !isOpen;
                leftTargetAngle = isOpen ? openAngle : 0f;
                rightTargetAngle = isOpen ? -openAngle : 0f; // opposite direction
            }
        }
        if (NeedsKey2 == true) {
            if (distance <= interactDistance && Input.GetKeyDown(KeyCode.E) && HasKey2 == true)
            {
                isOpen = !isOpen;
                leftTargetAngle = isOpen ? openAngle : 0f;
                rightTargetAngle = isOpen ? -openAngle : 0f; // opposite direction
            }
        } 
    

        // Smoothly rotate left hinge
        float currentLeftAngle = Mathf.LerpAngle(leftHinge.localEulerAngles.y, leftTargetAngle, Time.deltaTime * speed);
        leftHinge.localEulerAngles = new Vector3(0, currentLeftAngle, 0);

        // Smoothly rotate right hinge
        float currentRightAngle = Mathf.LerpAngle(rightHinge.localEulerAngles.y, rightTargetAngle, Time.deltaTime * speed);
        rightHinge.localEulerAngles = new Vector3(0, currentRightAngle, 0);
    }


    public void ActivateKey()
    {
        if (NeedsKey1)
        {
            HasKey1 = true;
        }
        if (NeedsKey2)
        {
            HasKey2 = true;
        }
    }

  
}
