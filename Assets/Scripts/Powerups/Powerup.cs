using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    //TODO: consider how to make this work with save files (as in permanently remember a powerup is unlocked)
    //give list of bools to represent which powerups this item grants
    public bool grantsPropel = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerPowerupInventory playerPowerupInventory = collision.GetComponent<PlayerPowerupInventory>();
        if (playerPowerupInventory != null)
        {
            if (grantsPropel)
            {
                playerPowerupInventory.propelUnlocked = true;
            }


            Destroy(this.gameObject);
        }
    }
}
