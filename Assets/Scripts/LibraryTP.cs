using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryTP : MonoBehaviour
{
    public GameObject player;
    public GameObject SpawnPoint;
    private bool isPlayerInTrigger = false;

    // Update is called once per frame
    void Update()
    {
        // Check if the player is in the trigger and interaction is required
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            MovePlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Check if the colliding object has the tag "Player"
        if (col.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
        }
    }

    // Update is called once per frame
    void MovePlayer()
    {
        Transform spawnPoint = SpawnPoint.transform;
        if (spawnPoint != null)
        {
            // Move the player to the correct spawn point
            player.transform.position = spawnPoint.position;
        }
    }
}
