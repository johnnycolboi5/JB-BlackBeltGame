using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class credits : MonoBehaviour
{
    public GameObject creditScreen;

  public  void turnOnScreen()
    {
        creditScreen.SetActive(true);
    }

   public  void turnOffScreen()
    {
        creditScreen.SetActive(false);
    }

}
