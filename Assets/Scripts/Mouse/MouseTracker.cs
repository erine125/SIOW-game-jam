using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTracker : MonoBehaviour
{
    //position on computer screen
    Vector3 screenPosition;
    //position in the game world
    Vector3 worldPosition;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        screenPosition = Input.mousePosition;

        //ignore the z part as it needs to be seen by camera
        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPosition.z = 0;

        //follow the position of the mouse
        this.transform.position = worldPosition;

    }

}
