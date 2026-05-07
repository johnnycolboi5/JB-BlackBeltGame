using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

using TMPro;

public class intera : MonoBehaviour
{ 

    public Tutorial tutorial;

    public GameObject Door;
    public GameObject Door2;
    private DoubleGateDoor doorScript;
    private DoubleGateDoor doorScript2;

    private GameObject nearbyKey;
    private GameObject nearbyDoor;
    private GameObject pickupUI;
    public TextMeshProUGUI DoorText;

    private bool iHaveKey1 = false;
    private bool iHaveKey2 = false;

    private bool portal = false;
    private bool Gate = false;

    public GameObject keycheck;

    void Start()
    {
        if (Door != null) doorScript = Door.GetComponent<DoubleGateDoor>();
        if (Door2 != null) doorScript2 = Door2.GetComponent<DoubleGateDoor>();

        if (keycheck != null) keycheck.SetActive(false);
    }

    

    void Update()
    {

        if (nearbyKey != null && Input.GetKeyDown(KeyCode.E))
        {
            if (nearbyKey.CompareTag("Key1"))
            {
                iHaveKey1 = true;
                Destroy(nearbyKey);
                keycheck.SetActive(true);
            }


            if (nearbyKey.CompareTag("Key2"))
            {
                iHaveKey2 = true;
                Destroy(nearbyKey);
            }

            if (tutorial != null)
            {
                tutorial.ShowNextText();
                
            }
            else
            {
                Debug.Log("YOU FORGOT TO ASSIGN THE SCRIPT IN THE UNITY YOU FOOL!!!");
            }
            DoorText.text = "Press E to open!";
            nearbyKey = null;

            if (pickupUI != null)
            {
                pickupUI.SetActive(false);
            }
            pickupUI = null;
        }


        if (nearbyDoor != null && Input.GetKeyDown(KeyCode.E))
        {
            DoubleGateDoor doorSc = nearbyDoor.GetComponentInParent<DoubleGateDoor>();

            bool unlocked = false;

            if (doorSc.NeedsKey1 && iHaveKey1)
            {
                doorSc.HasKey1 = true;
                unlocked = true;

                GetComponent<TargetPointer>().SwitchTarget();


                if (tutorial != null)
                {
                    tutorial.ShowNextText();

                }
            }

            if (doorSc.NeedsKey2 && iHaveKey2)
            {
                doorSc.HasKey2 = true;
                unlocked = true;
            }

          
            if (unlocked)
            {
             //   if (Collision.gameObject)
               // {
                    doorSc.ToggleDoor();
                //}
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
