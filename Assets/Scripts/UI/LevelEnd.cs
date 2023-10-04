using UnityEngine;
using UnityEngine.UI;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private Animator anim;

    [SerializeField] private GameObject background;

    [SerializeField] private Text stage;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OpenLevelEndScreen()
    {
        string stageName = gameManager.GetStageName();
        stage.text = "Stage " + stageName.Substring(5);
        anim.SetBool("visible", true);
    }

    public void EndLevel()
    {
        gameManager.NextStage();
    }
}
