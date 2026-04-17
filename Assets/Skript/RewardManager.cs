using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public RewardUI rewardUI; 
    public List<Card> listCard = new List<Card>();
    public List<Card> poolRewards = new List<Card>();
    private void Start()
    {

        for (int i = 0; i < 1; i++)
        {
            Card.AttackCard card = new Card.AttackCard(); //Создаётся экземпляр класса карты, задаются её значения
            card.cardName = "Attack";
            card.cost = 1;
            card.damage = 8;

            listCard.Add(card); // и добавляется в колоду
        }
        for (int i = 0; i < 1; i++)
        {
            Card.BlockCard card = new Card.BlockCard();
            card.cardName = "Block";
            card.cost = 1;
            card.block = 10;

            listCard.Add(card);
        }
        for (int i = 0; i < 1; i++)
        {
            Card.DaringAttackCard card = new Card.DaringAttackCard();
            card.cardName = "Daring Attack";
            card.cost = 1;
            card.damage = 12;

            listCard.Add(card);
        }
        for (int i = 0; i < 1; i++)
        {
            Card.potionCard card = new Card.potionCard();
            card.cardName = "Potion";
            card.cost = 1;

            listCard.Add(card);
        }
        for (int i = 0; i < 1; i++)
        {
            Card.stunCard card = new Card.stunCard();
            card.cardName = "Stun";
            card.cost = 2;
            listCard.Add(card);
        }
    }
    public void GenerateRewards()
    {
        for (int i = 0;  i < 3; i++)
        {
            Card randomCard = listCard[Random.Range(0, listCard.Count)];
            poolRewards.Add(randomCard);
        }
        rewardUI.Show(poolRewards);
    }
}
