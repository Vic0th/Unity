using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    float fireRate = 3.5f;
    [SerializeField]
    byte damage = 1;

    public GameObject bulletPrefab;
    Transform tr;

    
    float time4Shot;

    private void Awake() {
        time4Shot = (float)1 / fireRate;
        tr = GetComponentInChildren<SpriteRenderer>().transform;
    }
 
    /*
    IEnumerator shootRefresh() {
         yield return new WaitForSeconds(time4Shot);

         readyShooting = true;
    }
    */

    
    bool isShot = false;

    public void shooting(float isShooting) {

        isShot = (isShooting == 0) ? false : true;

    }



    float betweenShots = 0;
    bool readyShooting = true;
    Quaternion qu;

    private void Update() {
        if(!readyShooting) {
            betweenShots += Time.deltaTime;

            if(betweenShots >= time4Shot)
                readyShooting = true;

        }

        else if(isShot) {
            readyShooting = false;
            betweenShots = 0;

            //Sadece Quaternion kullanimina ornek olsun diye
            qu = tr.rotation;
            Instantiate(bulletPrefab,transform.position + (tr.up * 0.9f),qu);

        }

    }

}
