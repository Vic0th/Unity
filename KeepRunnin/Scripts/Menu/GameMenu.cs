using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [SerializeField]
    GameObject gameUI, gameMenu, gameOverMenu, gameFinishMenu;
    [SerializeField]
    PlayerInput pi;

    void StopGame() {
        Time.timeScale = 0;
        pi.enabled = false;
    }


    void ContinueGame() {
        Time.timeScale = 1;
        pi.enabled = true;
    }


    public void OpenMenu() {
        StopGame();

        gameUI.SetActive(false);
        gameMenu.SetActive(true);
        
    }


    public void CloseMenu() {
        ContinueGame();

        gameUI.SetActive(true);
        gameMenu.SetActive(false);
    }


    public void GameOverScreen() {
        StopGame();
        Checkpoints.RestartCP();
        gameUI.SetActive(false);
        gameOverMenu.SetActive(true);
    }


    public void FinishMapScreen() {
        StopGame();
        Checkpoints.RestartCP();
        gameUI.SetActive(false);
        gameFinishMenu.SetActive(true);
    }


    public void RestartGame() {
        ContinueGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void ToMainMenu() {
        ContinueGame();
        //PlayerPrefs.SetInt("lastLevel",SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("MainMenu");
    }
}
