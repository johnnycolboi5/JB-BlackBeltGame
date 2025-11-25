using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class intera : MonoBehaviour
{
    public GameObject Door;
    public GameObject Door2;
    private DoubleGateDoor doorScript;
    private DoubleGateDoor doorScript2;

    private GameObject nearbyKey;
    private GameObject nearbyDoor;
    private GameObject pickupUI;

    private bool iHaveKey1 = false;
    private bool iHaveKey2 = false;

    void Start()
    {
        doorScript = Door.GetComponent<DoubleGateDoor>();
        doorScript2 = Door2.GetComponent<DoubleGateDoor>();
    }

    

    void Update()
    {
        // ======================
        // KEY PICKUP
        // ======================
        if (nearbyKey != null && Input.GetKeyDown(KeyCode.E))
        {
            if (nearbyKey.CompareTag("Key1"))
                iHaveKey1 = true;

            if (nearbyKey.CompareTag("Key2"))
                iHaveKey2 = true;

            Destroy(nearbyKey);
            nearbyKey = null;

            if (pickupUI != null)
                pickupUI.SetActive(false);

            pickupUI = null;
        }

        // ======================
        // DOOR INTERACT
        // ======================
        if (nearbyDoor != null && Input.GetKeyDown(KeyCode.E))
        {
            DoubleGateDoor doorSc = nearbyDoor.GetComponentInParent<DoubleGateDoor>();

            bool unlocked = false;

            if (doorSc.NeedsKey1 && iHaveKey1)
            {
                doorSc.HasKey1 = true;
                unlocked = true;
            }

            if (doorSc.NeedsKey2 && iHaveKey2)
            {
                doorSc.HasKey2 = true;
                unlocked = true;
            }

            // If correct key is owned, toggle the door open
            if (unlocked)
            {
                doorSc.ToggleDoor();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Key1") ||
            collision.gameObject.CompareTag("Key2"))
        {
            nearbyKey = collision.gameObject;
        }

        if (collision.gameObject.CompareTag("Gate"))
        {
            nearbyDoor = collision.gameObject;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == nearbyKey)
        {
            if (pickupUI != null)
                pickupUI.SetActive(false);

            nearbyKey = null;
            pickupUI = null;
        }

        if (collision.gameObject == nearbyDoor)
        {
            nearbyDoor = null;
        }
    }
}
