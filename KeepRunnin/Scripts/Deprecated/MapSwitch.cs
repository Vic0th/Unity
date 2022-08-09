using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSwitch : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {

            if(SceneManager.GetActiveScene().buildIndex == 1) {
                SceneManager.LoadScene(2);
                collision.attachedRigidbody.position = new Vector3(-23f,5f,0f);
            }
            else {
                SceneManager.LoadScene(1);
                collision.attachedRigidbody.position = new Vector3(23f,5f,0);
            }
        }
    }

}
