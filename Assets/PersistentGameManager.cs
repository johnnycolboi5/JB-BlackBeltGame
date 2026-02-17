using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PersistentGameManager : MonoBehaviour
{
    public static Per0sistentGameManager Instance;

    [Header("Fade Settings")]
    public Image fadeImage;
    public float fadeSpeed = 2f;

    [Header("Level Unlocks")]
    public bool Level1Unlocked = true;
    public bool Level2Unlocked = false;
    public bool Level3Unlocked = false;

    [Header("Level Scene Names")]
    public string Level1Scene = "Level 1";
    public string Level2Scene = "Level 2";
    public string Level3Scene = "Level 3";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("PersistentGameManager Awake called, Instance set!");
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Start Scene")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void LoadSceneWithFade(string sceneName)
    {
        if (!gameObject.activeInHierarchy)
            return;

        StartCoroutine(FadeOutLoadIn(sceneName));
    }


    private IEnumerator FadeOutLoadIn(string sceneName)
    {
        if (fadeImage != null)
            fadeImage.gameObject.SetActive(true);
      

      /*  // Fade out
        for (float t = 0; t <= 1f; t += Time.deltaTime * fadeSpeed)
        {
            if (fadeImage != null)
                fadeImage.color = new Color(0, 0, 0, t);
            yield return null;
        }*/
 
        // Async load
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
            yield return null;

        // Fade in
        if (fadeImage != null)
        {
            for (float t = 1f; t >= 0f; t -= Time.deltaTime * fadeSpeed)
            {
                fadeImage.color = new Color(0, 0, 0, t);
                yield return null;
            }

            fadeImage.gameObject.SetActive(false);
        }
    }

    // Auto-create instance if missing
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

}
