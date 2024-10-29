using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public float CharacterPrintDelay = 0.35f;
    public string SelectedChoiceColor = "black";
    public Texture2D BackgroundTexture;

    // references
    private GameObject player;
    //private SpriteRenderer dialogBoxRenderer;
    //private TextMeshPro textRenderer;
    //private SpriteRenderer phoneRenderer;

    private Image dialogBox;
    private TextMeshProUGUI dialogText;
    private Image phone; 


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


        dialogBox = GameObject.Find("DialogBox").GetComponent<Image>();
        dialogText = GameObject.Find("DialogText").GetComponent<TextMeshProUGUI>();
        phone = GameObject.Find("Phone").GetComponent<Image>();

        dialogBox.enabled = false;
        dialogText.text = "";
        phone.enabled = false;


        optionStrings = new string[4];

        if (BackgroundTexture == null)
        {
            BackgroundTexture = Texture2D.whiteTexture;
        }
    }

    void Update()
    {
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

                if (Input.GetMouseButtonDown (0) || Input.anyKeyDown)
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
                            dialogText.text = "";
                            dialogBox.enabled = false;

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
                        phone.enabled = false;
                        mode = DlgMode.Inactive;
                        DialogLock (false);
                    }
                    else
                    {
                        Texture2D tex = phoneTextures[phoneTextureIndex];
                        //phoneRenderer.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero, 4);
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
            dialogBox.enabled = true;

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
            //phoneRenderer.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero, 4);
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
        dialogText.text = "";
        textToPrint = activeNode.GetData().text;
        timeSinceChar = 0;
    }

    //private Vector2 RelativePercentMousePos()
    //{
    //    Vector3 localPos = Camera.main.ScreenToWorldPoint(Input.mousePosition)
    //        - dialogBoxRenderer.transform.position;
    //    Vector2 scaledPos = new Vector2(
    //        localPos.x / transform.localScale.x,
    //        localPos.y / transform.localScale.y);
    //    scaledPos.y = 1 - scaledPos.y;

    //    return scaledPos;
    //}

    /// <summary></summary>
    /// <returns>The index from top to bottom, otherwise -1</returns>
    private int ChoiceThatMouseIsOver()
    {
        RectTransform dialogBoxRectTransform = dialogBox.GetComponent<RectTransform>();
        Vector2 localMousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            dialogBoxRectTransform, Input.mousePosition, null, out localMousePos);

        float height = dialogBoxRectTransform.rect.height;
        float posY = Mathf.InverseLerp(0, height, -localMousePos.y);

        if (posY < 0.25f) return 0;
        if (posY < 0.5f && activeNode.GetChildCount() >= 2) return 1;
        if (posY < 0.75f && activeNode.GetChildCount() >= 3) return 2;
        if (posY <= 1f && activeNode.GetChildCount() >= 4) return 3;

        return -1;
    }

    /// <summary></summary>
    /// <param name="mouseIsOver">The index from top to bottom, otherwise -1</param>
    private void BuildOptionsString(int mouseIsOver)
    {
        dialogText.text = "";
        for (int i = 0; i < Mathf.Min(4, activeNode.GetChildCount()); i++)
        {
            if (i > 0)
            {
                dialogText.text += "\n";
            }

            if (mouseIsOver == i)
            {
                dialogText.text += "<color=" + SelectedChoiceColor + ">" + optionStrings[i] + "</color>";
            }
            else
            {
                dialogText.text += optionStrings[i];
            }
        }
    }

    private void ApplyFormattingAndPrint()
    {
        dialogText.text = "<color=" + activeNode.GetData().color + ">" + preformattingPrinted + "</color>";
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
