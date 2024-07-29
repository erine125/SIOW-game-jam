using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for needle's rotation around player
public class NeedleRotation : MonoBehaviour
{
    //note that the position is the base of the weapon/knife, not the center

    public float rotationSpeed = 5.0f;

    Collider2D col;

    NeedleState needleState;

    //TODO: need reference to mouse cursor position; temporarily done by public reference
    public GameObject mouseCursor;
    public GameObject mouseCursorPrefab;
    

    public Quaternion defaultRotation = Quaternion.Euler(new Vector3(0, 0, -90));

    // Start is called before the first frame update
    void Start()
    {
        needleState = this.GetComponent<NeedleState>();

        //remember that the needle hitbox is in the child
        col = this.GetComponentInChildren<Collider2D>();

        //TODO: finds a mouse pointer or instantiates one if necessary
        if (mouseCursor == null)
        {
            mouseCursor = GameObject.Instantiate(mouseCursorPrefab);
        }
    }

    void Update()
    {
        //face mouse cursor if needle is currently equipped to player; and rotate around player (or needle base) accordingly
        if (needleState.IsEquipped())
        {
            Quaternion facingRotation = GetRotationFacingTarget(mouseCursor.transform.position);

            //rotation is based on the cursor
            this.transform.rotation = facingRotation * defaultRotation;

            //set position; 0.9 away from center relative to player
            Vector3 circleOffset = GetUnitCircleOffset(facingRotation);
            this.transform.position = needleState.wielder.transform.position + circleOffset * 0.9f;

        }

    }

    //based on given position, get the rotation to face in direction of target
    Quaternion GetRotationFacingTarget(Vector3 target)
    {
        Vector3 origin = needleState.wielder.transform.position;

        float xDiff = target.x - origin.x;
        float yDiff = target.y - origin.y;

        if (xDiff == 0)
        {
            //implies facing up or down
            if (yDiff > 0)
            {
                return Quaternion.Euler(0, 0, 90);
            }
            else if (yDiff < 0)
            {
                return Quaternion.Euler(0, 0, 270);
            }
            //default case if no direction
            else
            {
                return Quaternion.identity;
            }
        }
        else if (xDiff > 0)
        {
            Vector3 resultAngle = new Vector3(0, 0, Mathf.Atan(yDiff / xDiff) * Mathf.Rad2Deg);
            return Quaternion.Euler(resultAngle);
        }
        else
        {
            //xDiff < 0; the arctan needs to be adjusted another 180 degrees
            Vector3 resultAngle = new Vector3(0, 0, Mathf.Atan(yDiff / xDiff) * Mathf.Rad2Deg + 180);
            return Quaternion.Euler(resultAngle);
        }

    }

    //helper function to convert vector to quaternion
    public Quaternion DirectionToAngle(Vector3 d)
    {
        float angle = 0;
        if (d.x == 0)
        {
            //angle is 90 or 270
            if (d.y > 0)
            {
                angle = 90;
            }
            else
            {
                angle = 270;
            }
        }
        else
        {
            //get angle based on direction
            if (d.x > 0)
            {
                angle = Mathf.Atan(d.y / d.x) * Mathf.Rad2Deg;
            }
            //d.x < 0; needs to be adjsuted another 180 degrees
            else
            {
                angle = Mathf.Atan(d.y / d.x) * Mathf.Rad2Deg + 180;
            }

        }

        return Quaternion.Euler(new Vector3(0, 0, angle));
    }

    //helper function for a 2d position on a unit circle with a given rotation
    Vector3 GetUnitCircleOffset(Quaternion rotation)
    {
        //get angle in degrees
        float angle = rotation.eulerAngles.z;
        float x = Mathf.Cos(Mathf.Deg2Rad * angle);
        float y = Mathf.Sin(Mathf.Deg2Rad * angle);
        Vector3 result = new Vector3(x, y, 0);

        return result;
    }

    //reset weapon back to facing right while equipped
    public void ResetRotations()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, -90);
    }
}
