using UnityEngine;

public class SceneTransport : MonoBehaviour
{
    [SerializeField]
    GameObject[] ObjectsToKeep = new GameObject[2];

    void Awake() {

        if(GameObject.FindGameObjectsWithTag("GameController").Length > 1)
            DestroyAll();

        else {
            DontDestroyOnLoad(gameObject);
            foreach(GameObject x in ObjectsToKeep)
                DontDestroyOnLoad(x);
        }

    }
    

    public void DestroyAll() {
        foreach(GameObject x in ObjectsToKeep)
            Destroy(x);
        Destroy(gameObject);
    }
}
