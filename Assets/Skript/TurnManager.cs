using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour
{
    // ----------------------------------------------------------------
    //
    // Этот класс отвечает за управление ходами в бою. Он контролирует, чей сейчас ход (игрока или врага), и выполняет соответствующие действия в начале и конце каждого хода.
    // В начале хода игрока он восстанавливает энергию, сбрасывает блок и позволяет игроку тянуть карты. В конце хода игрока он сбрасывает руку и передает ход врагу, который выполняет свои действия в соответствии с выбранным намерением.
    //
    // ----------------------------------------------------------------
    // Методы:
    //    - Initialize(BattleManager bm, DeckManager dm, Player p, Enemy e): Этот метод инициализирует TurnManager, устанавливая ссылки на BattleManager, DeckManager, Player и Enemy.
    //    - StartBattle(): Этот метод запускает бой, устанавливая начальное состояние и начиная первый ход игрока.
    //    - StartPlayerTurn(): Этот метод выполняется в начале хода игрока. Он восстанавливает энергию, сбрасывает блок и позволяет игроку тянуть карты.
    //    - EndPlayerTurn(): Этот метод выполняется в конце хода игрока. Он сбрасывает руку и передает ход врагу, который выполняет свои действия.
    //    - ExecuteEnemyTurn(): Этот метод выполняется в начале хода врага. Он заставляет врага выполнить свое намерение и проверяет, не умер ли игрок от этого.
    //    - EndEnemyTurn(): Этот метод выполняется в конце хода врага. Он передает ход обратно игроку, начиная новый ход.
    //    - CheckPlayerDeath(): Этот метод проверяет, умер ли игрок, и если да, то загружает сцену "GameOverScene".
    //    - CheckEnemyDeath(): Этот метод проверяет, умер ли враг, и если да, то вызывает его метод Death().
    // Поля:
    //    - currentState: Текущее состояние хода, указывающее, чей сейчас ход (игрока или врага).
    //    - battleManager: Ссылка на BattleManager для доступа к информации о бою и управления им.
    //    - deckManager: Ссылка на DeckManager для управления картами игрока.
    //    - player: Ссылка на Player для управления состоянием игрока.
    //    - enemy: Ссылка на Enemy для управления состоянием врага.
    //
    // ----------------------------------------------------------------
    public enum TurnState { PlayerTurn, EnemyTurn }
    public TurnState currentState { get; private set; }

    private BattleManager battleManager;
    private DeckManager deckManager;
    private Player player;
    private Enemy enemy;

    public void Initialize(BattleManager bm, DeckManager dm, Player p, Enemy e)
    {
        battleManager = bm;
        deckManager = dm;
        player = p;
        enemy = e;
    }

    public void StartBattle()
    {
        currentState = TurnState.PlayerTurn;
        StartPlayerTurn();
    }

    
    public void StartPlayerTurn()
    {
        battleManager.Energy = GameManager.Instance.HasBuff(PlayerBuffType.ExtraEnergy) ? 4 : 3;
        player.Block = 0;

        int drawAmount = GameManager.Instance.HasBuff(PlayerBuffType.StartWithCard) ? 6 : 5;
        deckManager.DrawCards(drawAmount - deckManager.hand.Count);
    }

    public void EndPlayerTurn()
    {
        deckManager.DiscardHand();
        currentState = TurnState.EnemyTurn;
        ExecuteEnemyTurn();
    }

    private void ExecuteEnemyTurn()
    {
        enemy.ExecuteIntention(player);
        CheckPlayerDeath();
        EndEnemyTurn();
    }

    private void EndEnemyTurn()
    {
        currentState = TurnState.PlayerTurn;
        StartPlayerTurn();
    }

    public void CheckPlayerDeath()
    {
        if (GameManager.Instance.playerHP <= 0)
            SceneManager.LoadScene("GameOverScene");
    }

    public bool CheckEnemyDeath()
    {
        if (enemy.Health <= 0)
        {
            enemy.Death();
            return true;
        }
        return false;
    }
}