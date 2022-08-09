using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    uint maxHp = 5, curHp = 3;

    PlayerScript ps;
    Transform[] spawnPoints;
    GameMenu gm;

    void Awake(){
        gm = GetComponent<GameMenu>();
        ps = GameObject.Find("Player").GetComponent<PlayerScript>();
        spawnPoints = GameObject.Find("/Checkpoints/SpawnPoints").GetComponentsInChildren<Transform>();
    }


    public void HealPlayer(uint heal) => curHp = curHp + heal > maxHp ? maxHp : curHp + heal;


    public void DamagePlayer(uint dmg) {
        if(curHp <= dmg) {
            curHp = 0;
            gm.GameOverScreen();
            return;
        }

        curHp -= dmg;
        SpawnCheckpoint(Checkpoints.GetLastCP());
    }



    void SpawnCheckpoint(uint cp) {
        try {
            ps.rb.position = spawnPoints[cp+1].position;
        }
        catch{
            ps.rb.position = spawnPoints[0].position;
        }
        ps.rb.velocity = Vector2.zero;
    }



    public void FinishMap() {
        gm.FinishMapScreen();
        //Save the progression
    }
}
