using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardUI : MonoBehaviour
{
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

    private enum RewardMode { Upgrade, NewCard }

    private void Start()
    {
        ShowChoicePanel();

        upgradeButton.onClick.AddListener(() => ShowCardSelection(RewardMode.Upgrade));
        newCardButton.onClick.AddListener(() => ShowCardSelection(RewardMode.NewCard));
        skipButton.onClick.AddListener(Skip);
    }

    private void ShowChoicePanel()
    {
        choicePanel.SetActive(true);
        cardSelectionPanel.SetActive(false);

        // Если нечего улучшать — блокируем кнопку
        bool canUpgrade = UpgradeManager.Instance.GetUpgradeCandidates().Count > 0;
        upgradeButton.interactable = canUpgrade;
    }

    private void ShowCardSelection(RewardMode mode)
    {
        currentMode = mode;
        choicePanel.SetActive(false);
        cardSelectionPanel.SetActive(true);

        // Очищаем старые карты
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