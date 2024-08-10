using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;
using UnityEngine.U2D;

//when tapped, the switch stays on for X seconds
public class TapSwitch : Switch
{
    public float switchTimer = 3.0f;

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

    IEnumerator SwitchTriggered()
    {

        TurnOn();

        yield return new WaitForSeconds(switchTimer);

        TurnOff();

        yield return null;
    }


}
