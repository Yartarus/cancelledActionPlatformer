using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private MenuManager menuManager;

    [SerializeField] private GameObject[] menuOptionCursors;

    private bool menuActive = false;

    [SerializeField] [Range(0, 3)] private int selector = 0;
    private int shiftDir;

    private void Awake()
    {
        OpenMenu();
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

            Invoke("ExecuteMenuOption", 0.1f);

            menuActive = false;
            HideCursor();
        }
    }

    public void OnSubmitButtonDown(int _menuOption)
    {
        if (menuActive)
        {
            FindObjectOfType<AudioManager>().Play("Select");

            selector = Mathf.Clamp(_menuOption, 0, menuOptionCursors.Length - 1);

            Invoke("ExecuteMenuOption", 0.1f);

            menuActive = false;
            HideCursor();
        }
    }

    private void ExecuteMenuOption()
    {
        switch (selector)
        {
            case 0:
                Debug.Log("0");
                menuManager.OpenStartGameMenu();
                break;
            case 1:
                Debug.Log("1");
                menuManager.OpenOptionsMenu();
                break;
            case 2:
                Debug.Log("2");
                //SceneManager.LoadScene("Credits");
                break;
            case 3:
                Debug.Log("quit");
                Application.Quit();
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
