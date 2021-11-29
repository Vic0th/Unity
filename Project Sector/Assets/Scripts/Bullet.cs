using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    uint bulletSpeed = 20;

    float lifeTime = 2.5f;
    float life = 0;

    Rigidbody2D rb;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        life += Time.deltaTime;

        if(life > lifeTime)
            Destroy(gameObject);
        
        rb.velocity = transform.up * bulletSpeed;
    }


    private void OnTriggerEnter2D(Collider2D collision) {

        if(collision.tag == "Characters") {
            collision.gameObject.GetComponent<Char>().dealDamage();

            Destroy(gameObject);
        }

        
    }

}
