using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
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
            //if needle is in the middle of recalling, then this block should break against this needle
            if (needle.IsRecalling())
            {
                Destroy(this.gameObject);
            }
        }
    }
}
