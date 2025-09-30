using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
    public GameObject NextPortal;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Vector3 teleportPos = other.gameObject.transform.position;
            teleportPos.z = NextPortal.transform.position.z;

            other.gameObject.transform.position = NextPortal.transform.position;
            Debug.Log("WHATSUP");
        }
    }
}
