using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardPreview : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text nameText;
    public TMP_Text costText;
    public TMP_Text attackText;
    public TMP_Text healthText;
    public TMP_Text creatureTypeText;

    public Image characterImage;
    public Image bgImage;
    public Image creatureTypeImage;

    /// <summary>
    /// Call this to populate the preview from a live Card_U.
    /// </summary>
    public void SetupFrom(Card_U card)
    {
        var so = card.cardSO;
        Debug.Log($"[Preview] name={so.cardName}  cost={so.cardCost}  atk={so.attackPower}  " +
                  $"health={card.currentHealth}  bgSprite={so.bgSprite.name}");
        nameText.text = so.cardName;
        costText.text = so.cardCost.ToString();
        attackText.text = so.attackPower.ToString();
        healthText.text = so.currentHealth.ToString();
        creatureTypeText.text = so.creatureType.ToString();
        characterImage.sprite = so.characterSprite;
        bgImage.sprite = so.bgSprite;
        creatureTypeImage.sprite = so.creatureTypeSprite;
    }
}
