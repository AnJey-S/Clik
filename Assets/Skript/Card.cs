using UnityEngine;

public class Card
{
    public CardData data;

    public Card(CardData data)
    {
        this.data = data;
    }

    public void Use(Player player, Enemy enemy)
    {
        switch (data.cardType) // было data.type
        {
            case CardType.Attack:
                enemy.TakeDamage(data.value);
                break;
            case CardType.Block:
                player.GainBlock(data.value);
                break;
            case CardType.DaringAttack:
                enemy.TakeDamage(data.value);
                player.TakeDamage(5);
                break;
            case CardType.Poison:
                enemy.AddPoison(data.value);
                break;
            case CardType.Stun:
                enemy.Stun();
                break;
        }
    }
}
