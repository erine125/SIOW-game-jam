using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerupInventory : MonoBehaviour
{
    //TODO: This script will track what powerups player has unlocked
    public bool needleUnlocked = true;
    public GameObject needlePrefab;

    // Start is called before the first frame update
    void Start()
    {
        //if needle powerup is unlocked, ensure player has a needle equipped (or create one as needed)
        PlayerNeedle playerNeedle = this.GetComponent<PlayerNeedle>();
        if (needleUnlocked && playerNeedle.needle == null)
        {
            GameObject needleObject = GameObject.Instantiate(needlePrefab);
            //make sure player and needle recognize each other as the wielder & weapon/needle accordingly
            needleObject.GetComponent<NeedleState>().wielder = this.gameObject;
            playerNeedle.needle = needleObject.GetComponent<NeedleMovement>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}