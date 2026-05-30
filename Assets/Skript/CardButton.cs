using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.U2D;

public class CardButton : MonoBehaviour
{
    // ----------------------------------------------------------------
    // Этот класс отвечает за отображение карты в интерфейсе и обработку кликов по ней. Он может использоваться как для отображения карт в бою, так и для отображения карт и баффов в интерфейсе награды.
    // В зависимости от контекста, он может отображать информацию о карте или баффе и вызывать соответствующие методы при клике.
    // ----------------------------------------------------------------
    // Методы:
    // - Setup(Card newCard, BattleManager manager): Этот метод инициализирует кнопку для отображения карты в бою. Он устанавливает текст и иконки в соответствии с данными карты и сохраняет ссылку на BattleManager для обработки кликов.
    // - SetupRewardUI(CardData data, RewardUI ui): Этот метод инициализирует кнопку для отображения карты в интерфейсе награды. Он устанавливает текст и иконки в соответствии с данными карты и сохраняет ссылку на RewardUI для обработки кликов.
    // - SetupBuffUI(PlayerBuffType buff, RewardUI ui): Этот метод инициализирует кнопку для отображения баффа в интерфейсе награды. Он устанавливает текст и иконки в соответствии с типом баффа и сохраняет ссылку на RewardUI для обработки кликов.
    // - GetCard(): Этот метод возвращает карту, связанную с этой кнопкой, если она есть.
    // - OnClick(): Этот метод обрабатывает клик по кнопке. В зависимости от контекста, он может вызвать метод PlayCard в BattleManager для карты в бою, или вызвать методы SelectBuff или SelectCard в RewardUI для выбора баффа или карты в интерфейсе награды.
    // - DestroySelf(): Этот метод уничтожает объект кнопки, вызывая Destroy(gameObject).
    // ----------------------------------------------------------------
    // Поля:
    // - battleManager: Ссылка на BattleManager, используемая для обработки кликов по карте в бою.
    // - rewardUI: Ссылка на RewardUI, используемая для обработки кликов по карте или баффу в интерфейсе награды.
    // - card: Ссылка на карту, связанную с этой кнопкой, если она отображает карту в бою.
    // - cardData: Ссылка на данные карты, связанные с этой кнопкой, если она отображает карту в интерфейсе награды.
    // - buffType: Тип баффа, связанный с этой кнопкой, если она отображает бафф в интерфейсе награды.
    // - isBuff: Флаг, указывающий, отображает ли эта кнопка бафф в интерфейсе награды.
    // - cardTitle: Ссылка на TMP_Text для отображения названия карты или баффа.
    // - cardCost: Ссылка на TMP_Text для отображения стоимости карты.
    // - cardDescription: Ссылка на TMP_Text для отображения описания карты или баффа.
    // - Effects: Ссылка на SpriteAtlas, содержащий иконки для карт и баффов.
    // - energyIcon: Ссылка на Image, используемую для отображения иконки энергии или пустой иконки для баффов.
    // ----------------------------------------------------------------
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