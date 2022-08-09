using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorFriction: MonoBehaviour {
    [SerializeField]
    PhysicsMaterial2D def, fric;

    TilemapCollider2D tmc;
    PlayerScript ps;

    void OnEnable() {
        tmc = GetComponent<TilemapCollider2D>();
        ps = GameObject.Find("/Player").GetComponent<PlayerScript>();
    }

    void Update() => tmc.sharedMaterial = (ps.moveInpHor == 0 && ps.onFloor) ? fric : def;
}
