using UnityEngine;

public class Weapon: MonoBehaviour {
    [SerializeField]
    float FireRate = 3.5f, lifeTime = 2.5f;
    [SerializeField]
    byte damage = 1;
    [SerializeField]
    short bulletSpeed = 7500;

    public GameObject bulletPrefab;
    PlayerScript ps;


    float ShootTime;

    private void Awake() {
        ShootTime = (float)1 / FireRate;
        ps = transform.GetComponentInParent<PlayerScript>();
    }


    bool isShot;
    public void Shoot(float isShooting) => isShot = isShooting != 0;



    float nextShoot = 0;
    GameObject go;
    sbyte dir;

    void Fire() {
        if(Time.time >= nextShoot) {
            //dir = ps.getDirection();

            go = Instantiate(bulletPrefab,transform.position + (1f * dir * Vector3.right),Quaternion.Euler(0,0,-90 * dir));
            go.GetComponent<Bullet>().setStats(damage,(short)(bulletSpeed * dir),lifeTime);
            go.transform.parent = transform;

            nextShoot = Time.time + ShootTime;
        }
    }



    void Update() {
        if(isShot)
            Fire();
    }

}
