using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SpeedrunModeMenu : MonoBehaviour
{
    private Session session;

    [SerializeField] private MenuManager menuManager;

    [SerializeField] private GameObject[] menuOptionCursors;

    private bool menuActive = false;

    [SerializeField] [Range(0, 2)] private int selector = 0;
    private int shiftDir;

    private void Awake()
    {
        session = GameObject.FindGameObjectWithTag("Session").GetComponent<Session>();
    }

    public void OpenMenu()
    {
        selector = 0;
        menuActive = true;
        SelectionUpdated();
    }

    public void OnNavigate(InputAction.CallbackContext value)
    {
        if (value.started && menuActive)
        {
            shiftDir = -Mathf.RoundToInt(value.ReadValue<Vector2>().y);
            if (shiftDir == 1 || shiftDir == -1)
            {
                ShiftSelection();
            }
        }
    }

    public void OnSubmit(InputAction.CallbackContext ctx)
    {
        if (ctx.started && menuActive)
        {
            FindObjectOfType<AudioManager>().Play("Select");

            Invoke("ExecuteMenuOption", 0.333f);
            FindObjectOfType<AudioManager>().StopBGM();

            menuActive = false;
            HideCursor();
        }
    }

    public void OnSubmitButtonDown(int _menuOption)
    {
        if (menuActive)
        {
            selector = Mathf.Clamp(_menuOption, 0, menuOptionCursors.Length - 1);

            FindObjectOfType<AudioManager>().Play("Select");

            Invoke("ExecuteMenuOption", 0.333f);
            FindObjectOfType<AudioManager>().StopBGM();

            menuActive = false;
            HideCursor();
        }
    }

    public void OnCancel(InputAction.CallbackContext ctx)
    {
        if (ctx.started && menuActive)
        {
            menuActive = false;
            HideCursor();

            menuManager.Invoke("CloseSpeedrunModeMenu", 0.1f);
        }
    }

    private void ExecuteMenuOption()
    {
        Session.speedrunMode = true;

        switch (selector)
        {
            case 0:
                Debug.Log("0");

                PlayerPrefs.SetInt("HasContinueData", ExtensionMethods.BoolToInt(false));

                Session.singleStageRun = false;

                session.RestartStopwatch();

                SceneManager.LoadScene("Stage1");
                break;
            case 1:
                Debug.Log("1");

                PlayerPrefs.SetInt("HasContinueData", ExtensionMethods.BoolToInt(false));

                Session.singleStageRun = true;

                session.RestartStopwatch();

                SceneManager.LoadScene("Stage1");
                break;
            case 2:
                Debug.Log("2");

                PlayerPrefs.SetInt("HasContinueData", ExtensionMethods.BoolToInt(false));

                Session.singleStageRun = true;

                session.RestartStopwatch();

                SceneManager.LoadScene("Stage2");
                break;
        }
    }

    public void SelectMenuOption(int _menuOption)
    {
        if (menuActive)
        {
            selector = Mathf.Clamp(_menuOption, 0, menuOptionCursors.Length - 1);

            SelectionUpdated();
        }
    }

    private void ShiftSelection()
    {
        selector = Mathf.Clamp(selector + shiftDir, 0, menuOptionCursors.Length - 1);

        SelectionUpdated();
    }

    private void SelectionUpdated()
    {
        for (int i = 0; i < menuOptionCursors.Length; i++)
        {
            if (selector == i)
            {
                menuOptionCursors[i].SetActive(true);
            }
            else
            {
                menuOptionCursors[i].SetActive(false);
            }
        }
    }

    private void HideCursor()
    {
        for (int i = 0; i < menuOptionCursors.Length; i++)
        {
            menuOptionCursors[i].SetActive(false);
        }
    }
}
