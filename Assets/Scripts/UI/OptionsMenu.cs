using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    [SerializeField] private MenuManager menuManager;

    [SerializeField] private GameObject[] menuOptionCursors;

    private bool menuActive = false;

    [SerializeField] [Range(0, 1)] private int selector = 0;
    private int shiftDir;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private float musicVolume;
    private float sfxVolume;

    private void Awake()
    {
        GetVolumeSettings();
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

    /*public void OnSubmit(InputAction.CallbackContext ctx)
    {
        if (ctx.started && menuActive)
        {
            ExecuteMenuOption();

            menuActive = false;
            HideCursor();
        }
    }

    public void OnSubmitButtonDown(int _menuOption)
    {
        if (menuActive)
        {
            selector = Mathf.Clamp(_menuOption, 0, menuOptionCursors.Length - 1);

            ExecuteMenuOption();

            menuActive = false;
            HideCursor();
        }
    }*/

    public void OnMusicVolumeChanged(float value)
    {
        musicVolume = Mathf.Log10(value) * 20;

        audioMixer.SetFloat("musicVolume", musicVolume);
    }

    public void OnSfxVolumeChanged(float value)
    {
        sfxVolume = Mathf.Log10(value) * 20;

        audioMixer.SetFloat("sfxVolume", sfxVolume);
    }

    public void OnCancel(InputAction.CallbackContext ctx)
    {
        if (ctx.started && menuActive)
        {
            menuActive = false;
            SaveVolumeSettings();
            HideCursor();

            menuManager.Invoke("CloseOptionsMenu", 0.1f);
        }
    }

    /*private void ExecuteMenuOption()
    {
        switch (selector)
        {
            case 0:
                Debug.Log("0");
                break;
            case 1:
                Debug.Log("1");
                break;
        }
    }*/

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
                if (menuOptionCursors[i].transform.parent.GetComponentInChildren<Slider>())
                {
                    menuOptionCursors[i].transform.parent.GetComponentInChildren<Slider>().Select();
                }
            }
            else
            {
                menuOptionCursors[i].SetActive(false);
            }
        }
    }

    private void GetVolumeSettings()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0);
        musicSlider.value = Mathf.Pow(10, musicVolume / 20);

        sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0);
        sfxSlider.value = Mathf.Pow(10, sfxVolume / 20);
    }

    private void SaveVolumeSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);

        PlayerPrefs.SetFloat("SfxVolume", sfxVolume);
    }

    private void HideCursor()
    {
        for (int i = 0; i < menuOptionCursors.Length; i++)
        {
            menuOptionCursors[i].SetActive(false);
        }
    }
}
