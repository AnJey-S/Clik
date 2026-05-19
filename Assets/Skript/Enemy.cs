using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // ----------------------------------------------------------------
    //
    // Этот класс представляет врага в бою. Он хранит данные о враге, его здоровье, текущие эффекты (отравление, оглушение) и намерение на следующий ход.
    // Он выбирает намерение в начале каждого хода и выполняет его, нанося урон игроку, блокируя, усиливая себя или отравляя игрока. Он также обрабатывает получение урона и смерть врага.
    //
    // ----------------------------------------------------------------
    public EnemyData data;
    private Player player;

    public GameManager gameManager;
    private int health;
    public int poisonedTime = 0;
    public int stunTime = 0;
    private int damageBonus = 0;
    private EnemyIntention currentIntention;

    [SerializeField] private TMP_Text intentionText;
    [SerializeField] private TMP_Text healthText;

    public int Health => health;

    public void Initialize(EnemyData enemyData, Player playerRef)
    {
        data = enemyData;
        player = playerRef;
        health = data.maxHP;
        GetComponent<SpriteRenderer>().sprite = data.sprite;
        //healthText.text = $"HP: {health}";
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
        switch (currentIntention)
        {
            case EnemyIntention.Attack:
                intentionText.text = $"⚔ {data.attackDamage + damageBonus}";
                break;
            case EnemyIntention.DoubleAttack:
                intentionText.text = $"⚔⚔ {data.doubleAttackDamage + damageBonus}x2";
                break;
            case EnemyIntention.Block:
                intentionText.text = "🛡 Block";
                break;
            case EnemyIntention.BuffSelf:
                intentionText.text = "⬆ Buff";
                break;
            case EnemyIntention.PoisonPlayer:
                intentionText.text = "☠ Poison";
                break;
        }
    }

    public void ExecuteIntention(Player player)
    {
        if (stunTime != 0)
        {
            stunTime--;
            ChooseIntention();
            return;
        }

        switch (currentIntention)
        {
            case EnemyIntention.Attack:
                player.TakeDamage(data.attackDamage + damageBonus);
                break;
            case EnemyIntention.DoubleAttack:
                player.TakeDamage(data.attackDamage + damageBonus);
                player.TakeDamage(data.attackDamage + damageBonus);
                break;
            case EnemyIntention.Block:
                poisonedTime = Mathf.Max(0, poisonedTime - 1);
                break;
            case EnemyIntention.BuffSelf:
                damageBonus += data.buffDamageBonus;
                break;
            case EnemyIntention.PoisonPlayer:
                player.AddPoison(2);
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
        health -= damage;
        healthText.text = $"HP: {health}";
        if (health <= 0)
            Death();
    }

    public void AddPoison(int stacks)
    {
        poisonedTime += stacks;
    }

    public void Stun()
    {
        stunTime = 2;

    }
    public void Death()
    {
        GameManager.Instance.CompleteCurrentNode();
        Destroy(gameObject);
        GameManager.Instance.LoadReward();
        Debug.Log("Он умер");
        Destroy(gameObject);
        
    }


}
