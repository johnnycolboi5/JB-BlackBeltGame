using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GraveyardMenu : MonoBehaviour
{
    public float rayDistance = 10f;
    public Color normalColor = Color.green;
    public Color hoverColor = new Color(0.6f, 0, 1f);
    public Color lockedColor = new Color(0.4f, 0, 0);
    public AudioClip hoverSound;
    public AudioClip lockedSound;
    public credits creditsScript;
    public GameObject instructions1Object; // Assign in Inspector
    public GameObject instructions2Object; // Assign in Inspector
    private TextMeshPro hoveredText;

    void Start()
    {
        if (instructions1Object) instructions1Object.SetActive(false);
        if (instructions2Object) instructions2Object.SetActive(false);

        TextMeshPro[] allTexts = FindObjectsOfType<TextMeshPro>();
        foreach (TextMeshPro text in allTexts)
        {
            text.fontMaterial = new Material(text.fontSharedMaterial);
            if (!LevelUnlocks.IsLevelUnlocked(text.text))
                SetLockedAppearance(text);
            else
                SetNormalAppearance(text);
        }
    }

    void Update()
    {
        // Click anywhere to close instruction panels
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
            {
                TextMeshPro text = hit.collider.GetComponent<TextMeshPro>();
                if (text != null)
                {
                    HandleClick(text, hit.point);
                    return;
                }
            }
            // Clicked on nothing — close panels
            if (instructions1Object && instructions1Object.activeSelf)
                instructions1Object.SetActive(false);
            if (instructions2Object && instructions2Object.activeSelf)
                instructions2Object.SetActive(false);
        }

        Ray hoverRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(hoverRay, out RaycastHit hoverHit, rayDistance))
        {
            TextMeshPro text = hoverHit.collider.GetComponent<TextMeshPro>();
            if (text != null)
            {
                HandleHover(text, hoverHit.point);
                return;
            }
        }
        ResetHover();
    }

    void HandleHover(TextMeshPro text, Vector3 hitPoint)
    {
        if (hoveredText == text) return;
        ResetHover();
        hoveredText = text;

        bool isInteractable = LevelUnlocks.IsLevelUnlocked(text.text)
            || text.text == "Credits"
            || text.text == "Instructions 1"
            || text.text == "Instructions 2";

        if (isInteractable)
        {
            text.color = hoverColor;
            if (hoverSound)
                AudioSource.PlayClipAtPoint(hoverSound, hitPoint);
        }
    }

    void HandleClick(TextMeshPro text, Vector3 hitPoint)
    {
        if (text.text == "Credits")
        {
            if (creditsScript != null)
                creditsScript.turnOnScreen();
            return;
        }

        if (text.text == "Instructions 1")
        {
            if (instructions1Object)
                instructions1Object.SetActive(!instructions1Object.activeSelf);
            if (instructions2Object)
                instructions2Object.SetActive(false);
            return;
        }

        if (text.text == "Instructions 2")
        {
            if (instructions2Object)
                instructions2Object.SetActive(!instructions2Object.activeSelf);
            if (instructions1Object)
                instructions1Object.SetActive(false);
            return;
        }

        if (!LevelUnlocks.IsLevelUnlocked(text.text))
        {
            text.color = lockedColor * 1.5f;
            if (lockedSound)
                AudioSource.PlayClipAtPoint(lockedSound, hitPoint);
            return;
        }

        if (PersistentGameManager.Instance == null) return;
        string sceneToLoad = GetSceneNameForText(text.text);
        PersistentGameManager.Instance.LoadSceneWithFade(sceneToLoad);
    }

    string GetSceneNameForText(string buttonText)
    {
        switch (buttonText.Replace(" ", ""))
        {
            case "Level1": return PersistentGameManager.Instance.Level1Scene;
            case "Level2": return PersistentGameManager.Instance.Level2Scene;
            case "Level3": return PersistentGameManager.Instance.Level3Scene;
            default: return buttonText;
        }
    }

    void ResetHover()
    {
        if (hoveredText == null) return;
        bool isInteractable = LevelUnlocks.IsLevelUnlocked(hoveredText.text)
            || hoveredText.text == "Credits"
            || hoveredText.text == "Instructions 1"
            || hoveredText.text == "Instructions 2";

        if (isInteractable)
            SetNormalAppearance(hoveredText);
        else
            SetLockedAppearance(hoveredText);
        hoveredText = null;
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

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}