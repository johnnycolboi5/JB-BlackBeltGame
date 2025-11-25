using UnityEngine;

public class DoubleGateDoor : MonoBehaviour
{
    public Transform leftHinge;
    public Transform rightHinge;
    public Transform player;
    public float openAngle = 90f;
    public float speed = 2f;
    public float interactDistance = 20f;

    private bool isOpen = false;
    private float leftTargetAngle = 0f;
    private float rightTargetAngle = 0f;

    public bool HasKey1;
    public bool HasKey2;

    public bool NeedsKey1;
    public bool NeedsKey2;

    void Update()
    {

       // Debug.Log($"[{gameObject.name}] NeedsKey1={NeedsKey1}, HasKey1={HasKey1}, Dist={Vector3.Distance(player.position, (leftHinge.position + rightHinge.position) / 2f)}");
        
        // Rotate left hinge
        float currentLeftAngle = Mathf.LerpAngle(
            leftHinge.localEulerAngles.y, leftTargetAngle,
            Time.deltaTime * speed);
        leftHinge.localEulerAngles = new Vector3(0, currentLeftAngle, 0);

        // Rotate right hinge
        float currentRightAngle = Mathf.LerpAngle(
            rightHinge.localEulerAngles.y, rightTargetAngle,
            Time.deltaTime * speed);
        rightHinge.localEulerAngles = new Vector3(0, currentRightAngle, 0);
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;

        leftTargetAngle = isOpen ? openAngle : 0f;
        rightTargetAngle = isOpen ? -openAngle : 0f;
    }



 

}
