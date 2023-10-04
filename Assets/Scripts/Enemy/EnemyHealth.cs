using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private SpriteRenderer sprite;

    private SpriteFlash spriteFlash;
    private Death death;

    [SerializeField] private float maxHealth = 2f;
    public float health { get; private set; }

    public int subWeaponEnergyValue = 2;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();

        spriteFlash = GetComponent<SpriteFlash>();
        death = GetComponent<Death>();

        health = maxHealth;
    }

    public void ResetHealth()
    {
        health = maxHealth;
    }

    public void TakeDamage(float _damage)
    {
        health = Mathf.Clamp(health - _damage, 0, maxHealth);

        if (health <= 0f)
        {
            death.Die();
        }
        else
        {
            spriteFlash.Flash();
        }
    }
}
