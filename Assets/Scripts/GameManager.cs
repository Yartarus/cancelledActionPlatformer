using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameOver gameOver;
    [SerializeField] private LevelEnd levelEnd;

    public static bool isGameOver;

    [SerializeField] private GameObject[] checkpoints;
    public static int currentCheckpointIndex = 0;
    public static int currentCheckpointCameraWaypointIndex = 0;

    private void Awake()
    {
        isGameOver = false;
        GameObject.FindGameObjectWithTag("Player").transform.position = checkpoints[currentCheckpointIndex].transform.position;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>().SetWaypoint(currentCheckpointCameraWaypointIndex);
        Physics2D.IgnoreLayerCollision(6, 7, false);
    }

    private void Start()
    {
        FindObjectOfType<AudioManager>().ChangeBGM("Track0");
    }

    public void SetGameOver()
    {
        isGameOver = true;
        gameOver.OpenGameOverMenu();
    }

    public void EndLevel()
    {
        levelEnd.OpenLevelEndScreen();
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMainMenu()
    {
        currentCheckpointIndex = 0;
        currentCheckpointCameraWaypointIndex = 0;
        SceneManager.LoadScene(0);
    }

    public void NextStage()
    {
        currentCheckpointIndex = 0;
        currentCheckpointCameraWaypointIndex = 0;
        if (Session.speedrunMode)
        {
            if (Session.singleStageRun)
            {
                SceneManager.LoadScene("TitleScreen");
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
            }
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public string GetStageName()
    {
        return SceneManager.GetActiveScene().name;
    }
}
