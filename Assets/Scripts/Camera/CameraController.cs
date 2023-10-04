using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerPos;

    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;

    [SerializeField] private float speed;
    private Vector3 targetPos;
    private float targetXMin;
    private float targetXMax;
    private float targetYMin;
    private float targetYMax;

    private void Start()
    {
        GetBounds();

        targetPos = GetTargetPos();
        transform.position = targetPos;
    }

    private void Update()
    {
        targetPos = GetTargetPos();

        if (Vector2.Distance(transform.position, targetPos) < .75f)
        {
            transform.position = targetPos;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
    }

    private Vector3 GetTargetPos()
    {
        return new Vector3
        (
            Mathf.Clamp(playerPos.position.x, targetXMin, targetXMax),
            Mathf.Clamp(playerPos.position.y + 2, targetYMin, targetYMax),
            transform.position.z
        );
    }

    public void MoveToNewWaypoint(int _indexChange)
    {
        if (currentWaypointIndex + _indexChange > waypoints.Length - 2)
        {
            currentWaypointIndex = waypoints.Length - 2;
        }
        else if (currentWaypointIndex + _indexChange < 0)
        {
            currentWaypointIndex = 0;
        }
        else
        {
            currentWaypointIndex += _indexChange;
        }
        GetBounds();
    }

    public void SetWaypoint(int _index)
    {
        currentWaypointIndex = _index;
        GetBounds();
    }

    private void GetBounds()
    {
        if (waypoints[currentWaypointIndex].transform.position.x < waypoints[currentWaypointIndex + 1].transform.position.x)
        {
            targetXMin = waypoints[currentWaypointIndex].transform.position.x;
            targetXMax = waypoints[currentWaypointIndex + 1].transform.position.x;
        }
        else
        {
            targetXMin = waypoints[currentWaypointIndex + 1].transform.position.x;
            targetXMax = waypoints[currentWaypointIndex].transform.position.x;
        }

        if (waypoints[currentWaypointIndex].transform.position.y < waypoints[currentWaypointIndex + 1].transform.position.y)
        {
            targetYMin = waypoints[currentWaypointIndex].transform.position.y;
            targetYMax = waypoints[currentWaypointIndex + 1].transform.position.y;
        }
        else
        {
            targetYMin = waypoints[currentWaypointIndex + 1].transform.position.y;
            targetYMax = waypoints[currentWaypointIndex].transform.position.y;
        }
    }
}
