using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public Transform target;

    public int randomchance;
    void Update()
    {
        if (randomchance >= 1f)
        {
            if (target != null)
            {
                Vector3 lookPos = target.position - transform.position;
                lookPos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = rotation;
            }
        }
    }

    void Start ()
    {
        float randomchance = Random.Range(1f, 4f);
    }
}
