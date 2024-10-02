using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// There should only be one 
public class MasterState : MonoBehaviour
{
    private static MasterState instance;
    public static MasterState Get ()
    {
        return instance;
    }

    void Awake ()
    {
        if (instance != null)
        {
            Debug.LogWarning ("MasterState already had an instance.");
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }


    // Add game state below here

    public Dictionary<string, InteractorDetails> interactors = new Dictionary<string, InteractorDetails>();


    // Utility methods

    public void SetInteractorState (Interactor interactor)
    {
        interactors.Remove(interactor.Name);
        interactors.Add(interactor.Name,
            new InteractorDetails(interactor.Talkitiveness,
                                  interactor.SpeechRange,
                                  interactor.ActiveTree,
                                  interactor.Visible));
    }

}

public struct InteractorDetails
{
    public Talkitiveness Talkitiveness;
    public float SpeechRange;
    public int ActiveTree;
    public bool Visible;

    public InteractorDetails (Talkitiveness talkitiveness, float speechRange,
        int activeTree, bool visible)
    {
        Talkitiveness = talkitiveness;
        SpeechRange = speechRange;
        ActiveTree = activeTree;
        Visible = visible;
    }
}

