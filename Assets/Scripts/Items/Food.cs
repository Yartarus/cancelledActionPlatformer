using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private float healthValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Collect(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Collect(collision.gameObject);
        }
    }

    private void Collect(GameObject _collision)
    {
        _collision.GetComponent<PlayerCondition>().AddHealth(healthValue);
        Destroy(gameObject);
    }
}
