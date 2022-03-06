using UnityEngine;
using UnityEngine.Tilemaps;

public class Ramps : MonoBehaviour
{
    TilemapCollider2D tm;

    [SerializeField]
    PhysicsMaterial2D def, frict;

    PlayerScript ps;

    void Awake() {
        tm = GetComponent<TilemapCollider2D>();
        ps = GameObject.Find("/Player").GetComponent<PlayerScript>();
    } 
    void FixedUpdate() => tm.sharedMaterial = (ps.moveVar != 0) ? def : frict;
    
}
