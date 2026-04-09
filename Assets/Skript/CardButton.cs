using UnityEngine;

public class CardButton : MonoBehaviour
{
    public BattleManager battleManager;
    private Card.AttackCard card;
    void Start()
    {

    }
    private void OnMouseDown()
    {
        battleManager.PlayCard(card);
    }

}
