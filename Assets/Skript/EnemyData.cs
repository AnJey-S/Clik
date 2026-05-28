using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Enemies/Enemy")]
public class EnemyData : ScriptableObject
{
    public GameObject prefab;
    [Header("Основное")]
    public string enemyName;
    public int maxHP;
    public Sprite sprite;

    [Header("Атака")]
    public int attackDamage;
    public int doubleAttackDamage;

    [Header("Намерения")]
    public List<EnemyIntention> possibleIntensions;

    [Header("Баффы")]
    public int buffDamageBonus;

    [Header("Блок")]
    public int blockAmount;
}
