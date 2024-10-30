using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public LayerMask groundWallLayer;
    public float jumpPower = 450;

    Rigidbody2D rb;
    CapsuleCollider2D col;

    PlayerNeedle playerNeedle;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        col = this.GetComponent<CapsuleCollider2D>();

        playerNeedle = this.GetComponent<PlayerNeedle>();
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(rb.velocity.y);
        if (IsGrounded())
        {
            animator.SetBool("isGrounded", true);
        }
        else animator.SetBool("isGrounded", false);

        //if (Input.GetKeyDown(KeyCode.Space) && !IsGrounded()) Debug.Log("Tried to jump but not grounded");


            //only consider jumping as an option if player is allowed to receive player movement inputs & is currently grounded
        if (PlayerRun.receivePlayerMovementInput && IsGrounded())
        {
            //if grounded, you can jump with spacebar
            if (Input.GetKeyDown(KeyCode.Space))
            {
                
                animator.SetTrigger("isJumping");
                animator.SetBool("isGrounded", false);

                //Note: I applied a force instead of editing the y velocity
                //Note 2: Decided to reset y velocity before jumping because sometimes the y velocity isn't immediately 0 when grounded
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector3.up * jumpPower);
                
            }

            //reset throwing force if player is grounded so they can propel at max force again
            playerNeedle.ResetThrowingForce();
        }

    }

    //checks if player is grounded by performing a raycast to see if there's a ground right below them
    public bool IsGrounded()
    {


        //if (!Mathf.Approximately(rb.velocity.y, 0))
        //{
        //    return false;
        //}

        //Note: Trying out alternate Raycasting method for more accurate conditions to jump
        BoxCollider2D feetCollider = this.GetComponent<BoxCollider2D>();
        if (feetCollider != null)
        {
            float raycastDistance = 0.01f; //shift the box X distance downwards for raycast

            Vector2 raycastStartingPosition = (Vector2)(feetCollider.transform.position) + feetCollider.offset;

            bool result = Physics2D.BoxCast(raycastStartingPosition, feetCollider.size, 0, Vector2.down, raycastDistance, groundWallLayer);
            //Debug.Log("Using alternative method of raycasting which reports: " + result);
            return result;
        }
        else //if we don't find the box collider needed for alternate Raycasting method, we'll do the original old method
        {

            Ray ray = new(col.bounds.center, Vector3.down);

            // A bit below the bottom
            float fullDistance = col.bounds.extents.y + 0.05f - col.bounds.extents.x; // TODO: why is this here

            //Note: did 95% of collider's size in case the side's are already touching walls
            return Physics2D.CapsuleCast(this.transform.position, col.size * 0.95f, col.direction, 0, Vector2.down, fullDistance, groundWallLayer);
        }
        
    }
}
