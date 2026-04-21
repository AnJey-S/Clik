using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardButton : MonoBehaviour
{
    private BattleManager battleManager;
    private RewardUI rewardUI;
    private Card card;
    

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
    public void SetupReward(Card newCard, RewardUI manager)
    {
        card = newCard;
        rewardUI = manager;
        text.text = card.cardName;
    }
    public Card GetCard()
    {
        return card;
    }

    public void OnClick()
    {
        if (battleManager != null)
            battleManager.PlayCard(card);
        else if (rewardUI != null)
            rewardUI.SelectCard(card);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}