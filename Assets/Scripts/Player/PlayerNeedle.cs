using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNeedle : MonoBehaviour
{
    //TODO: player needs reference to needle that they wield; temporarily done by public reference
    public NeedleMovement needle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: consider using infrastructure for keybinds/controls instead of just fixed keycodes
        //checks whether to throw or recall needle based on input
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ThrowNeedle();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            RecallNeedle();
        }
    }

    public void ThrowNeedle()
    {
        needle.ThrowNeedle();
    }

    public void RecallNeedle()
    {
        needle.RecallNeedle();
    }
}
