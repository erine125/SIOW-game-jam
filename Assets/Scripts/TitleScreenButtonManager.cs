using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleScreenButtonManager : MonoBehaviour
{
    public Sprite hoverSprite;        // Sprite to show on hover
    public Sprite defaultSprite;      // Default sprite for the button
    private Image buttonImage;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        buttonImage.sprite = defaultSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.sprite = hoverSprite;  // Switch to hover sprite
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.sprite = defaultSprite; // Switch back to default sprite
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Ext-Pier");  // start game in pier 
    }
}
