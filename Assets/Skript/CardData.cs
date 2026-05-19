using UnityEngine;

public enum CardType
{
    Attack,
    Block,
    DaringAttack,
    Poison,
    Stun
}

[CreateAssetMenu(fileName = "NewCard", menuName = "Cards/Card")]
public class CardData : ScriptableObject
{
    [Header("Основное")]
    public string cardName;
    public int cost;
    public CardType cardType;
    public int value;

    [Header("Улучшение")]
    public CardData upgradedVersion;
    public bool isUpgraded;
}
