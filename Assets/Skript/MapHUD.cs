using UnityEngine;
using TMPro;

public class MapHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text hpText;

    private void Update()
    {
        if (GameManager.Instance != null)
            hpText.text = $"HP: {GameManager.Instance.playerHP} / {GameManager.Instance.playerMaxHP}";
    }
}