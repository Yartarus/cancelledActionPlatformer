using UnityEngine;

public class StartVelocity : MonoBehaviour
{
    private Rigidbody2D rb;

    public float xVelocity;
    public float yVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.velocity = new Vector2(xVelocity, yVelocity);
    }
}
