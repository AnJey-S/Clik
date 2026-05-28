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
        int attackBonus = GameManager.Instance.HasBuff(PlayerBuffType.BonusAttack) ? 3 : 0;

        switch (data.cardType)
        {
            case CardType.Attack:
                enemy.TakeDamage(data.value + attackBonus);
                break;
            case CardType.Block:
                player.GainBlock(data.value);
                break;
            case CardType.DaringAttack:
                enemy.TakeDamage(data.value + attackBonus);
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
