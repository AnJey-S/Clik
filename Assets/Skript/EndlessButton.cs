using UnityEngine;
using UnityEngine.UI;

public class EndlessButton : MonoBehaviour
{
    public GameManager gameManager;
    void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
            btn.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        GameManager.Instance.StartEndlessMode();
    }
}