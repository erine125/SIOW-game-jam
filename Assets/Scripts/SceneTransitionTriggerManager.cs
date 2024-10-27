using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTriggerManager : MonoBehaviour
{

    public string nextSceneName;
    public string buildingExitSpawnPoint;
    public bool needsInteract = false;

    private bool isPlayerInTrigger = false;

    // Update is called once per frame
    void Update()
    {
        // Check if the player is in the trigger and interaction is required
        if (isPlayerInTrigger && needsInteract && Input.GetKeyDown(KeyCode.E))
        {
            TriggerSceneTransition();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Check if the colliding object has the tag "Player"
        if (col.CompareTag("Player"))
        {
            isPlayerInTrigger = true;

            // If interaction is not required, trigger the scene transition immediately
            if (!needsInteract)
            {
                TriggerSceneTransition();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        // Reset the trigger flag when the player exits
        if (col.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
        }
    }

    private void TriggerSceneTransition()
    {
        // Set the last building if applicable
        if (nextSceneName == "Ext-Pier")
        {
            GameStateManager.Instance.SetLastBuilding(buildingExitSpawnPoint);
        }

        // Load the specified scene
        SceneManager.LoadScene(nextSceneName);
    }
}
