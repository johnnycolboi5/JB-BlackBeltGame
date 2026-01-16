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

    private TextMeshPro hoveredText;

    void Start()
    {
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            TextMeshPro text = hit.collider.GetComponent<TextMeshPro>();
            if (text != null)
            {

                Debug.Log(text.text);
                HandleHover(text, hit.point);

                if (Input.GetMouseButtonDown(0))
                    HandleClick(text, hit.point);

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

        if (LevelUnlocks.IsLevelUnlocked(text.text))
        {
            text.color = hoverColor;
            if (hoverSound)
                AudioSource.PlayClipAtPoint(hoverSound, hitPoint);
        }
    }

    void HandleClick(TextMeshPro text, Vector3 hitPoint)
    {
        if (!LevelUnlocks.IsLevelUnlocked(text.text))
        {
            text.color = lockedColor * 1.5f;
            if (lockedSound)
                AudioSource.PlayClipAtPoint(lockedSound, hitPoint);
            return;
        }

        if (PersistentGameManager.Instance == null) return;

        Debug.Log(text.text);

        string sceneToLoad = GetSceneNameForText(text.text);
        PersistentGameManager.Instance.LoadSceneWithFade(sceneToLoad);
    }

    string GetSceneNameForText(string buttonText)
    {
        // Map display text to actual scene name
        switch (buttonText.Replace(" ", ""))
        {
            case "Level1": return PersistentGameManager.Instance.Level1Scene;
            case "Level2": return PersistentGameManager.Instance.Level2Scene;
            case "Level3": return PersistentGameManager.Instance.Level3Scene;
            default: return buttonText; // fallback: use text as scene name
        }
    }

    void ResetHover()
    {
        if (hoveredText == null) return;

        if (LevelUnlocks.IsLevelUnlocked(hoveredText.text))
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
