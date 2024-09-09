using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [Header("Speech Basics")]
    public Talkitiveness Talkitiveness;
    public float SpeechRange = 1f;
    public TextAsset SpeechAsset;
    public int ActiveTree = 1;

    private Dictionary<int, SpeechTree> speechTrees;
    private DialogManager manager;
    
    public void Awake()
    {
        speechTrees = SpeechTree.FromTextAsset(SpeechAsset);

        manager = FindAnyObjectByType<DialogManager>();
    }

    public void Update()
    {
        if (!manager.InDialog() && distanceToPlayer() <= SpeechRange)
        {
            if (Talkitiveness == Talkitiveness.Reluctant && Input.GetKeyDown(KeyCode.C))
            {
                manager.TryToStartDialog(speechTrees[ActiveTree], this);
            }
            else if (Talkitiveness == Talkitiveness.Talkitive)
            {
                manager.TryToStartDialog(speechTrees[ActiveTree], this);
            }
        }
    }

    private float distanceToPlayer()
    {   
        return Mathf.Sqrt(
            Mathf.Pow(manager.GetPlayer().transform.position.x - transform.position.x, 2) +
            Mathf.Pow(manager.GetPlayer().transform.position.y - transform.position.y, 2));
    }
}