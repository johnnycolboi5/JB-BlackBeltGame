using UnityEngine;

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
        //KEY INTERACTION
        if (nearbyKey != null)
        {
            if (pickupUI != null)
                pickupUI.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (nearbyKey.tag == "Key1")
                {
                    iHaveKey1 = true;
                }
                if (nearbyKey.tag == "Key2")
                {
                    iHaveKey2 = true;
                }
                Destroy(nearbyKey);

                //doorScript.HasKey1 = true;
                //doorScript.HasKey2 = true;
                //nearbyKey = null;
                //nearbyKey2 = null;
                pickupUI = null;
            }
        }
        else
        {
            if (pickupUI != null)
                pickupUI.SetActive(false);
        }

        //DOOR INTERACTION
        if (nearbyDoor != null && Input.GetKeyDown(KeyCode.E))
        {
            DoubleGateDoor doorSc = nearbyDoor.GetComponentInParent<DoubleGateDoor>();
            if (doorSc.NeedsKey1 && iHaveKey1)
            {
                doorSc.ActivateKey();
                Debug.Log("yeah ur good twin");
            }
            if (doorSc.NeedsKey2 && iHaveKey2)
            {
                doorSc.ActivateKey();
                Debug.Log("yeah ur good twin");
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        
        if (collision.gameObject.CompareTag("Key1") || collision.gameObject.CompareTag("Key2"))
        {
            nearbyKey = collision.gameObject;
          // pickupUI = nearbyKey.transform.Find("PickupCanvas")?.gameObject;
            Debug.Log("Near key: " + nearbyKey.name);
        }
        if (collision.gameObject.CompareTag("Gate"))
        {
            nearbyDoor = collision.gameObject;
            // pickupUI = nearbyKey.transform.Find("PickupCanvas")?.gameObject;
            Debug.Log("Near door: " + nearbyDoor.transform.parent.name);
        }

        //if (collision.gameObject.CompareTag("Key2"))
        //{
        //    nearbyKey = collision.gameObject;
        //   // pickupUI = nearbyKey2.transform.Find("PickupCanvas")?.gameObject;
        //    Debug.Log("Near key: " + nearbyKey2.name);
        //    Destroy(collision.gameObject);
        //}
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
