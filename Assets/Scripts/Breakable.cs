using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] private GameObject[] deathParticles;

    [SerializeField] private DropGenerator dropGenerator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Weapon")
        {
            if (dropGenerator)
            {
                dropGenerator.GenerateDrop();
            }

            if (this.gameObject.transform.childCount > 0)
            {
                for (int i = transform.childCount - 1; i >= 0; --i)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                    transform.GetChild(i).SetParentRecursive(transform.parent);
                }
            }

            for (int i = 0; i < deathParticles.Length; i++)
            {
                GameObject deathParticle = Instantiate(deathParticles[i], transform.position, Quaternion.identity);

                if (i % 2 == 1)
                {
                    if (deathParticle.GetComponent<SpriteRenderer>())
                    {
                        deathParticle.GetComponent<SpriteRenderer>().flipX = true;
                    }
                    if (deathParticle.GetComponent<StartVelocity>())
                    {
                        deathParticle.GetComponent<StartVelocity>().xVelocity *= -1;
                    }
                }
            }

            Destroy(gameObject);
        }
    }
}
