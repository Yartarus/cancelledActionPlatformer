using UnityEngine;

public class Enemy_Basic : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerCondition>().TakeDamage(damage);
        }
    }
}
