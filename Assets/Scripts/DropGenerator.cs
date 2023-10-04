using UnityEngine;

public class DropGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] dropTable;
    private GameObject drop;

    [SerializeField] private bool generateOnAwake;

    private void Awake()
    {
        if (generateOnAwake)
        {
            GenerateDrop();
        }
    }

    public void GenerateDrop()
    {
        if (dropTable.Length == 0)
        {
            Destroy(this);
            return;
        }

        drop = Instantiate(dropTable[(int)RNG.getRN(0, dropTable.Length)], transform.position, Quaternion.identity);

        drop.transform.SetParentRecursive(gameObject.transform);

        drop.SetActive(false);

        Destroy(this);
    }
}
