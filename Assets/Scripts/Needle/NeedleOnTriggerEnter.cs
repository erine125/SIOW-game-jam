using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleOnTriggerEnter : MonoBehaviour
{
    NeedleState needleState;
    Collider2D col;
    Rigidbody2D rb;

    public LayerMask groundWall_layer;

    //TODO: temporary fix to finding out that the ground is no longer touched
    Collider2D touchedGround;


    // Start is called before the first frame update
    void Start()
    {
        needleState = this.GetComponent<NeedleState>();
        col = this.GetComponent<Collider2D>();
        rb = this.GetComponent<Rigidbody2D>();


        groundWall_layer = LayerMask.NameToLayer("GroundWall");

        //check if needle is equipped and turn off collider beforehand
        if (needleState.IsEquipped())
        {
            col.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        //TODO: testing changes. this checks that if weapon is still and the touched ground isn't an active groundwall, start falling
        if (needleState.IsStill())
        {
            //when weapon is still, but the touched ground is not there, fall
            if (touchedGround == null || !touchedGround.gameObject.activeSelf)
            {
                rb.gravityScale = 1;
            }
        }
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        OnTriggerStay2D(other); //same as OnTriggerStay2D
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //this was repeated in case the player aims weapon into the groundwall layer already or recalls while already in player

        //when being thrown, the throw stops when hitting a wall
        if (needleState.IsThrowing())
        {
            if (other.gameObject.layer == groundWall_layer)
            {
                StopThrow();

                //TODO: record the ground it's touching when becoming still
                touchedGround = other;
            }
        }

        //when recalling and colliding with a player, we are done recalling and begin equipping needle to player
        else if (needleState.IsRecalling())
        {
            if (other.GetComponent<PlayerNeedle>() != null)
            {
                StopRecall();
            }
        }

        //at this point, weapon is not equipped, throwing, nor recalling
        else if (!needleState.IsEquipped())
        {

            //if weapon encounters a wall/ground, stay still

            if (other.gameObject.layer == groundWall_layer)
            {
                rb.gravityScale = 0; //reset gravity as a precaution
                rb.velocity = Vector2.zero;

                //TODO: record the ground it's touching while still
                touchedGround = other;
            }

        }
    }



    void StopThrow()
    {
        //update state with the throw stopping
        needleState.SetStill();

        //weapon stops and hopefully sticks there when hitting the wall
        rb.velocity = Vector3.zero;
    }

    void StopRecall()
    {
        //update state with ending recall & starting equip
        needleState.SetEquipped();

        //reset orientation (as a precaution)
        needleState.GetComponent<NeedleRotation>().ResetRotations();

        //reset velocity as a precaution upon being equipped
        rb.velocity = Vector2.zero;

        //turn off hitbox
        col.enabled = false;


    }

}
