using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardButton : MonoBehaviour
{
    public BattleManager battleManager;
    public Card card;

    [SerializeField] TMP_Text text;

    void Awake()
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
            btn.onClick.AddListener(OnClick);
    }

    public void Setup(Card newCard, BattleManager manager)
    {
        card = newCard;
        battleManager = manager;
        text.text = card.cardName;
    }

    public Card GetCard()
    {
        return card;
    }

    public void OnClick()
    {
        battleManager.PlayCard(card);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}