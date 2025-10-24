using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
   

    public GameObject Player;

    public GameObject Cube1;
   
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //Vector3 teleportPos = other.gameObject.transform.position;
           // teleportPos.z = gameObject.transform.position.z;

           // teleportPos.y = Cube1.transform.position.y + 50f;

            other.gameObject.transform.position = Cube1.transform.position;
            Debug.Log("WHATSUP");

        }
    }

    void Update()
    {
        Vector3 cube1Pos = Cube1.transform.position;

        //Follow Player's Z-position
        cube1Pos.x = Player.transform.position.x;

        Cube1.transform.position = cube1Pos;


    }
}
