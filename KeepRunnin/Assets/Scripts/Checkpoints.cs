using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    [SerializeField]
    uint thisCP = 0;
    [SerializeField]
    bool isEnding = false;

    static uint lastCP = 0;
    static GameController gc = null;

    void Awake() {
        if(gc == null)
            gc = GameObject.Find("/MAIN").GetComponent<GameController>();
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.CompareTag("Player")) {
            if(isEnding)
                gc.FinishMap();
            else
                lastCP = lastCP < thisCP ? thisCP : lastCP;
        }     
    }

    public static uint GetLastCP() => lastCP;

    public static void RestartCP() => lastCP = 0;
}
