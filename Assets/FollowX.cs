using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowX : MonoBehaviour
{
    public GameObject Cube1;
    public GameObject Cube2;
    void Update()
    {
        Vector3 cube1Pos = Cube1.transform.position;

        //Follow Player's Z-position
        cube1Pos.x = transform.position.x;

        Cube1.transform.position = cube1Pos;


        Vector3 cube2Pos = Cube1.transform.position;

        //Follow Player's Z-position
        cube2Pos.x = transform.position.x;

        Cube2.transform.position = cube2Pos;
    }
}
