using UnityEngine;

public class EndingTrigger : MonoBehaviour
{
    [SerializeField] EndingType endingToTrigger;
    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Entered ending area ");
        switch (endingToTrigger)
        {
            case EndingType.Bad:
                GameManager._instance.OnGameStateChanged(GameState.Lose);
                break;
            case EndingType.Good:
                GameManager._instance.OnGameStateChanged(GameState.Win);
                break;
            case EndingType.Secret:
                GameManager._instance.OnGameStateChanged(GameState.SecretWin);
                break;
            default:
                Debug.LogError("Invalid ending type: " + endingToTrigger);
                break;
        }      
    }
}
