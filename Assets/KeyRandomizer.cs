using UnityEngine;

public class KeyRandomizer : MonoBehaviour
{
    [Header("Possible Key Locations")]
    public Transform[] possibleLocations; // Assign in inspector

    private void Start()
    {
        if (possibleLocations == null || possibleLocations.Length == 0)
        {
            Debug.LogError("No possible locations assigned for the key!");
            return;
        }

        // Pick a random location from the array
        int randomIndex = Random.Range(0, possibleLocations.Length);

        // Move the key to that location
        transform.position = possibleLocations[randomIndex].position;
        transform.rotation = possibleLocations[randomIndex].rotation;

        Debug.Log($"Key moved to location #{randomIndex}: {possibleLocations[randomIndex].name}");
    }
}
