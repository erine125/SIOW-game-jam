using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;
using UnityEngine.U2D;

//when tapped, the switch stays on for X seconds
public class TapSwitch : Switch
{
    public float switchTimer = 3.0f;

    //TODO: this section of comments (w/IEnumerator) is the old implementation of TapSwitch with a cycle timer ("timer can't be reset until it's over")
    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        //when trigger off and touched by knife or player, turn trigger on
        if (!IsTurnedOn())
        {
            if (other.gameObject.layer.Equals(playerLayer) || other.gameObject.layer.Equals(needleLayer))
            {
                StartCoroutine(SwitchTriggered());
            }
        }


    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //if not activated and still touching player or knife, turn trigger on
        if (!IsTurnedOn())
        {
            if (other.gameObject.layer.Equals(playerLayer) || other.gameObject.layer.Equals(needleLayer))
            {
                StartCoroutine(SwitchTriggered());
            }
        }
    }
    */

    IEnumerator SwitchTriggered()
    {
        TurnOn();

        yield return new WaitForSeconds(switchTimer);

        TurnOff();

        yield return null;
    }

    //TODO: this section of code below is the new implementation of TapSwitch ("timer only counts down when player/knife leaves it")
    //tracks if switch is touching anything (namely player or needle)
    int numCollisions = 0;
    Coroutine timerCoroutine = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if not activated and touching player or knife, track the collision count
        if (other.gameObject.layer.Equals(playerLayer) || other.gameObject.layer.Equals(needleLayer))
        {
            numCollisions += 1;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //while touching player or knife, keep objects on
        if (other.gameObject.layer.Equals(playerLayer) || other.gameObject.layer.Equals(needleLayer))
        {
            //stop timer and reset (turn all connected objects on)
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
            }
            TurnOn();
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //when player/needle leaves, start the timer for keeping connected objects on
        if (other.gameObject.layer.Equals(playerLayer) || other.gameObject.layer.Equals(needleLayer))
        {
            numCollisions -= 1;

            if (numCollisions == 0)
            {
                timerCoroutine = StartCoroutine(SwitchTriggered());
            }
        }
    }

}
