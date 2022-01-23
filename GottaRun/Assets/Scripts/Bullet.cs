using UnityEngine;

public class Bullet : MonoBehaviour
{  
    Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }



    float life = 0;

    void Update()
    {
        life += Time.deltaTime;

        if(life > lifeTime)
            Destroy(gameObject);
        
        else
            rb.velocity = bulletSpeed * Time.deltaTime * Vector2.right; //*transform.right
    }



    short bulletSpeed = 20;
    float lifeTime = 2.5f;
    byte damage = 1;

    public void setStats(byte dmg, short bulSpeed, float life) {
        damage = dmg;
        bulletSpeed = bulSpeed;
        lifeTime = life;
    }



    private void OnTriggerEnter2D(Collider2D collision) {

        if(collision.CompareTag("Characters")) {
            collision.gameObject.GetComponent<Char>().dealDamage(damage);
            //Add Effects Here 
        }
        Destroy(gameObject);
        
    }

}
