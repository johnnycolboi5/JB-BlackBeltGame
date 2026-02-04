using UnityEngine;
using UnityEngine.UI;

public class DirectionBar : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform target;
    public RectTransform dot;

    [Header("Settings")]
    public float maxAngle = 90f;
    public float smoothSpeed = 10f;

    [Header("Flicker Settings")]
    public float visibleDuration = 1f;   // dot ON time
    public float hiddenDuration = 3f;    // dot OFF time

    float barHalfWidth;
    float flickerTimer;
    bool isVisible = true;
    Image dotImage;

    void Start()
    {
        barHalfWidth = ((RectTransform)transform).rect.width / 2f;
        dotImage = dot.GetComponent<Image>();
        flickerTimer = visibleDuration;   // start visible
    }

    void Update()
    {
        UpdateDotPosition();
        UpdateFlicker();
    }

    void UpdateDotPosition()
    {
        Vector3 toTarget = target.position - player.position;
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
            // switch to hidden
            isVisible = false;
            dotImage.enabled = false;
            flickerTimer = hiddenDuration;
        }
        else if (!isVisible && flickerTimer <= 0f)
        {
            // switch to visible
            isVisible = true;
            dotImage.enabled = true;
            flickerTimer = visibleDuration;
        }
    }
}
