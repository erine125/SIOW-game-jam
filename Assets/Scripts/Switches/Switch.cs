using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    bool turnedOn = false;

    public List<GameObject> connectedObjects;

    //TODO: don't just leave it to the player & needle; consider other sources that can trigger switch (ex: anything but walls/ground)
    //the layers that will trigger the switch
    protected LayerMask playerLayer;
    protected LayerMask needleLayer;

    // Start is called before the first frame update
    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        needleLayer = LayerMask.NameToLayer("Needle");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsTurnedOn()
    {
        return turnedOn;
    }

    //activate switch and erase all related gameobjects
    public void TurnOn()
    {
        turnedOn = true;
        foreach (GameObject obj in connectedObjects)
        {
            obj.GetComponent<Collider2D>().enabled = false;
            obj.SetActive(false);
        }
    }

    //turn off switch and reactivate all related gameobjects
    public void TurnOff()
    {
        foreach (GameObject obj in connectedObjects)
        {
            obj.GetComponent<Collider2D>().enabled = true;
            obj.SetActive(true);
        }
        turnedOn = false;
    }
}
