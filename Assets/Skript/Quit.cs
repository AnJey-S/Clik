using UnityEngine;
using UnityEngine.UI;

public class Quit : MonoBehaviour
{
    // ----------------------------------------------------------------
    //
    // Этот класс отвечает за выход из игры.
    //
    // ----------------------------------------------------------------
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(QuitGame);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
