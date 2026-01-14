using UnityEngine;
using UnityEngine.UI;

public class DirectionBar : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform target;
    public RectTransform dot;

    [Header("Settings")]
    public float maxAngle = 90f;      // angle that maps to bar edge
    public float smoothSpeed = 10f;

    float barHalfWidth;

    void Start()
    {
        barHalfWidth = ((RectTransform)transform).rect.width / 2f;
    }

    void Update()
    {
        Vector3 toTarget = target.position - player.position;

        // Ignore vertical difference
        toTarget.y = 0f;

        // Signed angle around Y axis
        float angle = Vector3.SignedAngle(player.forward, toTarget, Vector3.up);

        // Normalize angle to bar range
        float normalized = Mathf.Clamp(angle / maxAngle, -1f, 1f);

        float targetX = normalized * barHalfWidth;

        // Smooth movement
        Vector2 current = dot.anchoredPosition;
        dot.anchoredPosition = Vector2.Lerp(
            current,
            new Vector2(targetX, current.y),
            Time.deltaTime * smoothSpeed
        );
    }
}
