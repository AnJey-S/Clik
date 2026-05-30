using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class BattleUI : MonoBehaviour
{
    // ----------------------------------------------------------------
    //
    // Этот класс отвечает за отображение информации о бою на экране. Он показывает здоровье врага, здоровье игрока, текущую энергию и предупреждение о нехватке энергии.
    // Он обновляет эту информацию каждый кадр, чтобы игрок всегда видел актуальные данные о состоянии боя.
    //
    // ----------------------------------------------------------------
    // Поля:
    // - enemyHealthText: Текстовое поле для отображения здоровья врага.
    // - playerHealthText: Текстовое поле для отображения здоровья игрока.
    // - playerBlockText: Текстовое поле для отображения текущего блока игрока.
    // - playerEnergyText: Текстовое поле для отображения текущей энергии игрока.
    // - energyWarningText: Текстовое поле для отображения предупреждения о нехватке энергии.
    // - remainingPoisonText: Текстовое поле для отображения количества оставшихся ходов отравления.
    // - Effects: Атлас спрайтов для отображения различных эффектов (отравление, оглушение, бонус к урону и т.д.).
    // - remainingPoisonIcon: Иконка для отображения эффекта отравления.
    // Методы:
    // - Initialize(BattleManager bm, Player p, Enemy e): Этот метод инициализирует BattleUI, устанавливая ссылки на BattleManager, Player и Enemy.
    // - Update(): Этот метод вызывается каждый кадр и обновляет отображаемую информацию о здоровье врага, здоровье игрока, блоке, энергии и эффектах.
    // ----------------------------------------------------------------
    [SerializeField] private TMP_Text enemyHealthText;
    [SerializeField] private TMP_Text playerHealthText;
    [SerializeField] private TMP_Text playerBlockText;
    [SerializeField] private TMP_Text playerEnergyText;
    [SerializeField] private TMP_Text energyWarningText;

    [SerializeField] private TMP_Text remainingPoisonText;
    public SpriteAtlas Effects;
    public Image remainingPoisonIcon;

    private BattleManager battleManager;
    private Enemy enemy;
    private Player player;

    public void Initialize(BattleManager bm, Player p, Enemy e)
    {
        battleManager = bm;
        player = p;
        enemy = e;
    }

    private void Update()
    {
        if (enemy == null || player == null) return;

        // Характеристики игрока
        playerHealthText.text = (GameManager.Instance.playerHP > 0) ? "" + GameManager.Instance.playerHP : "0";
        playerBlockText.text = "" + player.Block;
        playerEnergyText.text = "" + battleManager.Energy;
        energyWarningText.text = battleManager.energyWarning ? "Не хватает энергии!" : "";

        Sprite emptyEffect = Effects.GetSprite("blank");
        Sprite poisonEffect = Effects.GetSprite("poison_intent");
        Sprite stunEffect = Effects.GetSprite("stun_0");
        Sprite damageBonusEffect = Effects.GetSprite("powers_112");

        // Эффекты игрока
        remainingPoisonText.text = (player.poisonedTime > 0) ? player.poisonedTime.ToString() : "";
        remainingPoisonIcon.sprite = (player.poisonedTime > 0) ? poisonEffect : emptyEffect;

        // Эффекты врага
        enemy.remainingPoisonText.text = (enemy.poisonedTime > 0) ? enemy.poisonedTime.ToString() : "";
        enemy.remainingPoisonIcon.sprite = (enemy.poisonedTime > 0) ? poisonEffect : emptyEffect;
        enemy.remainingStunText.text = (enemy.stunTime > 0) ? enemy.stunTime.ToString() : "";
        enemy.remainingStunIcon.sprite = (enemy.stunTime > 0) ? stunEffect : emptyEffect;
        enemy.damageBonusText.text = (enemy.damageBonus > 0) ? $"+{enemy.damageBonus}" : "";
        enemy.damageBonusIcon.sprite = (enemy.damageBonus > 0) ? damageBonusEffect : emptyEffect;
    }
}
