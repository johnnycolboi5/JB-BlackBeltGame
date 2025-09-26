using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    public MeshFilter TreeMesh;
    public Mesh[] TreeMeshOptions;

    // Start is called before the first frame update
    void Start()
    {
        float randomAngle = Random.Range(1f, 361f);
        transform.rotation = Quaternion.Euler(0f, randomAngle, 0f);
        
        if (TreeMeshOptions.Length > 0)
        {
            TreeMesh.mesh = TreeMeshOptions[Random.Range(0, TreeMeshOptions.Length)];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
