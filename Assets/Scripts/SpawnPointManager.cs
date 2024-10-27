using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    public GameObject player;  // Reference to the player prefab or instance

    private void Start()
    {
        // Check if GameManager has a stored last building name
        if (!string.IsNullOrEmpty(GameStateManager.Instance.lastBuildingExited))
        {
            // Find the spawn point based on the building name
            Transform spawnPoint = GameObject.Find(GameStateManager.Instance.lastBuildingExited).transform;
            if (spawnPoint != null)
            {
                // Move the player to the correct spawn point
                player.transform.position = spawnPoint.position;
            }
        }
    }
}
