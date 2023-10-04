using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator anim;
    [SerializeField] BoxCollider2D coll;

    [SerializeField] private bool canOpenFromLeft;
    [SerializeField] private bool canOpenFromRight;

    [SerializeField] private bool locked;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (locked)
            {
                if (collision.GetComponent<PlayerCondition>().hasKey)
                {
                    locked = false;
                    collision.GetComponent<PlayerCondition>().hasKey = false;
                }
                else
                {
                    return;
                }
            }

            if (collision.transform.position.x < transform.position.x && canOpenFromLeft ||
                collision.transform.position.x > transform.position.x && canOpenFromRight)
            {
                Open();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Close();
        }
    }

    private void Open()
    {
        coll.enabled = false;
        anim.SetBool("open", true);
    }

    private void Close()
    {
        coll.enabled = true;
        anim.SetBool("open", false);
    }
}
