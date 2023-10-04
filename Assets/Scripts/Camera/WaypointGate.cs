using UnityEngine;

public class WaypointGate : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;

    [SerializeField] private int increment = 1;
    [SerializeField] private bool horizontal = true;

    private enum EntrySide { left, right, top, bottom }
    private EntrySide entrySide;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (horizontal)
            {
                if (collision.transform.position.x < transform.position.x)
                {
                    entrySide = EntrySide.left;
                }
                else
                {
                    entrySide = EntrySide.right;
                }
            }
            else
            {
                if (collision.transform.position.y < transform.position.y)
                {
                    entrySide = EntrySide.bottom;
                }
                else
                {
                    entrySide = EntrySide.top;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.transform.position.x < transform.position.x && entrySide == EntrySide.right ||
                collision.transform.position.y < transform.position.y && entrySide == EntrySide.top)
            {
                cameraController.MoveToNewWaypoint(-increment);
            }
            else if (collision.transform.position.x > transform.position.x && entrySide == EntrySide.left ||
                collision.transform.position.y > transform.position.y && entrySide == EntrySide.bottom)
            {
                cameraController.MoveToNewWaypoint(increment);
            }
        }
    }
}
