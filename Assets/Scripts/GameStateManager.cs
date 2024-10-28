using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    // Tracks the spawn point in the town scene
    public string lastBuildingExited;

    private void Awake()
    {
        // Singleton pattern to persist the GameManager across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetLastBuilding(string buildingName)
    {
        lastBuildingExited = buildingName;
    }
}
