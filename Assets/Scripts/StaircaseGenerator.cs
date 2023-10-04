using UnityEngine;

public class StaircaseGenerator : MonoBehaviour
{
    [SerializeField] private GameObject stairEnd;
    [SerializeField] private GameObject waypoint;

    [SerializeField] [Min(2)] private int size;
    private enum StairDirection { left, right, up }
    [SerializeField] private StairDirection stairDirection = StairDirection.right;

    private Vector3 pos;

    private void Awake()
    {
        pos = transform.position;

        for (int i = 0; i < size; i++)
        {
            if (i == 0)
            {
                GameObject _stairEnd = Instantiate(stairEnd, pos, Quaternion.identity);

                if (stairDirection == StairDirection.right)
                {
                    _stairEnd.gameObject.name = "LeftEnd";
                } 
                else if (stairDirection == StairDirection.left)
                {
                    _stairEnd.gameObject.name = "RightEnd";
                }
                else
                {
                    _stairEnd.gameObject.name = "End";
                }
                _stairEnd.gameObject.tag = "StairBottom";
                _stairEnd.transform.parent = gameObject.transform;
            }
            else if (i == size - 1)
            {
                GameObject _stairEnd = Instantiate(stairEnd, pos, Quaternion.identity);

                if (stairDirection == StairDirection.right)
                {
                    _stairEnd.gameObject.name = "RightEnd";
                }
                else if (stairDirection == StairDirection.left)
                {
                    _stairEnd.gameObject.name = "LeftEnd";
                }
                else
                {
                    _stairEnd.gameObject.name = "End";
                }
                _stairEnd.gameObject.tag = "StairTop";
                _stairEnd.transform.parent = gameObject.transform;
            }
            else
            {
                GameObject _waypoint = Instantiate(waypoint, pos, Quaternion.identity);
                _waypoint.gameObject.tag = "StairTop";
                _waypoint.transform.parent = gameObject.transform;
            }

            pos += new Vector3(0, 1f, 0);
            if (stairDirection == StairDirection.right)
            {
                pos += new Vector3(1f, 0, 0);
            }
            else if (stairDirection == StairDirection.left)
            {
                pos += new Vector3(-1f, 0, 0);
            }
        }
    }
}
