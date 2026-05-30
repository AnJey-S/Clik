using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardUI : MonoBehaviour
{
    // ----------------------------------------------------------------
    //
    // Этот класс отвечает за отображение интерфейса выбора награды после боя. Он позволяет игроку выбрать между улучшением существующей карты или добавлением новой карты в колоду.
    // В зависимости от выбора, он отображает соответствующий список карт для улучшения или добавления.
    // После выбора карты или пропуска, он возвращает игрока на карту.
    //
    // ----------------------------------------------------------------
    // Поля:
    // - choicePanel: Панель с выбором типа награды (улучшение или новая карта).
    // - upgradeButton: Кнопка для выбора улучшения карты.
    // - newCardButton: Кнопка для выбора новой карты.
    // - cardSelectionPanel: Панель с выбором конкретной карты для улучшения или добавления.
    // - cardContainer: Контейнер для отображения карточек в панели выбора
    // - cardPrefab: Префаб карточки для отображения в панели выбора.
    // - selectionTitle: Заголовок панели выбора, который меняется в зависимости от типа
    // - skipButton: Кнопка для пропуска выбора награды.
    // - currentMode: Текущий выбранный режим награды (улучшение или новая карта).
    // - isElite: Флаг, указывающий, является ли текущая комната элитной, что влияет на отображение интерфейса награды.
    // 
    // Методы:
    // - Start(): Инициализирует интерфейс и назначает обработчики событий для кнопок.
    // - ShowChoicePanel(): Показывает панель выбора типа награды и блокирует кнопку улучшения, если нечего улучшать.
    // - ShowCardSelection(RewardMode mode): Показывает панель выбора карты в зависимости от выбранного режима.
    // - SelectCard(CardData card): Вызывается при выборе карты, выполняет соответствующее действие (улучшение или добавление) и возвращает на карту.
    // - Skip(): Вызывается при нажатии кнопки пропуска, возвращает на карту без выбора награды.
    //
    // ----------------------------------------------------------------



    [Header("Бафф элиты")]
    [SerializeField] private GameObject eliteBuffPanel;
    [SerializeField] private Transform buffContainer;

    [Header("Выбор типа награды")]
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button newCardButton;

    [Header("Выбор карты")]
    [SerializeField] private GameObject cardSelectionPanel;
    [SerializeField] private Transform cardContainer;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private TMP_Text selectionTitle;
    [SerializeField] private Button skipButton;

    private RewardMode currentMode;
    private bool isElite;

    private enum RewardMode { Upgrade, NewCard }

    private void Start()
    {
        isElite = GameManager.Instance.currentNode.roomType == RoomType.Elite;

        upgradeButton.onClick.AddListener(() => ShowCardSelection(RewardMode.Upgrade));
        newCardButton.onClick.AddListener(() => ShowCardSelection(RewardMode.NewCard));
        skipButton.onClick.AddListener(Skip);

        if (isElite)
            ShowEliteBuffPanel();
        else
            ShowChoicePanel();
    }

    // ----------------------------------------------------------------
    // Бафф элиты
    // ----------------------------------------------------------------

    private void ShowEliteBuffPanel()
    {
        eliteBuffPanel.SetActive(true);
        choicePanel.SetActive(false);
        cardSelectionPanel.SetActive(false);

        foreach (Transform child in buffContainer)
            Destroy(child.gameObject);

        List<PlayerBuffType> allBuffs = new List<PlayerBuffType>
        {
            PlayerBuffType.BonusBlock,
            PlayerBuffType.BonusAttack,
            PlayerBuffType.ExtraEnergy,
            PlayerBuffType.BonusMaxHP,
            PlayerBuffType.Thorns,
            PlayerBuffType.Regeneration,
            PlayerBuffType.ExtraCard,
            PlayerBuffType.Berserk
        };

        // Перемешиваем и берём 3
        for (int i = 0; i < allBuffs.Count; i++)
        {
            int rnd = Random.Range(i, allBuffs.Count);
            (allBuffs[i], allBuffs[rnd]) = (allBuffs[rnd], allBuffs[i]);
        }

        for (int i = 0; i < 3; i++)
        {
            PlayerBuffType buff = allBuffs[i];
            GameObject obj = Instantiate(cardPrefab, buffContainer);
            CardButton btn = obj.GetComponent<CardButton>();
            btn.SetupBuffUI(buff, this);
        }
    }

    public void SelectBuff(PlayerBuffType buff)
    {
        GameManager.Instance.AddBuff(buff);
        eliteBuffPanel.SetActive(false);
        ShowChoicePanel();
    }

    // ----------------------------------------------------------------
    // Обычная награда
    // ----------------------------------------------------------------

    private void ShowChoicePanel()
    {
        choicePanel.SetActive(true);
        cardSelectionPanel.SetActive(false);

        bool canUpgrade = UpgradeManager.Instance.GetUpgradeCandidates().Count > 0;
        upgradeButton.interactable = canUpgrade;
    }

    private void ShowCardSelection(RewardMode mode)
    {
        currentMode = mode;
        choicePanel.SetActive(false);
        cardSelectionPanel.SetActive(true);

        foreach (Transform child in cardContainer)
            Destroy(child.gameObject);

        List<CardData> cards = mode == RewardMode.Upgrade
            ? UpgradeManager.Instance.GetUpgradeCandidates()
            : UpgradeManager.Instance.GetRewardCandidates();

        selectionTitle.text = mode == RewardMode.Upgrade
            ? "Выберите карту для улучшения"
            : "Выберите карту";

        foreach (CardData card in cards)
        {
            GameObject obj = Instantiate(cardPrefab, cardContainer);
            CardButton btn = obj.GetComponent<CardButton>();
            btn.SetupRewardUI(card, this);
        }
    }

    public void SelectCard(CardData card)
    {
        if (currentMode == RewardMode.Upgrade)
            UpgradeManager.Instance.UpgradeCard(card);
        else
            UpgradeManager.Instance.AddCard(card);

        GameManager.Instance.LoadMap();
    }

    public void Skip()
    {
        GameManager.Instance.LoadMap();
    }
}