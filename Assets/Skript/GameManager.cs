using UnityEngine;
using UnityEngine.Android;

public class GameManager : MonoBehaviour
{
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
        ShowRewards();
        rewardManager.GenerateRewards();
    }
    public CanvasGroup rewardPanel;
    void Start()
    {
        rewardPanel.alpha = 0;
        rewardPanel.interactable = false;
        rewardPanel.blocksRaycasts = false;
    }
    public void ShowRewards()
    {
        rewardPanel.alpha = 1;
        rewardPanel.interactable = true;
        rewardPanel.blocksRaycasts = true;
    }
}