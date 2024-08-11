using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//like a breakable block except it regenerates over time
public class RegenerativeBreakableBlock : MonoBehaviour
{
    public float timeToRegenerate = 6.0f; //time it takes block to regenerate (in seconds) after being destroyed

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //TODO: consider exact rulings of block breaking; should a specific part of needle touch block? Should any touch from recalling needle break it?
    //What if needle was already touching this block before it recalled?
    private void OnTriggerEnter2D(Collider2D other)
    {
        //check if a needle has touched this breakable block
        NeedleState needle = other.GetComponent<NeedleState>();
        if (needle != null)
        {
            //if needle is in the middle of recalling, then this block should break against this needle & prepare to regenerate
            if (needle.IsRecalling())
            {
                BreakBlock();
            }
        }
    }

    //TODO: break block and then regenerate in some time
    void BreakBlock()
    {
        StartCoroutine(BreakBlockAndRegenerate());
    }

    IEnumerator BreakBlockAndRegenerate()
    {
        Collider2D col = this.GetComponent<Collider2D>();
        SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();
        Color originalColor = sprite.color;

        //prepare a transparent version of original color
        Color transparentColor = originalColor;
        transparentColor.a = 0.2f;

        //TODO: break block; turn off colliders and make sprite transparent
        col.enabled = false;
        sprite.color = transparentColor;


        yield return new WaitForSeconds(timeToRegenerate);

        //TODO: regenerate block; turn on colliders and make sprite whole again (AKA original color)
        col.enabled = true;
        sprite.color = originalColor;
    }

}
