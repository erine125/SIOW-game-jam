using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//dummy script to force camera to follow player
public class DummyCamera : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float playerX = player.transform.position.x;
        float playerY = player.transform.position.y;
        this.transform.position = new Vector3(playerX, playerY, this.transform.position.z);
    }
}
