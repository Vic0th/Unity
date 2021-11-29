using UnityEngine;

public class Char : MonoBehaviour
{
    [SerializeField]
    ushort health = 2;

    RandomSpawner rp;

    private void Start() {
        rp = gameObject.GetComponentInParent<RandomSpawner>();
    }


    public void dealDamage() {
        health--;
        if(health < 1) {
            rp.increaseScoring();
            Destroy(gameObject);
        }
    }
    [SerializeField]
    float lifeTime = 7f;
    float life = 0;

    private void Update() {
        life += Time.deltaTime;

        if(life > lifeTime)
            Destroy(gameObject);
    }
}
