using UnityEngine;

public class TargetPointer : MonoBehaviour
{
    [SerializeField] private Transform[] targets; 
    private int currentTargetIndex = 0;
    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.useWorldSpace = true;
    }

    void Update()
    {
        if (targets.Length == 0) return;

      
        lineRenderer.SetPosition(0, transform.position);                 
        lineRenderer.SetPosition(1, targets[currentTargetIndex].position); 
    }

   
    public void SwitchTarget()
    {
        currentTargetIndex = (currentTargetIndex + 1) % targets.Length;
    }

   
    public void SwitchToTarget(int index)
    {
        if (index >= 0 && index < targets.Length)
            currentTargetIndex = index;
    }

    public void SwitchToTarget(Transform newTarget)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] == newTarget)
            {
                currentTargetIndex = i;
                return;
            }
        }
    }
}