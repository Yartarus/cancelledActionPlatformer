using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!Session.singleStageRun)
            {
                collision.GetComponent<PlayerCondition>().SaveCondition();
            }
            gameManager.EndLevel();
        }
    }
}
