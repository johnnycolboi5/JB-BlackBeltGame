using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapOnOff : MonoBehaviour
{
    public GameObject Map;
    void Start()
    {
        Map.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Map.SetActive(true);
        }


        if ( Input.GetKeyUp(KeyCode.M))
        {
            Map.SetActive(true);
        }
    }

   

}
