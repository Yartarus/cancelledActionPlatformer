using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]private GameObject mainMenu;
    [SerializeField]private GameObject startGameMenu;
    [SerializeField]private GameObject speedrunModeMenu;
    [SerializeField]private GameObject optionsMenu;

    [SerializeField]private GameObject records;

    private void Start()
    {
        FindObjectOfType<AudioManager>().ChangeBGM("Track0");
    }

    private void ReturnToMainMenu()
    {
        mainMenu.SetActive(true);
        mainMenu.GetComponent<MainMenu>().OpenMenu();
    }

    public void OpenStartGameMenu()
    {
        mainMenu.SetActive(false);
        startGameMenu.SetActive(true);
        startGameMenu.GetComponent<StartGameMenu>().OpenMenu();
    }
    public void CloseStartGameMenu()
    {
        startGameMenu.SetActive(false);
        ReturnToMainMenu();
    }

    public void OpenSpeedrunModeMenu()
    {
        startGameMenu.SetActive(false);
        speedrunModeMenu.SetActive(true);
        speedrunModeMenu.GetComponent<SpeedrunModeMenu>().OpenMenu();
        records.SetActive(true);
    }
    public void CloseSpeedrunModeMenu()
    {
        speedrunModeMenu.SetActive(false);
        records.SetActive(false);
        OpenStartGameMenu();
    }

    public void OpenOptionsMenu()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        optionsMenu.GetComponent<OptionsMenu>().OpenMenu();
    }
    public void CloseOptionsMenu()
    {
        optionsMenu.SetActive(false);
        ReturnToMainMenu();
    }
}
