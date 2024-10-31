using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// There should only be one 
public class MasterState : MonoBehaviour
{

    public GameObject Player;
    public GameObject needlePrefab;

    private static MasterState instance;
    public static MasterState Get ()
    {
        return instance;
    }

    private DialogManager manager;

    public int state; // making this public for debugging only, IMPORTANT: don't update directly, use UpdateState()
    public int GetState ()
    {
        return state;
    }

    Dictionary<string, InteractorDetails> interactorDetails; // lasts until state is changed


    // Triggers

    void Awake ()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoad;

            interactorDetails = new Dictionary<string, InteractorDetails>();
        } else
        {
            Debug.LogWarning("Master state exists already");
        }
    }

    public void OnSceneLoad (Scene scene, LoadSceneMode mode)
    {
        manager = FindAnyObjectByType<DialogManager>();
        HandleSceneInteractors ();
        HandlePlayerAbilities (); 
        Debug.Log("State: " + this.state);
    }

    public void UpdateState (int state)
    {
        this.state = state;

        if (state == 3)
        {
                PlayerNeedle playerNeedle = Player.GetComponent<PlayerNeedle>();
                GameObject needleObject = GameObject.Instantiate(needlePrefab);
                //make sure player and needle recognize each other as the wielder & weapon/needle accordingly
                needleObject.GetComponent<NeedleState>().wielder = Player;
                NeedleMovement needleMovement = needleObject.GetComponent<NeedleMovement>();
                playerNeedle.SetNeedle(needleMovement);
        } else if (state >= 11)
        { // set immediately
            PlayerPowerupInventory inventory = Player.GetComponent<PlayerPowerupInventory>();
            inventory.propelUnlocked = true;
        }

        interactorDetails.Clear ();
        HandleSceneInteractors ();
        HandlePlayerAbilities (); 
    }

    private void HandleSceneInteractors ()
    {
        Interactor[] arr = GameObject.FindObjectsByType<Interactor> (FindObjectsSortMode.None);

        foreach (Interactor a in arr)
        {
            if (!interactorDetails.ContainsKey (a.Name))
            {
                Debug.Log(a.Name);
                switch (a.Name)
                {
                    case "BoatRandos":
                        a.Talkitiveness = (state == 0) ? Talkitiveness.Talkitive : Talkitiveness.Quiet;
                        a.Visible = state == 0;
                        a.ActiveTree = 0;
                        break;
                    case "Scarlett-LibraryLobby":
                        if (state <= 3)
                        {
                            a.Talkitiveness = Talkitiveness.Reluctant;
                            a.Visible = true;
                            a.ActiveTree = 1;
                        }
                        else if (state == 4)
                        {
                            a.Talkitiveness = Talkitiveness.Reluctant;
                            a.Visible = true;
                            a.ActiveTree = 4;
                            Debug.Log(a.ActiveTree);
                        }
                        else if (state == 5)
                        {
                            a.Talkitiveness = Talkitiveness.Quiet;
                            a.Visible = false;
                        }
                        else if (state < 11)
                        {
                            a.Talkitiveness = Talkitiveness.Reluctant;
                            a.Visible = true;
                            a.ActiveTree = 6;
                        }
                        else
                        {
                            a.Talkitiveness = Talkitiveness.Reluctant;
                            a.Visible = true;
                            a.ActiveTree = 11;
                        }
                        break;
                    case "LibraryArchivesDoor":
                        if (state <= 4)
                        {
                            a.Talkitiveness = Talkitiveness.Talkitive;
                            a.Visible = true;
                            a.ActiveTree = 1;
                        }
                        else
                        {
                            a.Talkitiveness = Talkitiveness.Quiet;
                            a.Visible = false;
                        }
                        break;
                    case "Ginger-Bakery":
                        a.Talkitiveness = Talkitiveness.Reluctant;
                        a.Visible = true;
                        a.ActiveTree = state < 4 ? 1 : 4;
                        break;
                    case "Grandma":
                        a.ActiveTree = state;
                        a.Visible = true;
                        switch (state)
                        {
                            case 1:
                                a.Talkitiveness = Talkitiveness.Talkitive;
                                break;
                            case 2:
                                a.Talkitiveness = Talkitiveness.Reluctant;
                                break;
                            case 3:
                                a.Talkitiveness = Talkitiveness.Talkitive;
                                break;
                            case 4:
                                a.Talkitiveness = Talkitiveness.Reluctant;
                                break;
                            case 6:
                                a.Talkitiveness = Talkitiveness.Talkitive;
                                break;
                            case 7:
                                a.Talkitiveness = Talkitiveness.Reluctant;
                                break;
                            case 8:
                                a.Talkitiveness = Talkitiveness.Talkitive;
                                break;
                            case 9:
                                a.Talkitiveness = Talkitiveness.Reluctant;
                                break;
                            case 10:
                                a.Talkitiveness = Talkitiveness.Talkitive;
                                break;
                            case 11:
                                a.Talkitiveness = Talkitiveness.Reluctant;
                                break;
                        }
                        break;
                    case "GrandmasHouseExitDoor":
                        if (state != 2 && state != 3)
                        {
                            a.Visible = false;
                            a.Talkitiveness = Talkitiveness.Quiet;
                        }
                        else 
                        {
                            a.Visible = true;
                            a.Talkitiveness = Talkitiveness.Talkitive;
                            a.ActiveTree = 2;
                        }
                        break;
                    case "Indy-Town":
                        if (state < 4)
                        {
                            a.Visible = false;
                            a.Talkitiveness = Talkitiveness.Quiet;
                        }
                        else if (state < 9)
                        {
                            a.Visible = true;
                            a.Talkitiveness = Talkitiveness.Reluctant;
                            a.ActiveTree = 4;
                        }
                        else if (state < 11)
                        {
                            a.Visible = false;
                            a.Talkitiveness = Talkitiveness.Quiet;
                        }
                        else
                        {
                            a.Visible = true;
                            a.Talkitiveness = Talkitiveness.Reluctant;
                            a.ActiveTree = 11;
                        }
                        break;
                    case "LibraryExitDoor":
                        a.Talkitiveness = (state == 5) ? Talkitiveness.Talkitive : Talkitiveness.Quiet;
                        a.Visible = (state == 5);
                        a.ActiveTree = 5;
                        break;
                    case "Scarlett-LibraryArchives":
                        a.Talkitiveness = (state == 5) ? Talkitiveness.Talkitive : Talkitiveness.Quiet;
                        a.Visible = (state == 5);
                        a.ActiveTree = 5;
                        break;
                    case "Bakers":
                        if (state < 7)
                        {
                            a.Talkitiveness = Talkitiveness.Quiet;
                            a.Visible = false;
                        }
                        else if (state == 7)
                        {
                            a.Talkitiveness = Talkitiveness.Talkitive;
                            a.ActiveTree = 7;
                            a.Visible = true;
                        }
                        else if (state < 11)
                        {
                            a.Talkitiveness = Talkitiveness.Reluctant;
                            a.ActiveTree = 8;
                            a.Visible = true;
                        }
                        else
                        {
                            a.Talkitiveness = Talkitiveness.Reluctant;
                            a.ActiveTree = 11;
                            a.Visible = true;
                        }
                        break;
                    case "Indy-Sewer":
                        if (state < 9)
                        {
                            a.Talkitiveness = Talkitiveness.Quiet;
                            a.Visible = false;
                        }
                        else if (state == 9)
                        {
                            a.Talkitiveness = Talkitiveness.Talkitive;
                            a.Visible = true;
                            a.ActiveTree = 10;
                        }
                        else if (state == 10)
                        {
                            a.Talkitiveness = Talkitiveness.Reluctant;
                            a.Visible = true;
                            a.ActiveTree = 11;
                        }
                        else
                        {
                            a.Visible = false;
                            a.Talkitiveness = Talkitiveness.Quiet;
                        }
                        break;
                    case "ShrineStatue":
                        a.Talkitiveness = state >= 11 ? Talkitiveness.Talkitive : Talkitiveness.Quiet;
                        a.Visible = state >= 11;
                        a.ActiveTree = 11;
                        break;
                    case "MountainNPCs":
                        a.Talkitiveness = state >= 12 ? Talkitiveness.Talkitive : Talkitiveness.Quiet;
                        a.Visible = state >= 12;
                        a.ActiveTree = 12;
                        break;
                    case "NeedlesAttic":
                        if (state <= 2)
                        {
                            a.Visible = true;
                        }
                        else
                        {
                            a.Visible = false;
                        }
                        break;
                    case "Intro-Cutscene":
                        a.Talkitiveness = (state == 0) ? Talkitiveness.Talkitive : Talkitiveness.Quiet;
                        a.Visible = (state == 0);
                        a.ActiveTree = 0;
                        break;
                    case "NeedlesPickup":
                        a.Talkitiveness = (state == 2) ? Talkitiveness.Reluctant : Talkitiveness.Quiet;
                        a.Visible = (state == 2);
                        a.ActiveTree = 2;
                        break;
                }
            }       
        }
    }

    private void HandlePlayerAbilities ()
    {
        GameObject playerObject = GameObject.Find("Player");
        PlayerPowerupInventory inventory = playerObject.GetComponent<PlayerPowerupInventory>();
        inventory.needleUnlocked = (state >= 3);
        inventory.propelUnlocked = (state >= 11);


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
