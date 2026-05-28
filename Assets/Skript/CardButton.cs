using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardButton : MonoBehaviour
{
    private BattleManager battleManager;
    private RewardUI rewardUI;
    private Card card;
    private CardData cardData;
    private PlayerBuffType buffType;
    private bool isBuff = false;

    [SerializeField] TMP_Text text;

    void Awake()
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
            btn.onClick.AddListener(OnClick);
    }

    // Для боя
    public void Setup(Card newCard, BattleManager manager)
    {
        card = newCard;
        battleManager = manager;
        text.text = card.data.cardName;
    }

    // Для экрана награды — карта
    public void SetupRewardUI(CardData data, RewardUI ui)
    {
        cardData = data;
        rewardUI = ui;
        text.text = data.cardName;

        if (data.isUpgraded)
            text.text += "";
    }

    // Для экрана награды — бафф
    public void SetupBuffUI(PlayerBuffType buff, RewardUI ui)
    {
        buffType = buff;
        rewardUI = ui;
        isBuff = true;
        text.text = GetBuffName(buff);
    }

    private string GetBuffName(PlayerBuffType buff)
    {
        switch (buff)
        {
            case PlayerBuffType.BonusBlock: return "Блок в начале хода";
            case PlayerBuffType.BonusAttack: return "+3 к урону атак";
            case PlayerBuffType.ExtraEnergy: return "+1 энергия";
            case PlayerBuffType.BonusMaxHP: return "+15 максимального HP";
            case PlayerBuffType.Thorns: return "Шипы";
            case PlayerBuffType.Regeneration: return "Регенерация";
            case PlayerBuffType.ExtraCard: return "+1 карта в руку";
            case PlayerBuffType.Berserk: return "Берсерк";
            default: return buff.ToString();
        }
    }

    public Card GetCard()
    {
        return card;
    }

    public void OnClick()
    {
        if (battleManager != null && card != null)
            battleManager.PlayCard(card);
        else if (rewardUI != null && isBuff)
            rewardUI.SelectBuff(buffType);
        else if (rewardUI != null && cardData != null)
            rewardUI.SelectCard(cardData);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}