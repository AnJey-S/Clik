using UnityEngine;
using UnityEngine.UI;

public class EndlessButton : MonoBehaviour
{
    private void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
            btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        GameManager.Instance.StartEndlessMode();
    }
}