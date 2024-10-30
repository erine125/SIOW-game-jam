using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNeedle : MonoBehaviour
{
    //TODO: player needs reference to needle that they wield; temporarily done by public reference
    public NeedleMovement needle;

    PlayerPowerupInventory playerPowerupInventory;
    NeedleState needleState;

    //dictates force that player throws the needle which will consequently propel player mid-air
    public float throwingForce = 700.0f;
    //the actual max throwing force (throwingForce can vary so we track an original max value)
    float maxThrowingForce;

    Rigidbody2D rigid;
    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        playerPowerupInventory = this.GetComponent<PlayerPowerupInventory>();


        if (needle != null)
        {
            needleState = needle.GetComponent<NeedleState>();
        }

        maxThrowingForce = throwingForce;

        rigid = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
    }

    public void SetNeedle(NeedleMovement newNeedle)
    {
        needle = newNeedle;
        needleState = needle.GetComponent<NeedleState>();
    }

    // Update is called once per frame
    void Update()
    {
        //checks whether to throw or recall needle based on input

        if (PlayerRun.receivePlayerMovementInput)
        {

            //LMB to throw/recall needle depending on if needle is equipped or not
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (needleState.IsEquipped())
                {
                    ThrowNeedle();
                    animator.SetTrigger("isThrowing");
                }
                else
                {
                    RecallNeedle();
                }
            }
            /*
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ThrowNeedle();
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                RecallNeedle();
            }
            */
            //RMB to throw needle but also, if player is mid-air, player is propelled in opposite direction at same time
            if (playerPowerupInventory.HasPropelUnlocked() && Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (needleState.IsEquipped())
                {
                    if (!this.GetComponent<PlayerJump>().IsGrounded())
                        PropelNeedle();
                }
                //RMB when needle is unequipped will recall as well
                else
                {
                    RecallNeedle();
                }
            }
        }
        
    }

    public void ThrowNeedle()
    {
        needle.ThrowNeedle(throwingForce);
    }

    public void RecallNeedle()
    {
        needle.RecallNeedle();
    }


    public void PropelNeedle()
    {
        ThrowNeedle();

        //record direction that needle is thrown in relative to player
        Vector2 thrownDirection = needle.Direction(this.transform.position, needle.transform.position); //NOTE: maybe use mouseCursor position instead of needle

        //reset velocity to zero before being propelled
        rigid.velocity = Vector3.zero;
        //now actually propel player in opposite direction of thrown needle
        rigid.AddForce(thrownDirection * throwingForce * (-1));

        //when throwing in midair, the throwing/propelling power is weakened (as a balance measure against doing it consecutively)
        //reverts when on ground. for the revert change code, refer to PlayerJump
        throwingForce *= 0.8f;
    }

    //reset throwing force back to max value
    public void ResetThrowingForce()
    {
        throwingForce = maxThrowingForce;
    }
}
