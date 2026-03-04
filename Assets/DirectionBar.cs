using UnityEngine;
using UnityEngine.UI;

public class DirectionBar : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform originalTarget;
    public Transform keyTransform;
    public RectTransform dot;

    [Header("Settings")]
    public float maxAngle = 90f;
    public float smoothSpeed = 10f;

    [Header("Flicker Settings")]
    public float visibleDuration = 1f;
    public float hiddenDuration = 3f;

    float barHalfWidth;
    float flickerTimer;
    bool isVisible = true;
    Image dotImage;
    Transform currentTarget;

    void Start()
    {
        barHalfWidth = ((RectTransform)transform).rect.width / 2f;
        dotImage = dot.GetComponent<Image>();
        flickerTimer = visibleDuration;

        currentTarget = keyTransform != null ? keyTransform : originalTarget;
    }

    void Update()
    {
        currentTarget = keyTransform != null ? keyTransform : originalTarget;
        UpdateDotPosition();
        UpdateFlicker();
    }

    void UpdateDotPosition()
    {
        if (currentTarget == null) return;

        Vector3 toTarget = currentTarget.position - player.position;
        toTarget.y = 0f;

        float angle = Vector3.SignedAngle(player.forward, toTarget, Vector3.up);
        float normalized = Mathf.Clamp(angle / maxAngle, -1f, 1f);
        float targetX = normalized * barHalfWidth;

        Vector2 current = dot.anchoredPosition;
        dot.anchoredPosition = Vector2.Lerp(
            current,
            new Vector2(targetX, current.y),
            Time.deltaTime * smoothSpeed
        );
    }

    void UpdateFlicker()
    {
        flickerTimer -= Time.deltaTime;

        if (isVisible && flickerTimer <= 0f)
        {
            isVisible = false;
            dotImage.enabled = false;
            flickerTimer = hiddenDuration;
        }
        else if (!isVisible && flickerTimer <= 0f)
        {
            isVisible = true;
            dotImage.enabled = true;
            flickerTimer = visibleDuration;
        }
    }
}