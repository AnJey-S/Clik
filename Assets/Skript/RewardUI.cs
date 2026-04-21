using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class RewardUI : MonoBehaviour
{
    public BattleManager battleManager;
    public GameManager gameManager;
    public Transform rewardPanel;
    public GameObject cardPrefab;
    public void Show (List<Card> cards)
    {
        foreach (var card in cards)
        {
            var obj = Instantiate (cardPrefab, rewardPanel);
            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(160, 220);
            rt.localScale = Vector3.one;
            CardButton btn = obj.GetComponent<CardButton>();
            if (btn != null)
                btn.SetupReward(card, this);
        }
        
    }
    public void SelectCard(Card card)
    {
        battleManager.drawPile.Add(card);
        CloseRewardScreen();
    }

    public void SkipReward()
    {
        CloseRewardScreen();
    }

    private void CloseRewardScreen()
    {
        foreach (Transform child in rewardPanel)
            Destroy(child.gameObject);
        gameManager.EnterMapState();
    }
}
