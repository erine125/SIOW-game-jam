using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerupInventory : MonoBehaviour
{
    //TODO: This script will track what powerups player has unlocked
    //TODO: consider how to make this work with save files (as in permanently remember a powerup is unlocked)
    public bool needleUnlocked = true;
    public GameObject needlePrefab;

    public bool propelUnlocked = true;
    private PlayerNeedle playerNeedle = null;

    // Start is called before the first frame update
    void Start()
    {
        //if needle powerup is unlocked, ensure player has a needle equipped (or create one as needed)
        playerNeedle = this.GetComponent<PlayerNeedle>();
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
        if (needleUnlocked && playerNeedle.needle == null)
        {
            GameObject needleObject = GameObject.Instantiate(needlePrefab);
            //make sure player and needle recognize each other as the wielder & weapon/needle accordingly
            needleObject.GetComponent<NeedleState>().wielder = this.gameObject;
            playerNeedle.needle = needleObject.GetComponent<NeedleMovement>();
        }
    }

    public bool HasPropelUnlocked()
    {
        return propelUnlocked;
    }
}
