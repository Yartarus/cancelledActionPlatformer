using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public GameManager gameManager;

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    private PlayerCondition playerCondition;
    private PlayerAttack playerAttack;
    private PlayerStaircaseMovement playerStaircaseMovement;

    [SerializeField] private LayerMask jumpableGround;

    public float dirX = 0;
    public float dirY = 0;
    private float airX = 0;
    private float airTime = 0;
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float maxSpeed = 25;

    public bool canTurn = true;

    private bool wasGrounded = true;
    public bool crouching = false;
    public bool onStairs = false;
    public bool hurt = false;
    private bool dead = false;

    public bool cutsceneMode = false;
    private float cutsceneX = 0;
    private float cutsceneY = 0;

    private enum MovementState { idle, crouching, walking, ascending, descending, hurt }

    private void Awake()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        playerCondition = GetComponent<PlayerCondition>();
        playerAttack = GetComponent<PlayerAttack>();
        playerStaircaseMovement = GetComponent<PlayerStaircaseMovement>();
    }

    private void Update()
    {
        if (dead || PauseMenu.gameIsPaused)
        {
            return;
        }
        if (PauseMenu.playerControlsDisabled)
        {
            PauseMenu.playerControlsDisabled = false;
        }

        if (IsGrounded() && !playerAttack.inAttackAnimation && !crouching && !hurt)
        {
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        } 
        else if (IsGrounded() && (playerAttack.inAttackAnimation || crouching))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(airX * moveSpeed, rb.velocity.y);
        }

        if (!IsGrounded() && !onStairs)
        {
            airTime += Time.deltaTime;
        }

        if (IsGrounded() != wasGrounded)
        {
            airTime = 0;
            if (!hurt)
            {
                airX = dirX;
            }
            else if (hurt && IsGrounded())
            {
                hurt = false;
                if (playerCondition.health <= 0)
                {
                    Die();
                }
            }
        }
        wasGrounded = IsGrounded();

        if (dirY < 0 && !crouching && IsGrounded() && !playerAttack.inAttackAnimation && !playerStaircaseMovement.atStairTop && !cutsceneMode)
        {
            Crouch();
        }
        else if (dirY >= 0 && crouching && !playerAttack.inAttackAnimation || !IsGrounded() && !playerAttack.inAttackAnimation)
        {
            UnCrouch();
        }

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

        UpdateAnimationState();
    }

    public void SetCutsceneMode(float x, float y)
    {
        cutsceneMode = true;
        cutsceneX = x;
        cutsceneY = y;
        dirX = cutsceneX;
        dirY = cutsceneY;
    }

    public void EndCutsceneMode()
    {
        cutsceneMode = false;
        dirX = 0;
        dirY = 0;
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        if (dead || PauseMenu.playerControlsDisabled && !cutsceneMode)
        {
            dirX = 0;
            dirY = 0;
            return;
        }
        if (cutsceneMode)
        {
            dirX = cutsceneX;
            dirY = cutsceneY;
            return;
        }
        dirX = Mathf.RoundToInt(value.ReadValue<Vector2>().x);
        dirY = Mathf.RoundToInt(value.ReadValue<Vector2>().y);
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (dead || PauseMenu.playerControlsDisabled || cutsceneMode)
        {
            return;
        }
        if (ctx.started && IsGrounded() && !playerAttack.inAttackAnimation ||
            ctx.started && onStairs && !playerAttack.inAttackAnimation)
        {
            if (onStairs)
            {
                playerStaircaseMovement.UnMountStaircase();
            }
            airX = dirX;
            rb.velocity = new Vector2(airX * moveSpeed, jumpForce);
            FlipSprite();
        }
    }

    public void Knockback()
    {
        hurt = true;
        if (playerAttack.inAttackAnimation)
        {
            playerAttack.EndAttackAnimation();
        }
        if (onStairs)
        {
            playerStaircaseMovement.UnMountStaircase();
        }
        if (dirX != 0)
        {
            airX = -dirX;
        }
        else
        {
            airX = -CheckSpriteFlip();
        }
        rb.velocity = new Vector2(airX * moveSpeed, jumpForce / 3 * 2);
        FlipSprite();
    }

    private void Crouch()
    {
        crouching = true;

        coll.offset = new Vector2(0f, -0.6875f);
        coll.size = new Vector2(1.5f, 2.625f);
    }

    private void UnCrouch()
    {
        crouching = false;

        coll.offset = new Vector2(0f, -0.375f);
        coll.size = new Vector2(1.5f, 3.25f);
    }

    private void Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death");
        dead = true;
        gameManager.SetGameOver();
    }

    private void UpdateAnimationState()
    {
        MovementState state = MovementState.idle;

        if (canTurn && IsGrounded() || canTurn && onStairs || canTurn && playerAttack.inAttackAnimation)
        {
            FlipSprite();
        }

        if (IsGrounded())
        {
            if (dirX != 0f && !crouching)
            {
                state = MovementState.walking;
            }
            else
            {
                state = MovementState.idle;
            }
        }

        if (onStairs)
        {
            if (playerStaircaseMovement.stairDirection == PlayerStaircaseMovement.StairDirection.right)
            {
                if (CheckSpriteFlip() == 1)
                {
                    state = MovementState.ascending;
                } 
                else
                {
                    state = MovementState.descending;
                }
            }
            else if (playerStaircaseMovement.stairDirection == PlayerStaircaseMovement.StairDirection.left)
            {
                if (CheckSpriteFlip() == 1)
                {
                    state = MovementState.descending;
                }
                else
                {
                    state = MovementState.ascending;
                }
            }

            if (playerStaircaseMovement.atFinalWaypoint && !playerStaircaseMovement.movingOnStairs)
            {
                state = MovementState.idle;
            }
        }

        if (!IsGrounded() && !onStairs)
        {
            if (hurt)
            {
                state = MovementState.hurt;
            }
            else if (rb.velocity.y > 4.5f)
            {
                state = MovementState.ascending;
            }
            else if (rb.velocity.y < -5.5f)
            {
                state = MovementState.descending;
            }
            else if (airTime > 0.04f)
            {
                state = MovementState.crouching;
            }
        }

        anim.SetBool("grounded", IsGrounded());
        anim.SetBool("onStairs", onStairs);
        anim.SetBool("movingOnStairs", playerStaircaseMovement.movingOnStairs);
        anim.SetBool("crouching", crouching);
        anim.SetInteger("state", (int)state);
    }

    private void FlipSprite()
    {
        if (playerAttack.inAttackAnimation)
        {
            canTurn = false;
        }

        if (dirX > 0f && !onStairs || onStairs && playerStaircaseMovement.stairMovementDirY > 0f)
        {
            sprite.flipX = false;
            if (onStairs && playerStaircaseMovement.stairDirection == PlayerStaircaseMovement.StairDirection.left)
            {
                sprite.flipX = true;
            }
        }
        else if (dirX < 0f && !onStairs || onStairs && playerStaircaseMovement.stairMovementDirY < 0f)
        {
            sprite.flipX = true;
            if (onStairs && playerStaircaseMovement.stairDirection == PlayerStaircaseMovement.StairDirection.left)
            {
                sprite.flipX = false;
            }
        }
    }

    public float CheckSpriteFlip()
    {
        if (sprite.flipX)
        {
            return -1f;
        }
        return 1f;
    }

    public bool IsGrounded()
    {
        if (onStairs || rb.velocity.y > 0.01f || rb.velocity.y < -0.01f) {
            return false;
        }
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
