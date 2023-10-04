using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField] private DropGenerator dropGenerator;

    public void DropItems()
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
                transform.GetChild(i).SetParentRecursive(transform.parent.parent);
            }
        }

        Destroy(gameObject);
    }
}
