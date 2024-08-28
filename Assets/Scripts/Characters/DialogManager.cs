using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{

    // references
    private GameObject player;
    private SpriteRenderer dialogBoxRenderer;

    // state
    private SpeechTree activeTree;
    private Interactor activeInteractor;

    void Awake()
    {
        player = GameObject.Find("Player");
        dialogBoxRenderer = GetComponent<SpriteRenderer>();
    }

    public GameObject GetPlayer ()
    {
        return player;
    }

    public bool InDialog ()
    {
        return activeTree != null;
    }

    public void TryToStartDialog (SpeechTree tree, Interactor interactor)
    {
        if (!InDialog())
        {
            activeTree = tree;
            activeInteractor = interactor;

            // TODO - disable player movement and anything else similar

            Texture2D tex = Texture2D.whiteTexture;
            dialogBoxRenderer.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero, 4);
        }
    }

}
