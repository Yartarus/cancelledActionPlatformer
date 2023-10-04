using UnityEngine;

public class WeaponUpgrade : MonoBehaviour
{
    private SpriteRenderer sprite;

    private PlayerCondition playerCondition;

    [SerializeField] private Sprite[] weapons;

    private PlayerCondition.BladeType bladeType;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();

        playerCondition = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCondition>();
    }

    private void Start()
    {
        bladeType = playerCondition.bladeType + 1;

        if (weapons.Length <= (int)bladeType)
        {
            Destroy(gameObject);
            return;
        }

        sprite.sprite = weapons[(int)bladeType];
    }

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
        _collision.GetComponent<PlayerCondition>().PickupWeapon(bladeType); ;
        Destroy(gameObject);
    }
}
