using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// There should only be one 
public class MasterState : MonoBehaviour
{
    private static MasterState instance;
    public static MasterState Get ()
    {
        return instance;
    }

    private DialogManager manager;

    private GameState state;
    public GameState GetState ()
    {
        return state;
    }

    Dictionary<string, InteractorDetails> interactorDetails;

    // Triggers

    void Awake ()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoad;

            interactorDetails = new Dictionary<string, InteractorDetails>();
        }
    }

    public void OnSceneLoad (Scene scene, LoadSceneMode mode)
    {
        HandleSceneInteractors ();
        manager = FindAnyObjectByType<DialogManager>();
    }

    public void UpdateState (GameState state)
    {
        this.state = state;
        HandleSceneInteractors ();
    }

    private void HandleSceneInteractors ()
    {
        // TODO - logic to determine interactor state goes here
    }

    public void PossiblyTriggerPhoneCall (Texture2D[] phoneTextures)
    {
        manager.TryToStartPhoneCall (phoneTextures);
    }

    public void StoreUpdatedInteractor (Interactor interactor)
    {
        interactorDetails[interactor.name] = new InteractorDetails (interactor);
    }

    public void AssignInteractorDetailsFromStored (Interactor interactor)
    {
        if (interactorDetails.ContainsKey (interactor.name))
        {
            InteractorDetails dets = interactorDetails[interactor.name];
            interactor.Talkitiveness = dets.Talkitiveness;
            interactor.SpeechRange = dets.SpeechRange;
            interactor.ActiveTree = dets.ActiveTree;
            interactor.Visible = dets.Visible;
        }
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

    public InteractorDetails (Interactor interactor)
    {
        Talkitiveness = interactor.Talkitiveness;
        SpeechRange = interactor.SpeechRange;
        ActiveTree = interactor.ActiveTree;
        Visible = interactor.Visible;
    }
}

public enum GameState
{
    BEGINNING = 0,
    AFTER_GETTING_NEEDLES = 1,
    STARTED_LIBRARY_QUEST = 2,
    FINISHED_LIBRARY_QUEST = 3,
    STARTED_BAKERY_QUEST = 4,
    FINISHED_BAKERY_QUEST = 5,
    AFTER_TALKING_TO_INDY = 6,
    STARTED_MOUNTAIN_QUEST = 7,
    FINISHED_SHRINE_SCENE = 8,
    AFTER_GRANDMA_FUNERAL = 9
}
