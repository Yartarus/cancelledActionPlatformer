using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartGameMenu : MonoBehaviour
{
    private Session session;

    [SerializeField] private MenuManager menuManager;

    [SerializeField] private GameObject[] menuOptionCursors;

    private bool menuActive = false;

    [SerializeField] [Range(0, 2)] private int selector = 0;
    private int shiftDir;

    [SerializeField] private GameObject continueOption;

    private bool hasContinueData = false;

    private void Awake()
    {
        session = GameObject.FindGameObjectWithTag("Session").GetComponent<Session>();

        hasContinueData = ExtensionMethods.IntToBool(PlayerPrefs.GetInt("HasContinueData"));

        if (!hasContinueData)
        {
            continueOption.GetComponent<Text>().color = new Color32(139, 155, 180, 255);
            continueOption.GetComponentInChildren<Button>().gameObject.SetActive(false);
        }
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

            if (selector > 1)
            {
                Invoke("ExecuteMenuOption", 0.1f);
            }
            else
            {
                Invoke("ExecuteMenuOption", 0.333f);
                FindObjectOfType<AudioManager>().StopBGM();
            }

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

            if (selector > 1)
            {
                Invoke("ExecuteMenuOption", 0.1f);
            }
            else
            {
                Invoke("ExecuteMenuOption", 0.333f);
                FindObjectOfType<AudioManager>().StopBGM();
            }

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

            menuManager.Invoke("CloseStartGameMenu", 0.1f);
        }
    }

    private void ExecuteMenuOption()
    {
        switch (selector)
        {
            case 0:
                Debug.Log("0");

                PlayerPrefs.SetInt("PlayerTakeDefaultValues", ExtensionMethods.BoolToInt(false));
                PlayerPrefs.SetInt("HasContinueData", ExtensionMethods.BoolToInt(false));

                Session.speedrunMode = false;
                Session.singleStageRun = false;

                session.RestartStopwatch();

                SceneManager.LoadScene(PlayerPrefs.GetString("ContinueStage"));
                break;
            case 1:
                Debug.Log("1");

                PlayerPrefs.SetInt("HasContinueData", ExtensionMethods.BoolToInt(false));

                Session.speedrunMode = false;
                Session.singleStageRun = false;

                session.RestartStopwatch();

                SceneManager.LoadScene("Intro1");
                break;
            case 2:
                Debug.Log("2");

                menuManager.OpenSpeedrunModeMenu();
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
        if (selector == 0 && !hasContinueData)
        {
            selector++;
        }

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
