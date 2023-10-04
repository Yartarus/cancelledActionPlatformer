using System;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    private float direction;
    private int bladeType;

    private float damage = 2f;

    private List<Collider2D> hitTargets = new List<Collider2D>();

    private BoxCollider2D coll;
    private Animator anim;
    private SpriteRenderer sprite;

    private PlayerCondition playerCondition;

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();  
        sprite = GetComponent<SpriteRenderer>();

        playerCondition = GetComponentInParent<PlayerCondition>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if (hitTargets.Contains(collision))
            {
                return;
            }

            EnemyHealth hitTargetHealth = collision.GetComponent<EnemyHealth>();

            hitTargetHealth.TakeDamage(damage);

            if (hitTargetHealth.health == 0)
            {
                playerCondition.AddSubweaponEnergy(hitTargetHealth.subWeaponEnergyValue);
            }

            hitTargets.Add(collision);
        }
    }

    public void SetDirection(float _direction, int _bladeType)
    {
        direction = _direction;
        bladeType = _bladeType;

        gameObject.SetActive(true);
        anim.SetInteger("bladeType", bladeType);
        hitTargets.Clear();
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
        switch (bladeType)
        {
            case 0:
                damage = 2f;

                coll.offset = new Vector2(-0.4375f * direction, -0.125f);
                coll.size = new Vector2(1.125f, 0.75f);
                break;
            case 1:
                damage = 3f;

                coll.offset = new Vector2(0f, -0.125f);
                coll.size = new Vector2(2f, 0.75f);
                break;
            case 2:
                damage = 4f;

                coll.offset = new Vector2(0f, -0.125f);
                coll.size = new Vector2(2f, 0.75f);
                break;
        }
    }
}
