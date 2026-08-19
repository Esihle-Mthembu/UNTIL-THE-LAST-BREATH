using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // New game button
    public void NewGame()
    {
        SceneManager.LoadScene("Gameplay Scene");
    }

    // Continue button
    public void ContinueGame()
    {

    }

    // Exit button
    public void QuitGame()
    {
        Application.Quit();
    }
}
