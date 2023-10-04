using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private GameObject background;
    [SerializeField] private GameObject[] menuOptionCursors;

    public static bool gameIsPaused = false;
    public static bool playerControlsDisabled = false;

    private bool menuActive = false;

    [SerializeField] [Range(0, 2)] private int selector = 0;
    private int shiftDir;

    private void OpenPauseMenu()
    {
        background.SetActive(true);

        Time.timeScale = 0f;

        gameIsPaused = true;
        playerControlsDisabled = true;
        selector = 0;
        menuActive = true;
        SelectionUpdated();
    }

    private void ClosePauseMenu()
    {
        background.SetActive(false);

        Time.timeScale = 1f;

        gameIsPaused = false;
    }

    public void OnPause(InputAction.CallbackContext ctx)
    {
        if (ctx.started && !GameManager.isGameOver)
        {
            if (gameIsPaused)
            {
                OnSubmitButtonDown(0);
            }
            else
            {
                OpenPauseMenu();
            }
        }
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
            Time.timeScale = 1f;

            if (selector == 0)
            {
                ExecuteMenuOption();
            } 
            else
            {
                Invoke("ExecuteMenuOption", 0.333f);
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

            Time.timeScale = 1f;

            if (selector == 0)
            {
                ExecuteMenuOption();
            }
            else
            {
                Invoke("ExecuteMenuOption", 0.333f);
            }

            menuActive = false;
            HideCursor();
        }
    }

    private void ExecuteMenuOption()
    {
        gameIsPaused = false;

        switch (selector)
        {
            case 0:
                ClosePauseMenu();
                break;
            case 1:
                gameManager.Retry();
                break;
            case 2:
                if (!Session.speedrunMode)
                {
                    PlayerPrefs.SetInt("HasContinueData", ExtensionMethods.BoolToInt(true));
                    string stageName = gameManager.GetStageName();
                    PlayerPrefs.SetString("ContinueStage", "Stage" + stageName.Substring(5));
                }
                gameManager.QuitToMainMenu();
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
