using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
 
    [Header("Audio")]
    public AudioSource[] gameMusic;
    public AudioSource[] soundEffects;
    public Slider musicSlider;
    public Slider effectsSlider;
    public AudioSource gameAudio;

    [Header("Resolution")]
    public Toggle fullScreenToggle;
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;
    Resolution selectedResolution;

    public const string resolutionWidthPlayerPrefKey = "ResolutionWidth";
    public const string resolutionHeightPlayerPrefKey = "ResolutionHeight";
    public const string resolutionRefreshRatePlayerPrefKey = "RefreshRate";
    public const string fullScreenPlayerPrefKey = "FullScreen";

    [Header("Panels")]
    public GameObject pauseMenu;
    public LevelLoader levelLoader;

    [Header("Level Buttons")]
    public GameObject[] levelButtons;

    [Header("Other scripts")]
    public GameManager gm;


    private void Awake()
    {        
        musicSlider.value = UIStatic.GetMusicSettings();
        effectsSlider.value = UIStatic.GetSoundEffSettings();
    }

    public void Start()
    {
        float musicVolume = UIStatic.GetMusicSettings();
        float effectsVolume = UIStatic.GetSoundEffSettings();

        // Set music volume
        for (int i = 0; i < gameMusic.Length; i++)
        {
            gameMusic[i].volume = musicVolume;
        }

        // Set effects volume
        for (int i = 0; i < soundEffects.Length; i++)
        {
            soundEffects[i].volume = effectsVolume;
        }

        //Resolution
        resolutions = Screen.resolutions;
        Debug.Log("Moguce sranje: " + resolutions);
        CreateResolutionDropdown();
        LoadSettings();

        fullScreenToggle.onValueChanged.AddListener(SetFullScreen);
        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        EnablePassedLevels();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void EnablePassedLevels()
    {
        if (PlayerPrefs.GetInt("LevelOnePass") == 1)
        {
            levelButtons[0].GetComponent<Button>().interactable = true;
        }
        if (PlayerPrefs.GetInt("LevelTwoPass") == 1)
        {
            levelButtons[1].GetComponent<Button>().interactable = true;
        }
        if (PlayerPrefs.GetInt("LevelThreePass") == 1)
        {
            levelButtons[2].GetComponent<Button>().interactable = true;
        }
        if (PlayerPrefs.GetInt("LevelFourPass") == 1)
        {
            levelButtons[3].GetComponent<Button>().interactable = true;
        }
    }

    public void LoadSettings()
    {
        selectedResolution = new Resolution();
        selectedResolution.width = PlayerPrefs.GetInt(resolutionWidthPlayerPrefKey, Screen.currentResolution.width);
        selectedResolution.height = PlayerPrefs.GetInt(resolutionHeightPlayerPrefKey, Screen.currentResolution.height);
        selectedResolution.refreshRate = PlayerPrefs.GetInt(resolutionRefreshRatePlayerPrefKey, Screen.currentResolution.refreshRate);

        fullScreenToggle.isOn = PlayerPrefs.GetInt(fullScreenPlayerPrefKey, Screen.fullScreen ? 1 : 0) > 0;

        if (selectedResolution.width != 0 && selectedResolution.height != 0)
        {
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, fullScreenToggle.isOn);
        }
        else
        {
            Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreen);
        }

        Debug.Log(selectedResolution);
        Debug.Log(PlayerPrefs.GetInt(resolutionWidthPlayerPrefKey) + " i " + PlayerPrefs.GetInt(resolutionHeightPlayerPrefKey));
    }
    public void CreateResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (Mathf.Approximately(resolutions[i].width, selectedResolution.width) && Mathf.Approximately(resolutions[i].height, selectedResolution.height))
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt(fullScreenPlayerPrefKey, isFullScreen ? 1 : 0);
    }
    public void SetResolution(int resolutionIndex)
    {
        selectedResolution = resolutions[resolutionIndex];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt(resolutionWidthPlayerPrefKey, selectedResolution.width);
        PlayerPrefs.SetInt(resolutionHeightPlayerPrefKey, selectedResolution.height);
        PlayerPrefs.SetInt(resolutionRefreshRatePlayerPrefKey, selectedResolution.refreshRate);
        Debug.Log("postavljena je rezolucija " + PlayerPrefs.GetInt(resolutionWidthPlayerPrefKey) + "x" + PlayerPrefs.GetInt(resolutionHeightPlayerPrefKey));
    }

    public void ChangeMusicVolume()
    {
        for (int i = 0; i < gameMusic.Length; i++)
        {
            gameMusic[i].volume = musicSlider.value;
        }
        UIStatic.SaveMusicSettings(musicSlider.value);       
    }
    public void ChangeEffectsVolume()
    {
        for (int i = 0; i < soundEffects.Length; i++)
        {
            soundEffects[i].volume = effectsSlider.value;
        }
        UIStatic.SaveSoundEffSettings(effectsSlider.value);
    }

    public void ExitGame()
    {
        PlayerPrefs.DeleteKey("xPosition");
        PlayerPrefs.DeleteKey("yPosition");
        PlayerPrefs.DeleteKey("zPosition");

        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void PauseGame()
    {
        gameAudio.Pause();
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void UnpauseGame()
    {
        
        Time.timeScale = 1;
        gameAudio.UnPause();
        //pauseMenu.SetActive(false); ima na buttonu postavljeno, ali nek stoji tu
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteKey("CollectedHieroglyphs");
        PlayerPrefs.DeleteKey("AllCollectedHieroglyphs");
        PlayerPrefs.DeleteKey("LevelOnePass");
        PlayerPrefs.DeleteKey("LevelTwoPass");
        PlayerPrefs.DeleteKey("LevelThreePass");
        PlayerPrefs.DeleteKey("LevelFourPass");
        PlayerPrefs.DeleteKey("xPosition");
        PlayerPrefs.DeleteKey("yPosition");
        PlayerPrefs.DeleteKey("zPosition");

        int levelIndex = 1;
        StartCoroutine(levelLoader.LoadLevel(levelIndex));
    }
    public void StartLevelOne()
    {
        int levelIndex = 2;
        EnablePassedLevels();
        //SceneManager.LoadScene(levelIndex);
        UnpauseGame();
        StartCoroutine(levelLoader.LoadLevel(levelIndex));
    }
    public void StartLevelTwo()
    {
        //SceneManager.LoadScene("Level2");
        int levelIndex = 3;
        EnablePassedLevels();
        UnpauseGame();
        StartCoroutine(levelLoader.LoadLevel(levelIndex));
    }
    public void StartLevelThree()
    {
        //SceneManager.LoadScene("Level3");
        int levelIndex = 4;
        EnablePassedLevels();
        UnpauseGame();
        StartCoroutine(levelLoader.LoadLevel(levelIndex));
    }
    public void StartLevelFour()
    {
        //SceneManager.LoadScene("Level4");
        int levelIndex = 5;
        UnpauseGame();
        StartCoroutine(levelLoader.LoadLevel(levelIndex));
    }
    public void StartLevelFive()
    {
        //SceneManager.LoadScene("Level5");
        int levelIndex = 6;
        UnpauseGame();
        StartCoroutine(levelLoader.LoadLevel(levelIndex));
    }
    public void StartLevelSix()
    {
        //SceneManager.LoadScene("Level6");
        int levelIndex = 7;
        UnpauseGame();
        StartCoroutine(levelLoader.LoadLevel(levelIndex));
    }
    public void StartLevelSeven()
    {
        //SceneManager.LoadScene("Level7");
        int levelIndex = 8;
        UnpauseGame();
        StartCoroutine(levelLoader.LoadLevel(levelIndex));
    }
    public void StartLevelEight()
    {
        //SceneManager.LoadScene("Level8");
        int levelIndex = 9;
        UnpauseGame();
        StartCoroutine(levelLoader.LoadLevel(levelIndex));
    }
    public void StartLevelNine()
    {
        //SceneManager.LoadScene("Level9");
        int levelIndex = 10;
        UnpauseGame();
        StartCoroutine(levelLoader.LoadLevel(levelIndex));
    }
}
