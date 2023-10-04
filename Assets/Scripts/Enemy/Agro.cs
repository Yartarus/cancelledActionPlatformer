using UnityEngine;

public class Agro : MonoBehaviour
{
    public bool isAgrod = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isAgrod = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isAgrod = false;
        }
    }
}
