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
    [SerializeField] private TMP_Text playerBlockText;
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

        playerHealthText.text = (GameManager.Instance.playerHP > 0) ? "" + GameManager.Instance.playerHP : "0";
        playerBlockText.text = "" + player.Block;

        playerEnergyText.text = "" + battleManager.Energy;
        energyWarningText.text = battleManager.energyWarning ? "Не хватает энергии!" : "";
    }
}
