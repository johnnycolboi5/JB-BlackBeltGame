using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTreeBehavior : MonoBehaviour
{
    [Header("Mesh Options")]
    public MeshFilter TreeMesh;
    public Mesh[] TreeMeshOptions;

    [Header("Look At Settings")]
    public Transform target;
    private int lookAtChance; // 1 or 2

    void Start()
    {
        // Random rotation
        float randomAngle = Random.Range(1f, 361f);
        transform.rotation = Quaternion.Euler(0f, randomAngle, 0f);

        // Random mesh
        if (TreeMeshOptions.Length > 0)
        {
            TreeMesh.mesh = TreeMeshOptions[Random.Range(0, TreeMeshOptions.Length)];
        }

        // Decide if this tree will look at target (1 = yes, 2 = no)
        lookAtChance = Random.Range(1, 3); // gives 1 or 2
    }

    void Update()
    {
        if (lookAtChance == 1 && target != null)
        {
            Vector3 lookPos = target.position - transform.position;
            lookPos.y = 0; // ignore vertical rotation
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = rotation;
        }
    }
}
