using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 50;
    public int block;
    public void GainBlock (int amount)
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
}