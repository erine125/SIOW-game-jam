using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleState : MonoBehaviour
{
    //TODO: Consider revising these bool below to just enum states
    //states to tell us if needle is equipped by player, being thrown, or in the middle of recalling
    bool equipped = true;
    bool throwing = false;
    bool recalling = false;

    //TODO: need reference to wielder/player; temporarily done by public reference
    public GameObject wielder;

    // Start is called before the first frame update
    void Start()
    {
        SetEquipped();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsEquipped()
    {
        return equipped;
    }

    public bool IsThrowing()
    {
        return throwing;
    }

    public bool IsRecalling()
    {
        return recalling;
    }

    //needle is considered to be still if it's not equipped and not in a throwing/recalling state
    public bool IsStill()
    {
        return (!equipped && !throwing && !recalling);
    }

    public void SetEquipped()
    {
        equipped = true;
        throwing = false;
        recalling = false;
    }

    public void SetThrown()
    {
        equipped = false;
        throwing = true;
        recalling = false;
    }

    public void SetStill()
    {
        equipped = false;
        throwing = false;
        recalling = false;
    }

    public void SetRecalling()
    {
        equipped = false;
        throwing = false;
        recalling = true;
    }

}
