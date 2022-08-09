using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField]
    uint damage = 1;

    static GameController gc = null;

    void Awake() {
        if(gc == null)
            gc = GameObject.Find("/MAIN").GetComponent<GameController>();
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.CompareTag("Player"))
            gc.DamagePlayer(damage);  
    }
}
