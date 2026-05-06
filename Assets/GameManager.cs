using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [Header("Menu")]
    public GameObject pauseMenu;
    public GameObject oppPause;

    [Header("Player Control")]
    public MonoBehaviour playerMovement;  // Your player movement script
    public MonoBehaviour playerLook;      // Your mouse look script

    private bool isPaused = false;

    void Start()
    {
        pauseMenu.SetActive(false);
        LockCursor(true);                  // Start with cursor locked
    }

    void Update()
    {
        // Toggle pause menu with Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);

        // Stop/resume game time
        Time.timeScale = isPaused ? 0f : 1f;

        // Lock/unlock cursor
        LockCursor(!isPaused);

        // Enable/disable first-person controls
        if (playerMovement != null)
            playerMovement.enabled = !isPaused;
        if (playerLook != null)
            playerLook.enabled = !isPaused;
    }

    void LockCursor(bool locked)
    {
        Cursor.visible = !locked;
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
    }

    public void LoadStartScene()
    {
        isPaused = false;
        Time.timeScale = 1f;
        LockCursor(false); // Start scene probably wants cursor visible
        SceneManager.LoadScene("Start Scene");
    }

    public void LoadLevel1()
    {
        // Reset pause state before loading
        isPaused = false;
        Time.timeScale = 1f;
        LockCursor(true);
        if (playerMovement != null) playerMovement.enabled = true;
        if (playerLook != null) playerLook.enabled = true;

        PersistentGameManager.Instance.LoadSceneWithFade("Level 1");
    }
}
