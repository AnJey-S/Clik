using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.U2D;

public class CardButton : MonoBehaviour
{
    private BattleManager battleManager;
    private RewardUI rewardUI;
    private Card card;
    private CardData cardData;
    private PlayerBuffType buffType;
    private bool isBuff = false;

    [SerializeField] TMP_Text cardTitle;
    [SerializeField] TMP_Text cardCost;
    [SerializeField] TMP_Text cardDescription;

    public SpriteAtlas Effects;
    public Image energyIcon;

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
        cardTitle.text = card.data.cardName;
        energyIcon.sprite = Effects.GetSprite("powers_194");
        cardCost.text = card.data.cost.ToString();
        cardDescription.text = card.data.cardDesription;
    }

    // Для экрана награды — карта
    public void SetupRewardUI(CardData data, RewardUI ui)
    {
        cardData = data;
        rewardUI = ui;
        cardTitle.text = data.cardName;
        energyIcon.sprite = Effects.GetSprite("powers_194");
        cardCost.text = data.cost.ToString();
        cardDescription.text = data.cardDesription;

        if (data.isUpgraded)
            cardTitle.text += "";
    }

    // Для экрана награды — бафф
    public void SetupBuffUI(PlayerBuffType buff, RewardUI ui)
    {
        buffType = buff;
        rewardUI = ui;
        isBuff = true;
        cardTitle.text = GetBuffName(buff);
        energyIcon.sprite = Effects.GetSprite("blank");
        cardCost.text = "";
        cardDescription.text = GetBuffDescription(buff);
    }

    private string GetBuffName(PlayerBuffType buff)
    {
        switch (buff)
        {
            case PlayerBuffType.BonusBlock: return "Блок в начале хода";
            case PlayerBuffType.BonusAttack: return "+3 к урону атак";
            case PlayerBuffType.ExtraEnergy: return "+1 энергия";
            case PlayerBuffType.BonusMaxHP: return "+15 max HP";
            case PlayerBuffType.Thorns: return "Шипы";
            case PlayerBuffType.Regeneration: return "Регенерация";
            case PlayerBuffType.ExtraCard: return "+1 карта в руку";
            case PlayerBuffType.Berserk: return "Берсерк";
            default: return buff.ToString();
        }
    }

    private string GetBuffDescription(PlayerBuffType buff)
    {
        switch (buff)
        {
            case PlayerBuffType.BonusBlock: return "+4 блока";
            case PlayerBuffType.BonusAttack: return "";
            case PlayerBuffType.ExtraEnergy: return "";
            case PlayerBuffType.BonusMaxHP: return "";
            case PlayerBuffType.Thorns: return "+2 урона врагу от своей атаки";
            case PlayerBuffType.Regeneration: return "+3 HP в начале хода";
            case PlayerBuffType.ExtraCard: return "";
            case PlayerBuffType.Berserk: return "+1 энергия за атаку";
            default: return "";
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