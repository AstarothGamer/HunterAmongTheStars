using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Assign your pause menu UI in the Inspector
    private bool isPaused = false;
    public bool canOpenPauseMenu = false;
    public bool lockCursor = false;
    public bool playMusic = false;

    private void Start()
    {
        if (playMusic)
        AudioManager.PlayMusic(SoundType.CalmMusic, 0.7f);
    }
    void Update()
    {
        if (canOpenPauseMenu)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                    ResumeGame();
                else
                    PauseGame();
            }
        }
    }
    public void StartGame()
    {
        if (playMusic)
        AudioManager.StopMusicGradually(0.8f);

        SceneLoader.Instance.LoadScene("Bar");
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void Restart()
    {
        SceneLoader.Instance.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void PauseGame()
    {
        // freeze time
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        if (lockCursor)
        Cursor.lockState = CursorLockMode.Locked;
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = false;
        }
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; // Make sure time is normal
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
