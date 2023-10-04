using UnityEngine;

public class Egg : MonoBehaviour
{
    [SerializeField] private GameObject eggContentPrefab;

    [SerializeField] private float hatchTime = 300f;
    private float time = 0;

    private GameObject eggContent;

    private void Start()
    {
        eggContent = Instantiate(eggContentPrefab, transform.position, Quaternion.identity);
        eggContent.transform.parent = gameObject.transform;
        eggContent.SetActive(false);
    }

    private void Update()
    {
        time += Time.deltaTime;

        if (time > hatchTime)
        {
            Hatch();
        }
    }

    private void Hatch()
    {
        eggContent.SetActive(true);
        eggContent.transform.SetParent(this.gameObject.transform.parent);

        Destroy(gameObject);
    }
}
