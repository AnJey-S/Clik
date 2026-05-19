using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [Header("Пул карт для наград")]
    [SerializeField] private List<CardData> rewardCardPool;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public List<CardData> GetUpgradeCandidates()
    {
        List<CardData> deck = new List<CardData>(GameManager.Instance.playerDeck);
        deck.RemoveAll(card => card.isUpgraded || card.upgradedVersion == null);

        if (deck.Count == 0) return new List<CardData>();

        Shuffle(deck);
        return deck.GetRange(0, Mathf.Min(3, deck.Count));
    }

    public List<CardData> GetRewardCandidates()
    {
        List<CardData> pool = new List<CardData>(rewardCardPool);
        Shuffle(pool);
        return pool.GetRange(0, Mathf.Min(3, pool.Count));
    }

    public void UpgradeCard(CardData card)
    {
        GameManager.Instance.UpgradeCard(card);
    }

    public void AddCard(CardData card)
    {
        GameManager.Instance.AddCardToDeck(card);
    }
    void Shuffle(List<CardData> list) //Перемешивание карт в колоде
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = Random.Range(i, list.Count);
            (list[i], list[rnd]) = (list[rnd], list[i]);
        }
    }
}
