using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEngine.GraphicsBuffer;

public class NeedleMovement : MonoBehaviour
{
    public float recallSpeed = 10.0f;

    NeedleState needleState;

    Rigidbody2D rb;
    Collider2D col;



    // Start is called before the first frame update
    void Start()
    {
        needleState = this.GetComponent<NeedleState>();

        rb = this.GetComponent<Rigidbody2D>();
        col = this.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //throw needle in its current direction
    public void ThrowNeedle(float throwingForce)
    {
        if (!needleState.IsEquipped())
        {
            print("Can't throw the needle if it's not equipped ya dummy");
            return;
        }

        //at this point, needle is equipped and ready to be thrown
        //update state accordingly
        needleState.SetThrown();

        //turn off gravity just in case it's not off
        rb.gravityScale = 0;

        //turn on hitbox when throwing
        col.enabled = true;

        //calculate direction to throw (from player/needle to mouse cursor) as magnitude of 1
        Vector2 throwingDirection = Direction(this.transform.position, this.GetComponent<NeedleRotation>().mouseCursor.transform.position);

        //reset velocity and apply force to needle accordingly
        rb.velocity = Vector3.zero;
        rb.AddForce(throwingDirection * throwingForce);
    }

    //recall ability in needle
    public void RecallNeedle()
    {
        if (needleState.IsEquipped() || needleState.IsRecalling())
        {
            print("Needle currently cannot be recalled or is already being recalled");
            return;
        }

        //update states accordingly
        needleState.SetRecalling();

        //turn on hitbox when recalling
        col.enabled = true;

        StartCoroutine(RecallToWielder());
    }

    //begin the process of recalling the weapon to the wielder
    IEnumerator RecallToWielder()
    {
        while (needleState.IsRecalling())
        {
            //move towards direction of player while recalling
            Vector2 origin = this.transform.position;
            Vector2 target = needleState.wielder.transform.position;

            Vector2 velocityDirection = Direction(origin, target);
            rb.velocity = velocityDirection * recallSpeed;

            yield return null;
        }

        //at this point, you've reached the target
        needleState.SetEquipped();
    }

    //helper function to give a direction of magnitude 1 from origin to target
    public Vector2 Direction(Vector2 origin, Vector2 target)
    {
        float xDiff = target.x - origin.x;
        float yDiff = target.y - origin.y;

        float hypo = Mathf.Sqrt(xDiff * xDiff + yDiff * yDiff);
        //dummy case
        if (xDiff == 0 && yDiff == 0)
        {
            hypo = 1.0f;
        }

        Vector3 direction = new Vector3(xDiff / hypo, yDiff / hypo, 0);
        return direction;
    }
}
