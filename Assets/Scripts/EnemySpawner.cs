using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private EnemyHealth enemyHealth;
    private Death death;

    [SerializeField] private GameObject enemy;

    [SerializeField] private GameObject[] spawnArea; //(Should take up to two corners)

    [SerializeField] private bool respawnWhenNotInTrigger = true;

    private bool playerIsInTrigger = false;

    private float targetX;
    private float targetY;

    private void Awake()
    {
        enemyHealth = enemy.GetComponent<EnemyHealth>();
        death = enemy.GetComponent<Death>();

        death.respawnable = true;
    }

    private void Update()
    {
        if (!enemy.activeInHierarchy)
        {
            if (respawnWhenNotInTrigger && !playerIsInTrigger)
            {
                RespawnEnemy();
            }
            else if (!respawnWhenNotInTrigger && playerIsInTrigger)
            {
                RespawnEnemy();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerIsInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerIsInTrigger = false;
        }
    }

    private void RespawnEnemy()
    {
        enemy.transform.position = GetRespawnPoint();
        enemyHealth.ResetHealth();
        enemy.SetActive(true);
    }

    private Vector2 GetRespawnPoint()
    {
        if (spawnArea.Length > 1)
        {
            float x1 = spawnArea[0].transform.position.x;
            float x2 = spawnArea[1].transform.position.x;

            float y1 = spawnArea[0].transform.position.y;
            float y2 = spawnArea[1].transform.position.y;

            if (x1 < x2)
            {
                targetX = RNG.getFloatRN(x1, x2);
            }
            else
            {
                targetX = RNG.getFloatRN(x2, x1);
            }

            if (y1 < y2)
            {
                targetY = RNG.getFloatRN(y1, y2);
            }
            else
            {
                targetY = RNG.getFloatRN(y2, y1);
            }

            return new Vector2(targetX, targetY);
        }
        else
        {
            return spawnArea[0].transform.position;
        }
    }
}
