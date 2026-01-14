using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;


public class LevelEndPortal : MonoBehaviour
{
    public enum LevelToUnlock { Level1, Level2, Level3 }

    [Header("Level Unlock Settings")]
    public LevelToUnlock unlockLevel;  // Choose in inspector

    [Header("Scene to Load After Unlock")]
    public string sceneToLoad = "Start Scene"; // Assign any scene name

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        // Safely get or create the PersistentGameManager
        PersistentGameManager manager = PersistentGameManager.Instance;

        // Unlock the chosen level
        switch (unlockLevel)
        {
            case LevelToUnlock.Level1:
                manager.Level1Unlocked = true;
                break;
            case LevelToUnlock.Level2:
                manager.Level2Unlocked = true;
                break;
            case LevelToUnlock.Level3:
                manager.Level3Unlocked = true;
                break;
        }

        Debug.Log($"{unlockLevel} unlocked via portal!");

        // Load the specified scene with fade
        if (!string.IsNullOrEmpty(sceneToLoad))
            manager.LoadSceneWithFade(sceneToLoad);
    }
}
