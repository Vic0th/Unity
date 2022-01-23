using UnityEngine;

public class Char : MonoBehaviour
{
    [SerializeField]
    short health = 3;

    public void dealDamage(byte dmg) {
        health -= dmg;
        if(health < 1) {
            //Delete the whole hierarchy
            Destroy(gameObject);
        }
    }
}
