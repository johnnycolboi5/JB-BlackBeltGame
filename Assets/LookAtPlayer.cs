using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform player; // Reference to the player's transform

    void Update()
    {
        if (player != null)
        {
            transform.LookAt(player); // Make the object look at the player
        }
    }
}