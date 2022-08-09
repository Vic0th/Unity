using UnityEngine;

public class RopeHandle : MonoBehaviour
{
    PlayerScript player;

    void Awake() => player = GetComponentInParent<PlayerScript>();

    void OnCollisionEnter2D(Collision2D c) => 
        player.OnRopeSlide(c.GetContact(0).point.y > transform.position.y,
        transform.position.x + 0.06 <  c.GetContact(0).point.x,
        transform.position.x - 0.06 >  c.GetContact(0).point.x,
        c.gameObject.GetComponent<RopeDirection>().Direction);
    void OnCollisionExit2D(Collision2D c) => player.OffRopeSlide();
}
