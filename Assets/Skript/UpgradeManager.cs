using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    // ----------------------------------------------------------------
    //
    // Этот класс отвечает за управление процессом улучшения карт и выбора наград после боя. Он предоставляет методы для получения кандидатов на улучшение из колоды игрока, получения кандидатов на награды из пула карт, а также методы для применения улучшений и добавления новых карт в колоду игрока.
    // ----------------------------------------------------------------
    // Методы:
    //    - GetUpgradeCandidates(): Этот метод возвращает список карт из колоды игрока, которые могут быть улучшены (то есть те, которые ещё не улучшены и имеют версию для улучшения). Он перемешивает колоду и возвращает до 3 случайных карт в качестве кандидатов на улучшение.
    //    - GetRewardCandidates(): Этот метод возвращает список карт из пула карт для наград, перемешивает его и возвращает до 3 случайных карт в качестве кандидатов на награды.
    //    - UpgradeCard(CardData card): Этот метод принимает карту и вызывает метод UpgradeCard() в GameManager для применения улучшения к этой карте в колоде игрока.
    //    - AddCard(CardData card): Этот метод принимает карту и вызывает метод AddCardToDeck() в GameManager для добавления этой карты в колоду игрока.
    // Поля:
    //    - rewardCardPool: Список карт, которые могут быть предложены в качестве награды после боя. Этот список задаётся в инспекторе Unity и может включать любые карты, которые разработчик хочет предложить игроку в качестве награды.
    // ----------------------------------------------------------------
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
