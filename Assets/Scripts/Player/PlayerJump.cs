using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public LayerMask groundWallLayer;
    public float jumpPower = 450;

    Rigidbody2D rb;
    CapsuleCollider2D col;



    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        col = this.GetComponent<CapsuleCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {

        if (IsGrounded())
        {
            //if grounded, you can jump with spacebar
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Note: I applied a force instead of editing the y velocity
                rb.AddForce(Vector3.up * jumpPower);

            }
        }

        
    }

    //checks if player is grounded by performing a raycast to see if there's a ground right below them
    public bool IsGrounded()
    {
        Ray ray = new(col.bounds.center, Vector3.down);

        // A bit below the bottom
        float fullDistance = col.bounds.extents.y + 0.1f - col.bounds.extents.x;

        return Physics2D.CapsuleCast(this.transform.position, col.size, col.direction, 0, Vector2.down, fullDistance, groundWallLayer);
    }
}
