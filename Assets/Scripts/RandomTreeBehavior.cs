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
    private int lookAtChance; 
    private int lookAtChancedeath;
    void Start()
    {
       
        float randomAngle = Random.Range(1f, 361f);
        transform.rotation = Quaternion.Euler(0f, randomAngle, 0f);

      
        if (TreeMeshOptions.Length > 0)
        {
            TreeMesh.mesh = TreeMeshOptions[Random.Range(0, TreeMeshOptions.Length)];
        }

       
        lookAtChance = Random.Range(1, 3);
        lookAtChancedeath = Random.Range(1, 3);
    }

    void Update()
    {
        if (lookAtChancedeath == 1 && target != null)
        {
            Destroy(gameObject);
            if (lookAtChance == 1 && target != null)
            {
                Vector3 lookPos = target.position - transform.position;
                lookPos.y = 0; 
                Quaternion rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = rotation;
            }
        }
    }
}
