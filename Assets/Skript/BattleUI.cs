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
