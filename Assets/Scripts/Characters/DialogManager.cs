using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public float CharacterPrintDelay = 0.35f;
    public string SelectedChoiceColor = "black";
    public Texture2D BackgroundTexture;

    // references
    private GameObject player;
    private SpriteRenderer dialogBoxRenderer;
    private TextMeshPro textRenderer;
    private SpriteRenderer phoneRenderer;

    // high level state
    private DlgMode mode;
    private SpeechTree activeTree;
    private Interactor activeInteractor;
    private SpeechNode activeNode;

    // low level state
    private string preformattingPrinted;
    private string textToPrint;
    private float timeSinceChar;
    private string[] optionStrings;
    private Texture2D[] phoneTextures;
    private int phoneTextureIndex;

    void Awake()
    {
        mode = DlgMode.Inactive;
        player = GameObject.Find("Player");
        dialogBoxRenderer = GetComponent<SpriteRenderer>();
        dialogBoxRenderer.sortingLayerName = "fg-0";

        textRenderer = GetComponentInChildren<TextMeshPro>();
        textRenderer.sortingLayerID = dialogBoxRenderer.sortingLayerID;

        phoneRenderer = GetComponentInChildren<SpriteRenderer>();
        phoneRenderer.sortingLayerName = "fg-0";

        optionStrings = new string[4];

        if (BackgroundTexture == null)
        {
            BackgroundTexture = Texture2D.whiteTexture;
        }
        else
        {
            dialogBoxRenderer.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void Update()
    {
        Vector2 pos;
        switch (mode)
        {
            case DlgMode.Writing:
                timeSinceChar += Time.deltaTime;

                if (textToPrint.Length > 0 && timeSinceChar >= CharacterPrintDelay)
                {
                    timeSinceChar = 0;

                    preformattingPrinted += textToPrint[0];
                    textToPrint = textToPrint.Substring(1);
                    ApplyFormattingAndPrint();
                }
                else if (textToPrint.Length == 0)
                {
                    mode = DlgMode.WaitingNext;
                }
                break;
            case DlgMode.WaitingNext:
                pos = RelativePercentMousePos();
                if (pos.y > 0 && pos.y < 1 && pos.x > 0 && pos.x < 1
                    && Input.GetMouseButtonDown (0))
                {
                    if (activeNode.GetData().action != "")
                    {
                        SpeechUtil.Action(activeNode.GetData().action, activeInteractor);
                    }

                    switch (activeNode.GetChildCount())
                    {
                        case 0:
                            activeTree = null;
                            activeInteractor = null;
                            activeNode = null;
                            textRenderer.text = "";
                            dialogBoxRenderer.sprite = null;

                            mode = DlgMode.Inactive;
                            DialogLock(false);
                            break;
                        case 1:
                            activeNode = activeNode.GetChildAt(0);
                            PrepareWriting();
                            break;
                        default:
                            for (int i = 0; i < Mathf.Min(4, activeNode.GetChildCount()); i++)
                            {
                                if (activeNode.GetChildCount() > i)
                                {
                                    optionStrings[i] = "> " + activeNode.GetChildAt(i).GetData().text;
                                }
                                else
                                {
                                    optionStrings[i] = "";
                                }
                            }

                            BuildOptionsString(ChoiceThatMouseIsOver());
                            mode = DlgMode.WaitingChoice;
                            break;
                    }
                }

                break;
            case DlgMode.WaitingChoice:
                int choiceMouseOver = ChoiceThatMouseIsOver();
                BuildOptionsString(choiceMouseOver);
                if (choiceMouseOver != -1 && Input.GetMouseButtonDown(0))
                {
                    activeNode = activeNode.GetChildAt(choiceMouseOver);
                    PrepareWriting();
                }
                break;
            case DlgMode.ShowingPhone:
                if (Input.GetMouseButtonDown (0))
                {
                    phoneTextureIndex++;
                    if (phoneTextureIndex >= phoneTextures.Length)
                    {
                        phoneTextures = null;
                        phoneRenderer.sprite = null;
                        mode = DlgMode.Inactive;
                        DialogLock (false);
                    }
                    else
                    {
                        Texture2D tex = phoneTextures[phoneTextureIndex];
                        phoneRenderer.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero, 4);
                    }
                    
                }
                break;
        }
    }

    public GameObject GetPlayer ()
    {
        return player;
    }

    public bool InDialog ()
    {
        return mode != DlgMode.Inactive;
    }

    public void TryToStartDialog (SpeechTree tree, Interactor interactor)
    {
        if (!InDialog())
        {
            mode = DlgMode.Loading;
            activeTree = tree;
            activeInteractor = interactor;
            activeNode = activeTree.GetRoot();

            DialogLock(true);

            Texture2D tex = BackgroundTexture;
            dialogBoxRenderer.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero, 4);

            PrepareWriting();
        }
    }

    public void TryToStartPhoneCall (Texture2D[] textures)
    {
        if (!InDialog ())
        {
            mode = DlgMode.ShowingPhone;

            DialogLock (true);

            phoneTextureIndex = 0;
            phoneTextures = textures;

            Texture2D tex = textures[0];
            phoneRenderer.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero, 4);
        }
    }


    // Utility \\

    private void DialogLock(bool locking)
    {
        if (locking)
        {
            PlayerRun.receivePlayerMovementInput = false;
        }
        else
        {
            PlayerRun.receivePlayerMovementInput = true;
        }
    }

    private void PrepareWriting()
    {
        mode = DlgMode.Writing;
        preformattingPrinted = "";
        textRenderer.text = "";
        textToPrint = activeNode.GetData().text;
        timeSinceChar = 0;
    }

    private Vector2 RelativePercentMousePos()
    {
        Vector3 localPos = Camera.main.ScreenToWorldPoint(Input.mousePosition)
            - dialogBoxRenderer.transform.position;
        Vector2 scaledPos = new Vector2(
            localPos.x / transform.localScale.x,
            localPos.y / transform.localScale.y);
        scaledPos.y = 1 - scaledPos.y;

        return scaledPos;
    }

    /// <summary></summary>
    /// <returns>The index from top to bottom, otherwise -1</returns>
    private int ChoiceThatMouseIsOver()
    {
        Vector2 pos = RelativePercentMousePos();

        if (pos.x <= 0 || pos.x >= 1 || pos.y <= 0 || pos.y >= 1)
        {
            return -1;
        }
        
        if (pos.y <= 0.25)
        {
            return 0;
        }
        else if (pos.y <= 0.5 && activeNode.GetChildCount() >= 2)
        {
            return 1;
        }
        else if (pos.y < 0.75 && activeNode.GetChildCount() >= 3)
        {
            return 2;
        }
        else if (activeNode.GetChildCount() >= 4)
        {
            return 3;
        }

        return -1;
    }

    /// <summary></summary>
    /// <param name="mouseIsOver">The index from top to bottom, otherwise -1</param>
    private void BuildOptionsString(int mouseIsOver)
    {
        textRenderer.text = "";
        for (int i = 0; i < Mathf.Min(4, activeNode.GetChildCount()); i++)
        {
            if (i > 0)
            {
                textRenderer.text += "\n";
            }

            if (mouseIsOver == i)
            {
                textRenderer.text += "<color=" + SelectedChoiceColor + ">" + optionStrings[i] + "</color>";
            }
            else
            {
                textRenderer.text += optionStrings[i];
            }
        }
    }

    private void ApplyFormattingAndPrint()
    {
        textRenderer.text = "<color=" + activeNode.GetData().color + ">" + preformattingPrinted + "</color>";
    }
}

enum DlgMode
{
    Inactive,
    Loading,
    Writing,
    WaitingNext,
    WaitingChoice,

    ShowingPhone
}
