using UnityEngine;

public class Player : MonoBehaviour
{
    private int health = 50;
    private int block;

    public int Health { get => health; set => health = value; }
    public int Block { get => block; set => block = value; }

    public void GainBlock(int amount)
    {
        block += amount;
    }

    public void TakeDamage(int damage)
    {
        int finalDamage = Mathf.Max(0, damage - block);
        block = Mathf.Max(0, block - damage);
        health -= finalDamage;
        Debug.Log("ХП игрока: " + health);
    }

    public void Death()
    {
        Debug.Log("Упс! Вы умерли");
        Destroy(gameObject);
        // открыть какое-нибудь окно поражения
    }
}