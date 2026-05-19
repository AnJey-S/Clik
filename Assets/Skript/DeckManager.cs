using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    // ----------------------------------------------------------------
    //
    // Этот класс отвечает за управление колодой карт игрока. Он хранит три основных списка карт: колоду для тягивания, руку и колоду для сброса.
    // Он позволяет игроку тянуть карты, сбрасывать руку и отдельные карты, а также перемешивать колоду при необходимости.
    //
    // ----------------------------------------------------------------
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform handArea;

    public List<Card> drawPile = new List<Card>();
    public List<Card> hand = new List<Card>();
    public List<Card> discardPile = new List<Card>();

    private BattleManager battleManager;

    public void Initialize(BattleManager bm)
    {
        battleManager = bm;
        CreateDeck();
        Shuffle(drawPile);
    }

    private void CreateDeck()
    {
        foreach (CardData data in GameManager.Instance.playerDeck)
            drawPile.Add(new Card(data));
    }

    public void DrawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (drawPile.Count == 0)
            {
                drawPile.AddRange(discardPile);
                discardPile.Clear();
                Shuffle(drawPile);
            }

            if (drawPile.Count == 0) return;

            Card card = drawPile[0];
            drawPile.RemoveAt(0);
            hand.Add(card);

            GameObject obj = Instantiate(cardPrefab, handArea);
            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(160, 220);
            rt.localScale = Vector3.one;

            CardButton btn = obj.GetComponent<CardButton>();
            if (btn != null)
                btn.Setup(card, battleManager);
        }
    }

    public void DiscardHand()
    {
        foreach (Card card in hand)
        {
            CardButton btn = FindCardButton(card);
            if (btn != null)
                btn.DestroySelf();
        }
        discardPile.AddRange(hand);
        hand.Clear();
    }

    public void DiscardCard(Card card)
    {
        hand.Remove(card);
        discardPile.Add(card);

        CardButton btn = FindCardButton(card);
        if (btn != null)
            btn.DestroySelf();
    }

    public CardButton FindCardButton(Card card)
    {
        CardButton[] buttons = FindObjectsByType<CardButton>(FindObjectsSortMode.None);
        foreach (CardButton btn in buttons)
            if (btn.GetCard() == card)
                return btn;
        return null;
    }

    private void Shuffle(List<Card> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = Random.Range(i, list.Count);
            (list[i], list[rnd]) = (list[rnd], list[i]);
        }
    }
}
