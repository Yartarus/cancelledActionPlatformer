using UnityEngine;

public class SubweaponItem : MonoBehaviour
{
    private BoxCollider2D coll;
    private SpriteRenderer sprite;

    private PlayerCondition playerCondition;

    [SerializeField] private Sprite[] subweapons;

    [SerializeField] private PlayerCondition.SubWeapon subweapon;

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();

        playerCondition = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCondition>();
    }

    private void Start()
    {
        if (subweapon == PlayerCondition.SubWeapon.rock)
        {
            subweapon = Randomize();
        }

        switch ((int)subweapon)
        {
            case 0:
                coll.offset = new Vector2(0f, 0f);
                break;
            case 1:
                coll.offset = new Vector2(0f, 0.125f);
                break;
            case 2:
                coll.offset = new Vector2(0f, -0.5f);
                transform.GetChild(0).GetComponent<BoxCollider2D>().offset = new Vector2(0f, -0.5f);
                break;
            case 3:
                coll.offset = new Vector2(0f, -0.5f);
                transform.GetChild(0).GetComponent<BoxCollider2D>().offset = new Vector2(0f, -0.5f);
                break;
            case 4:
                coll.offset = new Vector2(0f, -0.5f);
                transform.GetChild(0).GetComponent<BoxCollider2D>().offset = new Vector2(0f, -0.5f);
                break;
        }

        sprite.sprite = subweapons[(int)subweapon];
    }

    private PlayerCondition.SubWeapon Randomize()
    {
        PlayerCondition.SubWeapon _subweapon;
        //_subweapon = (PlayerCondition.SubWeapon)Random.Range(1, 5);
        _subweapon = (PlayerCondition.SubWeapon)RNG.getRN(1, 5);
        Debug.Log("subweapondrop : " + (float)_subweapon);

        if (_subweapon == playerCondition.subweapon)
        {
            if (playerCondition.subweapon == PlayerCondition.SubWeapon.boomerang)
            {
                _subweapon = PlayerCondition.SubWeapon.dart;
            }
            else
            {
                _subweapon = playerCondition.subweapon + 1;
            }
        }

        return _subweapon;
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
        _collision.GetComponent<PlayerCondition>().PickupSubweapon(subweapon); ;
        Destroy(gameObject);
    }
}
