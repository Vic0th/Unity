using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField]
    Text time;
    [SerializeField]
    Scoring scr;
    [SerializeField]
    RandomSpawner rp;


    [SerializeField]
    float roundTime = 60;
    float leftTime = 60;


    void Start()
    {
        //leftTime = roundTime;
    }

    void Update()
    {
        if(Mathf.Round(leftTime) > 0) {
            leftTime -= Time.deltaTime;
            time.text = $"Time Left: {leftTime}";
        }
    }
}
