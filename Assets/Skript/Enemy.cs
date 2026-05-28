using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class Enemy : MonoBehaviour
{
    // ----------------------------------------------------------------
    //
    // Этот класс представляет врага в бою. Он хранит данные о враге, его здоровье, текущие эффекты (отравление, оглушение) и намерение на следующий ход.
    // Он выбирает намерение в начале каждого хода и выполняет его, нанося урон игроку, блокируя, усиливая себя или отравляя игрока. Он также обрабатывает получение урона и смерть врага.
    //
    // ----------------------------------------------------------------
    // Поля:
    // - data: Данные врага, включая его характеристики и возможные намерения.
    // - player: Ссылка на игрока, чтобы враг мог взаимодействовать с ним.
    // - gameManager: Ссылка на GameManager для управления состоянием игры.
    // - health: Текущее здоровье врага.
    // - poisonedTime: Количество ходов, в течение которых враг отравлен.
    // - stunTime: Количество ходов, в течение которых враг оглушен.
    // - damageBonus: Дополнительный урон, который враг наносит из-за баффов.
    // - currentIntention: Текущее намерение врага на следующий ход.
    // - UIIcons: Коллекция (атлас) иконок с намерениями врага.
    // - IntentionIcon: Ссылка на иконку текущего намерения в интерфейсе.
    // - IntentionIconExtra: Ссылка на дополнительную иконку текущего намерения в интерфейсе (для двойной атаки).
    // Методы:
    // - Initialize(EnemyData enemyData, Player playerRef): Инициализирует врага с данными и ссылкой на игрока.
    // - ChooseIntention(): Выбирает случайное намерение из возможных.
    // - UpdateIntentionText(): Обновляет текст и иконку, отображающие намерение врага.
    // - ExecuteIntention(Player player): Выполняет текущее намерение, взаимодействуя с игроком.
    // - TakeDamage(int damage): Наносит урон врагу и проверяет его здоровье.
    // - AddPoison(int stacks): Добавляет отравление врагу.
    // - Stun(): Оглушает врага на 2 хода.
    // - Death(): Обрабатывает смерть врага, уведомляя GameManager и уничтожая объект.
    // ----------------------------------------------------------------
    public EnemyData data;
    private Player player;

    public GameManager gameManager;
    private int health;
    public int poisonedTime = 0;
    public int stunTime = 0;
    private int damageBonus = 0;
    private int enemyBlock = 0;
    private EnemyIntention currentIntention;

    [SerializeField] private TMP_Text intentionText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text blockText;

    public SpriteAtlas UIIcons;
    public Image intentionIcon;
    public Image intentionIconExtra;
    private int scaledAttackDamage;
    private int scaledDoubleAttackDamage;
    private int scaledBuffDamageBonus;
    public int Health => health;
    public int EnemyBlock => enemyBlock;
    public void Initialize(EnemyData enemyData, Player playerRef)
    {
        data = enemyData;
        player = playerRef;

        float mult = GameManager.Instance.GetEndlessMultiplier();
        health = Mathf.RoundToInt(data.maxHP * mult);
        scaledAttackDamage = Mathf.RoundToInt(data.attackDamage * mult);
        scaledDoubleAttackDamage = Mathf.RoundToInt(data.doubleAttackDamage * mult);
        scaledBuffDamageBonus = Mathf.RoundToInt(data.buffDamageBonus * mult);

        GetComponent<SpriteRenderer>().sprite = data.sprite;
        transform.localScale = Vector3.one * enemyData.scale;
        healthText.text = health.ToString();
        blockText.text = "0";
        ChooseIntention();
    }

    public void ChooseIntention()
    {
        if (data.possibleIntensions.Count == 0) return;
        int index = Random.Range(0, data.possibleIntensions.Count);
        currentIntention = data.possibleIntensions[index];
        UpdateIntentionText();
    }

    private void UpdateIntentionText()
    {
        Sprite emptyIntention = UIIcons.GetSprite("blank");
        Sprite attackIntention = UIIcons.GetSprite("attack_intent_0");
        Sprite blockIntention = UIIcons.GetSprite("block_intent");
        Sprite buffIntention = UIIcons.GetSprite("buffself_intent");
        Sprite poisonIntention = UIIcons.GetSprite("poison_intent");

        switch (currentIntention)
        {
            case EnemyIntention.Attack:
                //intentionText.text = $"⚔ {data.attackDamage + damageBonus}";
                intentionText.text = $"{scaledAttackDamage + damageBonus}";
                intentionIcon.sprite = attackIntention;
                intentionIconExtra.sprite = emptyIntention;
                break;
            case EnemyIntention.DoubleAttack:
                //intentionText.text = $"⚔⚔ {data.doubleAttackDamage + damageBonus}x2";
                intentionText.text = $"{scaledDoubleAttackDamage + damageBonus}x2";
                intentionIcon.sprite = attackIntention;
                intentionIconExtra.sprite = attackIntention;
                break;
            case EnemyIntention.Block:
                //intentionText.text = "🛡 Block";
                intentionText.text = "Block";
                intentionIcon.sprite = blockIntention;
                intentionIconExtra.sprite = emptyIntention;
                break;
            case EnemyIntention.BuffSelf:
                //intentionText.text = "⬆ Buff";
                intentionText.text = "Buff";
                intentionIcon.sprite = buffIntention;
                intentionIconExtra.sprite = emptyIntention;
                break;
            case EnemyIntention.PoisonPlayer:
                //intentionText.text = "☠ Poison";
                intentionText.text = "Poison";
                intentionIcon.sprite = poisonIntention;
                intentionIconExtra.sprite = emptyIntention;
                break;
            default:
                intentionText.text = "";
                intentionIcon.sprite = emptyIntention;
                intentionIconExtra.sprite = emptyIntention;
                break;
        }
    }

    public void ExecuteIntention(Player player)
    {
        if (stunTime > 0)
        {
            stunTime--;
            ChooseIntention();
            return;
        }

        switch (currentIntention)
        {
            case EnemyIntention.Attack:
                player.TakeDamage(scaledAttackDamage + damageBonus);
                if (GameManager.Instance.HasBuff(PlayerBuffType.Thorns))
                    TakeDamage(2);
                break;
            case EnemyIntention.DoubleAttack:
                player.TakeDamage(scaledDoubleAttackDamage + damageBonus);
                if (GameManager.Instance.HasBuff(PlayerBuffType.Thorns))
                    TakeDamage(2);
                player.TakeDamage(scaledDoubleAttackDamage + damageBonus);
                if (GameManager.Instance.HasBuff(PlayerBuffType.Thorns))
                    TakeDamage(2);
                break;
            case EnemyIntention.Block:
                enemyBlock += 8;
                blockText.text = enemyBlock.ToString();
                poisonedTime = Mathf.Max(0, poisonedTime - 1);
                break;
            case EnemyIntention.BuffSelf:
                damageBonus += scaledBuffDamageBonus;
                break;
            case EnemyIntention.PoisonPlayer:
                player.AddPoison(4);
                break;
        }

        if (poisonedTime > 0)
        {
            TakeDamage(poisonedTime);
            poisonedTime--;
        }

        ChooseIntention();
    }

    public void TakeDamage(int damage)
    {
        int finalDamage = Mathf.Max(0, damage - enemyBlock);
        enemyBlock = Mathf.Max(0, enemyBlock - damage);
        health -= finalDamage;
        healthText.text = health.ToString();
        blockText.text = enemyBlock.ToString();
        if (health <= 0)
            Death();
    }

    public void AddPoison(int stacks)
    {
        poisonedTime += stacks;
    }

    public void ResetBlock()
    {
        enemyBlock = 0;
        blockText.text = "";
    }

    public void Stun()
    {
        stunTime = 2;

    }
    public void Death()
    {
        GameManager.Instance.CompleteCurrentNode();

        if (GameManager.Instance.currentNode.roomType == RoomType.Boss)
            GameManager.Instance.LoadVictory();
        else
            GameManager.Instance.LoadReward();

        Destroy(gameObject);
    }


}
