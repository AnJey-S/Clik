using UnityEngine;

public class Card
{
    // ----------------------------------------------------------------
    // Этот класс представляет карту в игре. Он содержит данные карты и метод Use, который выполняет действие карты
    // в зависимости от ее типа. Карты могут наносить урон врагу, давать блок игроку, наносить урон с отдачей, накладывать отравление или оглушать врага.
    // ----------------------------------------------------------------
    // Методы:
    // - Use(Player player, Enemy enemy): Этот метод выполняет действие карты в зависимости от ее типа. Он может наносить урон врагу, давать блок игроку, наносить урон с отдачей, накладывать отравление или оглушать врага. Он также учитывает бафф "BonusAttack", который увеличивает урон на 3.
    // ----------------------------------------------------------------
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
