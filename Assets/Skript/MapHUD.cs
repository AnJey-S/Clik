using UnityEngine;
using TMPro;

public class MapHUD : MonoBehaviour
{
    // ----------------------------------------------------------------
    //
    // Этот класс отвечает за отображение информации о здоровье игрока на экране во время игры.
    // ----------------------------------------------------------------
    // Поля:
    // - hpText: Ссылка на текстовый элемент TMP_Text, который отображает текущее здоровье игрока и его максимальное здоровье.
    // Методы:
    // - Update(): Этот метод вызывается каждый кадр и обновляет текст hpText, отображая текущее здоровье игрока и его максимальное здоровье. Он получает эти значения из GameManager.Instance.playerHP и GameManager.Instance.playerMaxHP соответственно.
    // ----------------------------------------------------------------
    [SerializeField] private TMP_Text hpText;

    private void Update()
    {
        if (GameManager.Instance != null)
            hpText.text = $"HP: {GameManager.Instance.playerHP} / {GameManager.Instance.playerMaxHP}";
    }
}