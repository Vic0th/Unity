using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField]
    float respawnInterval = 3, maxDist = 10;

    [SerializeField]
    GameObject doll;


    [SerializeField]
    Scoring scr;


    Vector2 ran;
    float x;
    bool isSpawning = true;

    IEnumerator spawn() {
        while(isSpawning) {
            ran = Random.insideUnitCircle * maxDist;

            if(ran.x > -5 && ran.x < 5) {
                x = Random.Range(5 - ran.x,maxDist - ran.x);
                ran.x += (ran.x > 0) ? x : -x;
            }
            if(ran.y > -5 && ran.y < 5) {
                x = Random.Range(5 - ran.y,maxDist - ran.y);
                ran.y += (ran.y > 0) ? x : -x;
            }

            Instantiate(doll,ran,Quaternion.identity).transform.SetParent(transform);

            yield return new WaitForSeconds(respawnInterval);
        }
    }


    void Start()
    {
        StartCoroutine(spawn());
    }

    public void increaseScoring() {
        scr.increase();
    }

    public void destroyChildren() {
        foreach (Transform child in transform) {
        Destroy(child.gameObject);
        }
        
    }
}
