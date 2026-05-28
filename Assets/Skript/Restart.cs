using UnityEngine;
using UnityEngine.UI;

public class Restart : MonoBehaviour
{
    // ----------------------------------------------------------------
    //
    // Этот класс отвечает за перезапуск игры.
    //
    // ----------------------------------------------------------------
    public GameManager gameManager;

    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(RestartGame);
    }

    public void RestartGame()
    {
        GameManager.Instance.ResetGame();
    }
}
