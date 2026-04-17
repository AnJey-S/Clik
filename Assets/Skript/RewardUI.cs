using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class RewardUI : MonoBehaviour
{
    public Transform rewardPanel;
    public GameObject cardPrefab;
    public void Show (List<Card> cards)
    {
        foreach (var card in cards)
        {
            var obj = Instantiate (cardPrefab, rewardPanel);
            CardButton btn = obj.GetComponent<CardButton>();
            if (btn != null)
                btn.SetupReward(card, this);
        }
    }
}
