using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private int checkpointNumber;
    [SerializeField] private int cameraWaypointIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (GameManager.currentCheckpointIndex < checkpointNumber)
            {
                GameManager.currentCheckpointIndex = checkpointNumber;
                GameManager.currentCheckpointCameraWaypointIndex = cameraWaypointIndex;
            }
        }
    }
}
