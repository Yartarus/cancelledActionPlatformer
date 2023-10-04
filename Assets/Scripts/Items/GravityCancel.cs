using UnityEngine;

public class GravityCancel : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    /*[SerializeField] private LayerMask ground;

    [SerializeField] private float maxSpeed = 25;
    private bool wasGrounded = false;*/

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        //coll.isTrigger = true;
    }

    /*private void Update()
    {
        if (IsGrounded() != wasGrounded)
        {
            if (IsGrounded())
            {
                rb.gravityScale = 0f;
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
            else
            {
                rb.gravityScale = 2f;
            }
        }
        wasGrounded = IsGrounded();

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .07f, ground);
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player")
        {
            rb.bodyType = RigidbodyType2D.Static;
            coll.isTrigger = true;
            Destroy(this);
        }
    }
}
