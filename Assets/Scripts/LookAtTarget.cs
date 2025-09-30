using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        if (target != null)
        {
            Vector3 lookPos = target.position - transform.position;
            lookPos.y = 0; // ignore vertical rotation
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = rotation;
        }
    }
}
