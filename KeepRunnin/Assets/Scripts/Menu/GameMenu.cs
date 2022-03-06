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
        gameUI.SetActive(false);
        gameOverMenu.SetActive(true);
    }


    public void FinishMapScreen() {
        StopGame();
        gameUI.SetActive(false);
        gameFinishMenu.SetActive(true);
    }



    public void RestartGame() {
        ContinueGame();
        Checkpoints.RestartCP();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



    public void ToMainMenu() {
        Time.timeScale = 1;

        //PlayerPrefs.SetInt("lastLevel",SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("MainMenu");
    }
}
