using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour

{
    bool turnedOn = false;

    //the ith connected Object will have default status (when switch is off) represented by connectedObjectsDefaultStatus[i]
    public List<GameObject> connectedObjects;
    List<bool> connectedObjectsDefaultStatus; //records default status of objects when switch is off

    SpriteRenderer sprite;
    Color defaultColor;

    public Sprite defaultSprite;
    public Sprite pressedSprite;

    //TODO: don't just leave it to the player & needle; consider other sources that can trigger switch (ex: anything but walls/ground)
    //the layers that will trigger the switch
    protected LayerMask playerLayer;
    protected LayerMask needleLayer;

    // Start is called before the first frame update
    void Start()
    {
        //record the default states of each connected object (based on collider being enabled)
        connectedObjectsDefaultStatus = new List<bool>();
        foreach (GameObject obj in connectedObjects)
        {
            connectedObjectsDefaultStatus.Add(obj.GetComponent<Collider2D>().enabled);
        }
        

        playerLayer = LayerMask.NameToLayer("Player");
        needleLayer = LayerMask.NameToLayer("Needle");

        sprite = this.GetComponent<SpriteRenderer>();
        defaultColor = sprite.color;


        //precautionary turn off switch to ensure switch & connected objects are setup accordingly
        TurnOff();
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
        sprite.sprite = pressedSprite; // change sprite instead of color

        //turn off connected objects
        SetConnectedObjects(true);
    }

    //turn off switch and reactivate all related gameobjects
    public void TurnOff()
    {
        SetConnectedObjects(false);

        turnedOn = false;
        sprite.sprite = defaultSprite;  // Revert to default sprite
    }

    //set connected objects active/inactive based on whether switch is on or not
    public void SetConnectedObjects(bool isSwitchOn)
    {
        for (int i = 0; i < connectedObjects.Count; i++)
        {
            //figure out what status connect obj should become; connected obj should be in default state if switch is off; otherwise, it's the opposite state
            bool desiredStatus = connectedObjectsDefaultStatus[i];
            if (isSwitchOn)
            {
                desiredStatus = !desiredStatus;
            }

            //set connected object based on desired status
            SetObject(connectedObjects[i], desiredStatus);
        }

        
    }

    //set an object's collider on or off according to desired status
    public void SetObject(GameObject obj, bool desiredStatus)
    {
        //if obj should be off, then make it transparent (visual indicator for being inactive); otherwise, make it whole
        SpriteRenderer objSprite = obj.GetComponent<SpriteRenderer>();
        Color c = objSprite.color;
        c.a = desiredStatus ? 1.0f : 0.2f;
        objSprite.color = c;

        obj.GetComponent<Collider2D>().enabled = desiredStatus;
       //obj.SetActive(desiredStatus);
    }
}
