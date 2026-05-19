using TMPro;
using UnityEngine;

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
    [SerializeField] private TMP_Text playerEnergyText;
    [SerializeField] private TMP_Text energyWarningText;

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

        enemyHealthText.text = "HP: " + enemy.Health;
        playerHealthText.text = "HP: " + GameManager.Instance.playerHP;

        if (player.Block != 0)
            playerHealthText.text += " + " + player.Block;

        playerEnergyText.text = "Энергия: " + battleManager.energy;
        energyWarningText.text = battleManager.energyWarning ? "Не хватает энергии!" : "";
    }
}
