using UnityEngine;
using UnityEngine.Android;

public class GameManager : MonoBehaviour
{
    public CanvasGroup battlePanel;
    public RewardManager rewardManager;
    public GameState currentState;
    public enum GameState
    {
        Combat,
        Reward,
        Map
    }
    public void EnterRewardState()
    {
        currentState = GameState.Reward;
        SetPanelVisible(rewardPanel, true);
        rewardManager.GenerateRewards();
        SetPanelVisible(battlePanel, false);
    }
    public CanvasGroup rewardPanel;
    void Start()
    {
        rewardPanel.alpha = 0;
        rewardPanel.interactable = false;
        rewardPanel.blocksRaycasts = false;
    }
    private void SetRewardPanelVisible(bool visible)
    {
        rewardPanel.alpha = visible ? 1f : 0f;
        rewardPanel.interactable = visible;
        rewardPanel.blocksRaycasts = visible;
    }
    private void SetPanelVisible(CanvasGroup panel, bool visible)
    {
        panel.alpha = visible ? 1f : 0f;
        panel.interactable = visible;
        panel.blocksRaycasts = visible;
    }
    public void EnterMapState()
    {
        currentState = GameState.Map;
        SetPanelVisible(rewardPanel, false);
        SetPanelVisible(battlePanel, true);
    }
}