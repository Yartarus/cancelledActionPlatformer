using System;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed;
    private float direction;
    private int subweapon;
    private bool piercing;
    private bool recoverable;

    private float lifetime;
    private float lifespan;
    private float damage = 1f;
    private float iFrames = 0.75f;

    private List<Collider2D> hitTargets = new List<Collider2D>();
    private List<float> enemyInvincibility = new List<float>();

    private BoxCollider2D coll;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (hitTargets.Count != 0)
        {
            for (int i = hitTargets.Count - 1; i >= 0; --i)
            {
                if (enemyInvincibility[i] <= lifetime)
                {
                    enemyInvincibility[i] += iFrames;
                    hitTargets[i].GetComponent<EnemyHealth>().TakeDamage(damage);
                }
            }
        }

        lifetime += Time.deltaTime;

        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        if (subweapon == 0 && lifetime > 0.125f)
        {
            speed += -5f * Time.deltaTime;
            rb.mass = 2f;
            rb.gravityScale += 5f * Time.deltaTime;
        } 
        else if (subweapon == 2)
        {
            transform.Translate(0, -speed * 0.75f * Time.deltaTime, 0);
        }
        else if (subweapon == 3)
        {
            if (lifetime > 1.8f)
            {
                speed = -10;
                rb.gravityScale = -.075f;
            }
            else if (lifetime > 0.8f)
            {
                speed += -20f * Time.deltaTime;
                rb.mass = 1f;
                rb.gravityScale = .225f;
                recoverable = true;
            }
        }

        if (lifetime > lifespan)
        {
            DeactivateProjectile();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !recoverable || 
            collision.tag != "Ground" && collision.tag != "Player" && 
            collision.tag != "Enemy")
        {
            return;
        }
        if (collision.tag == "Ground" && subweapon == 2)
        {
            speed = 0;
            anim.enabled = false;
            return;
        }

        if (collision.tag == "Enemy")
        {
            if (piercing)
            {
                hitTargets.Add(collision);
                enemyInvincibility.Add(lifetime);
            }
            else
            {
                collision.GetComponent<EnemyHealth>().TakeDamage(damage);

                DeactivateProjectile();
            }
        }


        if (collision.tag == "Ground" || collision.tag == "Enemy" && !piercing || collision.tag == "Player" && recoverable)
        {
            DeactivateProjectile();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && hitTargets.Count != 0)
        {
            int hitIndex = hitTargets.IndexOf(collision);
            hitTargets.RemoveAt(hitIndex);
            enemyInvincibility.RemoveAt(hitIndex);
        }
    }

    private void DeactivateProjectile()
    {
        hitTargets.Clear();
        enemyInvincibility.Clear();
        coll.enabled = false;
        gameObject.SetActive(false);
    }

    public void SetDirection(float _direction, int _subweapon)
    {
        lifetime = 0;
        recoverable = false;
        direction = _direction;
        subweapon = _subweapon;

        gameObject.SetActive(true);
        anim.enabled = true;
        anim.SetInteger("subWeapon", subweapon);
        coll.enabled = true;

        if (direction < 0f)
        {
            sprite.flipX = true;
        } 
        else
        {
            sprite.flipX = false;
        }

        SetProperties();
    }

    private void SetProperties()
    {
        switch (subweapon)
        {
            case 0:
                speed = 10;
                piercing = false;
                lifespan = 5;
                damage = 1f;

                rb.mass = 1f;
                rb.gravityScale = 0f;

                coll.offset = new Vector2(0f, -0.0625f);
                coll.size = new Vector2(1f, 0.625f);
                break;
            case 1:
                speed = 12;
                piercing = false;
                lifespan = 5;
                damage = 2f;

                rb.mass = 1f;
                rb.gravityScale = 0f;

                coll.offset = new Vector2(0f, -0.0625f);
                coll.size = new Vector2(1f, 0.625f);
                break;
            case 2:
                speed = 10;
                piercing = true;
                lifespan = 2f;
                damage = 3f;
                iFrames = 0.5f;

                rb.mass = 1f;
                rb.gravityScale = 0f;

                coll.offset = new Vector2(0f, 0f);
                coll.size = new Vector2(1.5f, 1.5f);
                break;
            case 3:
                speed = 10;
                piercing = true;
                lifespan = 7;
                damage = 3f;
                iFrames = 0.75f;

                rb.mass = 1f;
                rb.gravityScale = -.15f;

                coll.offset = new Vector2(0f, 0f);
                coll.size = new Vector2(1.5f, 1.5f);
                break;
            case 4:
                speed = 3;
                piercing = true;
                lifespan = 5;
                damage = 4f;
                iFrames = 0.75f;

                rb.mass = 1f;
                rb.gravityScale = 2.5f;

                coll.offset = new Vector2(0f, 0f);
                coll.size = new Vector2(1.5f, 1.5f);

                rb.velocity = new Vector2(rb.velocity.x, 20f);
                break;
        }
    }
}
