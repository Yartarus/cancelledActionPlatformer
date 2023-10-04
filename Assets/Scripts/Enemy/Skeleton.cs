using UnityEngine;

public class Skeleton : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;

    private Agro agro;

    [SerializeField] private float targetDirX;
    private float dirX = 0;

    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private int stuckCounterMax = 2;
    private int stuckCounter = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        agro = GetComponentInChildren<Agro>();
    }

    private void Update()
    {
        if (agro.isAgrod)
        {
            if (rb.velocity.x == 0)
            {
                stuckCounter++;
                if (stuckCounter > stuckCounterMax)
                {
                    targetDirX = -targetDirX;
                }
            }
            else
            {
                stuckCounter = 0;
            }

            dirX = targetDirX;
        } 
        else
        {
            dirX = 0;
        }

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        FlipSprite();

        if (dirX != 0f)
        {
            anim.SetBool("walking", true);
        }
        else
        {
            anim.SetBool("walking", false);
        }
    }

    private void FlipSprite()
    {
        if (dirX > 0f)
        {
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            sprite.flipX = true;
        }
    }
}
