using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: movement still needs refine as a sidescroller; consider altering acceleration/movement depending on if player is grounded or airborne
public class PlayerRun : MonoBehaviour
{
    public float maxSpeed = 5.0f;
    public float acceleration = 1.0f; //decides how quickly player increases speed
    public float decceleration = 2.0f; //decides how quickly player slows down when too fast or wanting to stop

    public static bool receivePlayerMovementInput = true; //tells us if player should receive inputs to move or not

    Rigidbody2D rb;
    float targetXVelocity = 0.0f;
    int xDirectionInput = 0;

    SpriteRenderer sr;
    Animator animator;

    //the variables below are for playing sound effects for running
    public AudioSource audioSource;
    public AudioClip runningClip;
    PlayerJump playerJump;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        sr = this.GetComponent<SpriteRenderer>();
        animator = this.GetComponent<Animator>();

        playerJump = this.GetComponent<PlayerJump>();
    }

    // Update is called once per frame
    void Update()
    {
        //record the current horizontal speed
        float horizontalSpeed = Mathf.Abs(rb.velocity.x);
        //if moving above certain speed while grounded, play running sound
        if (horizontalSpeed > 1.0f && playerJump.IsGrounded())
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(runningClip);
            }
            
        }
        else //otherwise, have no running sound
        {
            audioSource.Stop();
        }
        
    }

    private void FixedUpdate()
    {
        TargetHorizontalVelocityUpdate();
        VelocityUpdate();
    }

    //TODO: setup infrastructure for keybinds; for now we're assuming keyboard + mouse controls
    void TargetHorizontalVelocityUpdate()
    {
        xDirectionInput = 0;

        //only update target velocity direction if player is allowed to receive movement inputs; otherwise its goal is zero-velocity
        if (receivePlayerMovementInput)
        {
            if (Input.GetKey(KeyCode.A))
            {
                xDirectionInput -= 1;
                sr.flipX = false;
            }
            if (Input.GetKey(KeyCode.D))
            {
                xDirectionInput += 1;
                sr.flipX = true;
            }
        }

        if (xDirectionInput == 0)
        {
            animator.SetBool("keyDown", false);
        } else
        {
            animator.SetBool("keyDown", true);
        }

        targetXVelocity = xDirectionInput * maxSpeed;
    }

    void VelocityUpdate()
    {
        float xVelocity = rb.velocity.x;

        //decide if we're too slow & should accelerate OR player is too fast & needs to deccelerate
        bool shouldAccelerate = (targetXVelocity - xVelocity) * xDirectionInput > 0;

        //update velocity accordingly with acceleration or decceleration
        Vector2 updatedVelocity = rb.velocity;
        //float acc = shouldAccelerate ? acceleration : (-decceleration);

        float acc = 0;
        if (shouldAccelerate)
        {
            acc = acceleration;
        }
        else
        {
            acc = -decceleration;
        }

        updatedVelocity.x += Time.fixedDeltaTime * acc * xDirectionInput;


        rb.velocity = updatedVelocity;
        animator.SetFloat("horizVelocity", Mathf.Abs(updatedVelocity.x));   
        animator.SetFloat("vertVelocity", updatedVelocity.y);
    }

    
}
