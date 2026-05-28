using UnityEngine;
public class BattleManager : MonoBehaviour
{
    // ----------------------------------------------------------------
    //
    // Этот класс управляет всем процессом боя: от создания врага и управления колодой до обработки ходов игрока и врага.
    // Он взаимодействует с DeckManager для управления картами, TurnManager для контроля ходов и BattleUI для отображения информации на экране.
    //
    // ----------------------------------------------------------------
    // Методы:
    //    - PlayCard(Card card): Этот метод вызывается, когда игрок играет карту.
    // Он проверяет, достаточно ли энергии для использования карты, и если да, то применяет эффект карты к врагу и обновляет состояние боя.
    //    - EndTurn(): Этот метод вызывается, когда игрок заканчивает свой ход. Он передает управление TurnManager для выполнения действий врага и начала следующего хода игрока.
    //    - Start(): Этот метод вызывается при начале боя. Он создаёт врага на основе данных текущего узла карты, инициализирует все менеджеры и запускает бой.
    // Поля:
    //    - player:             Ссылка на объект игрока, который участвует в бою.
    //    - enemyPrefab:        Префаб врага, который будет создан в начале боя.
    //    - enemySpawnPoint:    Точка на сцене, где будет создан враг.
    //    - deckManager:        Ссылка на DeckManager для управления картами игрока.
    //    - turnManager:        Ссылка на TurnManager для управления ходами в бою.
    //    - battleUI:           Ссылка на BattleUI для отображения информации о бое.
    //    - energy:             Текущее количество энергии игрока, которое используется для игры картами.
    //    - energyWarning:      Флаг, указывающий, что игрок попытался сыграть карту, но у него не хватило энергии.
    //    - enemy:              Ссылка на текущего врага, с которым игрок сражается.
    //
    // -----------------------------------------------------------
    [Header("Компоненты")]
    private Player player;
    [SerializeField] private Transform enemySpawnPoint;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawnPoint;
    [Header("Менеджеры")]
    private DeckManager deckManager;
    private TurnManager turnManager;
    private BattleUI battleUI;

    private int energy = 3;
    public bool energyWarning = false;

    private Enemy enemy;

    public int Energy { get => energy; set => energy = value; }

    private void Start()
    {
        deckManager = GetComponent<DeckManager>();
        turnManager = GetComponent<TurnManager>();
        battleUI = GetComponent<BattleUI>();

        // Спавним игрока
        GameObject playerObj = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
        player = playerObj.GetComponent<Player>();
        player.Initialize(this); 

        // Спавним врага
        EnemyData enemyData = GameManager.Instance.currentNode.enemyData;
        GameObject enemyObj = Instantiate(enemyData.prefab, enemySpawnPoint.position, Quaternion.identity);
        enemy = enemyObj.GetComponent<Enemy>();
        enemy.Initialize(enemyData, player);

        deckManager.Initialize(this);
        turnManager.Initialize(this, deckManager, player, enemy);
        battleUI.Initialize(this, player, enemy);

        turnManager.StartBattle();
    }

    public void PlayCard(Card card)
    {
        energyWarning = false;

        if (turnManager.currentState != TurnManager.TurnState.PlayerTurn) return;
        if (deckManager.hand.Count == 0) return;
        if (energy < card.data.cost)
        {
            energyWarning = true;
            return;
        }

        energy -= card.data.cost;
        card.Use(player, enemy);
        deckManager.DiscardCard(card);

        // Сначала проверяем смерть врага — если умер, выходим
        if (turnManager.CheckEnemyDeath()) return;

        turnManager.CheckPlayerDeath();

        if (energy == 0)
            turnManager.EndPlayerTurn();
    }

    public void EndTurn()
    {
        turnManager.EndPlayerTurn();
    }
}
