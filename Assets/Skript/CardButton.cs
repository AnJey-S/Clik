using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardButton : MonoBehaviour
{
    private BattleManager battleManager;
    private RewardUI rewardUI;
    private Card card;           // для боя
    private CardData cardData;   // для награды

    [SerializeField] TMP_Text text;

    void Awake()
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
            btn.onClick.AddListener(OnClick);
    }

    // Для боя — принимает Card
    public void Setup(Card newCard, BattleManager manager)
    {
        card = newCard;
        battleManager = manager;
        text.text = card.data.cardName;
    }

    // Для экрана награды — принимает CardData
    public void SetupRewardUI(CardData data, RewardUI ui)
    {
        cardData = data;
        rewardUI = ui;
        text.text = data.cardName;

        if (data.isUpgraded)
            text.text += " ✦";
    }

    public Card GetCard()
    {
        return card;
    }

    public void OnClick()
    {
        if (battleManager != null && card != null)
            battleManager.PlayCard(card);
        else if (rewardUI != null && cardData != null)
            rewardUI.SelectCard(cardData);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}