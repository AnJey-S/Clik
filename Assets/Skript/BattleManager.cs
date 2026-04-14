using JetBrains.Annotations;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class BattleManager : MonoBehaviour
{
    public Player player; //Создание экземпляров классов
    public Enemy enemy;
    public int[] cardAmount = new int[14];
    
    void Start()
    {
        cardAmount[0] = 5;
        cardAmount[1] = 3;
        cardAmount[2] = 1;
        cardAmount[3] = 1;
        cardAmount[4] = 1;
        CreateDeck();
        Shuffle(drawPile);
        currentState = TurnState.PlayerTurn;
        StartPlayerTurn();
        RectTransform handRT = handArea.GetComponent<RectTransform>();
        handRT.localScale = Vector3.one;

        // Уничтожаем ContentSizeFitter если он есть
        ContentSizeFitter csf = handArea.GetComponent<ContentSizeFitter>();
        if (csf != null) Destroy(csf);


    }
    

    public enum TurnState
    {
        PlayerTurn,
        EnemyTurn
    }
    
    public TurnState currentState; //В переменную сохраняется текущий ход (игрок или противник)
    
    //Метод использования карты
    public void PlayCard(Card card)
    {
        if (currentState != TurnState.PlayerTurn) return; //Проверка условий использования карты (чей ход)
        if (hand.Count == 0) return; //В руке есть карты
        if (energy < card.cost) return; //Хватает энергии

        energy -= card.cost; //Учёт стоимости карты
        hand.Remove(card); //Удаление карты из списка
        card.Use(player, enemy); //Использование метода карты
        discardPile.Add(card); //Перемещение карты в сброс

        Debug.Log("Энергии: " + energy);
        Debug.Log("Карт в колоде набора: " + drawPile.Count);
        Debug.Log("Карт в руке: " + hand.Count);
        Debug.Log("Карт в колоде сброса: " + discardPile.Count);

        CardButton btn = FindCardButton(card); //Удаление карты со сцены 
        if (btn != null)
            btn.DestroySelf();

        if (energy == 0) //Энергия закончилась
            EndPlayerTurn();
        
    }

    CardButton FindCardButton(Card card)
    {
        CardButton[] buttons = FindObjectsOfType<CardButton>();
        foreach (CardButton btn in buttons)
        {
            if (btn.GetCard() == card)
                return btn;
        }
        return null;
    }

    

    //Создание карт в колоде (начальных карт)
    void CreateDeck()
    {
        for (int i = 0; i < cardAmount[0]; i++)
        {
            Card.AttackCard card = new Card.AttackCard(); //Создаётся экземпляр класса карты, задаются её значения
            card.cardName = "Attack";
            card.cost = 1;
            card.damage = 8;
            
            drawPile.Add(card); // и добавляется в колоду
        }
        for (int i = 0; i < cardAmount[1]; i++)
        {
            Card.BlockCard card = new Card.BlockCard();
            card.cardName = "Block";
            card.cost = 1;
            card.block = 10;
            
            drawPile.Add(card);
        }
        for (int i = 0; i < cardAmount[2]; i++)
        {
            Card.DaringAttackCard card = new Card.DaringAttackCard();
            card.cardName = "Daring Attack";
            card.cost = 1;
            card.damage = 12;

            drawPile.Add(card);
        }
        for (int i = 0; i < cardAmount[3]; i++)
        {
            Card.potionCard card = new Card.potionCard();
            card.cardName = "Potion";
            card.cost = 1;

            drawPile.Add(card);
        }
        for (int i = 0; i < cardAmount[4]; i++)
        {
            Card.stunCard card = new Card.stunCard();
            card.cardName = "Stun";
            card.cost = 2;
            drawPile.Add(card);
        }
    }
    void Shuffle(List<Card> list) //Перемешивание карт в колоде
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = Random.Range(i, list.Count);
            (list[i], list[rnd]) = (list[rnd], list[i]);
        }
    }
    [SerializeField] GameObject cardPrefab; //Инициализация объектов на сцене (связанных с картами)
    [SerializeField] Transform handArea;
    [SerializeField] TMP_Text enemyText;
    private void Update()
    {
        enemyText.text = enemy.Health.ToString();
    }
    void DrawCards(int amount) //Выдаёт карты в руку
    {
        for (int i = 0; i < amount; i++)
        {
            if (drawPile.Count == 0) //Обновления списка карт (колода-сброс)
            {
                drawPile.AddRange(discardPile);
                discardPile.Clear();
                Shuffle(drawPile);
            }

            if (drawPile.Count == 0)
                return;

            Card card = drawPile[0]; //Выдача карт в руку
            drawPile.RemoveAt(0);
            hand.Add(card);

            GameObject obj = Instantiate(cardPrefab, handArea); //Создание объектов карт на сцене

            
            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(160, 220);
            rt.localScale = Vector3.one;
            
            CardButton btn = obj.GetComponent<CardButton>();
            if (btn != null)
                btn.Setup(card, this);
        }
        Debug.Log("В руке: " + hand.Count);
    }
    void StartPlayerTurn() //Параметры хода игрока
    {
        energy = 3;
        DrawCards(5 - hand.Count);
        Debug.Log("Ход игрока");

    }

    void EnemyTurn() //Параметры хода противника
    {
        
        Debug.Log("Ход врага");
        if (enemy.stunTime == 0)
        {
            enemy.Attak(player);
            if (enemy.poisonedTime != 0)
            {
                enemy.poisoned(enemy.poisonedTime);
                Debug.Log("Отравление " + enemy.poisonedTime);
                enemy.poisonedTime--;
            }
            
        }
        EndEnemyTurn();
    }

    public void EndPlayerTurn() //Передача хода противнику
    {
        discardPile.AddRange(hand); //Добавление карт из руки в сброс

        foreach (Card card in hand) //Для каждой карты в руке:
        {
            CardButton btn = FindCardButton(card); //Удаление карты со сцены 
            if (btn != null)
                btn.DestroySelf();
        }
        hand.Clear(); //Очищение руки

        currentState = TurnState.EnemyTurn;
        EnemyTurn();

    }
    public void EndEnemyTurn() //Предача хода игроку
    {
        currentState = TurnState.PlayerTurn;
        StartPlayerTurn();
    }

    public List<Card> drawPile = new List<Card>(); //Список колоды
    public List<Card> hand = new List<Card>(); //Рука
    public List<Card> discardPile = new List<Card>(); //Сброс
    public int energy = 3;
    public int maxHandSize = 5;
}
