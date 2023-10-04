using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private bool deleteOnTrigger;

    [SerializeField] private bool endCutscene;

    [SerializeField] private float cutsceneDirX;
    [SerializeField] private float cutsceneDirY;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (endCutscene)
            {
                collision.GetComponent<PlayerMovement>().EndCutsceneMode();
            }
            else
            {
                collision.GetComponent<PlayerMovement>().SetCutsceneMode(cutsceneDirX, cutsceneDirY);
            }

            if (deleteOnTrigger)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
