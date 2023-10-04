using System.Diagnostics;
using UnityEngine;

public class Session : MonoBehaviour
{
    public static Session instance;

    private Stopwatch stopwatch;
    public float timeElapsed { get; private set; }
    private float runtime;

    public static bool speedrunMode = false;
    public static bool singleStageRun = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void RestartStopwatch()
    {
        UnityEngine.Debug.Log("woopwoop");
        if (stopwatch != null)
        {
            stopwatch = null;
        }

        stopwatch = new Stopwatch();
        stopwatch.Start();
    }

    private void Update()
    {
        if (stopwatch != null)
        {
            timeElapsed = stopwatch.ElapsedMilliseconds;
            UnityEngine.Debug.Log(timeElapsed / 1000);
        }
    }
}
