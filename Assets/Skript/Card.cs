using JetBrains.Annotations;
using UnityEngine;

public abstract class Card
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
    public class DaringAttackCard : Card
    {
        public int damage = 12;

        public override void Use(Player player, Enemy enemy)
        {
            enemy.TakeDamage(damage);
            player.TakeDamage(2);
        }
    }
    public class potionCard : Card
    {
        public override void Use(Player player, Enemy enemy)
        {
            enemy.poisonTime();
        }
    }

    public class stunCard : Card
    {
        public override void Use(Player player, Enemy enemy)
        {
            enemy.stunned(2);
        }
    }


}
