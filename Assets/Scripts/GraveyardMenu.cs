using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GraveyardMenu : MonoBehaviour
{
    [Header("Settings")]
    public float rayDistance = 10f;

    [Header("Colors")]
    public Color normalColor = Color.green;
    public Color hoverColor = new Color(0.6f, 0, 1f); // purple
    public Color lockedColor = new Color(0.4f, 0, 0); // dark red

    [Header("Sounds")]
    public AudioClip hoverSound;
    public AudioClip lockedSound;

    private TextMeshPro hoveredText;

    [Header("Level Unlocks")]
    public bool level2Unlocked = false;
    public bool level3Unlocked = false;

    void Start()
    {
        // Load unlock states (persistent)
        level2Unlocked = PlayerPrefs.GetInt("Level2Unlocked", 0) == 1;
        level3Unlocked = PlayerPrefs.GetInt("Level3Unlocked", 0) == 1;

        // Set up initial appearance of each tombstone
        TextMeshPro[] allTexts = FindObjectsOfType<TextMeshPro>();
        foreach (TextMeshPro text in allTexts)
        {
            if (!IsLevelUnlocked(text.text))
                SetLockedAppearance(text);
            else
                SetNormalAppearance(text);
        }
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            TextMeshPro text = hit.collider.GetComponent<TextMeshPro>();

            if (text != null)
            {
                // Hover start
                if (hoveredText != text)
                {
                    ResetHover();
                    hoveredText = text;

                    if (IsLevelUnlocked(text.text))
                    {
                        hoveredText.color = hoverColor;
                        if (hoverSound != null)
                            AudioSource.PlayClipAtPoint(hoverSound, hit.point);
                    }
                }

                // Click
                if (Input.GetMouseButtonDown(0))
                {
                    string sceneName = text.text;

                    if (IsLevelUnlocked(sceneName))
                    {
                        if (Application.CanStreamedLevelBeLoaded(sceneName))
                        {
                            SceneManager.LoadScene(sceneName);
                        }
                        else
                        {
                            Debug.LogWarning("Scene not found: " + sceneName);
                        }
                    }
                    else
                    {
                        // Locked feedback
                        Debug.Log("Level is locked: " + sceneName);
                        hoveredText.color = lockedColor * 1.5f; // brighten briefly
                        if (lockedSound != null)
                            AudioSource.PlayClipAtPoint(lockedSound, hit.point);
                    }
                }
            }
            else ResetHover();
        }
        else ResetHover();
    }

    bool IsLevelUnlocked(string sceneName)
    {
        switch (sceneName)
        {
            case "Level 1": return true;
            case "Level 2": return level2Unlocked;
            case "Level 3": return level3Unlocked;
            default: return true;
        }
    }

    void ResetHover()
    {
        if (hoveredText != null)
        {
            if (IsLevelUnlocked(hoveredText.text))
                SetNormalAppearance(hoveredText);
            else
                SetLockedAppearance(hoveredText);

            hoveredText = null;
        }
    }

    void SetLockedAppearance(TextMeshPro text)
    {
        text.color = lockedColor;
        var mat = text.fontMaterial;
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", lockedColor * 2f);
    }

    void SetNormalAppearance(TextMeshPro text)
    {
        text.color = normalColor;
        var mat = text.fontMaterial;
        mat.DisableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", Color.black);
    }

    // Optional: call these from other scripts when levels are completed
    public void UnlockLevel2()
    {
        PlayerPrefs.SetInt("Level2Unlocked", 1);
        PlayerPrefs.Save();
        level2Unlocked = true;
    }

    public void UnlockLevel3()
    {
        PlayerPrefs.SetInt("Level3Unlocked", 1);
        PlayerPrefs.Save();
        level3Unlocked = true;
    }
}
