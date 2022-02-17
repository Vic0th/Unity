using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameMenu : MonoBehaviour
{
    [SerializeField]
    GameObject gameUI, gameMenu;
    [SerializeField]
    PlayerInput pi;


    public void OpenMenu() {
        Time.timeScale = 0;
        pi.enabled = false;

        gameUI.SetActive(false);
        gameMenu.SetActive(true);
        
    }



    public void CloseMenu() {
        Time.timeScale = 1;
        pi.enabled = true;

        gameUI.SetActive(true);
        gameMenu.SetActive(false);
    }



    public void ToMainMenu() {
        Time.timeScale = 1;


        //PlayerPrefs.SetInt("lastLevel",SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("MainMenu");
    }
}
