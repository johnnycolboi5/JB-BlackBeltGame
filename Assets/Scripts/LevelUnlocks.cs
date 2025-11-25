using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;


public static class LevelUnlocks
{
    public static bool IsLevelUnlocked(string levelName)
    {
        if (PersistentGameManager.Instance == null)
        {
            Debug.LogWarning("PersistentGameManager instance not found!");
            return false;
        }

        switch (levelName.Replace(" ", ""))
        {
            case "Level1":
                return PersistentGameManager.Instance.Level1Unlocked;
            case "Level2":
                return PersistentGameManager.Instance.Level2Unlocked;
            case "Level3":
                return PersistentGameManager.Instance.Level3Unlocked;
            default:
                Debug.LogWarning("Unknown level name: " + levelName);
                return false;
        }
    }
}
