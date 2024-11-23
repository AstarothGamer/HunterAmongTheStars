using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneLoader.Instance.LoadScene("MainGame");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void Restart(string gameName)
    {
        SceneLoader.Instance.LoadScene(gameName);
    }
}