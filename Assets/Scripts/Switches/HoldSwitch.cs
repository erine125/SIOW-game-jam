using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;
using UnityEngine.U2D;

public class HoldSwitch : Switch
{
    //tracks if switch is touching anything (namely player or needle)
    //bool isTouching = false;
    int numCollisions = 0;

    void Update()
    {
        //if switch is currently on but is being touched by nothing, prepare to turn everything off
        if (IsTurnedOn() && numCollisions == 0)
        {
            TurnOff();
        }
        if (!IsTurnedOn() && numCollisions > 0)
        {
            TurnOn();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if not activated and touching player or knife, turn trigger
        if (other.gameObject.layer.Equals(playerLayer) || other.gameObject.layer.Equals(needleLayer))
        {
            numCollisions += 1;
        }
    }

    /*
    private void OnTriggerStay2D(Collider2D other)
    {
        //if not activated and touching player or knife, turn trigger
        if (other.gameObject.layer.Equals(playerLayer) || other.gameObject.layer.Equals(needleLayer))
        {
            isTouching = true;
        }
        
    }
    */

    private void OnTriggerExit2D(Collider2D other)
    {
        //when player/needle leaves, set isTouching to false (which will be immediately true if any other collider stays on switch)
        if (other.gameObject.layer.Equals(playerLayer) || other.gameObject.layer.Equals(needleLayer))
        {
            numCollisions -= 1;
            //isTouching = false;
        }
        /*
        //turn off trigger when player or weapon leaves
        if (IsTurnedOn())
        {
            if (other.gameObject.layer.Equals(playerLayer) || other.gameObject.layer.Equals(needleLayer))
            {
                TurnOff();
            }
        }
        */
    }

}
