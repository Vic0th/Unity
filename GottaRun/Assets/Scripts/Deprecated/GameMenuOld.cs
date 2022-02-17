using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameMenuOld : MonoBehaviour
{
    [SerializeField]
    GameObject gameUI, gameMenu;
    [SerializeField]
    PlayerInput pi;

    Transform tr;

    void Start() {
        tr = pi.transform;

        if(PlayerPrefs.HasKey("lastLevel")) {
            string[] pos = PlayerPrefs.GetString("position").Split(","); 
            string[] rot = PlayerPrefs.GetString("rotation").Split(",");
            string[] scl = PlayerPrefs.GetString("scale").Split(",");

            //Rigidbody2D rb = tr.gameObject.GetComponent<Rigidbody2D>();
            //Vector3 p = new Vector3(float.Parse(pos[0]),float.Parse(pos[1]),float.Parse(pos[2]));

            //rb.position = p;
            tr.position = new Vector3(float.Parse(pos[0]),float.Parse(pos[1]),float.Parse(pos[2]));
            tr.rotation = new Quaternion(float.Parse(rot[0]),float.Parse(rot[1]),float.Parse(rot[2]),float.Parse(rot[3]));
            tr.localScale = new Vector3(float.Parse(scl[0]),float.Parse(scl[1]),float.Parse(scl[2]));
        }
    }


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


        PlayerPrefs.SetInt("lastLevel",SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.SetString("position",$"{tr.position.x},{tr.position.y},{tr.position.z}");
        PlayerPrefs.SetString("rotation",$"{tr.rotation.x},{tr.rotation.y},{tr.rotation.z},{tr.rotation.w}");
        PlayerPrefs.SetString("scale",$"{tr.localScale.x},{tr.localScale.y},{tr.localScale.z}");
        SceneManager.LoadScene("MainMenu");

        gameObject.GetComponent<SceneTransport>().DestroyAll();
    }
}
