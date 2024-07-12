using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: movement still needs refine as a sidescroller; consider altering acceleration/movement depending on if player is grounded or airborne
public class PlayerRun : MonoBehaviour
{
    public float maxSpeed = 5.0f;
    public float acceleration = 1.0f; //decides how quickly player increases speed
    public float decceleration = 2.0f; //decides how quickly player slows down when too fast or wanting to stop


    Rigidbody2D rb;
    float targetXVelocity = 0.0f;
    int xDirectionInput = 0;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
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

        if (Input.GetKey(KeyCode.A))
        {
            xDirectionInput -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            xDirectionInput += 1;
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
    }

    
}
