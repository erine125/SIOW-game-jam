using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOneWayPlatform : MonoBehaviour
{
    GameObject currentOneWayPlatform;

    Collider2D col;

    // Start is called before the first frame update
    void Start()
    {
        col = this.GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (currentOneWayPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsAOneWayPlatform(collision.gameObject))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (IsAOneWayPlatform(collision.gameObject))
        {
            currentOneWayPlatform = null;
        }
    }

    IEnumerator DisableCollision()
    {
        Collider2D platformCollider = currentOneWayPlatform.GetComponent<Collider2D>();

        Physics2D.IgnoreCollision(col, platformCollider);

        yield return new WaitForSeconds(0.5f); //TODO: decide how long to disable collision with one-way

        Physics2D.IgnoreCollision(col, platformCollider, false); //stop ignoring this collision
    }

    //helper function to identify if an object is a one-way platform or not
    bool IsAOneWayPlatform(GameObject obj)
    {
        Collider2D c = obj.GetComponent<Collider2D>();

        if (c != null && c.usedByEffector)
        {
            PlatformEffector2D platEffect = obj.GetComponent<PlatformEffector2D>();
            if (platEffect != null & platEffect.useOneWay)
            {
                //needs to fulfill all the above traits/requirements to be considered a one-way platform
                return true;
            }
        }

        //at this point, we know the object failed the check and is not a one-way platform
        return false;
    }
}
