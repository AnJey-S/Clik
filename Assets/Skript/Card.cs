using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public string cardName;
    public int cost;
    public abstract void Use(Player player, Enemy enemy);

    public class AttackCard : Card
    {

        public int damage;
        public override void Use(Player player, Enemy enemy)
        {
            enemy.TakeDamage(damage);
        }
        
    }
    public class BlockCard : Card
    {
        public int block;
        public override void Use(Player player, Enemy enemy)
        {
            player.GainBlock(block);
        }
    }


    
}
