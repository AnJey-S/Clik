using JetBrains.Annotations;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public RewardUI rewardUI; 
    public List<Card> cardPool = new List<Card>();
    public List<Card> currentRewards = new List<Card>();
    private void Start()
    {

        for (int i = 0; i < 1; i++)
        {
            Card.AttackCard card = new Card.AttackCard(); //Создаётся экземпляр класса карты, задаются её значения
            card.cardName = "Attack";
            card.cost = 1;
            card.damage = 8;

            cardPool.Add(card); // и добавляется в колоду
        }
        for (int i = 0; i < 1; i++)
        {
            Card.BlockCard card = new Card.BlockCard();
            card.cardName = "Block";
            card.cost = 1;
            card.block = 10;

            cardPool.Add(card);
        }
        for (int i = 0; i < 1; i++)
        {
            Card.DaringAttackCard card = new Card.DaringAttackCard();
            card.cardName = "Daring Attack";
            card.cost = 1;
            card.damage = 12;

            cardPool.Add(card);
        }
        for (int i = 0; i < 1; i++)
        {
            Card.potionCard card = new Card.potionCard();
            card.cardName = "Potion";
            card.cost = 1;

            cardPool.Add(card);
        }
        for (int i = 0; i < 1; i++)
        {
            Card.stunCard card = new Card.stunCard();
            card.cardName = "Stun";
            card.cost = 2;
            cardPool.Add(card);
        }
    }
    public void GenerateRewards()
    {
        currentRewards.Clear();

        List<Card> shuffled = new List<Card>(cardPool);
        Shuffle(shuffled);

        int count = Mathf.Min(3, shuffled.Count);
        for (int i = 0; i < count; i++)
            currentRewards.Add(shuffled[i]);

        rewardUI.Show(currentRewards);
    }
    void Shuffle(List<Card> list) //Перемешивание карт в колоде
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = Random.Range(i, list.Count);
            (list[i], list[rnd]) = (list[rnd], list[i]);
        }
    }
}
