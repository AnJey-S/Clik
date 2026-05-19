using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    // ----------------------------------------------------------------
    //
    // Этот класс является центральным менеджером игры, который хранит состояние игрока, его колоду карт, активные баффы и текущую позицию на карте.
    // Он обеспечивает сохранение данных между сценами и предоставляет методы для управления здоровьем игрока, колодой карт, баффами и навигацией по сценам.
    //
    // ----------------------------------------------------------------
    public List<List<MapNode>> currentMap;
    public static GameManager Instance { get; private set; }
    [Header("Состояние игрока")]
    public int playerHP = 50;
    public int playerMaxHP = 50;
    public List<CardData> playerDeck = new List<CardData>();

    [Header("Баффы")]
    public List<PlayerBuffType> activeBuffs = new List<PlayerBuffType>();

    [Header("Карта")]
    public MapNode currentNode;

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
    public void LoadMap()
    {
        SceneManager.LoadScene("MapScene");
    }
    public void LoadBattle()
    {
        SceneManager.LoadScene("BattleScene");
    }
    public void LoadReward()
    {
        SceneManager.LoadScene("RewardScene");
    }
    public void HealPlayer (int amount)
    {
        playerHP = Mathf.Min(playerHP + amount, playerMaxHP);
    }

    public void HealPlayerPercent (float percent)
    {
        int amount = Mathf.RoundToInt(playerMaxHP * percent);
        HealPlayer(amount);
    }

    public void DamagePlayer(int amount) 
    {
        playerHP -= amount;
        if (playerHP <= 0) 
        {
            PlayerDied();
        }
    }
    private void PlayerDied()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    public void AddCardToDeck(CardData card)
    {
        playerDeck.Add(card);
    }

    public void UpgradeCard(CardData card)
    {
        int index = playerDeck.IndexOf(card);
        if (index != -1 && card.upgradedVersion != null)
            playerDeck[index] = card.upgradedVersion;
    }

    public void AddBuff (PlayerBuffType buff)
    {
        if (!activeBuffs.Contains(buff))
            activeBuffs.Add(buff);
    }

    public bool HasBuff(PlayerBuffType buff)
    {
        return activeBuffs.Contains(buff);
    }
    public void CompleteCurrentNode()
    {
        if (currentNode != null)
            currentNode.isCompleted = true;
    }
    public void ResetGame()
    {
        playerHP = playerMaxHP;
        playerDeck.Clear();
        activeBuffs.Clear();
        currentNode = null;
        LoadMap();
    }
}