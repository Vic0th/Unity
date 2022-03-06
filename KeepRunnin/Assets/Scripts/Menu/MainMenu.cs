using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    Button conButton;

    public void Start() {
        if(!PlayerPrefs.HasKey("lastLevel"))
            conButton.interactable = false;
    }

    public void StartNewGame() {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(1);
    }


    public void LoadGame() => SceneManager.LoadScene(PlayerPrefs.GetInt("lastLevel"));

}
