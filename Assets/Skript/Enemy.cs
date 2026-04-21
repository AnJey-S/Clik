using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameManager gameManager;
    private int health = 100;
    public int poisonedTime = 0;
    public int stunTime = 0;

    public int Health { get => health; set => health = value; }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("ХП врага: " + health);
    }
    public void Attack(Player player)
    {
        player.TakeDamage(5);
    }
    public void poisoned(int time)
    {
        TakeDamage(time);

    }
    public void poisonTime()
    {
        poisonedTime += 5;
    }

    public void stunned(int time)
    {
        stunTime = 2;

    }
    public void Death()
    {
        Debug.Log("Он умер");
        Destroy(gameObject);
        gameManager.EnterRewardState();
    }


}
