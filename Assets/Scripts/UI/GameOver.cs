using UnityEngine;
using UnityEngine.InputSystem;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private Animator anim;

    [SerializeField] private GameObject background;
    [SerializeField] private GameObject[] menuOptionCursors;

    private bool menuActive = false;

    [SerializeField] [Range(0, 1)] private int selector = 0;
    private int shiftDir;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OpenGameOverMenu()
    {
        anim.SetBool("menuVisible", true);
    }

    public void ActivateGameOverMenu()
    {
        menuActive = true;
        SelectionUpdated();
    }

    public void OnNavigate(InputAction.CallbackContext value)
    {
        if (value.started && menuActive)
        {
            shiftDir = Mathf.RoundToInt(value.ReadValue<Vector2>().x);
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
            Invoke("ExecuteMenuOption", 0.333f);
            menuActive = false;
            HideCursor();
        }
    }

    public void OnSubmitButtonDown(int _menuOption)
    {
        if (menuActive)
        {
            selector = Mathf.Clamp(_menuOption, 0, menuOptionCursors.Length - 1);

            Invoke("ExecuteMenuOption", 0.333f);
            menuActive = false;
            HideCursor();
        }
    }

    private void ExecuteMenuOption()
    {
        switch (selector)
        {
            case 0:
                gameManager.Retry();
                break;
            case 1:
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
        selector = Mathf.Clamp(selector + shiftDir, 0, menuOptionCursors.Length -1);

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
