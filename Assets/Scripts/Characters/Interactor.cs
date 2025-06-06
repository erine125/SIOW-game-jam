using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [Header("Speech State")]
    // when these are changed, you should call "ChangedState"
    public Talkitiveness Talkitiveness;
    public float SpeechRange = 1f;
    public int ActiveTree = 1;
    public bool Visible = false;
    [Header("Speech Description")]
    public string Name;
    public TextAsset SpeechAsset;

    private Dictionary<int, SpeechTree> speechTrees;
    private DialogManager manager;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;
    
    private double lastSpoken;
    
    public void Awake ()
    {
        speechTrees = SpeechTree.FromTextAsset(SpeechAsset);
        Name = SpeechAsset.name;

        manager = FindAnyObjectByType<DialogManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        lastSpoken = 1.0;
    }

    public void Start ()
    {
        MasterState.Get ().AssignInteractorDetailsFromStored (this);
    }

    public void Update ()
    {
        if (!manager.InDialog() && distanceToPlayer() <= SpeechRange && lastSpoken > 0.5)
        {
            if (Talkitiveness == Talkitiveness.Reluctant && Input.GetKeyDown(KeyCode.E))
            {
                manager.TryToStartDialog(speechTrees[ActiveTree], this);
            }
            else if (Talkitiveness == Talkitiveness.Talkitive)
            {
                manager.TryToStartDialog(speechTrees[ActiveTree], this);
            }
        }

        spriteRenderer.enabled = Visible;
        if (col != null)
        {
            col.enabled = Visible;
        }

    }

    private float distanceToPlayer ()
    {   
        return Mathf.Sqrt(
            Mathf.Pow(manager.GetPlayer().transform.position.x - transform.position.x, 2) +
            Mathf.Pow(manager.GetPlayer().transform.position.y - transform.position.y, 2));
    }
}