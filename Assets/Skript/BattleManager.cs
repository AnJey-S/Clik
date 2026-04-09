using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public Player player;
    public Enemy enemy;
    void Awake()
    {
        player = new Player();
        enemy = new Enemy();

    }
    public enum TurnState
    {
        PlayerTurn,
        EnemyTurn
    }

    public TurnState currentState;

    public void PlayCard(Card card) 
    {
        if (currentState != TurnState.PlayerTurn)
            return;
        if (hand.Count == 0)
            return;
        if (energy < card.cost)
            return;
        energy -= card.cost;
        Debug.Log(card);
        card.Use(player, enemy);
    }

    void Start()
    {
        CreateDeck();
        Shuffle(drawPile);
        currentState = TurnState.PlayerTurn;
        StartPlayerTurn();
    }
    void CreateDeck()
    {
        for (int i = 0; i < 5; i++)
        {
            Card.AttackCard card = new Card.AttackCard();
            card.cardName = "Attack";
            card.cost = 1;
            card.damage = 6;
            drawPile.Add(card);
        }
        for (int i = 0; i < 3; i++)
        {
            Card.BlockCard card = new Card.BlockCard();
            card.cardName = "Block";
            card.cost = 1;
            card.block = 10;
            drawPile.Add(card);
        }
    }
    void Shuffle(List<Card> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = Random.Range(i, list.Count);
            (list[i], list[rnd]) = (list[rnd], list[i]);
        }
    }
    void DrawCards(int amount)
    {
        for (int i = 0; i <= amount; i++)
        {
            if (drawPile.Count == 0)
            {
                drawPile.AddRange(discardPile);
                discardPile.Clear();
                Shuffle(drawPile);
            }

            if (drawPile.Count == 0)
                return;
            Card card = drawPile[0];
            drawPile.RemoveAt(0);
            hand.Add(card);
        }
        Debug.Log("В руке: " + hand.Count);
    }
    void StartPlayerTurn()
    {
        energy = 3;
        DrawCards(5 - hand.Count);
        Debug.Log("Ход игрока");
        
    }
    void EnemyTurn()
    {
        Debug.Log("Ход врага");
        enemy.Attak(player);
        EndEnemyTurn();
    }

    public void EndPlayerTurn()
    {
        discardPile.AddRange(hand);
        hand.Clear();
        currentState = TurnState.EnemyTurn;
        EnemyTurn();

    }
    public void EndEnemyTurn()
    {
        currentState = TurnState.PlayerTurn;
        StartPlayerTurn();
    }

    public List<Card> drawPile = new List<Card>();
    public List<Card> hand = new List<Card>();
    public List<Card> discardPile = new List<Card>();
    public int energy = 3;
    public int maxHandSize = 5;

}
