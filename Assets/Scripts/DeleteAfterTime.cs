using UnityEngine;

public class DeleteAfterTime : MonoBehaviour
{
    [SerializeField] private float deleteTime;
    private float time = 0;

    private void Update()
    {
        time += Time.deltaTime;

        if (time > deleteTime)
        {
            Destroy(gameObject);
        }
    }
}
