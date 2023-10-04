using UnityEngine;

public class Death : MonoBehaviour
{
    [SerializeField] private GameObject[] deathParticles;

    [SerializeField] private GameObject drops;

    [SerializeField] private float dropRate = 10f;

    public bool respawnable = false;

    public void Die()
    {
        if (drops && RNG.getRN(0, 101) <= dropRate)
        {
            drops.GetComponent<Drop>().DropItems();
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

        if (respawnable)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
