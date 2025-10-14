using UnityEngine;

public class intera : MonoBehaviour
{
    public GameObject Door;
    private DoubleGateDoor doorScript;

    private GameObject nearbyKey;
    private GameObject pickupUI;

    void Start()
    {
        doorScript = Door.GetComponent<DoubleGateDoor>();
    }

    void Update()
    {
        if (nearbyKey != null)
        {
            if (pickupUI != null)
                pickupUI.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Picked up key!");
                doorScript.HasKey = true;
                Destroy(nearbyKey);
                nearbyKey = null;
                pickupUI = null;
            }
        }
        else
        {
            if (pickupUI != null)
                pickupUI.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Key"))
        {
            nearbyKey = collision.gameObject;
            pickupUI = nearbyKey.transform.Find("PickupCanvas")?.gameObject;
            Debug.Log("Near key: " + nearbyKey.name);
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
    }

    void LateUpdate()
    {
        if (pickupUI != null)
        {
            pickupUI.transform.LookAt(Camera.main.transform);
            pickupUI.transform.Rotate(0, 180, 0); // Flip text
        }
    }
}
