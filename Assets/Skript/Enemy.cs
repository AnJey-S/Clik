using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 50;
    
    public void TakeDamage(int damage) 
    {
        health -= damage;
        Debug.Log("ХП врага: " + health);
    }
    public void Attak(Player player)
    {
        player.TakeDamage(5);
    }
}
